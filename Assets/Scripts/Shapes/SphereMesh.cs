using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SphereMesh : ShapeMesh
{
    [Header("Sphere Settings")]
    public float radius = 1f;
    public int longitudeSegments = 24; 
    public int latitudeSegments = 16;  


    public override void UpdateParameters(Dictionary<ShapeController.shapeParameters, object> parameters)
    {
        base.UpdateParameters(parameters);

        if (parameters.ContainsKey(ShapeController.shapeParameters.Radius))
            radius = (float)parameters[ShapeController.shapeParameters.Radius];
        else
            throw new ArgumentException($"Radius parameter missing");

        if (parameters.ContainsKey(ShapeController.shapeParameters.Smoothing))
        {
            longitudeSegments = (int)parameters[ShapeController.shapeParameters.Smoothing];
            latitudeSegments = (int)parameters[ShapeController.shapeParameters.Smoothing];
        }
        else
            throw new ArgumentException($"Smoothing parameter missing");
    }

    protected override Mesh CreateMesh()
    {
        
        Mesh mesh = new Mesh();
        mesh.name = "GeneratedSphere";

        int vertexCount = (longitudeSegments + 1) * (latitudeSegments + 1);
        vertices = new Vector3[vertexCount];
        normals = new Vector3[vertexCount];
        uv = new Vector2[vertexCount];

        int triangleCount = longitudeSegments * latitudeSegments * 6;
        triangles = new int[triangleCount];

        int vertexIndex = 0;
        int triangleIndex = 0;

        // Генерация вершин, нормалей и UV-координат
        for (int lat = 0; lat <= latitudeSegments; lat++)
        {
            float verticalAngle = Mathf.PI * lat / latitudeSegments; // Угол по широте (от 0 до PI)
            float sinLat = Mathf.Sin(verticalAngle);
            float cosLat = Mathf.Cos(verticalAngle);

            for (int lon = 0; lon <= longitudeSegments; lon++)
            {
                float horizontalAngle = 2 * Mathf.PI * lon / longitudeSegments; // Угол по долготе (от 0 до 2PI)
                float sinLon = Mathf.Sin(horizontalAngle);
                float cosLon = Mathf.Cos(horizontalAngle);

                // Позиция вершины
                Vector3 vertexPosition = new Vector3(
                    radius * sinLat * cosLon, // X
                    radius * cosLat,         // Y
                    radius * sinLat * sinLon // Z
                );

                vertices[vertexIndex] = vertexPosition;
                normals[vertexIndex] = vertexPosition.normalized; // Нормали направлены наружу

                // UV-координаты (с корректным учетом швов)
                uv[vertexIndex] = new Vector2((float)lon / longitudeSegments, (float)lat / latitudeSegments);

                vertexIndex++;
            }
        }

        // Генерация треугольников с правильным порядком (Counter-Clockwise)
        for (int lat = 0; lat < latitudeSegments; lat++)
        {
            for (int lon = 0; lon < longitudeSegments; lon++)
            {
                int current = lat * (longitudeSegments + 1) + lon;
                int next = current + longitudeSegments + 1;

                // Первый треугольник
                triangles[triangleIndex++] = current;
                triangles[triangleIndex++] = current + 1;
                triangles[triangleIndex++] = next;

                // Второй треугольник
                triangles[triangleIndex++] = next;
                triangles[triangleIndex++] = current + 1;
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
