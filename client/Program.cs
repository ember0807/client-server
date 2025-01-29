using System;
using System.Net.Sockets;
using System.Text;

class Client
{
	static void Main(string[] args)
	{
		string clientId;

		if (args.Length < 1)
		{
			Console.Write("Пожалуйста, введите идентификатор клиента: ");
			clientId = Console.ReadLine();
		}
		else
		{
			clientId = args[0]; // Идентификатор клиента
		}

		int port = 5000;

		ConnectToServer(clientId, port);
	}

	static void ConnectToServer(string clientId, int port)
	{
		try
		{
			using (TcpClient client = new TcpClient("127.0.0.1", port))
			{
				Console.WriteLine($"{clientId} подключён к серверу.");
				NetworkStream stream = client.GetStream();

				while (true)
				{
					Console.Write($"{clientId}, введите сообщение для отправки на сервер (или 'exit' для выхода): ");
					string message = Console.ReadLine();

					if (message.ToLower() == "exit")
					{
						break; // Выход из цикла, если введено "exit"
					}

					// Формируем сообщение с идентификатором клиента
					string formattedMessage = $"{clientId}: {message}";
					byte[] data = Encoding.UTF8.GetBytes(formattedMessage);

					// Отправка сообщения на сервер
					stream.Write(data, 0, data.Length);
					Console.WriteLine($"{clientId} отправлено сообщение: {message}");
				}

				stream.Close();
				Console.WriteLine($"{clientId} соединение с сервером закрыто.");
			}
		}
		catch (Exception e)
		{
			Console.WriteLine("Ошибка: {0}", e.Message);
		}
	}
}