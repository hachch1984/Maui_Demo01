using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

/// <summary>
/// exposicion de endpoint de notificaciones
/// </summary>
public class Notification_EndPoint : Hub
{
    private static readonly ConcurrentDictionary<string, string> Connections = new();


    /// <summary>
    /// Se ejecuta cuando un cliente se conecta al hub
    /// </summary>
    /// <returns></returns>
    public override async Task OnConnectedAsync()
    {
        string? userId = Context.GetHttpContext()?.Request.Query["userId"];

        if (!string.IsNullOrEmpty(userId))
        {           
            Connections.TryAdd(Context.ConnectionId, userId);
        }

        await base.OnConnectedAsync();
    }
    /// <summary>
    /// Se ejecuta cuando un cliente se desconecta del hub
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {

        Connections.TryRemove(Context.ConnectionId, out _);

        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Buscar el ConnectionId correspondiente al UserId
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public static string? GetConnectionId(string userId)
    {
       
        return Connections.FirstOrDefault(x => x.Value == userId).Key;
    }
}


