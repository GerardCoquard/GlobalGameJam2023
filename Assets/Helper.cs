using System.Collections;
using UnityEngine;

public class Helper : MonoBehaviour
{
    Animator anim;
    public float timeForNextHelp;
    bool helping;
    public ParticleSystem particle;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void Help()
    {
        if (!helping)
        {
            StartCoroutine(NextHelpTime());
            particle.Play();
            anim.Play("RecorridoPrueba");
        }
    }

    public void StopHelp()
    {
        particle.Stop();
    }
    IEnumerator NextHelpTime()
    {
        helping = true;
        yield return new WaitForSeconds(timeForNextHelp);
        helping = false;
    }
}
