using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonPuzleController :MonoBehaviour
{
    public List<Button> m_Buttons;
    public UnityEvent m_ActionWhenCompleted;
    public void CheckCompletion()
    {
        foreach (Button button in m_Buttons)
        {
            if (!button.GetPressed()) return;

        }
        m_ActionWhenCompleted?.Invoke();
    }


    public void Test()
    {
        Debug.Log("DO THING");
    }

    
}
