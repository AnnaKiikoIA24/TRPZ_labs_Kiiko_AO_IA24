using HttpServApp.Models;
using HttpServApp.State;
using HttpServApp.Processing;
using System.Net;

namespace HttpServApp.Fabric
{
  internal class CreatorRequestPage : ICreator
  {
    /// <summary>
    /// Повертає об'єкт запиту Web-сторінки
    /// </summary>
    /// <param name="validator"></param>
    /// <param name="repository"></param>
    /// <returns></returns>
    public (HttpRequest, IState) FactoryMethod(Validator validator, Repository repository)
    {
      try
      {
        HttpRequestPage httpRequest = new HttpRequestPage(
            repository, DateTime.Now,
            validator.GetVersionRequest(), validator.GetMethodRequest(),
            validator.GetRemoteEndPoint(), validator.GetContentTypeRequest(),
            Path.Combine(Configuration.ResourcePath ?? "C:\\", validator.GetFileRequest()));

        Console.WriteLine($"Processing: запит сторiнки {httpRequest.Path}!");

        return (httpRequest, new ValidatePageState());
      }
      catch (WebException webE)
      {
        Console.WriteLine($"CreatorRequestPage WebException: {webE.Message} статус={webE.Status}");
        throw;
      }
      catch (Exception exc)
      {
        Console.WriteLine($"CreatorRequestPage Exception: {exc.Message}");
        throw;
      }
    }
    
  }
}
