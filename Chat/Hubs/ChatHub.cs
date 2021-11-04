using Chat.Data;
using Chat.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Hubs
{
    public interface IChatClient
    {
        Task ReceiveMessage(string text, string sign, string when);
    }

    public class ChatHub : Hub<IChatClient>
    {
        readonly ApplicationDbContext _db;

        public ChatHub(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task SendToAll(string text)
        {
            var message = new Message { Text = text, Sign = Context.User.Identity.Name, When = DateTime.Now };

            // save the message in DB
            _db.Messages.Add(message);
            _db.SaveChanges();

            // send message to all
            await Clients.All.ReceiveMessage(message.Text, message.Sign, $"{message.When:HH%:mm}");
        }
    }
}
