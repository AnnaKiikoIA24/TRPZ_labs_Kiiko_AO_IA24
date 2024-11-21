using HttpServApp.Models;
using System.Net.Sockets;

namespace HttpServApp.State
{
    // Стан після валідації: невалідний запит 
    internal class InvalidState: IState
    {
        public void ProcessingHandler(HttpRequest httpRequest, Socket socket)
        {
            // Формуємо відповідь: статус 500, запит не валідний
            httpRequest.CreateResponse();
            // Відсилаємо відповідь клієнту
            httpRequest.SendResponse(socket);
            Console.WriteLine($"HttpRequest state: InvalidState");

            // Перехід у новий стан: після відправки відповіді клієнту
            httpRequest.TransitionTo(new SendedState(), socket);
        }
    }
}
