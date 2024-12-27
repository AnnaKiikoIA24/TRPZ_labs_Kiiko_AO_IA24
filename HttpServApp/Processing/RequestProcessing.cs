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
    /// (вiн є реентерабельним, тобто потокобезпечним, не залежить вiд стану об'єкта)
    /// </summary>
    protected virtual void DoWork(ProcessingArgs threadArgs)
    {
      Socket socket = threadArgs.Socket;
      Validator validator = new Validator(socket);
      // Об'єкт, метод якого створює запит необхiдного типу
      ICreatorRequest? creator;
      try
      {
        // Аналiзуємо строку запиту
        if (string.IsNullOrEmpty(validator.StrReceiveRequest))
        {
          socket.Close();
          socket.Dispose();
          return;
        }

        // Визначаємо тип запиту
        string typeRequest = validator.GetTypeRequest();

        switch (typeRequest)
        {
          // Запит сторiнки
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
      catch (WebException)
      {
        creator = new CreatorRequestInvalid();
      }

      if (creator != null)
      {
        // За допомогою фабрики створюємо tuple, що мiстить об'єкт запиту та його початковий стан.
        (HttpRequest httpRequest, IState startState) = creator.FactoryMethod(validator, threadArgs.Repository);
        // Запускаємо ланцюжок переходу станiв об'єкту запит
        httpRequest.TransitionTo(startState, socket);
      }

      socket.Close();
      socket.Dispose();
    }
  }
}
