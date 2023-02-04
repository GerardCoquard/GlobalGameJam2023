using System.Collections.Generic;
using UnityEngine;

public class FinalConditionManager : MonoBehaviour
{
    public static FinalConditionManager instance;
    public List<GameObject> collectables = new List<GameObject>();
    private void Awake()
    {
        if(instance == null) instance = this;
    }
    public void CompleteOne()
    {
        foreach (GameObject item in collectables)
        {
            if (!item.activeSelf) item.SetActive(true); return;
            
        }
        CheckIfAllCompleted();
    }
    public void CheckIfAllCompleted()
    {
        foreach (GameObject item in collectables)
        {
            if (!item.activeSelf) return;
            //Cinematic
        }
    }
}


