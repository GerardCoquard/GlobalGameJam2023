using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Collectables : MonoBehaviour
{
    public static Collectables instance;
    Animator anim;
    public List<GameObject> collectables = new List<GameObject>();
    public delegate void Collected();
    public static event Collected OnCollected;

    public Lake lake;
    private void Awake()
    {
        if(instance == null) instance = this;
    }
    private void Start() {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.O)) CheckIfAllCompleted();
    }
    public void Purify()
    {
        lake.Purify();
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene("Mapa2Simple");
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
