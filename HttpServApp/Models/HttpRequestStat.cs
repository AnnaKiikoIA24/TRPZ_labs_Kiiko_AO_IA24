namespace HttpServApp.Models
{
  /// <summary>
  /// Цей клас мiстить iнформацiю про запит статистичних даних та вiдповiдь 
  /// </summary>
  internal class HttpRequestStat : HttpRequest
  {
    public DateTime DateBeg { get; }
    public DateTime DateEnd { get; }

    public int CntRows { get; set; } = 0;
    public string? KeyAuthorization { get; }

    public HttpRequestStat(Repository repository,
        DateTime dateTimeRequest,
        string version, string method,
        string ipAddress, string contentType,
        DateTime dateBeg, DateTime dateEnd, string? keyAuthorization, long idRequest = -1)
        : base(repository, dateTimeRequest, version, method, ipAddress, contentType, null, idRequest)
    {
      DateBeg = dateBeg;
      DateEnd = dateEnd;
      KeyAuthorization = keyAuthorization;
      TypeRequest = TypeRequestEnum.СТАТИСТИКА;
    }
  }
}
