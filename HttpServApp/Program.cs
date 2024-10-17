using HttpServApp.Models;

IRepository dbRepository = new Repository();
// Завантажити з бази даних
dbRepository.LoadFromDb();
Console.WriteLine($"get {dbRepository.Requests.Count} rows");

// Записати новий об'єкт в базу даних
//HttpRequest newHttpRequest = new HttpRequest(DateTime.UtcNow, "1.1", "get", "127.0.0.1", "html")
//{
//    Status = StatusEnum.OK,
//};
//dbRepository.Requests.Add(newHttpRequest);
//dbRepository.SaveToDB(newHttpRequest, '+');
//Console.WriteLine("new row was added successfully");

// Оновити об'єкт у базі даних
//if (dbRepository.Requests.Count > 0)
//{
//    HttpRequest updHttpRequest = dbRepository.Requests[0];
//    updHttpRequest.Status = StatusEnum.NOT_FOUND;
//    dbRepository.SaveToDB(updHttpRequest, '=');
//    Console.WriteLine($"row with Id_Request = {updHttpRequest.IdRequest} was updated successfully");
//}

// Видалити об'єкт з бази даних
//if (dbRepository.Requests.Count > 0)
//{
//    HttpRequest delHttpRequest = dbRepository.Requests[0];
//    dbRepository.SaveToDB(delHttpRequest, '-');
//    Console.WriteLine($"row with Id_Request = {delHttpRequest.IdRequest} was deleted successfully");
//}