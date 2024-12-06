namespace HttpServApp.Models
{
  /// <summary>
  /// Цей клас містить інформацію про запит Web-сторінки та відповідь 
  /// </summary>
  internal class HttpRequestPage : HttpRequest
  {
    public string Path { get; } = string.Empty;
    public int ContentLength { get; }
    public List<TagTemplate> Tags { get; set; } = new List<TagTemplate>();

    public HttpRequestPage(Repository repository,
        DateTime dateTimeRequest,
        string version, string method,
        string ipAddress, string contentType,
        string path, long idRequest = -1)
        : base(repository, dateTimeRequest, version, method, ipAddress, contentType, null, idRequest)
    {
      Path = path;
      TypeRequest = TypeRequestEnum.СТОРІНКА;
    }
  }
}
