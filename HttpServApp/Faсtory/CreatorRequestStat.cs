using HttpServApp.Models;
using HttpServApp.Processing;
using HttpServApp.State;
using System.Net;

namespace HttpServApp.Factory
{
  internal class CreatorRequestStat : ICreatorRequest
  {

    /// <summary>
    /// Повертає об'єкт запиту статистики за період
    /// </summary>
    /// <param name="validator"></param>
    /// <param name="repository"></param>
    /// <returns></returns>
    public (HttpRequest, IState) FactoryMethod(Validator validator, Repository repository)
    {
      try 
      { 
        HttpRequestStat httpRequest = new HttpRequestStat(
            repository, DateTime.Now,
            validator.GetVersionRequest(), validator.GetMethodRequest(),
            validator.GetRemoteEndPoint(), validator.GetContentTypeRequest(),
            validator.GetDateBegRequest(), validator.GetDateEndRequest(),
            validator.GetKeyAuthorization());
        Console.WriteLine($"Processing: запит статистики за перiод " +
             $"{httpRequest.DateBeg}-{httpRequest.DateEnd}!");

        return (httpRequest, new ValidateStatisticState());

      }
      catch (WebException webE)
      {
        HttpRequestInvalid httpRequest = new HttpRequestInvalid(
          repository, DateTime.Now,
          validator.GetRemoteEndPoint(), $"{webE.Message} статус={webE.Status}");
        Console.WriteLine($"CreatorRequestPage WebException: {webE.Message} статус={webE.Status}");
        return (httpRequest, new InvalidState());
      }
      catch (Exception exc)
      {
        HttpRequestInvalid httpRequest = new HttpRequestInvalid(
          repository, DateTime.Now,
          validator.GetRemoteEndPoint(), exc.Message);
        Console.WriteLine($"CreatorRequestPage Exception: {exc.Message}");
        return (httpRequest, new InvalidState());
      }
    }

  }
}
