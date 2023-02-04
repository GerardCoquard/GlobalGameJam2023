using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    public UnityEvent action;
    public List<GameObject> pressers = new List<GameObject>();
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Root"))
        {
            AudioManager.instance.PlaySound("ButtonClicked", 0.5f);
            CheckList();
            if (pressers.Contains(other.gameObject)) return;
            pressers.Add(other.gameObject);
            if(pressers.Count == 1) action?.Invoke();

            
        }
       
    }
   
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Root"))
        {
            CheckList();
            if (pressers.Contains(other.gameObject)) pressers.Remove(other.gameObject);
            
        }
    }

    public bool GetPressed()
    {
        
        CheckList();
       
        return pressers.Count > 0;
    }

    void CheckList()
    {
        List<int> temp = new List<int>();
        for (int i = 0; i < pressers.Count; i++)
        {
            if (pressers[i] == null) temp.Add(i);
        }
        int idxOffset = 0;
        foreach (int t in temp)
        {
            pressers.RemoveAt(t-idxOffset);
            idxOffset++;
        }
    }
}
