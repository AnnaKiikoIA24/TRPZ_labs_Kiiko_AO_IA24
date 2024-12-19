
using HttpServApp.Models;
using HttpServApp.Processing;
using System.Net.Sockets;

namespace HttpServApp.Mediator
{
  /// <summary>
  /// Посередник обробки запиту від Http-клієнта
  /// </summary>
  internal class MediatorProcessing: IMediator
  {
    private readonly Listener listener;
    private readonly Repository repository;
    private readonly MultiThreadProcessing multiThreadProcessing;
    private readonly SingleThreadProcessing singleThreadProcessing;

    /// <summary>
    /// Конструктор об'єкта, у параметрах задані всі об'єкти-колеги, що взаємодіють з медіатором
    /// </summary>
    /// <param name="listener"></param>
    /// <param name="repository"></param>
    /// <param name="multiThreadProcessing"></param>
    /// <param name="singleThreadProcessing"></param>
    public MediatorProcessing(Listener listener, Repository repository, 
      MultiThreadProcessing multiThreadProcessing,
      SingleThreadProcessing singleThreadProcessing)
    {
      this.listener = listener;
      this.listener.Mediator = this;

      this.repository = repository;
      this.repository.Mediator = this;

      this.multiThreadProcessing = multiThreadProcessing;
      this.multiThreadProcessing.Mediator = this;
      
      this.singleThreadProcessing = singleThreadProcessing; 
      this.singleThreadProcessing.Mediator = this;
    }

    /// <summary>
    /// Метод, що використовується компонентами для сповіщення посередника про різні події.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="target"></param>
    public void Notify(object sender, object target)
    {
      // Якщо повідомлення надійшло від Listener
      if (sender is Listener)
      {
        // як цільовий об'єкт переданий Socket
        if (target is Socket clientSocket)
        {
          // Зчитуємо конфігурацію застосунку.
          // Якщо параметр багатопотоковості встановлений, то для обробки запиту використовуємо об'єкт типу MultiThreadProcessing,
          // що запускає окремий потік обробки запиту
          if (Configuration.MultiThread)
            multiThreadProcessing.Process(repository, clientSocket);
          // Якщо параметр багатопотоковості НЕ встановлений, то для обробки запиту використовуємо об'єкт типу SingleThreadProcessing,
          // що виконує обробку запиту в основному потоці
          else
            singleThreadProcessing.Process(repository, clientSocket);
        }
        else
        {
          Console.WriteLine(target.ToString());
        }

      }
      // Якщо сповіщення прийшло від Repository, просто виводимо цільове повідомлення
      else if (sender is Repository) 
      {
          Console.WriteLine(target.ToString());
      }
    }
  }
}
