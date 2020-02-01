using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Stitching))]
public class StitchingEditor : Editor
{
    private void OnSceneGUI()
    {
        DrawDefaultInspector();
        Stitching obj = (Stitching)target;

        if (obj.points.Length == 0)
        {
            obj.points = new Vector3[] { obj.transform.position };
        }

        for (int i = 0; i < obj.points.Length; i++)
        {
            Vector3 point = obj.points[i];
            EditorGUI.BeginChangeCheck();
            Vector3 newTargetPos = Handles.PositionHandle(point, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(obj, "Moved Stitching Target point");
                obj.points[i] = newTargetPos;
            }
            if (i != obj.points.Length - 1)
            {
                Handles.DrawLine(obj.points[i], obj.points[i + 1]);
            }
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Stitching obj = (Stitching)target;

        if (GUILayout.Button("Contract to mesh"))
        {
            Undo.RecordObject(obj, "Contracted Stitching points to target mesh");
            Collider[] targetColliders = new Collider[targets.Length];
            obj.normals = new Vector3[obj.points.Length];
            for (int i = 0; i < targets.Length; i++)
            {
                targetColliders[i] = obj.others[i].GetComponent<Collider>();
            }
            for (int i = 0; i < obj.points.Length; i++) {
                Vector3 point = obj.points[i];
                float dist = Mathf.Infinity;
                foreach (Collider col in targetColliders)
                {
                    Vector3 newPoint = col.ClosestPoint(point);
                    float newDist = Vector3.Distance(newPoint, point);
                    Debug.Log("yeet");
                    if (newDist < dist)
                    {
                        Physics.Raycast(new Ray(point, newPoint - point), out RaycastHit hit, Mathf.Infinity, int.MaxValue, QueryTriggerInteraction.Ignore);
                        obj.points[i] = newPoint;
                        obj.normals[i] = hit.normal;
                        dist = newDist;
                        Debug.Log("yote");
                    }
                }
            }
        }
    }
}