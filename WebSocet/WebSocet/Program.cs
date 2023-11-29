using System;
using System.Net.Sockets;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Введiть IP-адресу сервера:");
        string ipAddress = Console.ReadLine();

        Console.WriteLine("Введiть порт сервера:");
        int port = int.Parse(Console.ReadLine());

        Console.WriteLine("Введiть число для перевiрки:");
        int number = int.Parse(Console.ReadLine());

        Client client = new Client(ipAddress, port, number);
        client.Connect();
    }
}

class Client
{
    private string ipAddress;
    private int port;
    private int number;

    public Client(string ipAddress, int port, int number)
    {
        this.ipAddress = ipAddress;
        this.port = port;
        this.number = number;
    }

    public void Connect()
    {
        using (TcpClient tcpClient = new TcpClient(ipAddress, port))
        {
            using (NetworkStream stream = tcpClient.GetStream())
            {
                byte[] data = System.Text.Encoding.UTF8.GetBytes(number.ToString());
                stream.Write(data, 0, data.Length);

                data = new byte[1024];
                int bytesRead = stream.Read(data, 0, data.Length);
                string response = System.Text.Encoding.UTF8.GetString(data, 0, bytesRead);

                Console.WriteLine("Результат перевiрки: {0}", response);
            }
        }
    }
}