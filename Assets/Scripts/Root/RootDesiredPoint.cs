using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootDesiredPoint : MonoBehaviour
{
    public ParticleSystem pointerGraphics;
    public void SpawnPointer(Vector3 _poistion)
    {
        transform.position = _poistion;
        pointerGraphics.Play();
    }
    public void DespawnPointer()
    {
        pointerGraphics.Stop(); 
    }

    
}


