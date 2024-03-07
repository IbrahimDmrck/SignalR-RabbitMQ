using EmailSenderExample;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

internal class Program
{
    private static void Main(string[] args)
    {
        var factory = new ConnectionFactory { HostName = "whale.rmq.cloudamqp.com" };
        factory.Uri = new Uri("amqps://whkencda:eGKwGtVaoxyw41itUW2BuXrzDKTJU_Wr@whale.rmq.cloudamqp.com/whkencda");
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

        consumer.Received += (model, ea) =>
        {
            //izea nupa ntmq opdf
            //Email Gönderme İşlemi
            //ea.Body.Span
            string serializeData = Encoding.UTF8.GetString(ea.Body.Span);
            User user = JsonSerializer.Deserialize<User>(serializeData);
            EmailSender.Send(user.Email,user.Message);
            Console.WriteLine("Mail Gönderilmiştir");
        };
        Console.Read();

    }
}