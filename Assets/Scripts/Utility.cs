using UnityEngine;
using System.Collections;
using System;

public static class Utility {

    public class Pair<T, K>
    {
        public T first { get; private set; }
        public K second { get; private set; }
        
        public Pair(T a, K b)
        {
            first = a;
            second = b;
        }

        public void setFirst(T a)
        {
            first = a;
        }

        public void setSecond(K b)
        {
            second = b;
        }

        public override string ToString()
        {
            return first.ToString() + ", " + second.ToString();
        }
        

    }

    public static Vector2 toVector2(this Vector3 vect)
    {
        return new Vector2(vect.x, vect.y);
    }

    public static Vector3 toVector3(this Vector2 vect)
    {
        return new Vector3(vect.x, vect.y, 0);
    }

    public static Vector3 average(this Vector3[] vectArray)
    {
        Vector3 result = Vector3.zero;
        foreach (Vector3 vect in vectArray)
        {
            result += vect;
        }
        return result / vectArray.Length;
        
    }

    public static Vector2 average(this Vector2[] vectArray)
    {
        Vector2 result = Vector2.zero;
        foreach (Vector2 vect in vectArray)
        {
            result += vect;
        }
        return result / vectArray.Length;

    }

}
