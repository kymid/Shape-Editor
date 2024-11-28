using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShapeMesh : MonoBehaviour
{
    public Color shapeColor { get; private set; } = Color.gray;

    protected Material shapeMaterial;

    protected MeshFilter meshFilter;
    protected MeshRenderer meshRenderer;

    protected Vector3[] vertices;
    protected Vector3[] normals;
    protected Vector2[] uv;
    protected int[] triangles;

    public virtual void GenerateShape()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        meshFilter.mesh = CreateMesh();
        UpdateMaterialColor();
    }

    public virtual void SetMaterialColor(Color pickedColor)
    {
        shapeColor = pickedColor;
    }
    protected virtual void UpdateMaterialColor()
    {
        meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));
        shapeMaterial = meshRenderer.sharedMaterial;
        shapeMaterial.color = shapeColor;
    }

    public virtual void UpdateParameters(Dictionary<ShapeController.shapeParameters, object> parameters)
    {
        if (parameters.ContainsKey(ShapeController.shapeParameters.Color))
            SetMaterialColor((Color)parameters[ShapeController.shapeParameters.Color]);
        else
            throw new ArgumentException($"Color parameter missing");
    }

    protected abstract Mesh CreateMesh();
}
