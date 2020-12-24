using System;
using System.Collections.Concurrent;

public class Dispatcher
{
    private readonly ConcurrentQueue<Action> _queue = new ConcurrentQueue<Action>();
    
    public void Update() {
        while (_queue.TryDequeue(out var action))
        {
            action();
        }
    }

    public void Enqueue(Action action)
    {
        _queue.Enqueue(action);
    }

    public void Clear()
    {
        while (_queue.TryDequeue(out var action)) { }
    }
}
