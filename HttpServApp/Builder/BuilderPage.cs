using HttpServApp.Models;
using System.Text;

namespace HttpServApp.Builder
{
  /// <summary>
  /// Builder-клас побудови відповіді на запит Web-сторінки
  /// </summary>
  internal class BuilderPage : IBuilder
  {
    HttpRequestPage httpRequestPage;
    public BuilderPage(HttpRequest httpRequest)
    {
      httpRequestPage = httpRequest as HttpRequestPage ?? throw new ArgumentNullException(nameof(httpRequestPage));
    }

    public string BuildVersion() => "HTTP / " + (httpRequestPage.Version ?? "1.1");
    public string BuildStatus()
    {
      // Якщо сторінка, що запитується, не знайдена в репозиторії => STATUS = NOT_FOUND
      // Якщо знайдена => STATUS = OK
      httpRequestPage.Status = !File.Exists(httpRequestPage.Path) ? StatusEnum.NOT_FOUND : StatusEnum.OK;

      return $"{(int)httpRequestPage.Status} {httpRequestPage.Status}";
    }

    public string BuildHeaders() =>
        $"\nContent-Type:{httpRequestPage.ContentTypeRequest ?? "text/plain"};charset=UTF-8;" +
        $"\nConnection: close\n";

    public virtual string BuildContentBody()
    {
      // Якщо сторінка, що запитується, не знайдена в репозиторії => виводимо повідомлення про це у відповідь
      if (!File.Exists(httpRequestPage.Path))
      {
        httpRequestPage.Message = $"Статус: {httpRequestPage.Status}. Файл сторiнки '{httpRequestPage.Path}' не знайдений";
        httpRequestPage.Response = new HttpResponse(
            DateTime.Now, Encoding.UTF8.GetByteCount(httpRequestPage.Message));

        Console.WriteLine(httpRequestPage.Message);
        return $"Content-Length:{httpRequestPage.Response?.ContentLength ?? 0}\n\n{httpRequestPage.Message}";
      }
      // Якщо сторінка, що запитується, знайдена в репозиторії => повертаємо зміст сторінки
      else
      {
        // Зчитування змісту Web-сторінки
        string htmlResponse = string.Empty;
        using (StreamReader reader = new StreamReader(httpRequestPage.Path))
        {
          htmlResponse = reader.ReadToEnd();
        }

        if (httpRequestPage.ContentTypeRequest.IndexOf("image") != -1)
          httpRequestPage.Response = new HttpResponse(
              DateTime.Now, Encoding.ASCII.GetByteCount(htmlResponse));
        else
          httpRequestPage.Response = new HttpResponse(
            DateTime.Now, Encoding.UTF8.GetByteCount(htmlResponse));

        Console.WriteLine($"Сторiнка {httpRequestPage.Path} сформована");
        return $"Content-Length:{httpRequestPage.Response?.ContentLength ?? 0}\n\n{htmlResponse}";
      }
    }
  }
}
