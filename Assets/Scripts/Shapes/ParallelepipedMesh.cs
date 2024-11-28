using System;
using System.Collections.Generic;
using UnityEngine;

public class ParallelepipedMesh : ShapeMesh
{

    private float width = 1f;
    private float height = 1f;
    private float depth = 1f;

    public override void UpdateParameters(Dictionary<ShapeController.shapeParameters, object> parameters)
    {
        base.UpdateParameters(parameters);

        if (parameters.ContainsKey(ShapeController.shapeParameters.Width))
            width = (float)parameters[ShapeController.shapeParameters.Width];
        else
            throw new ArgumentException($"Width parameter missing");

        if (parameters.ContainsKey(ShapeController.shapeParameters.Height))
            height = (float)parameters[ShapeController.shapeParameters.Height];
        else
            throw new ArgumentException($"parallelepipedHeight parameter missing");

        if (parameters.ContainsKey(ShapeController.shapeParameters.Depth))
            depth = (float)parameters[ShapeController.shapeParameters.Depth];
        else
            throw new ArgumentException($"Depth parameter missing");
    }

    protected override Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.name = "GeneratedParallelepiped";

        float halfWidth = width * 0.5f;
        float halfHeight = height * 0.5f;
        float halfDepth = depth * 0.5f;

        vertices = new Vector3[]
        {
            // �������� �����
            new Vector3(-halfWidth, -halfHeight, halfDepth), // 0
            new Vector3(halfWidth, -halfHeight, halfDepth),  // 1
            new Vector3(halfWidth, halfHeight, halfDepth),   // 2
            new Vector3(-halfWidth, halfHeight, halfDepth),  // 3

            // ������ �����
            new Vector3(halfWidth, -halfHeight, -halfDepth), // 4
            new Vector3(-halfWidth, -halfHeight, -halfDepth),// 5
            new Vector3(-halfWidth, halfHeight, -halfDepth), // 6
            new Vector3(halfWidth, halfHeight, -halfDepth),  // 7

            // ������� �����
            new Vector3(-halfWidth, halfHeight, halfDepth),  // 8
            new Vector3(halfWidth, halfHeight, halfDepth),   // 9
            new Vector3(halfWidth, halfHeight, -halfDepth),  // 10
            new Vector3(-halfWidth, halfHeight, -halfDepth), // 11

            // ������ �����
            new Vector3(-halfWidth, -halfHeight, -halfDepth),// 12
            new Vector3(halfWidth, -halfHeight, -halfDepth), // 13
            new Vector3(halfWidth, -halfHeight, halfDepth),  // 14
            new Vector3(-halfWidth, -halfHeight, halfDepth), // 15

            // ������ �����
            new Vector3(halfWidth, -halfHeight, halfDepth),  // 16
            new Vector3(halfWidth, -halfHeight, -halfDepth), // 17
            new Vector3(halfWidth, halfHeight, -halfDepth),  // 18
            new Vector3(halfWidth, halfHeight, halfDepth),   // 19

            // ����� �����
            new Vector3(-halfWidth, -halfHeight, -halfDepth),// 20
            new Vector3(-halfWidth, -halfHeight, halfDepth), // 21
            new Vector3(-halfWidth, halfHeight, halfDepth),  // 22
            new Vector3(-halfWidth, halfHeight, -halfDepth)  // 23
        };

        normals = new Vector3[]
        {
            // �������� �����
            Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward,

            // ������ �����
            Vector3.back, Vector3.back, Vector3.back, Vector3.back,

            // ������� �����
            Vector3.up, Vector3.up, Vector3.up, Vector3.up,

            // ������ �����
            Vector3.down, Vector3.down, Vector3.down, Vector3.down,

            // ������ �����
            Vector3.right, Vector3.right, Vector3.right, Vector3.right,

            // ����� �����
            Vector3.left, Vector3.left, Vector3.left, Vector3.left
        };

        uv = new Vector2[]
        {
            // �������� �����
            new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),

            // ������ �����
            new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),

            // ������� �����
            new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),

            // ������ �����
            new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),

            // ������ �����
            new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),

            // ����� �����
            new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1)
        };

        triangles = new int[]
        {
            // �������� �����
            0, 1, 2, 0, 2, 3,
            // ������ �����
            4, 5, 6, 4, 6, 7,
            // ������� �����
            8, 9, 10, 8, 10, 11,
            // ������ �����
            12, 13, 14, 12, 14, 15,
            // ������ �����
            16, 17, 18, 16, 18, 19,
            // ����� �����
            20, 21, 22, 20, 22, 23
        };

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uv;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();

        return mesh;
    }


}
