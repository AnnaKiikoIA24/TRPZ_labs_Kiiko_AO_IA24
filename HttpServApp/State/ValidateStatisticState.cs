using HttpServApp.Builder;
using HttpServApp.Models;
using System.Net.Sockets;

namespace HttpServApp.State
{
  // Стан після валідації: валідний запит статистичних даних
  internal class ValidateStatisticState : IState
  {
    public void ProcessingHandler(HttpRequest httpRequest, Socket socket)
    {
      // Будуємо відповідь за допомогою методів інтерфейсу IBuilder
      IBuilder builder = new BuilderStat(httpRequest);
      string htmlResponse =
          builder.BuildVersion() +
          builder.BuildStatus() +
          builder.BuildHeaders() +
          builder.BuildContentBody();

      // Відсилаємо відповідь клієнту
      httpRequest.SendResponse(socket, htmlResponse);
      Console.WriteLine($"HttpRequest state: ValidateStatisticState");

      // Перехід у новий стан: після відправки відповіді клієнту
      httpRequest.TransitionTo(new SendedState(), socket);

    }
  }
}
