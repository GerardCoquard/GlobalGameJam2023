using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour
{
    Animator anim;
    public float timeForNextHelp;
    bool helping;
    private void Start() {
        anim = GetComponent<Animator>();
    }
    public void Help()
    {
        if(!helping)
        {
            StartCoroutine(NextHelpTime());
            anim.Play("Help");
        }
    }
    IEnumerator NextHelpTime()
    {
        helping = true;
        yield return new WaitForSeconds(timeForNextHelp);
        helping = false;
    }
}
