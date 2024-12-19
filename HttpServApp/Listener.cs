using HttpServApp.Mediator;
using HttpServApp.Models;
using HttpServApp.Processing;
using System.Net;
using System.Net.Sockets;

namespace HttpServApp
{
  internal class Listener
  {
    // Ознака запуску потока прослуховування вхідних підключень
    private bool isRunning = false;
    // Об'єкт потоку
    private readonly Thread listenerThread;

    public IMediator? Mediator { get; set; }

    public Listener()
    {
      listenerThread = new Thread(DoListen);
    }

    public void Start()
    {
      if ((listenerThread.ThreadState != ThreadState.Running) &&
          (listenerThread.ThreadState != ThreadState.Background))
      {
        isRunning = true;
        listenerThread.Start();
      }
    }

    public void Stop()
    {
      // Зупиняємо потік прослуховування
      isRunning = false;
    }

    protected void DoListen()
    {
      // Сокет для очікування надходження вхідних з'єднань
      Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      try
      {

        // Створення локальної точки для прослуховування вхідних підключень
        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, Configuration.Port);
        // Прив'язка сокету до локальної точки 
        listenSocket.Bind(ipPoint);
        // Запуск прослуховування вхідних підключень
        // Configuration.BackLog - кількість вхідних підключень у черзі на обробку
        listenSocket.Listen(Configuration.BackLog);
        Console.WriteLine("Сервер запущений. Очiкування пiдключень...");

        while (isRunning)
        {
          // Очікуємо спробу з'єднання,
          // після з'єднання створюється новий сокет для його обробки (вхідне підключення)
          Socket responseSocket = listenSocket.Accept();
          Console.WriteLine($"\n================ Адреса пiдключеного клiєнта: {responseSocket.RemoteEndPoint}");
          if (responseSocket.Connected)
          {
            // Відправка сповіщення медіатору про надходження нового запиту від клієнта 
            Mediator?.Notify(this, responseSocket);
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Помилка пiдключення: {ex.Message}");
      }
      finally
      {
        listenSocket.Close();
      }
    }

  }
}
