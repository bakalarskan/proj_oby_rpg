using System.Net.Sockets;
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
            return GameModelSerializer.Deserialize(json);
        }

        public void Disconnect()
        {
            _client?.Close();
        }
    }
}