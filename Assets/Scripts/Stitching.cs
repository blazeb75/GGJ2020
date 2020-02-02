using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class Stitching : MonoBehaviour
{
    public LayerMask lm;
    public UnityEvent OnComplete;
    public GameObject targetDecal;
    public GameObject stitch;
    public GameObject[] others;
    public Vector3[] points;
    [HideInInspector] public Vector3[] normals;

    public int currentTarget = 0;

    private Vector3 previousClick;
    private Collider targetCol;

    //private Mesh[] targetMeshes;
    //private Collider[] targetColliders;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log((int)lm);
        targetDecal = Instantiate(targetDecal, this.transform);
        PlaceTargetDecal();
        targetCol = targetDecal.GetComponent<Collider>();
        //targetMeshes = new Mesh[targets.Length];
        //targetColliders = new Collider[targets.Length];
        ////for(int i = 0; i < targets.Length; i++)
        //{
        //    targetMeshes[i] = targets[i].GetComponent<MeshFilter>().mesh;
        //    targetColliders[i] = targets[i].GetComponent<Collider>();
        //}
        
    }

    private void Update()
    {
        if(IsTargetClicked(out Vector3 point))
        {
            TargetHit(point);
        }
    }

    void PlaceTargetDecal()
    {
        targetDecal.transform.position = points[currentTarget];
        targetDecal.transform.forward = -normals[currentTarget];
        targetDecal.GetComponent<DecalSystem.Decal>().BuildAndSetDirty();
    }

    void TargetHit(Vector3 clickPos)
    {
        //Place stitch
        if (currentTarget != 0)
        {
            //Position & rotation
            Vector3 stitchPos = (clickPos + previousClick) / 2f;
            GameObject newStitch = Instantiate(stitch, stitchPos, Quaternion.Euler(-normals[currentTarget]));
            newStitch.transform.right = previousClick - clickPos;

            //Scale
            newStitch.transform.localScale = new Vector3(
                Vector3.Distance(clickPos, previousClick),
                newStitch.transform.localScale.y,
                newStitch.transform.localScale.z);
        }

        if (currentTarget == points.Length - 1)
        {
            Complete();
        }
        else
        {
            previousClick = clickPos;
            currentTarget++;
            PlaceTargetDecal();
        }
    }

    private bool IsTargetClicked(out Vector3 point)
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit targetHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.green, 1000);
            if (Physics.Raycast(ray: ray, hitInfo: out targetHit, maxDistance: Mathf.Infinity, layerMask: lm, queryTriggerInteraction: QueryTriggerInteraction.Collide) 
                && targetHit.collider.gameObject == targetDecal)
            {
                Debug.Log("hit");
                if (Physics.Raycast(ray: ray, hitInfo: out targetHit, maxDistance: Mathf.Infinity, layerMask: lm, queryTriggerInteraction: QueryTriggerInteraction.Ignore)
                    && others.Contains(targetHit.collider.gameObject))
                {
                    point = targetHit.point;
                    return true;
                }
            }

        }
        point = Vector3.zero;
        return false;
    }

    void Complete()
    {
        targetDecal.SetActive(false);
        OnComplete.Invoke();
        this.enabled = false;
    }
}
