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
      Validator validator = new Validator(repository, socket);
      // Отримуємо строку запиту
      string strReceiveRequest = validator.GetStringRequest();

      if (string.IsNullOrEmpty(strReceiveRequest))
      {
        socket.Close();
        socket.Dispose();
        return;
      }

      Console.WriteLine($"================ Змiст запиту:\n{strReceiveRequest}");
      HttpRequest httpRequest;
      try
      {
        // Визначаємо тип запиту
        string typeRequest = validator.GetTypeRequest();
        switch (typeRequest)
        {
          // Запит сторінки
          case "page":
            {
              // Запит сторінки валідний, інакше - Exception
              httpRequest = validator.ParsePageRequest();
              Console.WriteLine($"Processing: запит сторiнки {((HttpRequestPage)httpRequest).Path}!");
              // Початковий стан запиту: валідний запит Web-сторінки 
              // Далі викликаємо метод TransitionTo, що в процесі виконання змінює стан запиту
              // Початковий стан ValidatePageState
              httpRequest.TransitionTo(new ValidatePageState(), socket);
              break;
            }
          // Запит статистики
          case "stat":
            {
              // Запит статистики валідний, інакше - Exception
              httpRequest = validator.ParseStatisticRequest();
              Console.WriteLine($"Processing: запит статистики за перiод " +
                  $"{((HttpRequestStat)httpRequest).DateBeg}-{((HttpRequestStat)httpRequest).DateEnd}!");
              // Початковий стан запиту: валідний запит статистики 
              // Далі викликаємо метод TransitionTo, що в процесі виконання змінює стан запиту,
              // Початковий стан ValidateStatisticState
              httpRequest.TransitionTo(new ValidateStatisticState(), socket);
              break;
            }
          default:
            Console.WriteLine("Processing: Невизначений тип запиту!");
            throw new WebException("Невизначений тип запиту", WebExceptionStatus.ProtocolError);

        }
      }
      catch (WebException webExc)
      {
        // Запит favicon ігноруємо
        if (webExc.Status == WebExceptionStatus.ReceiveFailure)
          return;

        httpRequest = new HttpRequestInvalid(repository, DateTime.Now,
            socket.RemoteEndPoint?.ToString() ?? "", webExc.Message);
        // Початковий стан запиту: InvalidState
        httpRequest.TransitionTo(new InvalidState(), socket);
      }

      socket.Close();
      socket.Dispose();
    }
  }
}
