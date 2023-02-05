using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Collectables : MonoBehaviour
{
    public static Collectables instance;
    Animator anim;
    public List<GameObject> collectables = new List<GameObject>();
    public delegate void Collected();
    public static event Collected OnCollected;
    private void Awake()
    {
        if(instance == null) instance = this;
    }
    private void Start() {
        anim = GetComponent<Animator>();
    }
    public void CompleteOne()
    {
        foreach (GameObject item in collectables)
        {
            if (!item.activeInHierarchy)
            {
                item.SetActive(true);
                OnCollected?.Invoke();
                break;
            }
        }
        CheckIfAllCompleted();
    }
    public void CheckIfAllCompleted()
    {
        foreach (GameObject item in collectables)
        {
            if (!item.activeInHierarchy) return;
        }
        anim.Play("Cinematic");
    }
}
