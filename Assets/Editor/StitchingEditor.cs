using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Stitching))]
public class StitchingEditor : Editor
{
    protected virtual void OnSceneGUI()
    {
        Stitching obj = (Stitching)target;

        if (obj.points.Length == 0)
        {
            obj.points = new Vector3[] { obj.transform.point };
        }

        for (int i = 0; i < obj.points.Length; i++)
        {
            Vector3 point = obj.points[i];
            EditorGUI.BeginChangeCheck();
            Vector3 newTargetPos = Handles.pointHandle(point, Quaternion.identity);
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
}