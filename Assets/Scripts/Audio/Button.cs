using System;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    public UnityEvent myEnterAction;
    public UnityEvent myExitAction;

    public void OnEnterAction()
    {
        myEnterAction.Invoke();
    }

    public void OnExitAction()
    {
        myExitAction.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        OnEnterAction();
    }

    private void OnTriggerExit(Collider other)
    {
        OnExitAction();
    }
}
