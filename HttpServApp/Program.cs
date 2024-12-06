using HttpServApp;
using System.ServiceProcess;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Якщо запуск в режимі служби
if (!args.Contains("run=1"))
{
  ServiceHttp serviceHttp = new ServiceHttp();
  ServiceBase.Run(serviceHttp);

}
// Якщо запуск в режимі консольного застосунку (для відладки)
else
{
  ServiceHttp conRun = new ServiceHttp();
  conRun.StartAsProgram(args);

  Console.WriteLine("Натиснiть клавишу Esc для виходу ...");

  while (Console.ReadKey().Key != ConsoleKey.Escape)
  {
    Thread.Sleep(500);
  }

  conRun.StopAsProgram();
  Thread.Sleep(3000);
  Environment.Exit(0);
}
