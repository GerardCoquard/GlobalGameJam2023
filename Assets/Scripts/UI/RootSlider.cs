using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RootSlider : MonoBehaviour
{
    public GameObject sliderGraphics;
    public Image fill;
    public Color color1;
    public Color color2;
    public Color color3;
    public Image vignette;
    public float vignetteSpeed;
    private void Start()
    {
        sliderGraphics.SetActive(false);
    }
    public void ChangeState(bool state)
    {
        sliderGraphics.SetActive(state);
        if(state)
        {
            StopAllCoroutines();
            StartCoroutine(PopVignette());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(HideVignette());
        }
    }

    public void CalculateFillInPercent(float value1, float value2)
    {
        float fillAmount =  value1 / value2;
        fill.fillAmount = fillAmount;
        if(fillAmount <= 0.5f) fill.color = Color.Lerp(color1,color2,fillAmount/0.5f);
        else fill.color = Color.Lerp(color2,color3,(fillAmount-0.5f)/0.5f);
    }
    IEnumerator PopVignette()
    {
        while(vignette.color.a < 1)
        {
            vignette.color += new Color(0,0,0,vignetteSpeed*Time.deltaTime);
            yield return null;
        }
        vignette.color = new Color(vignette.color.r,vignette.color.g,vignette.color.b,1);
    }
    IEnumerator HideVignette()
    {
        while(vignette.color.a > 0)
        {
            vignette.color -= new Color(0,0,0,vignetteSpeed*Time.deltaTime);
            yield return null;
        }
        vignette.color = new Color(vignette.color.r,vignette.color.g,vignette.color.b,0);
    }
}


