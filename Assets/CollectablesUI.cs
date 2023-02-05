using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectablesUI : MonoBehaviour
{
    int amount;
    public List<GameObject> images;
    private void Start() {
        foreach (GameObject item in images)
        {
            item.SetActive(false);
        }
    }
    private void OnEnable() {
        Collectables.OnCollected += AddCollected;
    }
    private void OnDisable() {
        Collectables.OnCollected -= AddCollected;
    }

    void AddCollected()
    {
        if(amount<images.Count) images[amount].SetActive(true);
        amount++;
    }
}
