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
        foreach (Button button in m_Buttons)
        {
            if (!button.GetPressed()) return;

        }
        anim.Play("Cutscene");
    }
}
