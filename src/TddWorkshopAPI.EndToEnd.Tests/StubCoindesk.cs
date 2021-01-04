using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Coindesk;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace TddWorkshopAPI.EndToEnd.Tests
{
    internal class StubCoindesk : IDisposable
    {
        internal readonly int Port = GetRandomUnusedPort();
        
        private readonly HttpListener _listener;
        private readonly ILogger _logger;
        private BitcoinPriceIndexResponse _responseToReturn;

        public StubCoindesk(ILogger logger)
        {
            var url = $"http://+:{Port}/";
            
            _logger = logger;
            
            _listener = new HttpListener();
            _listener.Prefixes.Add(url);
            _listener.Start();
            
            _listener.BeginGetContext(ListenerCallback, _listener);
            
            _logger.Log(LogLevel.Information, $"StubCoindesk is listening at {url}");
        }

        public void OnRequestReturns(BitcoinPriceIndexResponse response)
        {
            _responseToReturn = response;
        }
        
        private void ListenerCallback(IAsyncResult result)
        {
            var listener = (HttpListener)result.AsyncState;
            listener.BeginGetContext(ListenerCallback, listener);

            var context = listener.EndGetContext(result);
            var request = context.Request;
            
            _logger.Log(LogLevel.Information, $"Received request at '{request.RawUrl}'");
            
            var response = context.Response;

            var responseContent = JsonConvert.SerializeObject(_responseToReturn);

            _logger.Log(LogLevel.Information, $"Returning response content '{responseContent}'");
            
            response.ContentLength64 = responseContent.Length;
            var output = response.OutputStream;
            output.Write(Encoding.UTF8.GetBytes(responseContent), 0, responseContent.Length);
            output.Close();
        }
        
        private static int GetRandomUnusedPort()
        {
            var listener = new TcpListener(IPAddress.Any, 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }

        public void Dispose()
        {
            _listener?.Stop();
        }
    }
}