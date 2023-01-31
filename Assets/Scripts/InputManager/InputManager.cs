using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.SceneManagement;
public static class InputManager
{
    static List<InputUnityEvent> events = new List<InputUnityEvent>(); //List of all action events
    public static PlayerInput playerInput; //Current PlayerInput
    public static EventSystem eventSystem; //Current EventSystem
    static string path; //Path of the InputManager prefab
    static InputManager()
    {
        //Constructor called only one time when a static method of this class is called
        path = "InputManager"; //Modify path how you want, but remember the root file is Resources
        //Checks if prefab is there
        if(Resources.Load(path) == null)
        {
            Debug.LogWarning("Prefab not found. Your input prefab have to be at " + path);
            return;
        }
        //Instantiates InputManager prefab, sets the current EventSystem and PlayerInput, and creates all events for each action it has
        GameObject sceneInput = CreateInputOnScene();
        MonoBehaviour.DontDestroyOnLoad(sceneInput);
        playerInput = sceneInput.GetComponent<PlayerInput>();
        eventSystem = sceneInput.GetComponent<EventSystem>();
        CreateEvents(playerInput);
        //Subscribe to scene changes and device changes
        SceneManager.activeSceneChanged += ClearListeners;
        InputUser.onChange += OnInputDeviceChange;
    }
    public static void ClearListeners(Scene a,Scene b)
    {
        //Removes all subscribed listeners of all events
        if(a.buildIndex == -1) return;
        foreach (InputUnityEvent _event in events)
        {
            _event.ClearListeners();
        }
    }
    public static void OnInputDeviceChange(InputUser user, InputUserChange change, InputDevice device)
    {
        //Do stuff when device changed
    }
    public static void CreateEvents(PlayerInput playerInput)
    {
        //Creates one event for each action in PlayerInput
        events = new List<InputUnityEvent>();

        foreach (InputActionMap actionMap in playerInput.actions.actionMaps)
        {
            foreach (InputAction act in actionMap)
            {
                InputUnityEvent newActionEvent = new InputUnityEvent(act);
                events.Add(newActionEvent);
            }
        }
    }
    static GameObject CreateInputOnScene()
    {
        //Creates the InputManager GameObject on Scene, wich works alongside with InputManager(this)
        return MonoBehaviour.Instantiate(Resources.Load(path) as GameObject,Vector3.zero,Quaternion.identity);
    }
    public static InputAction GetAction(string _actionName)
    {
        //Subscribe to event of action named _actionName with _method
        return GetEvent(_actionName).GetAction();
    }
    public static void AddInputAction(string _actionName, UnityEngine.Events.UnityAction<InputAction.CallbackContext> _method)
    {
        //Subscribe to event of action named _actionName with _method
        GetEvent(_actionName)?.AddListener(_method);
    }
    public static void AddInputAction(string _actionName,InputType _type, UnityEngine.Events.UnityAction _method)
    {
        //Subscribe to specific interaction event of action named _actionName with _method
        switch (_type)
        {
            case InputType.Started:
                GetEvent(_actionName)?.startedAction.AddListener(_method);
            break;

            case InputType.Performed:
                GetEvent(_actionName)?.performedAction.AddListener(_method);
            break;

            case InputType.Canceled:
                GetEvent(_actionName)?.canceledAction.AddListener(_method);
            break;
        }
    }
    public static void RemoveInputAction(string _actionName, UnityEngine.Events.UnityAction<InputAction.CallbackContext> _method)
    {
        //Unsubscribe to event of action named _actionName with _method
        GetEvent(_actionName)?.RemoveListener(_method);
    }
    public static void RemoveInputAction(string _actionName,InputType _type, UnityEngine.Events.UnityAction _method)
    {
        //Unsubscribe to specific interaction event of action named _actionName with _method
        switch (_type)
        {
            case InputType.Started:
                GetEvent(_actionName)?.startedAction.RemoveListener(_method);
            break;

            case InputType.Performed:
                GetEvent(_actionName)?.performedAction.RemoveListener(_method);
            break;

            case InputType.Canceled:
                GetEvent(_actionName)?.canceledAction.RemoveListener(_method);
            break;
        }
    }

    public static void ActionEnabled(string _actionName,bool _enabled)
    {
        //Sets event enabled of action named _ActionName to _enabled
        GetEvent(_actionName)?.SetEnabled(_enabled);
    }
    public static void ActionEnabled(string[] _actionNames,bool _enabled)
    {
        //Sets events enabled of actions named _ActionName to _enabled
        foreach (string _actionName in _actionNames)
        {
            ActionEnabled(_actionName,_enabled);
        }
    }
    public static void ChangeActionMap(string _actionMapName)
    {
        //Change PlayerInput Action Map to one named _actionMapName
        playerInput.SwitchCurrentActionMap(_actionMapName);
    }
    static InputUnityEvent GetEvent(string _actionName)
    {
        //Returns event of action named _actionName
        foreach (InputUnityEvent _event in events)
        {
            if(_event.actionName == _actionName)
            {
                return _event;
            }
        }
        if(playerInput!=null) Debug.LogWarning("Action named " + _actionName + " doesn't exist");
        return null;
    }
}
public enum InputType
{
    Started,
    Performed,
    Canceled
}
