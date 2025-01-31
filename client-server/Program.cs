﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Server
{
	static List<TcpClient> clients = new List<TcpClient>();

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
			clients.Add(client); // Добавляем клиента в список
			DisplayConnectedClients(); // Отображаем количество подключенных клиентов

			Task.Run(() => HandleClient(client));
		}
	}

	static void HandleClient(TcpClient client)
	{
		try
		{
			NetworkStream stream = client.GetStream();
			byte[] buffer = new byte[256];
			int bytesRead;

			while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
			{
				string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
				Console.WriteLine("Получено сообщение: " + message);
				byte[] response = Encoding.UTF8.GetBytes(message);
				stream.Write(response, 0, response.Length);
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine("Ошибка: " + ex.Message);
		}
		finally
		{
			client.Close();
			clients.Remove(client); // Удаляем клиента из списка
			Console.WriteLine("Клиент отключился.");
			DisplayConnectedClients(); // Обновляем количество подключенных клиентов
		}
	}

	static void DisplayConnectedClients()
	{
		Console.WriteLine($"Количество подключенных клиентов: {clients.Count}");
	}
}