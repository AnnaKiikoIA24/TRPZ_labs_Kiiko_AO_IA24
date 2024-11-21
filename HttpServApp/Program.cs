using HttpServApp;
using System.ServiceProcess;

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

    Console.WriteLine("Натисніть клавишу Esc для виходу ...");
    Thread.Sleep(500);
    while (Console.ReadKey().Key != ConsoleKey.Escape)
    {
        ;
    }

    conRun.StopAsProgram();
    Thread.Sleep(3000);
    Environment.Exit(0);
}
