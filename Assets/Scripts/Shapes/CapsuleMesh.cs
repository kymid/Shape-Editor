using System;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleMesh : ShapeMesh
{

    private float radius = 0.5f;
    private float height = 2f;
    private int radialSegments = 16;
    private int heightSegments = 8;

    public override void UpdateParameters(Dictionary<ShapeController.shapeParameters, object> parameters)
    {
        base.UpdateParameters(parameters);

        if (parameters.ContainsKey(ShapeController.shapeParameters.Radius))
            radius = (float)parameters[ShapeController.shapeParameters.Radius];
        else
            throw new ArgumentException($"Radius parameter missing");

        if (parameters.ContainsKey(ShapeController.shapeParameters.Height))
            height = (float)parameters[ShapeController.shapeParameters.Height];
        else
            throw new ArgumentException($"Height parameter missing");

        if (parameters.ContainsKey(ShapeController.shapeParameters.Smoothing))
        {
            radialSegments = (int)parameters[ShapeController.shapeParameters.Smoothing];
            heightSegments = (int)parameters[ShapeController.shapeParameters.Smoothing];
        }
        else
            throw new ArgumentException($"Smoothing parameter missing");
    }

    protected override Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.name = "GeneratedCapsule";

        int hemisphereSegments = heightSegments / 2;
        int vertexCount = (radialSegments + 1) * (heightSegments + 1);
        int triangleCount = radialSegments * (heightSegments + 1) * 2;

        Vector3[] vertices = new Vector3[vertexCount];
        Vector3[] normals = new Vector3[vertexCount];
        Vector2[] uv = new Vector2[vertexCount];
        int[] triangles = new int[triangleCount * 3];

        float cylinderHeight = height - 2 * radius; 
        float angleStep = Mathf.PI * 2 / radialSegments;
        float verticalStep = 1f / heightSegments;
        float halfHeight = cylinderHeight / 2f;

        int vertexIndex = 0;
        int triangleIndex = 0;

        // Генерация вершин, нормалей и UV
        for (int y = 0; y <= heightSegments; y++)
        {
            float v = y * verticalStep; // Нормализованное вертикальное положение [0, 1]
            float yPos;

            if (y < hemisphereSegments) // Нижняя полусфера
            {
                float theta = Mathf.PI / 2 * (y / (float)hemisphereSegments - 1); // Угол от -90° до 0°
                yPos = -halfHeight + radius * Mathf.Sin(theta);
                float ringRadius = radius * Mathf.Cos(theta);

                for (int x = 0; x <= radialSegments; x++)
                {
                    float angle = x * angleStep;
                    float xPos = Mathf.Cos(angle) * ringRadius;
                    float zPos = Mathf.Sin(angle) * ringRadius;

                    vertices[vertexIndex] = new Vector3(xPos, yPos, zPos);
                    normals[vertexIndex] = new Vector3(xPos, Mathf.Sin(theta) * radius, zPos).normalized;
                    uv[vertexIndex] = new Vector2(x / (float)radialSegments, v);

                    vertexIndex++;
                }
            }
            else if (y >= heightSegments - hemisphereSegments) // Верхняя полусфера
            {
                float theta = Mathf.PI / 2 * ((y - (heightSegments - hemisphereSegments)) / (float)hemisphereSegments);
                yPos = halfHeight + radius * Mathf.Sin(theta);
                float ringRadius = radius * Mathf.Cos(theta);

                for (int x = 0; x <= radialSegments; x++)
                {
                    float angle = x * angleStep;
                    float xPos = Mathf.Cos(angle) * ringRadius;
                    float zPos = Mathf.Sin(angle) * ringRadius;

                    vertices[vertexIndex] = new Vector3(xPos, yPos, zPos);
                    normals[vertexIndex] = new Vector3(xPos, Mathf.Sin(theta) * radius, zPos).normalized;
                    uv[vertexIndex] = new Vector2(x / (float)radialSegments, v);

                    vertexIndex++;
                }
            }
            else // Цилиндрическая часть
            {
                yPos = (v - 0.5f) * cylinderHeight;

                for (int x = 0; x <= radialSegments; x++)
                {
                    float angle = x * angleStep;
                    float xPos = Mathf.Cos(angle) * radius;
                    float zPos = Mathf.Sin(angle) * radius;

                    vertices[vertexIndex] = new Vector3(xPos, yPos, zPos);
                    normals[vertexIndex] = new Vector3(xPos, 0, zPos).normalized;
                    uv[vertexIndex] = new Vector2(x / (float)radialSegments, v);

                    vertexIndex++;
                }
            }
        }

        // Генерация треугольников
        for (int y = 0; y < heightSegments; y++)
        {
            for (int x = 0; x < radialSegments; x++)
            {
                int current = y * (radialSegments + 1) + x;
                int next = current + radialSegments + 1;

                // Первый треугольник
                triangles[triangleIndex++] = current;
                triangles[triangleIndex++] = next;
                triangles[triangleIndex++] = current + 1;

                // Второй треугольник
                triangles[triangleIndex++] = current + 1;
                triangles[triangleIndex++] = next;
                triangles[triangleIndex++] = next + 1;
            }
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uv;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        return mesh;
    }
}
