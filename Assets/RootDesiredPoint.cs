using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootDesiredPoint : MonoBehaviour
{
    public GameObject pointerGraphics;
    public void SpawnPointer(Vector3 _poistion)
    {
        transform.position = _poistion;
        pointerGraphics.SetActive(true);
    }
    public void DespawnPointer()
    {
        if(pointerGraphics.activeSelf) pointerGraphics.SetActive(false);
    }
}


