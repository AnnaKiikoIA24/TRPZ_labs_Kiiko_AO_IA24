using HttpServApp.Fabric;
using HttpServApp.Models;
using HttpServApp.State;
using System.Net;
using System.Net.Sockets;

namespace HttpServApp.Processing
{
  internal class ThreadProcessing
  {
    // Об'єкт ля забезпечення обміну даними через мережу
    private readonly Socket socket;
    // Потік обробки даних
    private readonly Thread workThread;
    // Посилання на репозиторій
    private readonly Repository repository;

    public ThreadProcessing(Repository repository, Socket socket)
    {
      this.repository = repository;
      this.socket = socket;

      // Створюємо та запускаємо потік обробки даних запиту
      workThread = new Thread(DoWork) { Name = "requestThread" };
      workThread.Start();
    }

    /// <summary>
    /// Метод, що виконується при запуску потока обробки даних запиту
    /// </summary>
    public void DoWork()
    {
      Validator validator = null;
      // Об'єкт, метод якого створює запит необхідного типу
      ICreator? creator;
      try
      {
        validator = new Validator(socket);
        // Аналізуємо строку запиту
        if (string.IsNullOrEmpty(validator.StrReceiveRequest))
        {
          Console.WriteLine("================ Строка запиту пуста. Подальша обробка не можлива.");
          socket.Close();
          socket.Dispose();
          return;
        }

        Console.WriteLine($"================ Змiст запиту:\n{validator.StrReceiveRequest}");

        // Визначаємо тип запиту
        string typeRequest = validator.GetTypeRequest();

        switch (typeRequest)
        {
          // Запит сторінки
          case "page":
            {
              creator = new CreatorRequestPage();
              break;
            }
          // Запит статистики
          case "stat":
            {
              creator = new CreatorRequestStat();
              break;
            }
          default:
            throw new WebException("Невизначений тип запиту", WebExceptionStatus.ProtocolError);

        }
      }
      catch (WebException webExc)
      {
        // Запит favicon ігноруємо
        if (webExc.Status == WebExceptionStatus.ReceiveFailure)
          return;
        creator = new CreatorRequestInvalid();
      }

      if (creator != null)
      {
        (HttpRequest httpRequest, IState startState) = creator.FactoryMethod(validator, repository);
        httpRequest.TransitionTo(startState, socket);
      }

      socket.Close();
      socket.Dispose();
    }
  }
}
