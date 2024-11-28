using HttpServApp.Models;
using System.Net.Sockets;

namespace HttpServApp.State
{
    // Стан після валідації: валідний запит даних Web-сторінки
    internal class ValidatePageState: IState
    {
        public void ProcessingHandler(HttpRequest httpRequest, Socket socket)
        {
            // Формуємо відповідь: метод віртуальний, повертає дані сторінки, що запитується
            httpRequest.CreateResponse();
            // Відсилаємо відповідь клієнту
            httpRequest.SendResponse(socket);
            Console.WriteLine($"HttpRequest state: ValidatePageState");

            // Перехід у новий стан: після відправки відповіді клієнту
            httpRequest.TransitionTo(new SendedState(), socket);
        }
    }
}
