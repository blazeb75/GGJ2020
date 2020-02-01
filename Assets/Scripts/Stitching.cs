using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stitching : MonoBehaviour
{
    public GameObject targetDecal;
    public GameObject stitch;
    public GameObject[] targets;
    public Vector3[] points;
    [HideInInspector] public Vector3[] normals;

    public int currentTarget = 0;

    private Vector3 previousClick;

    //private Mesh[] targetMeshes;
    //private Collider[] targetColliders;

    // Start is called before the first frame update
    void Start()
    {
        targetDecal = Instantiate(targetDecal, this.transform);
        //targetMeshes = new Mesh[targets.Length];
        //targetColliders = new Collider[targets.Length];
        ////for(int i = 0; i < targets.Length; i++)
        //{
        //    targetMeshes[i] = targets[i].GetComponent<MeshFilter>().mesh;
        //    targetColliders[i] = targets[i].GetComponent<Collider>();
        //}
        
    }

    void PlaceTargetDecal()
    {
        targetDecal.transform.position = points[currentTarget];
        targetDecal.transform.LookAt(Camera.main.transform.forward);
    }

    void TargetHit(Vector3 clickPos)
    {
        //Place stitch
        if(currentTarget != 0)
        {
            //Position & rotation
            Vector3 stitchPos = (clickPos + previousClick) / 2f;
            GameObject newStitch = Instantiate(stitch, stitchPos, Quaternion.Euler(-normals[currentTarget]));
            newStitch.transform.right = previousClick - clickPos;

            //Scale TODO
        }

        previousClick = clickPos;
        currentTarget++;
        PlaceTargetDecal();
    }

    private void OnMouseDown()
    {
        //This code needs to be on the target decal TODO
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            TargetHit(hit.point);
        }
    }
}
