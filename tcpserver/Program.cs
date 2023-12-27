using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;

class Program
{
    static void Main()
    {
        string configFile = "config.json";
        string[] messages = File.ReadAllLines("messages.txt");

        try
        {
            //  десериализация в список
            List<Config> configs = JsonConvert.DeserializeObject<List<Config>>(File.ReadAllText(configFile));

            foreach (Config config in configs)
            {
                foreach (string message in messages)
                {
                    ProcessMessage(config, message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        Console.ReadLine();
    }

    static void ProcessMessage(Config config, string message)
    {
        try
        {
            using (TcpClient client = new TcpClient(config.ServerAddress, config.ServerPort))
            using (NetworkStream stream = client.GetStream())
            using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                writer.WriteLine(message);
                writer.Flush();

                string response = reader.ReadLine();
                Console.WriteLine($"Ответ от сервера: {response}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при обработке сообщения: {ex.Message}");
        }
    }
}

class Config
{
    public string ServerAddress { get; set; }
    public int ServerPort { get; set; }
}
