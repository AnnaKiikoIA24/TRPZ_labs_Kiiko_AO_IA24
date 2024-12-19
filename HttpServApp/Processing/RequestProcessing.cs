using HttpServApp.Factory;
using HttpServApp.Mediator;
using HttpServApp.Models;
using HttpServApp.State;
using System.Net;
using System.Net.Sockets;

namespace HttpServApp.Processing
{
  internal class ProcessingArgs
  {
    public required Repository Repository { get; set; }
    public required Socket Socket { get; set; }
  }
  internal class RequestProcessing
  {
    public IMediator? Mediator { get; set; }

    /// <summary>
    /// Метод, що виконує обробку даних запиту 
    /// (він є реентерабельним, тобто потокобезпечним, не залежить від стану об'єкта)
    /// </summary>
    protected virtual void DoWork(ProcessingArgs threadArgs)
    {
      Socket socket = threadArgs.Socket;
      Validator validator = new Validator(socket);
      // Об'єкт, метод якого створює запит необхідного типу
      ICreatorRequest? creator;
      try
      {
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
        (HttpRequest httpRequest, IState startState) = creator.FactoryMethod(validator, threadArgs.Repository);
        httpRequest.TransitionTo(startState, socket);
      }

      socket.Close();
      socket.Dispose();
    }
  }
}
