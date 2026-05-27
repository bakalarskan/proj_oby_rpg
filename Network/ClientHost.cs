using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using lab_game.dto;

namespace lab_game.network
{
    public sealed class ClientHost
    {
        private readonly string _host;
        private readonly int _port;
        private TcpClient? _client;

        public ClientHost(string host, int port)
        {
            _host = host;
            _port = port;
        }

        public GameModelDto ConnectAndReceiveInitialState()
        {
            _client = new TcpClient();
            _client.Connect(_host, _port);
            NetworkStream stream = _client.GetStream();
            string json = NetworkMessage.ReceiveJson(stream);
            NetworkUpdateDto update = NetworkUpdateSerializer.Deserialize(json);
            return update.State ?? new GameModelDto();
        }

        public void StartReceiveLoop(Action<NetworkUpdateDto> onUpdate)
        {
            if (_client == null)
            {
                throw new InvalidOperationException("Brak połączenia z serwerem.");
            }

            NetworkStream stream = _client.GetStream();
            Task.Run(() =>
            {
                while (_client.Connected)
                {
                    string json = NetworkMessage.ReceiveJson(stream);
                    NetworkUpdateDto update = NetworkUpdateSerializer.Deserialize(json);
                    onUpdate(update);
                }
            });
        }

        public void Disconnect()
        {
            _client?.Close();
        }
    }
}