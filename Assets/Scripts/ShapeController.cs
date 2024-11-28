using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ShapeController 
{
    public enum shapeType { Parallelepiped = 0, Sphere = 1, Prism = 2, Capsule = 3 }

    public enum shapeParameters { Width, Height, Depth, Radius, Smoothing, SideCount, Color }
}
