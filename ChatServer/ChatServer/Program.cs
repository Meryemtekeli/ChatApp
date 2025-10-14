using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ChatServer
{
    class Program
    {
        static List<TcpClient> clients = new List<TcpClient>();
        static TcpListener listener;

        static void Main(string[] args)
        {
            int port = 8080;
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Console.WriteLine($"Server {port} portunda başladı...");

            Thread acceptThread = new Thread(AcceptClients);
            acceptThread.Start();

            Console.WriteLine("Çıkmak için ENTER tuşuna basın.");
            Console.ReadLine();
            listener.Stop();
        }

        static void AcceptClients()
        {
            while (true)
            {
                try
                {
                    TcpClient client = listener.AcceptTcpClient();
                    clients.Add(client);
                    Console.WriteLine("Yeni bir client bağlandı.");

                    Thread clientThread = new Thread(() => HandleClient(client));
                    clientThread.Start();
                }
                catch { break; }
            }
        }

        static void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int byteCount;
            string clientName = "Bilinmeyen";

            try
            {
                // İlk mesaj kullanıcı adı
                byteCount = stream.Read(buffer, 0, buffer.Length);
                if (byteCount > 0)
                    clientName = Encoding.UTF8.GetString(buffer, 0, byteCount);

                Console.WriteLine($"{clientName} bağlandı.");

                while ((byteCount = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, byteCount);
                    Console.WriteLine($"{clientName}: {message}");
                    Broadcast($"{clientName}: {message}", client);
                }
            }
            catch
            {
                Console.WriteLine($"{clientName} bağlantısı koptu.");
            }
            finally
            {
                clients.Remove(client);
                client.Close();
            }
        }

        static void Broadcast(string message, TcpClient sender)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);

            foreach (var client in clients)
            {
                try
                {
                    if (client.Connected)
                        client.GetStream().Write(data, 0, data.Length);
                }
                catch { }
            }
        }
    }
}
