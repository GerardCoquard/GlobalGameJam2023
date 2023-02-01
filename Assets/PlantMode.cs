using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlantMode : MonoBehaviour
{
    RootController controller;
    public float distance;
    public LayerMask layerMask;
    public Transform cam;
    public float interactPlantDistance;
    public LayerMask interactPlantLayerMask;

    public GameObject pointerPrefab;
    GameObject currentPointer;
    private void LateUpdate() {
        if (InputManager.GetAction("Interact").WasPressedThisFrame()) CheckIfControllerChanged();

        if(controller == null) return;

        if(InputManager.GetAction("Fire").WasPressedThisFrame())Grow();
        if(InputManager.GetAction("Fire").WasReleasedThisFrame())StopGrow();
        if(InputManager.GetAction("Aim").WasPressedThisFrame())Decrease();
        if(InputManager.GetAction("Aim").WasReleasedThisFrame())StopDecrease();
    }
    void Grow()
    {
        RaycastHit hit;
        
        if(Physics.Raycast(cam.transform.position,cam.transform.forward,out hit, Mathf.Infinity,layerMask)) controller.StrartGrowing(hit.point);
        
        if(currentPointer != null)
        {
            Destroy(currentPointer.gameObject);
            currentPointer = null;
        }
        GameObject pointer = Instantiate(pointerPrefab, hit.point, Quaternion.identity);
        currentPointer = pointer;

        
    }
    void StopGrow()
    {
        controller.StopGrow();
    }
    void Decrease()
    {
        controller.StartDecreasing();
        //controller.Reset();
    }
    void StopDecrease()
    {
        controller.StopDecreasing();
        //controller.Reset();
    }

    void UpdatePointerState()
    {
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity, layerMask)) controller.StrartGrowing(hit.point);
    }
    void CheckIfControllerChanged()
    {
        Ray ray = new Ray(cam.transform.position,cam.transform.forward);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, interactPlantDistance, interactPlantLayerMask))
        {
            SetNewController(hit.collider.gameObject.GetComponentInParent<RootController>());
        }
    }

    void SetNewController(RootController newController)
    {
        controller = newController;
    }
}
