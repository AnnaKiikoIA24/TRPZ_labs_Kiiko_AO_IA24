using HttpServApp.Models;
using HttpServApp.Processing;
using HttpServApp.State;

namespace HttpServApp.Factory
{
  internal class CreatorRequestInvalid : ICreatorRequest
  {
    /// <summary>
    /// Повертає об'єкт не валідного запиту 
    /// </summary>
    /// <param name="validator"></param>
    /// <param name="repository"></param>
    /// <returns></returns>
    public (HttpRequest, IState) FactoryMethod(Validator validator, Repository repository)
    {
      HttpRequestInvalid httpRequest = new HttpRequestInvalid(
        repository, DateTime.Now,
        validator.GetRemoteEndPoint(), "Невизначений тип запиту.");
      Console.WriteLine("Processing: Невизначений тип запиту!");

      return (httpRequest, new InvalidState());
    }
  }
}
