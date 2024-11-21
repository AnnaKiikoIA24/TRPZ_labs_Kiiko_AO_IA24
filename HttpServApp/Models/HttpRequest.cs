using HttpServApp.State;
using System.Text;
using System.Net.Sockets;

namespace HttpServApp.Models
{
    public enum StatusEnum {
        PROCESSING = 0,
        OK = 200,
        BAD_REQUEST = 400,
        NOT_FOUND = 404,
        BAD_SERVER = 500,
        STATISTIC = 600
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

        public long IdRequest { get; set; }
        public DateTime DateTimeRequest { get; } = DateTime.UtcNow;
        public string? Version { get; }
        public string? Method { get; }
        public string IpAddress { get; } = string.Empty;

        // Http-статус відповіді, початково статус запиту ініціалізується значенням PROCESSING,
        // відповідь не сформована і не відправлена
        public StatusEnum Status { get; set; } = StatusEnum.PROCESSING; 
        public string? StatusStr { get => Status.ToString(); }

        public string? ContentTypeRequest { get; }
        public HttpResponse? Response { get; set; }

        public HttpRequest() { }

        public HttpRequest(Repository repository, 
            DateTime dateTimeRequest, 
            string? version, string? method, string ipAddress, 
            string? contentType, long idRequest = -1)
        {
            // Заповнили всі поля
            DateTimeRequest = dateTimeRequest;
            Version = version;
            Method = method;
            IpAddress = ipAddress;
            ContentTypeRequest = contentType;
            IdRequest = idRequest;

            Repository = repository;
            // Додали запит до репозиторію
            Repository.AddRequest(this);
        }

        // Віртуальний метод, реалізація логики у класах-нащадках
        public virtual bool CreateResponse() 
        {
            Response = new HttpResponse();
            return true;
        }

        // Відправка відповіді клієнту
        public void SendResponse(Socket socket)
        {
            if (Response == null) return;
            try
            {
                if (socket != null)
                {
                    // Відправляємо відповідь як масив байт в сокеті
                    socket.Send(Encoding.UTF8.GetBytes(Response.ContentTypeResponse));
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