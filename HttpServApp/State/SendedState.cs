using HttpServApp.Models;
using System.Net.Sockets;


namespace HttpServApp.State
{
    // Стан після відправки даних клієнту: необхідно зберегти інформацію про запит в БД
    internal class SendedState: IState
    {
        public void ProcessingHandler(HttpRequest httpRequest, Socket socket)
        {
            // Викликаємо метод запису даних про запит до БД
            httpRequest.Repository.SaveToDB(httpRequest, '+');

            // Перехід у фінальний стан
            httpRequest.TransitionTo(new DoneState(), socket);
        }
    }
}
