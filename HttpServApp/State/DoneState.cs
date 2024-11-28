using HttpServApp.Models;
using System.Net.Sockets;

namespace HttpServApp.State
{
    // Фінальний стан об'єкта
    internal class DoneState: IState
    {
        public void ProcessingHandler(HttpRequest httpRequest, Socket socket)
        {
            // Цей стан - останній, перехід не потрібен
            return;
        }
    }
}
