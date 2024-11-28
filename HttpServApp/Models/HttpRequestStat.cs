using HttpServApp.State;
using System.Net.Sockets;


namespace HttpServApp.Models
{
    /// <summary>
    /// Цей клас містить інформацію про запит статистичних даних та відповідь 
    /// </summary>
    internal class HttpRequestStat: HttpRequest
    {
        public DateTime DateBeg {  get; }
        public DateTime DateEnd { get; }
        public string? KeyAuthorization { get; }
        public HttpRequestStat() { }

        public HttpRequestStat(Repository repository, 
            DateTime dateTimeRequest, 
            string version, string method, 
            string ipAddress, string contentType, 
            DateTime dateBeg, DateTime dateEnd, string? keyAuthorization, long idRequest = -1)
            : base(repository, dateTimeRequest, version, method, ipAddress, contentType, idRequest)
        {
            DateBeg = dateBeg;
            DateEnd = dateEnd;
            KeyAuthorization = keyAuthorization;
        }

        public override bool CreateResponse() 
        {
            try
            {
                List<HttpRequest>? requests = Repository?.GetRequestsByPeriod(DateBeg, DateEnd);
                Status = requests?.Count > 0 ? StatusEnum.OK : StatusEnum.NOT_FOUND;

                Console.WriteLine($"Дані статистики за період " +
                    $"з {DateBeg:dd.MM.yyyy HH:mm:ss} по {DateEnd:dd.MM.yyyy HH:mm:ss} " +
                    $"{(requests?.Count > 0 ? "вибрані" : "відсутні")}");

                // Тут - реалізація вихідного html....
                string htmlResponse = "";
                Response = new HttpResponse(DateTime.Now, htmlResponse);

                return true;
            }
            catch (Exception exc) 
            {
                Console.WriteLine($"Відповідь на запит статистики не сформована: {exc.Message}");
                return false;
            }
        }
    }
}
