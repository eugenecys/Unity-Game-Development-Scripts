using UnityEngine;
using System.Collections;

public static class Utility {
    //Add utility methods here

    public static Vector2 toVector2(this Vector3 vec)
    {
        return new Vector2(vec.x, vec.y);
    }

    
}
