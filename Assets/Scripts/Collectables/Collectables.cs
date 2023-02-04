using System.Collections.Generic;
using UnityEngine;

public class Collectables : MonoBehaviour
{
    public static Collectables instance;
    public Animator anim;
    public List<GameObject> collectables = new List<GameObject>();
    private void Awake()
    {
        if(instance == null) instance = this;
    }
    public void CompleteOne()
    {
        foreach (GameObject item in collectables)
        {
            if (!item.activeSelf) item.SetActive(true); break;
            
        }
        CheckIfAllCompleted();
    }
    public void CheckIfAllCompleted()
    {
        foreach (GameObject item in collectables)
        {
            if (!item.activeSelf) return;
        }
        anim.Play("Cinematic");
    }
}


