using HttpServApp.Models;
using System.Text;

namespace HttpServApp.Builder
{
  /// <summary>
  /// Builder-клас побудови відповіді на не валідний запит
  /// </summary>
  internal class BuilderInvalid : IBuilder
  {
    HttpRequestInvalid httpRequestInvalid;
    public BuilderInvalid(HttpRequest httpRequest)
    {
      httpRequestInvalid = httpRequest as HttpRequestInvalid ?? throw new ArgumentNullException(nameof(httpRequestInvalid));
    }
    public string BuildVersion() => "HTTP / " + (httpRequestInvalid.Version ?? "1.1");

    public virtual string BuildStatus() => $"{(int)httpRequestInvalid.Status} {httpRequestInvalid.Status}";

    public string BuildHeaders() =>
        $"\nContent-Type:{httpRequestInvalid.ContentTypeRequest ?? "text/plain"};charset=UTF-8;" +
        $"\nConnection: close\n";

    public string BuildContentBody()
    {
      httpRequestInvalid.Response = new HttpResponse(
          DateTime.Now, Encoding.UTF8.GetByteCount(httpRequestInvalid.Message ?? string.Empty));

      return $"Content-Length:{httpRequestInvalid.Response?.ContentLength ?? 0}\n\n" +
          $"{httpRequestInvalid.Message ?? string.Empty}";
    }

  }
}
