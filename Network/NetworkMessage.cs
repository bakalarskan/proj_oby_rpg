using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace lab_game.network
{
    public static class NetworkMessage
    {
        public static void SendJson(NetworkStream stream, string json)
        {
            byte[] payload = Encoding.UTF8.GetBytes(json);
            byte[] length = BitConverter.GetBytes(payload.Length);

            stream.Write(length, 0, length.Length);
            stream.Write(payload, 0, payload.Length);
            stream.Flush();
        }

        public static string ReceiveJson(NetworkStream stream)
        {
            byte[] lengthBytes = ReadExact(stream, sizeof(int));
            int length = BitConverter.ToInt32(lengthBytes, 0);
            if (length <= 0)
            {
                return string.Empty;
            }

            byte[] payload = ReadExact(stream, length);
            return Encoding.UTF8.GetString(payload);
        }

        private static byte[] ReadExact(NetworkStream stream, int size)
        {
            byte[] buffer = new byte[size];
            int read = 0;
            while (read < size)
            {
                int chunk = stream.Read(buffer, read, size - read);
                if (chunk == 0)
                {
                    throw new IOException("Połączenie przerwane.");
                }
                read += chunk;
            }
            return buffer;
        }
    }
}