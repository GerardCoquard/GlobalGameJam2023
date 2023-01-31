using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlantMode : MonoBehaviour
{
    public RootController controller;
    public float distance;
    public LayerMask layerMask;
    public Transform cam;
    bool growing;
    private void Update() {
        if(InputManager.GetAction("Fire").WasPressedThisFrame())Grow();
        if(InputManager.GetAction("Fire").WasReleasedThisFrame())StopGrow();
    }
    void Grow()
    {
        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position,cam.transform.forward,out hit, Mathf.Infinity,layerMask)) controller.StrartGrowing(hit.point);
    }
    void StopGrow()
    {
        
        controller.StopGrow();
    }
}
