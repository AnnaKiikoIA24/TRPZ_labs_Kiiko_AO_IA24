using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace HttpServApp.Processing
{
  /// <summary>
  /// Клас парсингу даних Http-запиту 
  /// </summary>
  internal class Validator
  {
    private readonly Socket socket;
    public string StrReceiveRequest { get; }  = string.Empty;

    public Validator(Socket socket)
    {
      this.socket = socket ?? throw new ArgumentNullException(nameof(socket));
      StrReceiveRequest = GetStringRequest();
    }

    // Отримання строки запиту з потоку байтiв у сокетi
    private string GetStringRequest()
    {
      // Встановлюємо розмiр блоку даних
      byte[] bufferBytes = new byte[1024];
      // Зчитуємо данi
      try
      {
        //socket.ReceiveTimeout = 1000;

        int bytes = socket.Receive(bufferBytes, bufferBytes.Length, SocketFlags.None);
        string strReceiveRequest = Encoding.UTF8.GetString(bufferBytes, 0, bytes);
        // Цикл, поки не досягли закiнчення масиву
        while (socket.Available > 0)
        {
          bytes = socket.Receive(bufferBytes, bufferBytes.Length, SocketFlags.None);
          strReceiveRequest += Encoding.UTF8.GetString(bufferBytes, 0, bytes);
        }

        Console.WriteLine($"==== Змiст запиту ({socket.RemoteEndPoint}):\n{strReceiveRequest}");
        return strReceiveRequest;
      }
      catch (Exception ex)
      {
        //Console.WriteLine($"==== GetStringRequest exception ({socket.RemoteEndPoint}):\n{ex.Message}");
        return string.Empty;
      }
    }

    /// <summary>
    /// Повертає адресу Http-клiєнта
    /// </summary>
    /// <returns></returns>
    public string GetRemoteEndPoint() =>
      socket.RemoteEndPoint?.ToString() ?? string.Empty;

    // Метод, що повертає данi (частина строки), що вiдповiдає шаблону пошуку
    // Якщо строка не вiдповiдає шаблону, то exception
    private string ParseValue(string pattern, string exceptionStr)
    {
      Match match = Regex.Match(StrReceiveRequest, pattern,
          RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
      if (match != Match.Empty && match.Groups.Count > 1)
        return match.Groups[1].Value;

      throw new WebException(exceptionStr, WebExceptionStatus.ProtocolError);
    }

    /// <summary>
    /// Повертає назву файлу Web-сторiнки Http-запиту
    /// </summary>
    /// <returns></returns>
    public string GetFileRequest() {
      // Якщо виклик default-сторiнки, то повертаємо стартову сторiнку index.html
      string pattern = @"\s/\sHTTP";
      Match match = Regex.Match(StrReceiveRequest, pattern, RegexOptions.Compiled);
      if (match != Match.Empty)
        return "index.html";

      return ParseValue(@"\s/([^?]*)[?]?.*\sHTTP", "Iм'я сторiнки не задано в параметрах Http-запиту!").ToLower();
    }
    /// <summary>
    /// Повертає тип запиту
    /// </summary>
    /// <returns></returns>
    public string GetTypeRequest()
    {
      // Якщо знайдено iм'я файлу, вважаємо, що це запит сторiнки (за замовчуванням)
      if (GetFileRequest() != string.Empty)
        return "page";
      return ParseValue(@"type_request=([^\s&]+)", "Не визначений тип запиту").ToLower();
    }
    /// <summary>
    /// Повертає метод запиту (OPTIONS, GET, POST, PUT, DELETE)
    /// </summary>
    /// <returns></returns>
    public string GetMethodRequest() =>
        ParseValue(@"^(\S+)", "Не визначений метод запиту").ToUpper();

    /// <summary>
    /// Повертає версiю протоколу Http
    /// </summary>
    /// <returns></returns>
    public string GetVersionRequest() =>
        ParseValue(@"HTTP/(.+)\r", "Не визначена версiя протоколу Http");


    /// <summary>
    /// Повертає тип змiсту (html, xml, json тощо) Http-запиту
    /// </summary>
    /// <returns></returns>
    public string GetContentTypeRequest() =>
        ParseValue(@"Accept:\s([^,\r\n]+)[,|\r|\n]", "Не визначений тип змiсту Http-запиту").ToLower();

    /// <summary>
    /// Повертає змiст ключа авторизацiї (використовується для запиту статистики)
    /// </summary>
    /// <returns></returns>
    public string GetKeyAuthorization() =>
        //ParseValue(@"key-authorization:\s([^\s\r\n]+)[;|\r|\n]", "Вiдсутнiй заголовок ключа авторизацiї");
      ParseValue(@"key-authorization=([^\s\r\n]+)[;|\r|\n]", "Вiдсутнiй заголовок ключа авторизацiї");

    /// <summary>
    /// Повертає дату/час початку перiода
    /// </summary>
    /// <returns></returns>
    public DateTime GetDateBegRequest()
    {
      string sDate = ParseValue(@"date_beg=(\d{4}-\d{1,2}-\d{1,2}(T\d{1,2}[:]\d{1,2})?)",
          "Запит статистики: параметр date_beg має неправильний формат." +
          "Очiкується: yyyy-MM-ddTHH:mm");
      DateTime newDate = DateTime.ParseExact(sDate, "yyyy-MM-ddTHH:mm", null);
      return newDate;

    }

    /// <summary>
    ///  Повертає дату/час закiнчення перiода
    /// </summary>
    /// <returns></returns>
    public DateTime GetDateEndRequest()
    {
      string sDate = ParseValue(@"date_end=(\d{4}-\d{1,2}-\d{1,2}(T\d{1,2}[:]\d{1,2})?)",
          "Запит статистики: параметр date_end має неправильний формат." +
          "Очiкується: yyyy-MM-ddTHH:mm");
      DateTime newDate = DateTime.ParseExact(sDate, "yyyy-MM-ddTHH:mm", null);
      return newDate;
    }

  }
}
