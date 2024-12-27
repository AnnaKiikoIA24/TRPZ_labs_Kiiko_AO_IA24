using HttpServApp.Models;
using System.Net.Sockets;

namespace HttpServApp.State
{
  // Фiнальний стан об'єкта
  internal class DoneState : IState
  {
    public void ProcessingHandler(HttpRequest httpRequest, Socket socket)
    {
      // Цей стан - останнiй, перехiд не потрiбен
      return;
    }
  }
}
