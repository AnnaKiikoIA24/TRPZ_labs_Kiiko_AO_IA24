using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServApp.Models
{
    internal class HttpResponse
    {
        public long IdRequest { get; }
        public DateTime DateTimeResponse { get; }
        public int StatusSend { get; }
        public string ContentTypeResponse { get; } = string.Empty;
        public HttpResponse() { }

        public HttpResponse(long idRequest, DateTime dateTimeResponse, int statusSend, string contentTypeResponse)
        {
            IdRequest = idRequest;
            DateTimeResponse = dateTimeResponse;
            StatusSend = statusSend;
            ContentTypeResponse = contentTypeResponse;
        }
    }
}
