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
        public string? Path { get; }
        public string? Version { get; }
        public string? ContentType { get; }
        public string? Method { get; }
        public int? ContentLength { get; }
        public DateTime DateRequest { get; } = DateTime.Now;
        public string? Body { get; }
        public StatusEnum Status { get; set; } = 0;
        public string? StatusStr { get => Status.ToString(); }
        public DateTime DateResponse { get; set;  }

        public HttpRequest() { }

        public HttpRequest(string? path, string? version, string? contentType, string? method, int? contentLenght, DateTime dateRequest, string? body, long idRequest =-1)
        {
            Path = path;
            Version = version;
            ContentType = contentType;
            Method = method;
            ContentLength = contentLenght;
            DateRequest = dateRequest;
            Body = body;
            IdRequest = idRequest;
        }

        // Віртуальний метод, реалізація логики у класах-нащадках
        public virtual string CreateResponseStr ()
        {
            return string.Empty;
        }

     }
}