using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantMode : MonoBehaviour
{
    [SerializeField] float distance;
    [SerializeField] Transform cam;
    [SerializeField] LayerMask interactPlantLayerMask;
    [SerializeField] LayerMask pointerLayerMask;
    [SerializeField] RootDesiredPoint scenePointer;
    [SerializeField] Sprite defaultCursor;
    [SerializeField] Sprite plantCursor;

    public delegate void CursorChanged(Sprite image);
    public static event CursorChanged OnCursorChanged;
    RootController controller;
    Sprite currentCursor;
    RootSlider fillRoot;

    private void Start() {
        currentCursor = SetCursor(defaultCursor);
        scenePointer.transform.SetParent(null);
        scenePointer.DespawnPointer();
        fillRoot = FindObjectOfType<RootSlider>();
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
        if(!controller.GetGrowing()) scenePointer.DespawnPointer();
        fillRoot.CalculateFillInPercent(controller.GetCurrentDistance() + controller.GetDistance(), controller.GetMaxDistance());
    }
    void Grow()
    {
        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, distance, pointerLayerMask))
        {
            if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground") && !controller.GetGrowing() && !controller.GetDecreasing())
            {
               
                controller.StrartGrowing(hit.point);
                scenePointer.SpawnPointer(hit.point);
            }
        } 
    }

    void StopGrow()
    {
        if(controller.GetGrowing())
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
    Sprite UpdatePointerState()
    {
        if(controller.GetGrowing() || controller.GetDecreasing())
        {
            return SetCursor(defaultCursor);
        }
        else
        {
            RaycastHit hit;
            Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, distance, pointerLayerMask);
            if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, distance, pointerLayerMask))
            {
                if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground")) return SetCursor(plantCursor);
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
            fillRoot.ChangeState(true);
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
            fillRoot.ChangeState(false);
            if (controller != null)
            {
                controller.StopGrow();
                controller.StopDecreasing();
                scenePointer.DespawnPointer();
            }
            controller = null;
        }
    }
    Sprite SetCursor(Sprite cursor)
    {
        if(cursor!=currentCursor) OnCursorChanged?.Invoke(cursor);
        return cursor;
    }
}


