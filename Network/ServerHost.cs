using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using lab_game.dto;
using lab_game.model;

namespace lab_game.network
{
    public sealed class ServerHost
    {
        private const int MaxClients = 9;
        private readonly TcpListener _listener;
        private readonly List<TcpClient> _clients = new List<TcpClient>();
        private readonly Dictionary<TcpClient, Player> _clientPlayers = new Dictionary<TcpClient, Player>();
        private readonly Dictionary<TcpClient, int> _clientSlots = new Dictionary<TcpClient, int>();
        private readonly GameModel _model;
        private readonly ConcurrentQueue<PlayerActionDto> _actions = new ConcurrentQueue<PlayerActionDto>();
        private CancellationTokenSource? _cts;

        public ServerHost(GameModel model, int port)
        {
            _model = model;
            _listener = new TcpListener(IPAddress.Any, port);
        }

        public void Start()
        {
            _cts = new CancellationTokenSource();
            _listener.Start();
            Task.Run(() => AcceptLoop(_cts.Token));
        }

        public void Stop()
        {
            _cts?.Cancel();
            _listener.Stop();
            lock (_clients)
            {
                foreach (TcpClient c in _clients)
                {
                    c.Close();
                }
                _clients.Clear();
                _clientPlayers.Clear();
                _clientSlots.Clear();
            }
        }

        public bool TryDequeueAction(out PlayerActionDto action)
        {
            return _actions.TryDequeue(out action);
        }

        public void BroadcastState()
        {
            NetworkUpdateDto update;
            lock (_model)
            {
                update = NetworkUpdateSerializer.CreateState(_model);
            }
            string json = NetworkUpdateSerializer.Serialize(update);
            SendToAll(json);
        }

        public void BroadcastAction(PlayerActionDto action)
        {
            NetworkUpdateDto update = NetworkUpdateSerializer.CreateAction(action);
            string json = NetworkUpdateSerializer.Serialize(update);
            SendToAll(json);
        }

        private void AcceptLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                TcpClient client = _listener.AcceptTcpClient();
                Player player;
                lock (_clients)
                {
                    if (_clients.Count >= MaxClients)
                    {
                        client.Close();
                        continue;
                    }

                    _clients.Add(client);
                    int slot = GetNextSlot();
                    _clientSlots[client] = slot;

                    player = CreatePlayer(slot);
                    _clientPlayers[client] = player;
                }

                lock (_model)
                {
                    _model.AddPlayer(player);
                }

                NetworkStream stream = client.GetStream();
                NetworkUpdateDto update;
                lock (_model)
                {
                    update = NetworkUpdateSerializer.CreateState(_model);
                }
                string json = NetworkUpdateSerializer.Serialize(update);
                NetworkMessage.SendJson(stream, json);

                BroadcastState();
                Task.Run(() => ClientLoop(client, player, token));
            }
        }

        private void ClientLoop(TcpClient client, Player player, CancellationToken token)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                while (!token.IsCancellationRequested && client.Connected)
                {
                    string json = NetworkMessage.ReceiveJson(stream);
                    NetworkUpdateDto update = NetworkUpdateSerializer.Deserialize(json);
                    if (update.Action != null)
                    {
                        update.Action.PlayerName = player.Name;
                        _actions.Enqueue(update.Action);
                    }
                }
            }
            catch
            {
                // rozłączenie klienta
            }
            finally
            {
                Player? removedPlayer = null;
                lock (_clients)
                {
                    _clients.Remove(client);
                    if (_clientPlayers.TryGetValue(client, out Player playerToRemove))
                    {
                        removedPlayer = playerToRemove;
                        _clientPlayers.Remove(client);
                    }
                    _clientSlots.Remove(client);
                }

                if (removedPlayer != null)
                {
                    lock (_model)
                    {
                        _model.RemovePlayer(removedPlayer);
                    }
                    BroadcastState();
                }

                client.Close();
            }
        }

        private int GetNextSlot()
        {
            for (int i = 1; i <= MaxClients; i++)
            {
                if (!_clientSlots.Values.Contains(i))
                {
                    return i;
                }
            }
            return MaxClients;
        }

        private static Player CreatePlayer(int slot)
        {
            Player player = new Player($"Gracz {slot}");
            player.SetSymbol((char)('0' + slot));
            return player;
        }

        private void SendToAll(string json)
        {
            lock (_clients)
            {
                for (int i = _clients.Count - 1; i >= 0; i--)
                {
                    TcpClient client = _clients[i];
                    if (!client.Connected)
                    {
                        _clients.RemoveAt(i);
                        continue;
                    }

                    NetworkStream stream = client.GetStream();
                    NetworkMessage.SendJson(stream, json);
                }
            }
        }
    }
}