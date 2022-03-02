using Chat_Application.Crud;
using Chat_Application.Help_Classes;
using Chat_Application.Utilities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.JSInterop;
namespace Chat_Application.Hubs
{
    public class ChatHub : Hub
    {
        private IJSRuntime _jsRuntime;
        public ChatHub(IJSRuntime jsRuntime)   
        {
            _jsRuntime = jsRuntime;
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendPrivateMessage(string from, string to, string message)
        {
            var identity = Context.UserIdentifier;

            var id = Context.ConnectionId;

            var conn = ConnectionUtility.AllConnections.Where(x => x.Value == to).FirstOrDefault();

            await Clients.Client(conn.Key).SendAsync("ReceiveMessage", from, message);





            // await Clients.Client(connection.Result.ConnectionId).SendAsync("ReceiveMessage", message);
        }
    }
}
