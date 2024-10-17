using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    /// Базовий клас, що описує запит до серверу (відповідає таблиці бази даних Http_Request)
    /// </summary>    
    internal class HttpRequest
    {
        public long IdRequest { get; set; }
        public DateTime DateTimeRequest { get; } = DateTime.UtcNow;
        public string? Version { get; }
        public string? Method { get; }
        public string IpAddress { get; } = string.Empty;
        public StatusEnum Status { get; set; } = StatusEnum.PROCESSING; // Початково статус запиту ініціалізується значенням PROCESSING
        public string? StatusStr { get => Status.ToString(); }
        public string? ContentTypeRequest { get; }
        public HttpResponse? Response { get; set; }

        public HttpRequest() { }

        public HttpRequest(DateTime dateTimeRequest, string? version, string? method, string ipAddress, string? contentType, long idRequest =-1)
        {
            DateTimeRequest = dateTimeRequest;
            Version = version;
            Method = method;
            IpAddress = ipAddress;
            ContentTypeRequest = contentType;
            IdRequest = idRequest;
        }

        // Віртуальний метод, реалізація логики у класах-нащадках
        public virtual void CreateResponse () { }

     }
}