using System.Collections.Generic;
using UnityEngine;

public class HelperManager : MonoBehaviour
{
    List<Helper> myHelperList = new List<Helper>();
    public static HelperManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
       
    }

    private void Start()
    {
        FillHelperList();
    }

    public void FillHelperList()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).GetComponent<Helper>() != null) myHelperList.Add(gameObject.transform.GetChild(i).GetComponent<Helper>());
        }
    }
    private void Update()
    {
        if (InputManager.GetAction("Help").WasPressedThisFrame()) ShowHelp();
    }

    void ShowHelp()
    {
        foreach (Helper help in myHelperList)
        {
            help.Help();
        }
    }
}