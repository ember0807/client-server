using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
	static void Main(string[] args)
	{
		int port = 5000;
		IPAddress localAddr = IPAddress.Parse("127.0.0.1");

		TcpListener server = new TcpListener(localAddr, port);
		server.Start();
		Console.WriteLine("Сервер запущен. Ожидание подключения...");

		using (TcpClient client = server.AcceptTcpClient())
		{
			Console.WriteLine("Клиент подключился.");

			// Выводим IP-адрес и порт клиента
			IPEndPoint clientEndPoint = (IPEndPoint)client.Client.RemoteEndPoint;
			Console.WriteLine($"IP-адрес клиента: {clientEndPoint.Address}, Порт клиента: {clientEndPoint.Port}");

			// Получение сообщения от клиента
			NetworkStream stream = client.GetStream();
			byte[] buffer = new byte[256];
			int bytesRead = stream.Read(buffer, 0, buffer.Length);
			string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
			Console.WriteLine($"Полученное сообщение: {message}");

			// Закрываем соединение
			stream.Close();
		}

		server.Stop();
	}
}