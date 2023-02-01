using UnityEngine;
using UnityEngine.UI;

public class RootSlider : MonoBehaviour
{
    public GameObject sliderGraphics;
    public Image fill;
    private void Start()
    {
        sliderGraphics.SetActive(false);
    }
    public void ChangeState()
    {
        if(sliderGraphics.activeSelf) sliderGraphics.SetActive(false); else { sliderGraphics.SetActive(true); }
    }

    public void CalculateFillInPercent(float value1, float value2)
    {
        float fillAmount =  value1 / value2;
        fill.fillAmount = fillAmount;
    }

}


