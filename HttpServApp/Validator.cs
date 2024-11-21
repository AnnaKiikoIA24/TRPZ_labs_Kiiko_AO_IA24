using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using HttpServApp.Models;
using HttpServApp.State;

namespace HttpServApp
{
    /// <summary>
    /// Клас для валідації та парсингу запиту в об'єкт HttpRequest (в один із його нащадків)
    /// </summary>
    internal class Validator
    {
        private Repository repository;
        private Socket socket;
        private string strReceiveRequest = string.Empty;

        public Validator(Repository repository, Socket socket)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.socket = socket ?? throw new ArgumentNullException(nameof(socket));
        }

        public string GetStringRequest()
        {
            // Встановлюємо розмір блоку даних
            byte[] bufferBytes = new byte[1024];
            // Зчитуємо дані
            int bytes = socket.Receive(bufferBytes, bufferBytes.Length, 0);
            string strReceiveRequest = Encoding.UTF8.GetString(bufferBytes, 0, bytes);
            // Цикл, поки не досягли закінчення масиву
            while (bytes > 0)
            {
                bytes = socket.Receive(bufferBytes, bufferBytes.Length, 0);
                strReceiveRequest += Encoding.UTF8.GetString(bufferBytes, 0, bytes);
            }
            return strReceiveRequest;
        }

        public HttpRequestPage ParsePageRequest()
        {
            try
            {
                // Тут код для розбору
                return new HttpRequestPage();
            }
            catch (Exception exc)
            {
                Console.WriteLine($"ParsePageRequest: {exc.Message}");
                throw new Exception(exc.Message);
            }
        }

        public HttpRequestStat ParseStatisticRequest()
        {
            try
            {
                // Тут код для розбору
                return new HttpRequestStat() { };
            }
            catch (Exception exc)
            {
                Console.WriteLine($"ParseStatisticRequest: {exc.Message}");
                throw new Exception(exc.Message);
            }
        }
    }
}
