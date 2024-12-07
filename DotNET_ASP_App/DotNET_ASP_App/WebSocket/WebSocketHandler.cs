using System.Net.WebSockets;
using System.Text;

namespace DotNET_ASP_App.WebSocket;

public class WebSocketHandler
{
    private readonly List<System.Net.WebSockets.WebSocket> _sockets = new();

    public void AddSocket(System.Net.WebSockets.WebSocket socket)
    {
        _sockets.Add(socket);
    }

    public async Task BroadcastMessageAsync(string message)
    {
        var buffer = Encoding.UTF8.GetBytes(message);
        var tasks = _sockets.Select(async socket =>
        {
            if (socket.State == WebSocketState.Open)
            {
                await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        });

        await Task.WhenAll(tasks);
    }
}