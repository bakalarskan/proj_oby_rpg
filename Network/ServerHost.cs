using System;
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
        private readonly GameModel _model;
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
            }
        }

        public void BroadcastState()
        {
            NetworkUpdateDto update = NetworkUpdateSerializer.CreateState(_model);
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
                lock (_clients)
                {
                    if (_clients.Count >= MaxClients)
                    {
                        client.Close();
                        continue;
                    }
                    _clients.Add(client);
                }

                NetworkStream stream = client.GetStream();
                NetworkUpdateDto update = NetworkUpdateSerializer.CreateState(_model);
                string json = NetworkUpdateSerializer.Serialize(update);
                NetworkMessage.SendJson(stream, json);
            }
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