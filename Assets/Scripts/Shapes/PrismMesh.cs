using System;
using System.Collections.Generic;
using UnityEngine;

public class PrismMesh : ShapeMesh
{
    private float radius = 1f;
    private float height = 2f;
    private int sideCount = 3;

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

        if (parameters.ContainsKey(ShapeController.shapeParameters.SideCount))
            sideCount = (int)parameters[ShapeController.shapeParameters.SideCount];
        else
            throw new ArgumentException($"SideCount parameter missing");
    }

    protected override Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.name = "GeneratedPrism";

        int vertexCount = 2 + sideCount * 4; 
        int triangleCount = sideCount * 4; 

        vertices = new Vector3[vertexCount + sideCount * 2];
        normals = new Vector3[vertexCount + sideCount * 2];
        uv = new Vector2[vertexCount + sideCount * 2];
        triangles = new int[triangleCount * 3];

        float angleStep = 2 * Mathf.PI / sideCount;
        float halfHeight = height / 2f;

        int vertexIndex = 0;
        int triangleIndex = 0;

        // Генерация боковых граней
        for (int i = 0; i < sideCount; i++)
        {
            float angle = i * angleStep;
            float nextAngle = ((i + 1) % sideCount) * angleStep;

            // Текущая и следующая вершины
            Vector3 bottomCurrent = new Vector3(Mathf.Cos(angle) * radius, -halfHeight, Mathf.Sin(angle) * radius);
            Vector3 topCurrent = new Vector3(Mathf.Cos(angle) * radius, halfHeight, Mathf.Sin(angle) * radius);
            Vector3 bottomNext = new Vector3(Mathf.Cos(nextAngle) * radius, -halfHeight, Mathf.Sin(nextAngle) * radius);
            Vector3 topNext = new Vector3(Mathf.Cos(nextAngle) * radius, halfHeight, Mathf.Sin(nextAngle) * radius);

            // вершины для текущей грани
            vertices[vertexIndex] = bottomCurrent; 
            vertices[vertexIndex + 1] = topCurrent; 
            vertices[vertexIndex + 2] = bottomNext; 
            vertices[vertexIndex + 3] = topNext; 

            // Нормали направлены строго наружу
            Vector3 normal = Vector3.Cross(topCurrent - bottomCurrent, bottomNext - bottomCurrent).normalized;
            normals[vertexIndex] = normal;
            normals[vertexIndex + 1] = normal;
            normals[vertexIndex + 2] = normal;
            normals[vertexIndex + 3] = normal;

            // UV координаты
            uv[vertexIndex] = new Vector2(i / (float)sideCount, 0);
            uv[vertexIndex + 1] = new Vector2(i / (float)sideCount, 1);
            uv[vertexIndex + 2] = new Vector2((i + 1) / (float)sideCount, 0);
            uv[vertexIndex + 3] = new Vector2((i + 1) / (float)sideCount, 1);

            // Треугольники для текущей грани
            triangles[triangleIndex++] = vertexIndex;
            triangles[triangleIndex++] = vertexIndex + 1;
            triangles[triangleIndex++] = vertexIndex + 2;

            triangles[triangleIndex++] = vertexIndex + 2;
            triangles[triangleIndex++] = vertexIndex + 1;
            triangles[triangleIndex++] = vertexIndex + 3;

            vertexIndex += 4;
        }

        // Добавляем вершины для нижнего и верхнего оснований
        int baseVertexIndex = vertexIndex;
        for (int i = 0; i < sideCount; i++)
        {
            float angle = i * angleStep;
            vertices[vertexIndex] = new Vector3(Mathf.Cos(angle) * radius, -halfHeight, Mathf.Sin(angle) * radius);
            normals[vertexIndex] = Vector3.down;
            uv[vertexIndex] = new Vector2((vertices[vertexIndex].x / radius + 1) * 0.5f, (vertices[vertexIndex].z / radius + 1) * 0.5f);
            vertexIndex++;

            vertices[vertexIndex] = new Vector3(Mathf.Cos(angle) * radius, halfHeight, Mathf.Sin(angle) * radius);
            normals[vertexIndex] = Vector3.up;
            uv[vertexIndex] = new Vector2((vertices[vertexIndex].x / radius + 1) * 0.5f, (vertices[vertexIndex].z / radius + 1) * 0.5f);
            vertexIndex++;
        }

        // Треугольники для нижнего и верхнего оснований
        int centerBottomIndex = vertexIndex;
        int centerTopIndex = vertexIndex + 1;

        vertices[centerBottomIndex] = new Vector3(0, -halfHeight, 0);
        vertices[centerTopIndex] = new Vector3(0, halfHeight, 0);

        normals[centerBottomIndex] = Vector3.down;
        normals[centerTopIndex] = Vector3.up;

        uv[centerBottomIndex] = new Vector2(0.5f, 0.5f);
        uv[centerTopIndex] = new Vector2(0.5f, 0.5f);

        for (int i = 0; i < sideCount; i++)
        {
            int next = (i + 1) % sideCount;

            // Нижнее основание
            triangles[triangleIndex++] = centerBottomIndex;
            triangles[triangleIndex++] = baseVertexIndex + i * 2;
            triangles[triangleIndex++] = baseVertexIndex + next * 2;

            // Верхнее основание
            triangles[triangleIndex++] = centerTopIndex;
            triangles[triangleIndex++] = baseVertexIndex + next * 2 + 1;
            triangles[triangleIndex++] = baseVertexIndex + i * 2 + 1;
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uv;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        return mesh;

    }
}


