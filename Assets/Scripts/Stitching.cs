using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stitching : MonoBehaviour
{
    public GameObject[] targets;
    public GameObject targetDecal;
    public Vector3[] points;

    public int currentTarget = 0;

    //private Mesh[] targetMeshes;
    //private Collider[] targetColliders;

    // Start is called before the first frame update
    void Start()
    {
        targetDecal = Instantiate(targetDecal);
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
    }
}
