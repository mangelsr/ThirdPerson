using System;
using System.Collections.Generic;

public static class EventBus<T> where T : IEvent
{
    private static readonly HashSet<Action<T>> bindings = new HashSet<Action<T>>();

    public static void Register(Action<T> binding)
    {
        bindings.Add(binding);
    }

    public static void Deregister(Action<T> binding)
    {
        bindings.Remove(binding);
    }

    public static void Raise(T @event)
    {
        foreach (var binding in bindings)
        {
            binding.Invoke(@event);
        }
    }
}

public interface IEvent { }
