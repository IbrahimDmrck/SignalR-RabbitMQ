using EmailSenderExample;
using Microsoft.AspNetCore.SignalR.Client;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

internal class Program
{
    private static void Main(string[] args)
    {
        HubConnection hubConnection = new HubConnectionBuilder().WithUrl("https://localhost:7036/messagehub").Build();
        hubConnection.StartAsync().Wait();


        var factory = new ConnectionFactory { HostName = "your cloudamqp Hostname" };
        factory.Uri = new Uri("your cloudamqp URL");
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "messagequeue",
                 durable: false,
                 exclusive: false,
                 autoDelete: false,
                 arguments: null);

        var consumer = new EventingBasicConsumer(channel);

        channel.BasicConsume(queue: "messagequeue",
                     autoAck: true,
                     consumer: consumer);

        consumer.Received += async (model, ea) =>
        {
            //Email Gönderme İşlemi
            //ea.Body.Span
            string serializeData = Encoding.UTF8.GetString(ea.Body.Span);
            User user = JsonSerializer.Deserialize<User>(serializeData);
            EmailSender.Send(user.Email,user.Message);
            Console.WriteLine("Mail Gönderilmiştir");
           await hubConnection.InvokeAsync("SendMessageAsync",$"{user.Email} mail adresine eposta gönderilmiştir");

        };

        Console.Read();

    }
}