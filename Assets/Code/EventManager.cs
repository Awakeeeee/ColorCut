using System;
using System.Collections;
using System.Collections.Generic;

//TODO which is better this class Singleton or Static?
public class EventManager
{
    public delegate void EventHandlerWithNoArgs(Object sender, EventArgs e);  //'handler' rather than 'callback' is kind of MS convention

    public static Dictionary<EventType, EventHandlerWithNoArgs> eventDic = new Dictionary<EventType, EventHandlerWithNoArgs>();

    //像某个event对应的delegate中添加方法，这叫做注册监听者
    //这个方法定义者就是监听者，event触发时，广播至他的所有监听者，然后各个监听者做自己注册的事情
    public static void RegisterListener(EventType e, EventHandlerWithNoArgs listener)
    {
        if (!eventDic.ContainsKey(e))
        {
            eventDic.Add(e, null);
        }
        eventDic[e] += listener;
    }

    public static void RemoveListener(EventType e, EventHandlerWithNoArgs listener)
    {
        if (!eventDic.ContainsKey(e))
            return;
        if (eventDic[e] != null)
        {
            eventDic[e] -= listener;
        }
    }

    public static void Reset()
    {
        eventDic.Clear();
    }

    public static void Broadcast(EventType t, Object sender, EventArgs e)
    {
        if (!eventDic.ContainsKey(t))
            return;
        EventHandlerWithNoArgs handler = eventDic[t];
        if (handler != null)
        {
            handler.Invoke(sender, e);
        }
    }
}

public enum EventType
{ }