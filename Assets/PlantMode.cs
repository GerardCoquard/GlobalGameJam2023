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

    public float interactPlantDistance;
    public LayerMask interactPlantLayerMask;
    public LayerMask pointerLayerMask;
    public GameObject pointerPrefab;

    public Material currentPointerMaterial;

    private bool planting;
    private void Update() {
        if(InputManager.GetAction("Fire").WasPressedThisFrame())Grow();
        if(InputManager.GetAction("Fire").WasReleasedThisFrame())StopGrow();
        if(InputManager.GetAction("Aim").WasPressedThisFrame())Reset();
        if (InputManager.GetAction("Interact").WasPressedThisFrame()) CheckIfControllerChanged();

        if (planting)
        {
            UpdatePointerState();
        }
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
    void Reset()
    {
        controller.Reset();
    }

    void UpdatePointerState()
    {
        RaycastHit hit;
        if(InputManager.GetAction("Fire").WasPressedThisFrame()) pointerPrefab.GetComponent<MeshRenderer>().material.color = Color.white;
        if (InputManager.GetAction("Fire").WasReleasedThisFrame())
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity, pointerLayerMask))
            {
                if (hit.collider != null)
                {
                    pointerPrefab.transform.position = hit.point;
                }

                if (hit.collider.tag == "Ground") pointerPrefab.GetComponent<MeshRenderer>().material.color = Color.green;
                else pointerPrefab.GetComponent<MeshRenderer>().material.color = Color.red;
            }
        }
       

    }
    void CheckIfControllerChanged()
    {
        Ray ray = new Ray(cam.transform.position,cam.transform.forward);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, interactPlantDistance, interactPlantLayerMask))
        {
            SetNewController(hit.collider.gameObject.GetComponentInParent<RootController>());
            planting = true;
            pointerPrefab.SetActive(true);
        }
    }

    void SetNewController(RootController newController)
    {
        controller = newController;
    }
}
