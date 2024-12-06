using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HttpServApp.Models;
using HttpServApp.State;

namespace HttpServApp
{
    /// <summary>
    /// Клас для валiдацiї та парсингу запиту в об'єкт HttpRequest (в один iз його нащадкiв)
    /// </summary>
    internal class Validator
    {
        private readonly Repository repository;
        private readonly Socket socket;
        private string strReceiveRequest = string.Empty;

        public Validator(Repository repository, Socket socket)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.socket = socket ?? throw new ArgumentNullException(nameof(socket));
        }

        // Отримання строки запиту з потоку байтiв у сокетi
        public string GetStringRequest()
        {
            // Встановлюємо розмiр блоку даних
            byte[] bufferBytes = new byte[1024];
            // Зчитуємо данi
            try
            {
                socket.ReceiveTimeout = 1000;
                int bytes = socket.Receive(bufferBytes, bufferBytes.Length, SocketFlags.None);
                strReceiveRequest = Encoding.UTF8.GetString(bufferBytes, 0, bytes);
                // Цикл, поки не досягли закiнчення масиву
                while (socket.Available > 0)
                {
                    bytes = socket.Receive(bufferBytes, bufferBytes.Length, SocketFlags.None);
                    strReceiveRequest += Encoding.UTF8.GetString(bufferBytes, 0, bytes);
                }
                return strReceiveRequest;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetStringRequest exception: {ex.Message}");
                return "";
            }
        }

        private string ParseValue(string pattern, string exceptionStr)
        {
            Match match = Regex.Match(strReceiveRequest, pattern,
                RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
            if (match != Match.Empty && match.Groups.Count > 1)
                return match.Groups[1].Value;

            throw new WebException(exceptionStr, WebExceptionStatus.ProtocolError);
        }

        /// <summary>
        /// Повертає назву файлу Web-сторiнки Http-запиту
        /// </summary>
        /// <returns></returns>
        private string GetFileRequest() =>
            ParseValue(@"\s/([^?]*)[?]?.*\sHTTP", "Iм'я сторiнки не задано в параметрах Http-запиту!").ToLower();

        /// <summary>
        /// Повертає тип запиту
        /// </summary>
        /// <returns></returns>
        public string GetTypeRequest()
        {
            if (strReceiveRequest?.IndexOf("favicon") != -1)
                throw new WebException("Запит iконки favicon, ignore", WebExceptionStatus.ReceiveFailure);

            // Якщо знайдено ім'я файлу, вважаємо, що це запит сторінки (за замовчуванням)
            if (GetFileRequest() != string.Empty)
                return "page";
            return ParseValue(@"type_request=([^\s&]+)", "Не визначений тип запиту").ToLower();
        }
        /// <summary>
        /// Повертає метод запиту (OPTIONS, GET, POST, PUT, DELETE)
        /// </summary>
        /// <returns></returns>
        private string GetMethodRequest() =>
            ParseValue(@"^(\S+)", "Не визначений метод запиту").ToUpper();

        /// <summary>
        /// Повертає версiю протоколу Http
        /// </summary>
        /// <returns></returns>
        private string GetVersionRequest() =>
            ParseValue(@"HTTP/(.+)\r", "Не визначена версiя протоколу Http");


        /// <summary>
        /// Повертає тип змiсту (html, xml, json тощо) Http-запиту
        /// </summary>
        /// <returns></returns>
        public string GetContentTypeRequest() =>
            ParseValue(@"Accept:\s([^,]+)", "Не визначений тип змiсту Http-запиту").ToLower();

        /// <summary>
        /// Повертає дату/час початку перiода
        /// </summary>
        /// <returns></returns>
        private DateTime GetDateBegRequest()
        {
            try
            {
                return Convert.ToDateTime(
                    ParseValue(@"date_beg=(\d{1,2}[.|/]\d{1,2}[.|/]\d{4}(%20\d{1,2}[:]\d{1,2}[:]\d{1,2})?)", 
                    "Запит статистики: не визначена дата початку перiоду").Replace("%20", " "));
            }
            catch (WebException)
            {
                // Якщо параметр date_beg не заданий, повертаємо значення: поточний час "мiнус" 1 година
                return DateTime.Now.AddHours(-1);
            }
            catch (FormatException)
            {
                Console.WriteLine($"ParseValue: параметр date_beg має неправильний формат." +
                    $"Очiкується: dd.MM.yyyy HH:mm:ss");
                // Якщо параметр date_beg має неправильний формат, повертаємо значення: поточний час "мiнус" 1 година
                return DateTime.Now.AddHours(-1);
            }
        }

        /// <summary>
        ///  Повертає дату/час закiнчення перiода
        /// </summary>
        /// <returns></returns>
        private DateTime GetDateEndRequest()
        {
            try
            {
                return Convert.ToDateTime(
                    ParseValue(@"date_end=(\d{1,2}[.|/]\d{1,2}[.|/]\d{4}(%20\d{1,2}[:]\d{1,2}[:]\d{1,2})?)",
                    "Запит статистики: не визначена дата закiнчення перiоду").Replace("%20", " "));
            }
            catch (WebException)
            {
                // Якщо параметр date_end не заданий, повертаємо значення поточний час
                return DateTime.Now;
            }
            catch (InvalidCastException)
            {
                Console.WriteLine($"ParseValue: параметр date_end має неправильний формат." +
                    $"Очiкується: dd.MM.yyyy HH:mm:ss");
                // Якщо параметр date_end має неправильний формат, повертаємо значення: поточний час 
                return DateTime.Now;
            }
        }
        /// <summary>
        /// Запит Web-сторiнки
        /// </summary>
        /// <returns></returns>
        public HttpRequestPage ParsePageRequest()
        {
            try
            {

                // Повертаємо об'єкт запиту
                return new HttpRequestPage(
                    repository, DateTime.Now, 
                    GetVersionRequest(), GetMethodRequest(), 
                    socket.RemoteEndPoint?.ToString() ?? "", 
                    GetContentTypeRequest(), 
                    Path.Combine(Configuration.ResourcePath ?? "C:\\", GetFileRequest()));
            }
            catch (WebException webE)
            {
                Console.WriteLine($"ParsePageRequest WebException: {webE.Message} статус={webE.Status}");
                throw;
            }
            catch (Exception exc)
            {
                Console.WriteLine($"ParsePageRequest Exception: {exc.Message}");
                throw;
            }
        }

        /// <summary>
        /// Запит статистики за перiод
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public HttpRequestStat ParseStatisticRequest()
        {
            try
            {
                // Тут код для розбору
                return new HttpRequestStat(
                    repository, DateTime.Now,
                    GetVersionRequest(), GetMethodRequest(),
                    socket.RemoteEndPoint?.ToString() ?? "",
                    GetContentTypeRequest(), GetDateBegRequest(), GetDateEndRequest(), "");

            }
            catch (WebException webE)
            {
                Console.WriteLine($"ParseStatisticRequest WebException: {webE.Message} статус={webE.Status}");
                throw;
            }
            catch (Exception exc)
            {
                Console.WriteLine($"ParseStatisticRequest Exception: {exc.Message}");
                throw;
            }
        }
    }
}
