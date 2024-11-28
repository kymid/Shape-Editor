using UnityEditor;
using UnityEngine;

public static class SceneViewRayHelper
{

    public static Vector3 GetWorldPositionFromSceneView(float distance = 20f)
    {

        if (SceneView.lastActiveSceneView == null)
        {
            Debug.LogError("Scene View не активно!");
            return Vector3.zero;
        }

        Camera sceneCamera = SceneView.lastActiveSceneView.camera;

        if (sceneCamera == null)
        {
            Debug.LogError("Не удалось найти камеру Scene View.");
            return Vector3.zero;
        }

        Ray ray = new Ray(sceneCamera.transform.position, sceneCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, distance))
            return hit.point;


        return ray.GetPoint(distance);
    }
}
