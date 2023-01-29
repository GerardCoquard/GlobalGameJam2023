using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputUnityEvent : UnityEvent<InputAction.CallbackContext>
{
    //Base event class of action events
    public string actionName; //Action name
    bool _enabled; //If this event can be triggered

    //Specific events
    public UnityEvent startedAction; 
    public UnityEvent performedAction;
    public UnityEvent canceledAction;
    public InputUnityEvent(InputAction _action)
    {
        //Constructor of the class
        //Sets Action name
        //Sets Action enabled
        //Links action event to this event
        //Links specific action events to this specific events (started, performed, canceled)
        actionName = _action.name;
        _enabled = true;
        startedAction = new UnityEvent();
        performedAction = new UnityEvent();
        canceledAction = new UnityEvent();
        _action.started += InvokeEvent;
        _action.performed += InvokeEvent;
        _action.canceled += InvokeEvent;
    }
    void InvokeEvent(InputAction.CallbackContext context)
    {
        //Invokes of this event and his specifics if input is enabled
        if(!_enabled) return;

        this?.Invoke(context);
        if(context.started) startedAction?.Invoke();
        if(context.performed) performedAction?.Invoke();
        if(context.canceled) canceledAction?.Invoke();
    }
    public void SetEnabled(bool enable)
    {
        //Sets if this event is enabled or not
        _enabled = enable;
    }
    public void ClearListeners()
    {
        //Clears all listeners of this event and his specifics
        this.RemoveAllListeners();
        startedAction.RemoveAllListeners();
        performedAction.RemoveAllListeners();
        canceledAction.RemoveAllListeners();
    }
}
