using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServApp.Models
{
    /// <summary>
    /// Цей клас формує відповідь для валідного запиту сторінки
    /// </summary>
    internal class HttpRequestPage: HttpRequest
    {
        public string Path { get; } = string.Empty;
        public int ContentLength { get; }
        public List<TagTemplate> Tags { get; } = new List<TagTemplate>();
        public HttpRequestPage() { }

        public HttpRequestPage(DateTime dateTimeRequest, string version, string method, string ipAddress, string contentType, string path, int contentLength, long idRequest = -1)
            : base(dateTimeRequest, version, method, ipAddress, contentType, idRequest)
        {
            Path = path;
            ContentLength = contentLength;
        }

        public override void CreateResponse() { }
    }
}
