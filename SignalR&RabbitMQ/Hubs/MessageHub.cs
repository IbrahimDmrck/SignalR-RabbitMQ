using Microsoft.AspNetCore.SignalR;

namespace SignalR_RabbitMQ.Hubs
{
    public class MessageHub : Hub
    {
        public async Task SendMessageAsync(string message)
        {
            await Clients.All.SendAsync("receiveMessage", message);
        }
    }
}
