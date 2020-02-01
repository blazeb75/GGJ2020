using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class StitchTogether : MonoBehaviour
{
    public UnityEvent OnComplete;
    public GameObject targetDecal;
    public float parentPointOffset = 0.1f;
    public GameObject stitch;
    public GameObject other;
    public Vector3[] points;
    [HideInInspector] public Vector3[] normals;

    public int currentTarget = 0;


    private List<Vector3> allPoints = new List<Vector3>();
    private List<Vector3> allNormals = new List<Vector3>();
    private Collider col;
    private Collider otherCol;
    private Collider targetCol;

    private Vector3 previousClick;

    // Start is called before the first frame update
    void Start()
    {
        targetDecal = Instantiate(targetDecal, this.transform);
        PlaceTargetDecal();
        col = GetComponent<Collider>();
        otherCol = other.GetComponent<Collider>();
        targetCol = targetDecal.GetComponent<Collider>();

        for (int i = 0; i < points.Length; i++)
        {
            Vector3 point = points[i];
            Vector3 normal = normals[i];
            allPoints.Add(point);
            allNormals.Add(normal);

            Vector3 newPoint = point + normal * parentPointOffset;
            Vector3 newNearest = otherCol.ClosestPoint(newPoint);

            Physics.Raycast(new Ray(newPoint, newNearest - newPoint), out RaycastHit hit, Mathf.Infinity, int.MaxValue, QueryTriggerInteraction.Ignore);

            allPoints.Add(newNearest);
            allNormals.Add(hit.normal);
        }
    }

    private void Update()
    {
        if (IsTargetClicked(out Vector3 point))
        {
            TargetHit(point);
        }
    }

    void PlaceTargetDecal()
    {
        targetDecal.transform.position = allPoints[currentTarget];
        targetDecal.transform.LookAt(-allNormals[currentTarget]);
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
            //This code needs to be on the target decal TODO
            RaycastHit targetHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out targetHit) && targetHit.collider == targetCol)
            {
                if (Physics.Raycast(ray: ray, hitInfo: out targetHit, maxDistance: Mathf.Infinity, layerMask: int.MaxValue, queryTriggerInteraction: QueryTriggerInteraction.Ignore) && (this.gameObject == targetHit.collider.gameObject || other == targetHit.collider.gameObject))
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
    }
}
