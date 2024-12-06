namespace HttpServApp.Builder
{
  /// <summary>
  /// Інтерфейс IBuilder визначає всі можливі кроки з формування HTTP-відповіді
  /// </summary>
  internal interface IBuilder
  {
    /// <summary>
    /// Версія протоколу
    /// </summary>
    /// <returns></returns>
    public string BuildVersion();

    /// <summary>
    /// Статус виконання запиту
    /// </summary>
    /// <returns></returns>
    public string BuildStatus();

    /// <summary>
    /// Заголовки відповіді
    /// </summary>
    /// <returns></returns>
    public string BuildHeaders();

    /// <summary>
    /// Зміст (тіло) відповіді
    /// </summary>
    /// <returns></returns>
    public string BuildContentBody();

  }
}
