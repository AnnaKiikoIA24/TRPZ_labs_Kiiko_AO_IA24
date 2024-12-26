
namespace HttpServApp.Mediator
{
  /// <summary>
  /// iнтерфейс посередника обробки
  /// </summary>
  internal interface IMediator
  {
    // iнтерфейс Посередника надає метод, що використовується компонентами для
    // сповiщення посередника про рiзнi подiї. Посередник може реагувати
    // на цi подiї i передавати виконання iншим компонентам.
    public void Notify(object sender, object target);
  }
}
