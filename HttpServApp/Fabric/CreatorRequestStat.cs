using HttpServApp.Models;
using HttpServApp.Processing;
using HttpServApp.State;
using System.Net;

namespace HttpServApp.Fabric
{
  internal class CreatorRequestStat : ICreator
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
        Console.WriteLine($"CreateRequest WebException: {webE.Message} статус={webE.Status}");
        throw;
      }
      catch (Exception exc)
      {
        Console.WriteLine($"CreateRequest Exception: {exc.Message}");
        throw;
      }
    }

  }
}
