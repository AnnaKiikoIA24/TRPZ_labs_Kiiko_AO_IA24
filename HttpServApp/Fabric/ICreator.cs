using HttpServApp.Models;
using HttpServApp.Processing;
using HttpServApp.State;

namespace HttpServApp.Fabric
{
  /// <summary>
  /// Інтерфейс ICreator визначає методи 
  /// створення об'єкту запиту та його початкового стану
  /// </summary>
  internal interface ICreator
  {
    /// <summary>
    /// Створення об'єкту-кортежу (tuple) запиту та його початкового стану
    /// </summary>
    /// <returns></returns>
    public (HttpRequest, IState) FactoryMethod(Validator validator, Repository repository);

  }
}
