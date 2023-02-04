using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lake : MonoBehaviour
{
    Renderer _renderer;
    [ColorUsage(true, true)]
    public Color toxicColor;
    [ColorUsage(true, true)]
    public Color voronoiToxicColor;
    [ColorUsage(true, true)]
    public Color waterColor;
    [ColorUsage(true, true)]
    public Color voronoiWaterColor;
    public float timeToTransition;
    private void Start() {
        _renderer = GetComponent<Renderer>();
    }
    public void Purify()
    {
        StartCoroutine(LerpIt());
    }
    IEnumerator LerpIt()
    {
        float time = 0;
        while(time < timeToTransition)
        {
            _renderer.material.SetColor("_BaseColor", Color.Lerp(toxicColor,waterColor,time/timeToTransition));
            _renderer.material.SetColor("_VoronoiColor", Color.Lerp(voronoiToxicColor,voronoiWaterColor,time/timeToTransition));
            time+=Time.deltaTime;
            yield return null;
        }
        _renderer.material.SetColor("_BaseColor", waterColor);
        _renderer.material.SetColor("_VoronoiColor", voronoiWaterColor);
    }
}
