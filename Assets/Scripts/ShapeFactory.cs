using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class ShapeFactory
{
    public static GameObject CreateShape(ShapeController.shapeType shapeType, Dictionary<ShapeController.shapeParameters, object> parameters)
    {
        GameObject newShape = new GameObject();
        newShape.name = shapeType.ToString();

        newShape.AddComponent<MeshFilter>();
        newShape.AddComponent<MeshRenderer>();
        ShapeMesh shapeMesh = newShape.GetComponent<ShapeMesh>();

        switch (shapeType)
        {
            case ShapeController.shapeType.Parallelepiped:
                shapeMesh = newShape.AddComponent<ParallelepipedMesh>();
                break;
            case ShapeController.shapeType.Sphere:
                shapeMesh = newShape.AddComponent<SphereMesh>();
                break;
            case ShapeController.shapeType.Prism:
                shapeMesh = newShape.AddComponent<PrismMesh>();
                break;
            case ShapeController.shapeType.Capsule:
                shapeMesh = newShape.AddComponent<CapsuleMesh>();
                break;
            default:
                throw new ArgumentException($"Unknown shape type: {shapeType}");
        }

        shapeMesh.UpdateParameters(parameters);
        shapeMesh.GenerateShape();

        return newShape;
    }
}
