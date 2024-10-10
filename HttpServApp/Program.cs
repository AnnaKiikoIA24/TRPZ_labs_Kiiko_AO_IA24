﻿using HttpServApp.Models;

DBContext dBContext = new DBContext();
// Завантажити з бази даних
dBContext.LoadFromDb();
Console.WriteLine($"get {dBContext.Requests.Count} rows");

// Записати новий об'єкт в базу даних
//HttpRequest newHttpRequest = new HttpRequest("123456j", "1.1", "", "", 11, DateTime.Now, "")
//{
//    Status = StatusEnum.OK,
//    DateResponse = DateTime.Now
//};
//dBContext.SaveToDB(newHttpRequest, '+');
//Console.WriteLine("new row was added successfully");

// Оновити об'єкт у базі даних
//dBContext.LoadFromDb();
//if (dBContext.Requests.Count > 0)
//{
//    HttpRequest updHttpRequest = dBContext.Requests[0];
//    updHttpRequest.Status = StatusEnum.NOT_FOUND;
//    updHttpRequest.DateResponse = DateTime.Now;
//    dBContext.SaveToDB(updHttpRequest, '=');
//    Console.WriteLine($"row with Id_Request = {updHttpRequest.IdRequest} was updated successfully");
//}

// Видалити об'єкт з бази даних
//dBContext.LoadFromDb();
//if (dBContext.Requests.Count > 0)
//{
//    HttpRequest delHttpRequest = dBContext.Requests[0];
//    dBContext.SaveToDB(delHttpRequest, '-');
//    Console.WriteLine($"row with Id_Request = {delHttpRequest.IdRequest} was deleted successfully");
//}