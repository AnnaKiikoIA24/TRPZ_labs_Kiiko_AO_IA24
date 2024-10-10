using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServApp.Models
{
    /// <summary>
    /// Цей клас формує відповідь для невалідного запиту сторінки
    /// </summary>
    internal class HttpRequestError: HttpRequest
    {
        public HttpRequestError()
        {
            // Для помилковго запиту одразу встановлюємо статус BAD_REQUEST
            Status = StatusEnum.BAD_REQUEST;
        }

        public HttpRequestError(string? path, string? version, string? contentType, string? method, int? contentLenght, DateTime dateRequest, string? body)
            : base(path, version, contentType, method, contentLenght, dateRequest, body)
        {
            // Для помилковго запиту одразу встановлюємо статус BAD_REQUEST
            Status = StatusEnum.BAD_REQUEST;
        }

        public override string CreateResponseStr()
        {
            DateResponse = DateTime.Now;
            // Тут буде логіка формування відповіді
            return string.Empty;
        }
    }
}
