using System;
using System.Threading;
using System.Threading.Tasks;

public abstract class MinecraftServer
{
    public abstract void StartServer();
    public abstract void StopServer();
    
    public static T CreateServer<T>(Func<Thread, T> factory) where T : MinecraftServer
    {
        var promise = new TaskCompletionSource<T>();
        new Thread(() =>
        {
            var server = factory(Thread.CurrentThread);
            promise.SetResult(server);

            server.StartServer();
        }).Start();
        return promise.Task.Result;
    }
}
