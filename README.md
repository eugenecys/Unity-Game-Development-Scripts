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


