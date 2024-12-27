using HttpServApp.Models;
using HttpServApp.State;
using HttpServApp.Processing;
using System.Net;

namespace HttpServApp.Factory
{
  internal class CreatorRequestPage : ICreatorRequest
  {
    /// <summary>
    /// Повертає об'єкт запиту Web-сторiнки
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
            validator.GetFileRequest());

        Console.WriteLine($"Processing: запит сторiнки {httpRequest.Path}!");

        return (httpRequest, new ValidatePageState());
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
