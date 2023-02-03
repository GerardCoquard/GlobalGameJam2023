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
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Root"))
        {
            OnEnterAction();
        }
       
    }
   
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Root"))
        {
            OnExitAction();
        }
    }
}
