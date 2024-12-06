namespace HttpServApp.Models
{

  /// <summary>
  /// Клас, що описує помилковий запит до серверу
  /// </summary>    
  internal class HttpRequestInvalid : HttpRequest
  {

    public HttpRequestInvalid(Repository repository,
    DateTime dateTimeRequest, string ipAddress, string message, long idRequest = -1)
        : base(repository, dateTimeRequest, null, null, ipAddress, null, message, idRequest)
    {
      Status = StatusEnum.BAD_REQUEST;
    }

  }
}