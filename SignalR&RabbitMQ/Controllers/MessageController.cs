using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using SignalR_RabbitMQ.Models;
using System.Text;
using System.Text.Json;

namespace SignalR_RabbitMQ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromForm] User model)
        {
            var factory = new ConnectionFactory { HostName = "your cloudamqp Hostname" };
            factory.Uri = new Uri("your cloudamqp URL");
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "messagequeue",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
            string serializeData = JsonSerializer.Serialize(model);
            var body = Encoding.UTF8.GetBytes(serializeData);

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: "messagequeue",
                                 basicProperties: null,
                                 body: body);
            return Ok();
        }
    }
}
