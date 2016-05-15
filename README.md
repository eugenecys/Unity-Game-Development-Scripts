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

##Config 
In many of the games I made, there were changes I needed to make fairly often, and most of them were simply environmental variables. Rather than having to compile the game every single time I need to change a variable, I created a config file to store the values that I needed to change on the fly. 

One example is IP address - Sometimes there are different computers that the game needed testing on, and there are various server ports that the game has to read from for each machine. Rather than having to compile a different executable for each machine, simply change the ip address accordingly.

There are a few sample uses written in the config file already.

To write the config.txt file, there two things to follow:
1. All config values are to be stored in this format: 
```Name: Value```
and in the config script, there should be a case in the switch-case statement:
```
  case "name":
  //do stuff
```
2. Comments can be written in the file using the # key.


##Input Controller
Rather than having to have keyboard inputs detected and read in various scripts scattered across Unity, I decided to centralize all of them into a single Input Controller, and have all classes that require keyboard input detected use the input controller to do so. 

The input controller works by constantly polling all possible key presses, and if there is a function mapped to that key, that function is called. 

The input controller is a singleton, and therefore should be used as such.

To map a function to a key, simply call:
```C# 
InputController.registerTrigger(InputTrigger trigger, KeyCode key);
```

The first parameter is a delegate, so all you need to do is pass a function into it. The second parameter is the key you wish to map it to.

Example usage:
```C#
public void foo() {
  //Do stuff
}

public void bar(int value) {
  //Do other stuff
}

void Start() {
  registerTrigger(foo, KeyCode.W);
  registerTrigger(() => bar(1), KeyCode.Q);
}
```

Everytime W is pressed, foo is called. Everytime Q is pressed, bar is called with an integer value of 1.

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

In the Event Manager class, functions are stored as GameEvent delegates. If you don't understand what this means, just take it that the functions are stored as is, meaning they are not executed. 

There are two functions for adding events to the Event Manager:
1. Stored events:
These are events that are stored and can be called repeatedly.
```C#
EventManager.registerEvent(GameEvent evt, float delay);
```
The three parameters in the registerEvent function:
1. The first parameter is your function.
2. The second parameter is the amount of time you want the EventManager to wait before executing the function.

There is an optional integer third parameter to indicate the stage you want to store the function in. (See Stage below)


. Single-use events: 
These are events that you want to call once and never be called. These events will never be stored. This also means that if the stage is changed before any event added by addEvent has been executed, those functions will never run. 
```C#
EventManager.addEvent(GameEvent evt, float delay);
```
There are two parameters in the addEvent function:
1. The first parameter is your function.
2. The second parameter is the amount of time you want the Event Manager to wait before executing the function.

There is an optional boolean third parameter to indicate if it will be relative to the current time. Entering false would mean that it will execute relative to the start time of the event manager stage (see Stage below) instead, which I almost never use. By default, the third parameter is true.


To re-execute all the functions stored, call:
```C#
restartStage();
```

Example use:
Take for example these two functions:
```C#
public void foo() 
{

}

public int sum(int a, int b) 
{
  return a+b;
}
```

There are two ways to pass functions as parameters in C#:
1. 
```C#
evtMgr.addEvent(foo, 0);
evtMgr.registerEvent(foo, 0);
```
This method only works for unparameterized functions.

2. 
```C#
evtMgr.addEvent(() => foo(), 0);
evtMgr.addEvent(() => bar(2, 3));
```
To pass a parameterized function as-is, you have to add a '() => ' in front to prevent it from evaluating.

> Warning: Events that have the same delay time may not be called in added order. If you want one event to be called right after another, add a very small delay in the second one: 0f vs 0.001f.

####Stages
To group events together, I came up with the idea of stages, which are basically integer groups for functions. Functions can be stored in different stages, and the default stage is 0.

When a stage is accessed, the timer is reset to 0, so all events in the stage are executed relative to that time. 
To go to a certain stage, use the function:
```C#
goToStage(int i);
```

To restart a stage, call:
```C#
restartStage();
```

To go to the next stage:
```C#
advanceStage();
```

####Starting the Event Manager
The Event Manager is started by calling `run()`.
To ensure that all code runs according to intended means, do these checks:
1. All registerEvent code is written before run(), so they are all stored before the Event Manager starts.
2. All addEvent code is writted after run(), so they are all added during Event Manager runtime.
3. All addEvent code have delays shorter than any code that changes or restarts the stage. 

#####Example code
```C#

    EventManager evtMgr;

    void Awake()
    {
        evtMgr = EventManager.Instance;
    }

    void Start()
    {
        exampleMethod();
    }

    void foo()
    {
        Debug.Log("Hello");
    }

    void bar(int i)
    {
        Debug.Log(i);
    }

    void exampleMethod()
    {
        evtMgr.registerEvent(foo, 0);
        evtMgr.registerEvent(() => bar(1), 0.1f);
        evtMgr.registerEvent(() => foo(), 0.2f);


        //Stage 1 functions below. The above functions are all in stage 0.
        evtMgr.registerEvent(() => Debug.Log("Going to stage: "), 0f, 1);
        evtMgr.registerEvent(() => bar(2), 0.1f, 1);
        //Go back to stage 0 after 1 second
        evtMgr.registerEvent(() => evtMgr.goToStage(0), 1, 1);

        //Once you have finished with all the registerEvents, start the Event Manager.
        evtMgr.run();

        //Add all the temporary events below.
        
        //Events do not need to be written in chronological order
        evtMgr.addEvent(() => Debug.Log("This prints once"), 0.3f);
        //Goes to next stage after 1 second. As this is executed in the default stage (stage 0),
        //This will occur after the first three events above. 
        evtMgr.addEvent(evtMgr.advanceStage, 1);
        
        //As the stage is changed before this executes, this function will not be executed.
        evtMgr.addEvent(() => Debug.Log("This won't print"), 4f);
    }
```

The printout are as follows:
```
Hello
1
Hello
This prints once
Going to stage:
2
Hello
1
Hello
```

The first 4 lines happen at stage 0, followed by the next 2 at stage 1, and then the last 3 at stage 0 again. 


##AVL
The AVL class is a generic Self-Balancing Binary Search Tree, which I included both for general purpose use if needed, since certain versions of Unity do not yet have in-built priority lists or trees. 

This AVL tree stores all objects in a binary tree, ordered from smallest to largest, so it is useful if there is a need to maintain a list of ordered objects. 

The Event Manager uses the AVL tree, so make sure you have it in your project if you intend to use the Event Manager.

To add objects to the tree, call either: ```Add(T arg);``` or ```Push(T arg);```

To add a whole collection, call: ```AddAll(ICollection<T> collection);```

To check if an object if stored in the tree, call ```Contains(T arg)```

To retrieve the index of a specific object, call ```Find(T arg)```

To remove and retrieve the first object, call: ```Pop();```

To remove a specific object, call: ```Remove(T arg);```

The AVL Tree can also be accessed like an array (read only), by such:
```C#
AVL<int> myTree = new AVL<int>();
myTree.add(1);
myTree[0];
```

The size of the tree can be accessed by: 
```C#
myTree.Count;
```

To delete everything from the tree, call ```Clear()```

To make a copy of a collection, call ```CopyOf(ICollection<T> collection)
