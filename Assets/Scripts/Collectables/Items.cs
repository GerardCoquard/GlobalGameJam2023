using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    bool picked;
    Animator anim;
    private void Start() {
        anim = GetComponent<Animator>();
    }
    public void PickItem()
    {
        if(picked) return;
        picked = true;
        AudioManager.instance.PlaySoundOneShot("Collectable","Collectable",0.5f);
        anim.SetBool("Picked",true);
    }
    public void AnimEvent()
    {
        Collectables.instance.CompleteOne();
        Destroy(gameObject);
    }
}
