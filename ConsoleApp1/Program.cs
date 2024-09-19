using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalRClient
{
    class Program
    {
        private static HubConnection _connection;

        static async Task Main(string[] args)
        {
            string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoic3RyaW5nc3RyaW5nXzgwYWFhOTE0LTlkNTMtNGRhMS1iMGRmLTViZWNiNzE0M2Q0NiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiMjM4ZWIxMDktZjY3OS00Y2U2LWFhNGUtMjFiZTRlNzYzNWZjIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvZW1haWxhZGRyZXNzIjoic3RyaW5nQGdtYWlsLmNvbSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluIiwiZXhwIjoxNzI2NzMzNDEwLCJpc3MiOiJodHRwczovL2xhd3llci5heiIsImF1ZCI6Imh0dHBzOi8vbGF3eWVyLmF6In0.u-yrYeKnrXoxeg2cQ4MBZYc0bqhpnmGGn7X6kFia8Oc";
            _connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7100/hubaction", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(token);
                })
                .Build();

            // Set up the handler for receiving chat history
            _connection.On<List<Guid>>("ReceiveChatHistory", userIds =>
            {
                Console.WriteLine("Received User IDs:");
                foreach (var userId in userIds)
                {
                    Console.WriteLine(userId);
                }
            });

            try
            {
                await _connection.StartAsync();
                Console.WriteLine("SignalR Connected.");

                // Request user message history
                await GetUserMessageHistoryAsync();

                // Example of sending a message
                Guid receiverId = Guid.NewGuid();
                string message = "Hi, this is an error message.";
                await SendMessageAsync(receiverId, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.ReadLine();
        }

        public static async Task SendMessageAsync(Guid receiverId, string message)
        {
            var senderId = Guid.NewGuid();
            await _connection.InvokeAsync("SendMessage", receiverId, message);
        }

        public static async Task GetUserMessageHistoryAsync()
        {
            await _connection.InvokeAsync("GetUserMessageHistoryAsync");
        }
    }
}
