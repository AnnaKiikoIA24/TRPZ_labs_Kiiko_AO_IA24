using HttpServApp.Models;
using HttpServApp.State;
using System.Net.Sockets;

namespace HttpServApp.Processing
{
    internal class ThreadProcessing
    {
        // Об'єкт для забезпечення обміну даними через мережу
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
            // Визначаємо тип запиту
            string typeRequest = strReceiveRequest.Substring(10, 1);
            HttpRequest httpRequest;
            try
            {
                switch (typeRequest)
                {
                    // Запит сторінки
                    case "0":
                        {
                            // Запит сторінки валідний, інакше - Exception
                            httpRequest = validator.ParsePageRequest();
                            // Початковий стан запиту: валідний запит Web-сторінки 
                            // Далі викликаємо метод TransitionTo, що в процесі виконання змінює стан запиту
                            // Початковий стан ValidatePageState
                            httpRequest.TransitionTo(new ValidatePageState(), socket);
                            break;
                        }
                    // Запит статистики
                    case "1":
                        {
                            // Запит статистики валідний, інакше - Exception
                            httpRequest = validator.ParseStatisticRequest();
                            // Початковий стан запиту: валідний запит статистики 
                            // Далі викликаємо метод TransitionTo, що в процесі виконання змінює стан запиту,
                            // Початковий стан ValidateStatisticState
                            httpRequest.TransitionTo(new ValidateStatisticState(), socket);
                            break;
                        }
                    default:
                        Console.WriteLine("Processing: Невизначений тип запиту!");
                        throw new Exception();

                }
            }
            catch
            {
                // Початковий стан запиту: InvalidState
                httpRequest = new HttpRequest();
                httpRequest.TransitionTo(new InvalidState(), socket);
            }

            socket.Close();
            socket.Dispose();
        }
    }
}
