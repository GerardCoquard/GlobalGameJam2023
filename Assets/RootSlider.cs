﻿using UnityEngine;
using UnityEngine.UI;

public class RootSlider : MonoBehaviour
{
    public GameObject sliderGraphics;
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
        sliderGraphics.GetComponent<Image>().fillAmount = fillAmount;
    }

}


