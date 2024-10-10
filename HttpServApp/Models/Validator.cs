using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServApp.Models
{
    /// <summary>
    /// Статичний клас для парсингу строки запиту в об'єкт HttpRequest (в один із його нащадків)
    /// </summary>
    internal static class Validator
    {
        public static HttpRequest GetRequest(string value)
        {
            HttpRequest request = null;
            return request;
        }
    }
}
