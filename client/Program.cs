using System;
using System.Net.Sockets;
using System.Text;

class Client
{
	static void Main(string[] args)
	{
		int port = 5000;

		// Устанавливаем соединение с сервером
		try
		{
			using (TcpClient client = new TcpClient("127.0.0.1", port))
			{
				Console.WriteLine("Клиент подключён к серверу.");

				// Ввод сообщения с клавиатуры
				Console.Write("Введите сообщение для отправки на сервер: ");
				string message = Console.ReadLine();
				byte[] data = Encoding.UTF8.GetBytes(message);

				// Отправка сообщения на сервер
				NetworkStream stream = client.GetStream();
				stream.Write(data, 0, data.Length);
				Console.WriteLine("Сообщение отправлено: {0}", message);

				// Закрываем соединение
				stream.Close();
			}
		}
		catch (Exception e)
		{
			Console.WriteLine("Ошибка: {0}", e.Message);
		}
	}
}