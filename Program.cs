using System;
using System.Threading;
using Client = GameClient;
using Server = GameServer;

class Program
{
    private static bool isRunning = false;

    static void Main(string[] args)
    {
        Console.Title = "Game Server";
        isRunning = true;

        Thread mainThread = new Thread(new ThreadStart(MainThread));
        mainThread.Start();

        Server.Server.Start(50, 26950);
        //Server.Server.Edit(1);

        // --< Client >-- //
        Client.Client.instance.ConnectToServer();
        Client.ThreadManager.StartUpdateing();
        // --< ====== >-- //
    }

    private static void Input()
    {
        Console.WriteLine("started input thread");
        while (isRunning)
        {
            Console.ReadLine();
        }
    }

    private static void MainThread()
    {
        Console.WriteLine($"> Main thread started. Running at {Server.Constants.TICKS_PER_SEC} ticks per second.");
        DateTime _nextLoop = DateTime.Now;

        while (isRunning)
        {
            while (_nextLoop < DateTime.Now)
            {
                Server.GameLogic.Update();

                _nextLoop = _nextLoop.AddMilliseconds(Server.Constants.MS_PER_TICK);

                if (_nextLoop > DateTime.Now)
                {
                    Thread.Sleep(_nextLoop - DateTime.Now);
                }
            }
        }
    }
}
