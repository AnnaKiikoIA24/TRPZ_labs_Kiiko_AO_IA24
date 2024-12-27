using HttpServApp.Mediator;
using HttpServApp.Models;
using HttpServApp.Processing;
using System.Net;
using System.Net.Sockets;

namespace HttpServApp
{
  internal class Listener
  {
    // Ознака запуску потока прослуховування вхiдних пiдключень
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
      // Зупиняємо потiк прослуховування
      isRunning = false;
    }

    protected void DoListen()
    {
      // Сокет для очiкування надходження вхiдних з'єднань
      Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      try
      {

        // Створення локальної точки для прослуховування вхiдних пiдключень
        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, Configuration.Port);
        // Прив'язка сокету до локальної точки 
        listenSocket.Bind(ipPoint);
        // Запуск прослуховування вхiдних пiдключень
        // Configuration.BackLog - кiлькiсть вхiдних пiдключень у черзi на обробку
        listenSocket.Listen(Configuration.BackLog);
        Console.WriteLine("Сервер запущений. Очiкування пiдключень...");

        while (isRunning)
        {
          // Очiкуємо спробу з'єднання,
          // пiсля з'єднання створюється новий сокет для його обробки (вхiдне пiдключення)
          Socket responseSocket = listenSocket.Accept();
          if (responseSocket.Connected)
          {
            Console.WriteLine($"\n==== Адреса пiдключеного клiєнта: {responseSocket.RemoteEndPoint}");
            // Вiдправка сповiщення медiатору про надходження нового запиту вiд клiєнта 
            Mediator?.Notify(this, responseSocket);
          }
          else
          {
            responseSocket.Close();
            responseSocket.Dispose();
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
