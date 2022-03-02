using Chat_Application.Crud;
using Chat_Application.Help_Classes;
using Chat_Application.Utilities;
using Microsoft.AspNetCore.SignalR;
namespace Chat_Application.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", DateTime.Now.ToString(), user, message);
        }

        public async Task SendPrivateMessage(string from, string to, string message)
        {

            var connections = ConnectionUtility.AllConnections
                .Where(x => x.Value
                .Equals(to, StringComparison.OrdinalIgnoreCase) && x.Value != from)
                .ToDictionary(a => a.Key, b => b.Value);

            if (connections.Count == 0)
                return;

            await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", DateTime.Now.ToString("HH:mm"), from, message);
            foreach (var item in connections)
            {
                await Clients.Client(item.Key).SendAsync("ReceiveMessage", DateTime.Now.ToString("HH:mm"), from, message);
            }
        }
    }
}
