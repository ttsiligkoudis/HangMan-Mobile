using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangMan.Helpers
{
    public class HubConnectionHelper
    {
        public HubConnection hubConnection { get; set; }
        public string baseUrl { get; set; }
        public string lblChat { get; set; }

        public string userName { get; set; }

        public HubConnectionHelper(string userName)
        {
            baseUrl = "http://localhost";

            // Android can't connect to localhost
            if (DeviceInfo.Current.Platform == DevicePlatform.Android)
            {
                baseUrl = "http://10.0.2.2";
            }
            hubConnection = new HubConnectionBuilder()
                .WithUrl($"{baseUrl}:5127/chatHub")
                .Build();
            this.userName = userName;
        }

        public string ReceiveMessage()
        {
            hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                lblChat += $"<b>{user}</b>: {message}<br/>";
            });
            return lblChat;
        }

        public async void SendMessageToAll(string message)
        {
            await hubConnection.InvokeCoreAsync("SendMessageToAll", args: new[]
            {
                userName,
                message
            });
        }

        public void Run()
        {
            Task.Run(async () => await hubConnection.StartAsync());
        }
    }
}
