using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ShapeEditor : EditorWindow
{
    private int shapeIndex = 0;
    ShapeController.shapeType currentShape = ShapeController.shapeType.Parallelepiped;

    /* Parallelepiped Parameters */
    private Vector3 parallelepipedDimensions;

    /* Sphere Parameters */
    private float sphereRadius;
    private int sphereSmoothing;

    /* Prism Parameters */
    private float prismRadius;
    private float prismHeight;
    private int prismSideCount;

    /* Capsule Parameters */
    private float capsuleRadius;
    private float capsuleHeight;
    private int capsuleSmoothing;


    private Dictionary<ShapeController.shapeParameters, object> parameters;

    private Color shapeColor = Color.gray;

    [MenuItem("Window/Shape Editor")]
    public static void ShowWindow()
    {
        GetWindow<ShapeEditor>("Shape Editor");
    }


    private void OnGUI()
    {
        GUILayout.Space(10);
        GUILayout.Label("Shape selection", EditorStyles.boldLabel);
        GUILayout.Space(5);

        shapeIndex = EditorGUILayout.Popup(shapeIndex, System.Enum.GetNames(typeof(ShapeController.shapeType)));
        currentShape = (ShapeController.shapeType)shapeIndex;

        switch (currentShape)
        {
            case ShapeController.shapeType.Parallelepiped:
                GUILayout.Space(20);

                GUILayout.Label("parallelepipedWidth", EditorStyles.boldLabel);
                Rect position = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
                parallelepipedDimensions.x = EditorGUI.Slider(position, parallelepipedDimensions.x, 0.1f, 10f);

                GUILayout.Space(10);

                GUILayout.Label("parallelepipedHeight", EditorStyles.boldLabel);
                position = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
                parallelepipedDimensions.y = EditorGUI.Slider(position, parallelepipedDimensions.y, 0.1f, 10f);

                GUILayout.Space(10);

                GUILayout.Label("parallelepipedDepth", EditorStyles.boldLabel);
                position = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
                parallelepipedDimensions.z = EditorGUI.Slider(position, parallelepipedDimensions.z, 0.1f, 10f);

                break;

            case ShapeController.shapeType.Sphere:
                GUILayout.Space(20);

                GUILayout.Label("sphereRadius", EditorStyles.boldLabel);
                position = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
                sphereRadius = EditorGUI.Slider(position, sphereRadius, 0.1f, 10f);

                GUILayout.Space(10);

                GUILayout.Label("sphereSmoothing", EditorStyles.boldLabel);
                position = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
                sphereSmoothing = EditorGUI.IntSlider(position, sphereSmoothing, 12, 250);

                break;

            case ShapeController.shapeType.Prism:
                GUILayout.Space(20);

                GUILayout.Label("prismRadius", EditorStyles.boldLabel);
                position = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
                prismRadius = EditorGUI.Slider(position, prismRadius, 0.1f, 10f);

                GUILayout.Space(10);

                GUILayout.Label("prismHeight", EditorStyles.boldLabel);
                position = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
                prismHeight = EditorGUI.Slider(position, prismHeight, 0.1f, 10f);

                GUILayout.Space(10);

                GUILayout.Label("prismSideCount", EditorStyles.boldLabel);
                position = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
                prismSideCount = EditorGUI.IntSlider(position, prismSideCount, 3, 256);
                break;

            case ShapeController.shapeType.Capsule:
                GUILayout.Space(20);

                GUILayout.Label("capsuleRadius", EditorStyles.boldLabel);
                position = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
                capsuleRadius = EditorGUI.Slider(position, capsuleRadius, 0.1f, 10f);

                GUILayout.Space(10);

                GUILayout.Label("capsuleHeight", EditorStyles.boldLabel);
                position = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
                capsuleHeight = EditorGUI.Slider(position, capsuleHeight, capsuleRadius * 2, capsuleRadius * 20);

                GUILayout.Space(10);

                GUILayout.Label("capsuleSmoothing", EditorStyles.boldLabel);
                position = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
                capsuleSmoothing = EditorGUI.IntSlider(position, capsuleSmoothing, 12, 256);
                break;

            default:
                throw new ArgumentException($"Unknown shape type: {currentShape}");

        }

        GUILayout.Space(10);
        shapeColor = EditorGUILayout.ColorField(shapeColor);
        GUILayout.Space(10);

        if (GUILayout.Button("Create"))
        {
            switch (currentShape)
            {
                case ShapeController.shapeType.Parallelepiped:
                    parameters = new Dictionary<ShapeController.shapeParameters, object>
                                {
                                    { ShapeController.shapeParameters.Width , parallelepipedDimensions.x },
                                    { ShapeController.shapeParameters.Height, parallelepipedDimensions.y },
                                    { ShapeController.shapeParameters.Depth, parallelepipedDimensions.z }
                                };
                    break;
                case ShapeController.shapeType.Sphere:
                    parameters = new Dictionary<ShapeController.shapeParameters, object>
                                {
                                    { ShapeController.shapeParameters.Radius , sphereRadius },
                                    { ShapeController.shapeParameters.Smoothing, sphereSmoothing }
                                };
                    break;
                case ShapeController.shapeType.Prism:
                    parameters = new Dictionary<ShapeController.shapeParameters, object>
                                {
                                    { ShapeController.shapeParameters.Radius , prismRadius },
                                    { ShapeController.shapeParameters.Height, prismHeight },
                                    { ShapeController.shapeParameters.SideCount, prismSideCount }
                                };
                    break;
                case ShapeController.shapeType.Capsule:
                    parameters = new Dictionary<ShapeController.shapeParameters, object>
                    {
                        { ShapeController.shapeParameters.Radius , capsuleRadius },
                        { ShapeController.shapeParameters.Height , capsuleHeight },
                        { ShapeController.shapeParameters.Smoothing , capsuleSmoothing }
                    };
                    break;
                default:
                    throw new ArgumentException($"Unknown shape type: {currentShape}");
            }

            parameters.Add(ShapeController.shapeParameters.Color, shapeColor);

            GameObject newShape = ShapeFactory.CreateShape(currentShape, parameters);
            Selection.activeGameObject = newShape;
            newShape.transform.position = SceneViewRayHelper.GetWorldPositionFromSceneView();
        }

        if (GUILayout.Button("Delete"))
        {
                foreach( GameObject obj in Selection.gameObjects)
                {
                    if(obj.TryGetComponent<ShapeMesh>(out ShapeMesh shapeMesh))
                    {
                        DestroyImmediate(obj);
                    }
                }
        }
    }
}
