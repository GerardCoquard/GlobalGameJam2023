using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene1 : MonoBehaviour
{
    public Camera cam;
    public Animator anim;
    public void PlayCutScene()
    {
        anim.Play("CutScene");
    }
}
