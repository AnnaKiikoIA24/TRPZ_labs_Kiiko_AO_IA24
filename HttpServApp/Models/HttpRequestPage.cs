using HttpServApp.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HttpServApp.Models
{
    /// <summary>
    /// Цей клас містить інформацію про запит сторінки та відповідь 
    /// </summary>
    internal class HttpRequestPage: HttpRequest
    {
        public string Path { get; } = string.Empty;
        public int ContentLength { get; }
        public List<TagTemplate> Tags { get; } = new List<TagTemplate>();
        public HttpRequestPage() { }

        public HttpRequestPage(Repository repository, 
            DateTime dateTimeRequest, 
            string version, string method, 
            string ipAddress, string contentType, 
            string path, int contentLength, long idRequest = -1)
            : base(repository, dateTimeRequest, version, method, ipAddress, contentType, idRequest)
        {
            Path = path;
            ContentLength = contentLength;
        }

        public override bool CreateResponse() 
        {
            try
            {
                // Якщо сторінка, що запитується не знайдена в репозиторії
                if (!File.Exists(Configuration.ResourcePath + Path))
                    Status = StatusEnum.NOT_FOUND;
                else Status = StatusEnum.OK;

                Console.WriteLine($"Сторінка {Path} " +
                    $"{(Status == StatusEnum.OK ? "сформована" : "відсутня у сховищі")}");

                // Тут - реалізація вихідного html....
                string htmlResponse = "";
                Response = new HttpResponse(DateTime.Now, htmlResponse);

                return true;
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Відповідь на запит сторінки {Path} не сформована: {exc.Message}");
                return false;
            }
        }
    }
}
