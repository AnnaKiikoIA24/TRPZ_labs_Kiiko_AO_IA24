using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using HttpServApp.Models;
using HttpServApp.Processing;

namespace HttpServApp
{
    internal class Listener
    {
        public int Port { get; } = Configuration.Port;
        // сокет обміну повідомленнями
        private readonly Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        // події сокета
        private readonly SocketAsyncEventArgs socketAsyncArgs = new SocketAsyncEventArgs(); 

        public Listener() 
        { }
        public Listener(int port)
        {
            Port = port;
        }

        // Запуск сервера 
        public void RunServer() 
        {
            try
            {
                Repository repository = new Repository();
                repository.LoadFromDb();

                // починаємо прослуховування
                socket.Bind(new IPEndPoint((long)AddressFamily.InterNetwork, Port));
                // прослуховуємо max 4000 підключень
                socket.Listen(4000);
                while (true)
                {
                    ThreadProcessing threadProcessing = new ThreadProcessing(repository, socket);
                    Thread.Sleep(1000);
                }

            }
            catch (Exception exc)
            {
                Console.WriteLine($"RunServer error: {exc.Message}");
            }
        }

    }
}
