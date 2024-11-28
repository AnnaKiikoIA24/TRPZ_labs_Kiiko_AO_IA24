using HttpServApp.Models;
using System.Net.Sockets;

namespace HttpServApp.State
{
    // Інтерфейс, що відповідає за стан об'єкта HttpRequest
    interface IState
    {
        public void ProcessingHandler(HttpRequest httpRequest, Socket socket);
    }
}
