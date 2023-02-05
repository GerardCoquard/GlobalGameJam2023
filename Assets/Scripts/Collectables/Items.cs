using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    bool picked;
    public Animator anim;
    public void PickItem()
    {
        if(picked) return;
        picked = true;
        anim.Play("Picked");
    }
    public void AnimEvent()
    {
        Collectables.instance.CompleteOne();
        DestroyImmediate(gameObject);
    }
}
