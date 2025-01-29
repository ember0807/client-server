using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Server
{
	static void Main(string[] args)
	{
		int port = 5000;
		IPAddress localAddr = IPAddress.Parse("127.0.0.1");

		TcpListener server = new TcpListener(localAddr, port);
		server.Start();
		Console.WriteLine("Сервер запущен. Ожидание подключения...");

		while (true)
		{
			TcpClient client = server.AcceptTcpClient();
			Console.WriteLine("Клиент подключился.");

			Task.Run(() => HandleClient(client));
		}
	}

	static void HandleClient(TcpClient client)
	{
		try
		{
			IPEndPoint clientEndPoint = (IPEndPoint)client.Client.RemoteEndPoint;
			Console.WriteLine($"IP-адрес клиента: {clientEndPoint.Address}, Порт клиента: {clientEndPoint.Port}");

			NetworkStream stream = client.GetStream();
			byte[] buffer = new byte[256];

			while (true)
			{
				int bytesRead = stream.Read(buffer, 0, buffer.Length);
				if (bytesRead == 0)
				{
					Console.WriteLine("Клиент закрыл соединение.");
					break;
				}
				string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
				Console.WriteLine($"Полученное сообщение: {message}");
			}
		}
		catch (Exception e)
		{
			Console.WriteLine($"Ошибка при обработке клиента: {e.Message}");
		}
		finally
		{
			client.Close();
			Console.WriteLine("Соединение с клиентом закрыто.");
		}
	}
}