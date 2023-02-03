using Microsoft.AspNetCore.SignalR.Client;

namespace HangMan.Helpers
{
    public static class ConnectionHelper
    {
        public static bool ConnectionEstablished(HubConnection connection)
        {
            return connection != null && !string.IsNullOrEmpty(connection.ConnectionId) && connection.State == HubConnectionState.Connected;
        }
    }
}
