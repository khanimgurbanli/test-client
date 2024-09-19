using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRClient
{
    class Program
    {
        private static HubConnection _connection;

        static async Task Main(string[] args)
        {
            string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiZWdoZ2hTYWxhbV84ODA3YmVkNC1jYjIzLTRiYmYtYWEwMi01YzdlNjI3NDdiNGIiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImRiMTk1NjRiLWViODctNDUxYS1hODA5LWUxZjlmNmU3MjI1ZSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6InNhbGFtQGdtYWlsLmNvbSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluIiwiZXhwIjoxNzI2NzMyMDgyLCJpc3MiOiJodHRwczovL2xhd3llci5heiIsImF1ZCI6Imh0dHBzOi8vbGF3eWVyLmF6In0.q9w5nL3zcgF0qCXia849hxwoioMFjwCCarPjjCv4xVE"; // Buraya geçerli JWT tokenınızı ekleyin.

            _connection = new HubConnectionBuilder()
                .WithUrl("https://legalis-api.webconsole.az/hubaction", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(token);
                })
                .Build();

            _connection.On<Guid, string>("RecieveMessage", (senderId, message) =>
            {
                Console.WriteLine($"Error! Sended: {senderId}, Message: {message}");
            });

            try
            {
                await _connection.StartAsync();
                Console.WriteLine("SignalR Connected.");

                Guid receiverId = Guid.NewGuid(); 
                string message = "Hi, this is error message.";
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
    }
}
