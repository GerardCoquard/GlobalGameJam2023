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

    public GameObject pointerPrefab;
    GameObject currentPointer;
    private void Update() {
        if(InputManager.GetAction("Fire").WasPressedThisFrame())Grow();
        if(InputManager.GetAction("Fire").WasReleasedThisFrame())StopGrow();
        if(InputManager.GetAction("Aim").WasPressedThisFrame())Reset();
        if (InputManager.GetAction("Interact").WasPressedThisFrame()) CheckIfControllerChanged();
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
    void Reset()
    {
        controller.Reset();
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
