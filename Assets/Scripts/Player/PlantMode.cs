using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantMode : MonoBehaviour
{
    [SerializeField] float distance;
    [SerializeField] float collectableDistance;
    [SerializeField] Transform cam;
    [SerializeField] LayerMask interactPlantLayerMask;
    [SerializeField] LayerMask pointerLayerMask;
    [SerializeField] LayerMask collectLayerMask;
    [SerializeField] RootDesiredPoint scenePointer;
    [SerializeField] Sprite defaultCursor;
    [SerializeField] Sprite canPlantCursor;
    [SerializeField] Sprite canPlantCursorNotRanged;
    [SerializeField] Sprite notPlantCursor;
    [SerializeField] Sprite notPlantCursorNotRanged;
    [SerializeField] Sprite lookingPlantCursor;

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

        if (controller == null)
        {
            RaycastHit hit;
            if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit,Mathf.Infinity))
            {
                if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Root") && Vector3.Distance(transform.position,hit.point)<distance) currentCursor = SetCursor(lookingPlantCursor);
                else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Collectable") && Vector3.Distance(transform.position, hit.point) < collectableDistance) currentCursor = SetCursor(lookingPlantCursor);
                else currentCursor = SetCursor(defaultCursor);
            }
            return;
        }
        
        if (InputManager.GetAction("Aim").WasPressedThisFrame()) Decrease();
        if (InputManager.GetAction("Aim").WasReleasedThisFrame()) StopDecrease();

        if(controller.FullyGrown())
        {
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
                AudioManager.instance.PlaySound("RaizesCreciendo", 0.5f, true);
                controller.StrartGrowing(hit.point);
                scenePointer.SpawnPointer(hit.point);
            }
        } 
    }

    void StopGrow()
    {

        AudioManager.instance.StopSoundLoop("RaizesCreciendo");
        if (controller.GetGrowing())
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
            if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit,Mathf.Infinity))
            {
                if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Root") && Vector3.Distance(transform.position,hit.point)<distance) return SetCursor(lookingPlantCursor);
                if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Collectable") && Vector3.Distance(transform.position, hit.point) < collectableDistance)return currentCursor = SetCursor(lookingPlantCursor);
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    if(Vector3.Distance(transform.position,hit.point) < distance) return SetCursor(canPlantCursor);
                    else return SetCursor(canPlantCursorNotRanged);
                }
                else if(Vector3.Distance(transform.position,hit.point) < distance) return SetCursor(notPlantCursor);
                else return SetCursor(notPlantCursorNotRanged);
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
        if(Physics.Raycast(ray, out hit, distance, collectLayerMask))
        {
            if (InputManager.GetAction("Interact").WasPressedThisFrame())
            {
                hit.collider.GetComponent<Items>().PickItem();
            }
        }
        if (Physics.Raycast(ray, out hit, distance, interactPlantLayerMask))
        {
            RootController newController = hit.transform.GetComponentInParent<RootController>();
            if(controller==null)
            {
                controller = newController;
                controller.picked = true;
                fillRoot.ChangeState(true);
                return;
            }
            else if(newController != controller)
            {
                controller.StopGrow();
                controller.StopDecreasing();
                controller.SetUnselected();
                scenePointer.DespawnPointer();
                controller = newController;
                controller.picked = true;
                fillRoot.ChangeState(true);
            }       
        }
        else
        {
            if (controller != null)
            {
                controller.StopGrow();
                controller.StopDecreasing();
                controller.SetUnselected();
                scenePointer.DespawnPointer();
                fillRoot.ChangeState(false);
                controller = null;
            }
        }
    }
    Sprite SetCursor(Sprite cursor)
    {
        if(cursor!=currentCursor) OnCursorChanged?.Invoke(cursor);
        return cursor;
    }
}


