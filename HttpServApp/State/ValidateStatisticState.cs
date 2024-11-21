using HttpServApp.Models;
using System.Net.Sockets;

namespace HttpServApp.State
{
    // Стан після валідації: валідний запит статистичних даних
    internal class ValidateStatisticState: IState
    {
        public void ProcessingHandler(HttpRequest httpRequest, Socket socket)
        {
            // Формуємо відповідь: всередині віртуального метода запитуються дані статистики
            httpRequest.CreateResponse();
            // Відсилаємо відповідь клієнту
            httpRequest.SendResponse(socket);
            Console.WriteLine($"HttpRequest state: ValidateStatisticState");

            // Перехід у новий стан: після відправки відповіді клієнту
            httpRequest.TransitionTo(new SendedState(), socket);

        }
    }
}
