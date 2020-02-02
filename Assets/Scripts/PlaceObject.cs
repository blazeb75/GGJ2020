﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlaceObject : MonoBehaviour
{
    public UnityEvent OnPlaced;
    public enum State {idle, held, placed}
    public State state;
    public GameObject other;

    private Collider col;
    private Collider otherCol;
    private Vector3 defaultPos;
    private Quaternion defaultRot;
    private void Awake()
    {
        col = GetComponent<Collider>() ?? gameObject.AddComponent<MeshCollider>();
        otherCol = other.GetComponent<Collider>();
        defaultPos = transform.position;
        defaultRot = transform.rotation;
    }

    private void Update()
    {
        if(IsClicked(0))
        {
            if (state == State.idle)
            {
                state = State.held;
            }
        }
        if (IsClicked(1))
        {
            if(state == State.held)
            {
                state = State.idle;
                GoToDefault();
            }
        }

        if(state == State.held)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray: ray, hitInfo: out RaycastHit hit, maxDistance: Mathf.Infinity, ~0, queryTriggerInteraction: QueryTriggerInteraction.Collide);
            if (hit.collider != null)
            {
                transform.position = hit.point;
                transform.rotation = Quaternion.Euler(hit.normal);
            }
            else
            {
                GoToDefault();
            }

            otherCol.Raycast(ray, out hit, Mathf.Infinity);
            if (Input.GetMouseButtonDown(0) && hit.collider.gameObject == other)
            {
                state = State.placed;
                OnPlaced.Invoke();
            }
        }
    }

    void GoToDefault()
    {
        transform.position = defaultPos;
        transform.rotation = defaultRot;
    }

    private bool IsClicked(int button)
    {
        if (Input.GetMouseButtonDown(button))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (col.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                return true;
            }

        }
        return false;
    }
}
