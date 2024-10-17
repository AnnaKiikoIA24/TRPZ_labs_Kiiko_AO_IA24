using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServApp.Models
{
    internal class HttpRequestStat: HttpRequest
    {
        public DateTime DateBeg {  get; }
        public DateTime DateEnd { get; }
        public string? KeyAuthorization { get; }
        public HttpRequestStat() { }

        public HttpRequestStat(DateTime dateTimeRequest, string version, string method, string ipAddress, string contentType, DateTime dateBeg,
            DateTime dateEnd, string? keyAuthorization, long idRequest = -1)
            : base(dateTimeRequest, version, method, ipAddress, contentType, idRequest)
        {
            DateBeg = dateBeg;
            DateEnd = dateEnd;
            KeyAuthorization = keyAuthorization;
        }

        public override void CreateResponse() { }
    }
}
