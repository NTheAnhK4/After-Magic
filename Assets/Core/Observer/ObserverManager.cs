using System;

using System.Collections.Generic;
using UnityEngine;

public static class ObserverManager<T> where T:Enum
{
    private static Dictionary<T, Action<object>> _events = new Dictionary<T, Action<object>>();
    private static readonly Dictionary<T, object> _eventData = new();

    public static void Attach(T eventID,Action<object> callback)
    {
        if (callback == null)
        {
        
            Debug.LogWarning($"Callback for event {eventID.GetType().Name} is NULL.");
            return;
        }

        if (eventID == null)
        {
            Debug.LogWarning($"EventID is NULL. Cannot register event.");
            return;
        }
        if (!_events.TryAdd(eventID, callback))
        {
            _events[eventID] += callback;
        }
    }

    public static void Detach(T eventID,Action<object> callback)
    {
        if (_events.ContainsKey(eventID))
        {
            _events[eventID] -= callback;
            if (_events[eventID] == null)
            {
                _events.Remove(eventID);
            }
        }
        else
        {
            Debug.LogWarning($"Event '{eventID}' not found in EventDispatcher_ "+typeof(T).Name);
        }
    }

    public static void Notify(T eventID, object param = null)
    {
      
        if (!_events.ContainsKey(eventID))
        {
            Debug.LogWarning($"Event:{eventID.GetType().Name} has no Listener EventDispatcher_ "+typeof(T).Name);
            return;
        }

        if (_events[eventID] == null)
        {
            Debug.LogWarning($"Callback for event {eventID.GetType().Name} is NULL.");
            _events.Remove(eventID);
            return;
        }
        _events[eventID]?.Invoke(param);
    }

    public static void DetachAll()
    {
        _events.Clear();
    }
    public  static _T GetData<_T>(T eventID)
    {
        if (_eventData.TryGetValue(eventID, out object data))
        {
            return (_T)data;
        }

        return default;
    }

    public static void EmitEvent(T eventID, object data = null)
    {
        if (_eventData.ContainsKey(eventID))
        {
            _eventData[eventID] = data;
        }
        else _eventData[eventID] = data;

        if (_events.TryGetValue(eventID, out Action<object> thisEvent))
        {
            thisEvent.Invoke(data);
        }
    }
}

public enum GameEventID
{
    DisplaySignal,
    DisplayFirstGameSignal,
    AttackCastle,
    Lose,
    SpawnWay,
  
    UpdateGold,
    Win
}


