using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServApp.Models
{
    internal class HttpResponse
    {
        public DateTime DateTimeResponse { get; }
        // Початкове значення = 0: відповідь не відправлена
        public int StatusSend { get; set; } = 0;
        public string ContentTypeResponse { get; } = string.Empty;
        public HttpResponse() { }

        public HttpResponse(DateTime dateTimeResponse, string contentTypeResponse)
        {
            DateTimeResponse = dateTimeResponse;
            ContentTypeResponse = contentTypeResponse;
        }

    }
}
