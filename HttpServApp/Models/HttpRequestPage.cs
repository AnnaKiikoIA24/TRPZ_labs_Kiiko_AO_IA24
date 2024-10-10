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
        public HttpRequestPage()
        {
            // Для помилковго запиту одразу встановлюємо статус BAD_REQUEST
            Status = StatusEnum.BAD_REQUEST;
        }

        public HttpRequestPage(string path, string version, string contentType, string method, int contentLenght, DateTime dateRequest, string? body)
            : base(path, version, contentType, method, contentLenght, dateRequest, body)
        {  }

        public override string CreateResponseStr()
        {
            DateResponse = DateTime.Now;
            // Тут буде логіка формування відповіді
            // Для доступу до сторінок будемо використовувати статичний клас Configuration
            return string.Empty;
        }
    }
}
