using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StitchTogetherEditor : Editor
{
    void OnSceneGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Contract to mesh"))
        {
            StitchTogether obj = (StitchTogether)target;
            Undo.RecordObject(obj, "Contracted StitchTogether points to target mesh");
            for (int i = 0; i < obj.points.Length; i++)
            {
                Vector3 point = obj.points[i];
                Collider col = obj.GetComponent<Collider>();
                Vector3 newPoint = col.ClosestPoint(point);
                Physics.Raycast(new Ray(point, newPoint - point), out RaycastHit hit, Mathf.Infinity, int.MaxValue, QueryTriggerInteraction.Ignore);
                point = newPoint;
                obj.normals[i] = hit.normal;

            }
        }
    }
}
