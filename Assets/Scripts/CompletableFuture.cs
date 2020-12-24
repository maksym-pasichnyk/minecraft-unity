using System;
using System.Threading.Tasks;

public static class CompletableFuture
{
    // public static void Drive<T>(this Task<T> self)
    // {
    //     if (self.Status == TaskStatus.Created)
    //     {
    //         self.Start();
    //     }
    // }

    public static Task<U> ThenComposeAsync<T, U>(this Task<T> self, Func<T, Task<U>> func)
    {
        return self.ContinueWith(task => func(task.Result).GetAwaiter().GetResult());
    }
    
    public static Task<U> ThenApplyAsync<T, U>(this Task<T> self, Func<T, U> func)
    {
        return self.ContinueWith(task => func(task.Result));
    }
    
    public static Task ThenAcceptAsync<T>(this Task<T> self, Action<T> func)
    {
        return self.ContinueWith(task => func(task.Result));
    }

    public static T GetNow<T>(this Task<T> self, T defaultReturn)
    {
        return self.IsCompleted ? self.Result : defaultReturn;

    }
}
