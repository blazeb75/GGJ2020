using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPoints : MonoBehaviour
{
    public GameObject[] points;

    public KeyCode nextKey;
    public KeyCode prevKey;

    private DragMouseOrbit dmo;
    private int currentTarget = 0;

    // Start is called before the first frame update
    void Start()
    {
        dmo = GetComponent<DragMouseOrbit>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(nextKey))
        {
            currentTarget++;
            if(currentTarget == points.Length)
            {
                currentTarget = 0;
            }
            dmo.target = points[currentTarget].transform;
        }
        if (Input.GetKeyDown(prevKey))
        {
            currentTarget--;
            if(currentTarget < 0)
            {
                currentTarget = points.Length - 1;
            }
            dmo.target = points[currentTarget].transform;
        }
    }
}
