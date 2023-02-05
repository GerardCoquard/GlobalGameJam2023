using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonPuzleController : MonoBehaviour
{
    public List<Button> m_Buttons;
    public Animator anim;
    public void CheckCompletion()
    {
        Debug.Log("checking!");
        foreach (Button button in m_Buttons)
        {
            if (!button.GetPressed()) return;
        }
        Debug.Log("DONE!");
        anim.Play("Cutscene");
    }
}
