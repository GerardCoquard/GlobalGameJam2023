using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlantMode : MonoBehaviour
{
    public RootController controller;
    public float distance;
    public Transform cam;
    public LayerMask interactPlantLayerMask;
    public LayerMask pointerLayerMask;
    public RootDesiredPoint scenePointer;
    public Sprite defaultCursor;
    public Sprite canPlantCursor;
    public delegate void CursorChanged(Sprite image);
    public static event CursorChanged OnCursorChanged;
    Sprite currentCursor;
    private void Start() {
        currentCursor = SetCursor(defaultCursor);
        scenePointer.transform.SetParent(null);
        scenePointer.DespawnPointer();
    }
    private void Update()
    {
        if (InputManager.GetAction("Interact").WasPressedThisFrame()) CheckIfControllerChanged();

        if (controller == null) return;
        
        if (InputManager.GetAction("Aim").WasPressedThisFrame()) Decrease();
        if (InputManager.GetAction("Aim").WasReleasedThisFrame()) StopDecrease();

        if(controller.FullyGrown())
        {
            currentCursor = SetCursor(defaultCursor);
            scenePointer.DespawnPointer();
            return;
        }

        if (InputManager.GetAction("Fire").WasPressedThisFrame()) Grow();
        if (InputManager.GetAction("Fire").WasReleasedThisFrame()) StopGrow();

        currentCursor = UpdatePointerState();
    }
    void Grow()
    {
        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, distance, pointerLayerMask))
        {
            if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground") && !controller.growing && !controller.decreasing)
            {
                controller.StrartGrowing(hit.point);
                scenePointer.SpawnPointer(hit.point);
            }
        } 
    }

    void StopGrow()
    {
        if(controller.growing)
        {
            controller.StopGrow();
            scenePointer.DespawnPointer();
        }
    }
    void Decrease()
    {
        controller.StartDecreasing();
    }
    void StopDecrease()
    {
        controller.StopDecreasing();
    }
    Sprite SetCursor(Sprite cursor)
    {
        if(cursor!=currentCursor) OnCursorChanged?.Invoke(cursor);
        return cursor;
    }
    Sprite UpdatePointerState()
    {
        if(controller.growing || controller.decreasing)
        {
            return SetCursor(defaultCursor);
        }
        else
        {
            RaycastHit hit;
            Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, distance, pointerLayerMask);
            if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, distance, pointerLayerMask))
            {
                if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground")) return SetCursor(canPlantCursor);
                else return SetCursor(defaultCursor);
            }
            else
            {
                return SetCursor(defaultCursor);
            }
        }
    }
    void CheckIfControllerChanged()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance, interactPlantLayerMask))
        {
            RootController newController = hit.transform.GetComponentInParent<RootController>();
            if(newController != controller && controller != null)
            {
                controller.StopGrow();
                controller.StopDecreasing();
                scenePointer.DespawnPointer();
            }
            controller = newController;
        }
        else
        {
            if(controller != null)
            {
                controller.StopGrow();
                controller.StopDecreasing();
                scenePointer.DespawnPointer();
            }
            controller = null;
        }
    }
}