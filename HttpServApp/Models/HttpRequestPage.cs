using System.Web;

namespace HttpServApp.Models
{
  /// <summary>
  /// Цей клас мiстить iнформацiю про запит Web-сторiнки та вiдповiдь 
  /// </summary>
  internal class HttpRequestPage : HttpRequest
  {
    public string Path { get; set; } = string.Empty;
    public int ContentLength { get; }
    public List<TagTemplate> Tags { get; set; } = new List<TagTemplate>();

    public HttpRequestPage(Repository repository,
        DateTime dateTimeRequest,
        string version, string method,
        string ipAddress, string contentType,
        string path, string? message = null,long idRequest = -1)
        : base(repository, dateTimeRequest, version, method, ipAddress, contentType, message, idRequest)
    {
      Path = HttpUtility.UrlDecode(path);
      TypeRequest = TypeRequestEnum.СТОРІНКА;
    }
  }
}
