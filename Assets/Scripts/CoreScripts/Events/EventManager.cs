using System;
using System.Collections.Generic;
using UnityEngine;
using Firat0667.WesternRoyaleLib.Patterns;

public class EventManager : FoundationSingleton<EventManager>, IFoundationSingleton
{
    private Dictionary<string, Action<object>> _eventDictionary = new();

    public bool Initialized { get; set ; }

    private void Awake()
    {
        if (!Initialized)
        {
            _eventDictionary = new Dictionary<string, Action<object>>();
            Initialized = true;
        }
    }

    /// <summary>
    /// Subscribes a method to an event.
    /// </summary>
    public void Subscribe(string eventName, Action<object> listener)
    {
        if (!_eventDictionary.ContainsKey(eventName))
        {
            _eventDictionary[eventName] = listener;
        }
        else
        {
            _eventDictionary[eventName] += listener;
        }
    }

    /// <summary>
    /// Unsubscribes a method from an event.
    /// </summary>
    public void Unsubscribe(string eventName, Action<object> listener)
    {
        if (_eventDictionary.ContainsKey(eventName))
        {
            _eventDictionary[eventName] -= listener;
            if (_eventDictionary[eventName] == null)
            {
                _eventDictionary.Remove(eventName);
            }
        }
    }

    /// <summary>
    /// Triggers an event and notifies all subscribers.
    /// </summary>
    public void Trigger(string eventName, object eventData = null)
    {
        if (_eventDictionary.ContainsKey(eventName))
        {
            _eventDictionary[eventName]?.Invoke(eventData);
            Debug.Log($"[EventManager] Event triggered: {eventName}");
        }
        else
        {
            Debug.LogWarning($"[EventManager] No listeners found for event: {eventName}");
        }
    }
}
/*
 * 
 * 
 * 
 * 
  void Start()
{
    EventManager.Instance.Subscribe("EnemyDied", OnEnemyDeath);
}
void OnDestroy()
{
    EventManager.Instance.Unsubscribe("EnemyDied", OnEnemyDeath);
}

void OnEnemyDeath(object eventData)
{
    Debug.Log("An enemy has died!");
}



EventManager.Instance.Trigger("EnemyDied");

EventManager.Instance.Trigger("EnemyDied", 100);


void EnemyDied(object eventData)
{
    int score = (int)eventData;
    Debug.Log($"Enemy killed! Score: {score}");
}


 * 
 * 
 * 
 * 
 * 
 * 
 */