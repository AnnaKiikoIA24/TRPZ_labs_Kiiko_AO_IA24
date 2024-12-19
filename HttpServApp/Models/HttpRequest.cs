using HttpServApp.State;
using System.Net.Sockets;
using System.Text;

namespace HttpServApp.Models
{
  public enum StatusEnum
  {
    PROCESSING = 0,
    OK = 200,
    BAD_REQUEST = 400,
    UNAUTHORIZED = 401,
    NOT_FOUND = 404,
    BAD_SERVER = 500,
    STATISTIC = 600
  }

  public enum TypeRequestEnum
  {
    НЕ_ВИЗНАЧЕНО = 0,
    СТОРІНКА = 1,
    СТАТИСТИКА = 2
  }
  /// <summary>
  /// Базовий клас, що описує запит до серверу
  /// </summary>    
  internal class HttpRequest
  {

    // Посилання на поточний стан об'єкта.
    private IState? state = null;

    // Посилання на репозиторій, де зберігається об'єкт
    public Repository Repository { get; }

    public long IdRequest { get; set; } = -1;
    public DateTime DateTimeRequest { get; } = DateTime.Now;

    public TypeRequestEnum TypeRequest { get; set; } = TypeRequestEnum.НЕ_ВИЗНАЧЕНО;
    public string? Version { get; }
    public string? Method { get; }
    public string IpAddress { get; } = string.Empty;

    // Http-статус відповіді, початково статус запиту ініціалізується значенням PROCESSING,
    // відповідь не сформована і не відправлена
    public StatusEnum Status { get; set; } = StatusEnum.PROCESSING;
    public string? StatusStr { get => Status.ToString(); }

    public string? ContentTypeRequest { get; }

    public string? Message { get; set; }
    public HttpResponse? Response { get; set; }


    public HttpRequest(Repository repository,
        DateTime dateTimeRequest,
        string? version, string? method, string ipAddress,
        string? contentType, string? message = "", long idRequest = -1)
    {
      // Заповнили всі поля
      DateTimeRequest = dateTimeRequest;
      Version = version;
      Method = method;
      IpAddress = ipAddress;
      ContentTypeRequest = contentType;
      Message = message;
      IdRequest = idRequest;

      Repository = repository;
    }

    protected string CreateHeader()
    {
      return $"HTTP/{Version ?? "1.1"} {Enum.Format(typeof(StatusEnum), Status, "d")} {Status} " +
          //$"\nContent-Type:{ContentTypeRequest ?? "text/html"};charset=UTF-8;" +
          $"\nContent-Type:{ContentTypeRequest ?? "text/html"};" +
          $"\nContent-Length:{Response?.ContentLength ?? 0}" +
          $"\nConnection: close\n\n";
    }

    // Відправка відповіді клієнту
    public void SendResponse(Socket socket, string htmlResponse)
    {
      if (Response == null) return;
      try
      {
        if (socket != null)
        {
          // Відправляємо відповідь як масив байт в сокеті
          if (ContentTypeRequest.IndexOf("image") != -1)
            socket.Send(Encoding.ASCII.GetBytes(htmlResponse));
          else
            socket.Send(Encoding.UTF8.GetBytes(htmlResponse));
          // socket.Send(Convert.FromBase64String(htmlResponse));
          // Встановлюємо ознаку відправленої відповіді 
          Response.StatusSend = 1;
        }
        else
        {
          Console.WriteLine("Сформована відповідь не відправлена: Socket=null");
        }
      }
      catch (Exception exc)
      {
        Console.WriteLine($"Сформована відповідь не відправлена: {exc.Message}");
      }
      finally
      {
        socket?.Shutdown(SocketShutdown.Both);
        socket?.Close();
      }
    }

    // Метод дозволяє змінювати стан об'єкта під час виконання
    public void TransitionTo(IState state, Socket socket)
    {
      Console.WriteLine($"HttpRequest state: Transition to {state.GetType().Name}.");
      this.state = state;
      // Виклик методу обробки нового стану запиту
      this.state.ProcessingHandler(this, socket);
    }
  }
}