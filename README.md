# Game Development Scripts for Unity
Over the course of making several games in Unity, I wrote a few scripts which I used in many of the games I made, and proved useful in the initial development process of the game. 

These scripts are all useful in most game types, and have generic applications.

## Singleton
The singleton is single most useful design pattern for me when developing in Unity, and so I wrote a script that converts a Unity class into a Unity singleton class. 

#### To use:
Typical class in Unity
```C#
public class MyClass : MonoBehaviour 
{

}
```

Turning the class into a singleton:
```C#
public class MyClass : Singleton<MyClass> 
{

}
```

To use the singleton in other classes:
```C#
public class Foo : MonoBehaviour 
{
  MyClass myClass;
  
  void Awake() 
  {
    myClass = MyClass.Instance;
  }
}
```
This ensures that the instance stored in myClass is the same instance as all other instances called by other methods or classes, i.e. one instance, which is basically the singleton design pattern.


Note that the singleton derives MonoBehaviour, so there is no need to explicitly derive it anymore.


##Event Manager
I found the Invoke function in Unity rather primitive and limited, e.g. it does not support parameterized functions. At times that I needed to make games that are very heavily event driven, I found Invoke extremely troublesome, and decided to write an Event Manager to replace that.

Note that this is completely separate from Unity Event class, so do not confuse them at all. All "events" in this Event Manager are basically functions that are stored for execution. I have not found a better name for this class yet, so let me know if you can think of one.

The Event Manager works by storing your functions with a certain time delay tied to it, which is then executed when the time has elapsed. Setting it to 0 means that the function is executed immediately. 

Here are some of the features of the Event Manager:
1. Persistent storage of functions/events - the events can be re-executed if needed.
2. Grouping of events by stages, which means that you can traverse between stages to call the events that are stored in the needed stage. 
3. Single-use events - events can be called once and not be stored.

To use the Event Manager:
```C#
public class Foo : MonoBehaviour 
{
  EventManager evtMgr;
  
  void Awake() 
  {
    evtMgr = EventManager.Instance;
  }
}
```

The Event Manager will start running on Awake().

In the Event Manager class, functions are stored as GameEvent delegates. If you don't understand what this means, just take it that the functions are stored as is, meaning they are not executed. 

There are two functions for adding events to the Event Manager:

1. Single-use events: 
```C#
EventManager.addEvent(GameEvent evt, float delay, bool relative);
```
There are three parameters in the addEvent function:
1. The first parameter is your function.
2. The second parameter is the amount of time you want the Event Manager to wait before executing the function.
3. The third parameter is to indicate if it will be relative to the current time. Entering false would mean that it will execute relative to the start time of the event manager stage (see Stage below) instead, which I almost never use. 
