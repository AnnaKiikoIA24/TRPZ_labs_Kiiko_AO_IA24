
namespace HttpServApp.Mediator
{
  /// <summary>
  /// Інтерфейс посередника обробки
  /// </summary>
  internal interface IMediator
  {
    // Інтерфейс Посередника надає метод, що використовується компонентами для
    // сповіщення посередника про різні події. Посередник може реагувати
    // на ці події і передавати виконання іншим компонентам.
    public void Notify(object sender, object target);
  }
}
