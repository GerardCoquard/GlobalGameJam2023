using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlantMode : MonoBehaviour
{
    public RootController controller;
    public float distance;
    public Transform cam;

    public float interactPlantDistance;
    public LayerMask interactPlantLayerMask;
    public LayerMask pointerLayerMask;
    public GameObject pointerPrefab;

    private bool planting;
    private bool growing;
    private bool shouldGrow;
    private void Update()
    {

        if (InputManager.GetAction("Interact").WasPressedThisFrame()) CheckIfControllerChanged();

        if (controller == null) return;

        if (InputManager.GetAction("Fire").WasPressedThisFrame() && shouldGrow) Grow();
        if (InputManager.GetAction("Fire").WasReleasedThisFrame()) StopGrow();
        if (InputManager.GetAction("Aim").WasPressedThisFrame()) Decrease();
        if (InputManager.GetAction("Aim").WasReleasedThisFrame()) StopDecrease();


        if (planting) UpdatePointerState();
    }
    void Grow()
    {
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity, pointerLayerMask)) controller.StrartGrowing(hit.point);

    }

    void StopGrow()
    {
        controller.StopGrow();
    }
    void Decrease()
    {
        controller.StartDecreasing();
    }
    void StopDecrease()
    {
        controller.StopDecreasing();
    }

    void UpdatePointerState()
    {
        RaycastHit hit;
        if (InputManager.GetAction("Fire").WasPerformedThisFrame())
        {
            pointerPrefab.GetComponent<MeshRenderer>().material.color = Color.white;
            growing = true;

        }
        if (InputManager.GetAction("Fire").WasReleasedThisFrame()) growing = false;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity, pointerLayerMask) && !growing)
        {
            if (hit.collider != null)
            {
                pointerPrefab.transform.position = hit.point;
            }

            if (hit.collider.tag == "Ground")
            {
                pointerPrefab.GetComponent<MeshRenderer>().material.color = Color.green;
                shouldGrow = true;
            }

            else
            {
                pointerPrefab.GetComponent<MeshRenderer>().material.color = Color.red;
                shouldGrow = false;
            }
        }


    }
    void CheckIfControllerChanged()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactPlantDistance, interactPlantLayerMask))
        {
            controller = hit.collider.gameObject.GetComponentInParent<RootController>();
            planting = true;
            pointerPrefab.SetActive(true);
        }
        else
        {
            controller = null;
            planting = false;
            pointerPrefab.SetActive(false);
        }
    }
}