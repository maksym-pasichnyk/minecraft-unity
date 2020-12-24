using System;
using System.Threading;

public abstract class MinecraftServer
{
    public abstract void StartServer();
    public abstract void StopServer();
    
    public static T CreateServer<T>(Func<Thread, T> factory) where T : MinecraftServer
    {
        T server = null;
        var thread = new Thread(() =>
        {
            server.StartServer();
        });
        server = factory(thread);
        thread.Start();
        return server;
    }
}
