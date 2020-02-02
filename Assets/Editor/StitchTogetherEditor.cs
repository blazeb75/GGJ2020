using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(StitchTogether))]
public class StitchTogetherEditor : Editor
{
    private void OnSceneGUI()
    {
        DrawDefaultInspector();
        StitchTogether obj = (StitchTogether)target;

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
                Undo.RecordObject(obj, "Moved StitchTogether target point");
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
