using System;
using System.Threading;
using Microsoft.AspNetCore.TestHost;
using StreamJsonRpc;

namespace Gherkin.Testing.Utils.Extensions
{
   public static class SocketConnectionExtensions
    {

        public static (CancellationToken, T) CreateServiceFor<T>(this WebSocketClient socketClient, string socketUri ) where T : class
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;           

            var socket = socketClient.ConnectAsync(new Uri(socketUri), cancellationToken).GetAwaiter().GetResult();

            var rpcProvider = JsonRpc.Attach<T>(new WebSocketMessageHandler(socket));
            return (cancellationToken, rpcProvider);
        }
    }
}
