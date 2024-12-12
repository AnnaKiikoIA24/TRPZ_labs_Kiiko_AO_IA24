using System.Configuration;

namespace HttpServApp.Models
{
  internal static class Configuration
  {
    // Порт Http-сервера
    public static int Port { get; set; } = Convert.ToInt32(ConfigurationManager.AppSettings["port"]);
    // Кількість вхідних підключень у черзі на обробку
    public static int BackLog { get; set; } = Convert.ToInt32(ConfigurationManager.AppSettings["back_log"]);
    // Шлях для доступу до репозиторію Web-сторінок
    public static string? ResourcePath { get; set; } = Convert.ToString(ConfigurationManager.AppSettings["resource_path"]);
    // Строка підключення до БД
    public static string? DBConnStr { get; set; } = Convert.ToString(ConfigurationManager.AppSettings["db_conn_str"]);

    // Ключ доступа адміністратора до даних статистики
    public static string? KeyAuthorization { get; } = Convert.ToString(ConfigurationManager.AppSettings["key_authorization"]);
  }
}
