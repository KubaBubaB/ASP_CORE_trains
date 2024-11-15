namespace DotNET_ASP_App.WebSocket;

public class NotificationService
{
    private readonly WebSocketHandler _webSocketHandler;

    public NotificationService(WebSocketHandler webSocketHandler)
    {
        _webSocketHandler = webSocketHandler;
    }

    public async Task NotifyClientsAsync(string message)
    {
        await _webSocketHandler.BroadcastMessageAsync(message);
    }
}