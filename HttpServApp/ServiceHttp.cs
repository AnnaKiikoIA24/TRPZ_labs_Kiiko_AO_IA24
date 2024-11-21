using System.ServiceProcess;

namespace HttpServApp
{
    partial class ServiceHttp : ServiceBase
    {
        // Ознака наявності аргументів: якщо true, то в режимі консольного застосунку (debug configuration)
        bool isArgs = false;

        public ServiceHttp()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // TODO: Код для запуска служби
            isArgs = args.Contains("run=1");
            Listener listener = new Listener();
            listener.RunServer();
        }

        protected override void OnStop()
        {
            // TODO: Код, що виконує підготовку до зупинки служби.
            if (isArgs)
                Environment.Exit(1);
            else
                base.OnStop();
        }

        // запуск через консоль (для відладки debug)
        public void StartAsProgram(string[] args)
        {
            OnStart(args);
        }


        // зупинка через консоль (для відладки)
        public void StopAsProgram()
        {
            OnStop();
        }
    }
}
