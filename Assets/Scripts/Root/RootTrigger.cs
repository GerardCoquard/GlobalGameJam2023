using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "")
        {
            //DoThing
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "")
        {
            //DoThing
        }
    }
}
