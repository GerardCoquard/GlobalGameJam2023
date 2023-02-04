using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonRootAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,ISelectHandler,IDeselectHandler
{
    
    public List<Sprite> animationSprites;
    public float animationDuration;
    public Image image;
    int currentIndex;
    float frameTime;
    private void Start() {
        image.sprite = animationSprites[currentIndex];
        frameTime = animationDuration/animationSprites.Count;
    }
    public void Enter()
    {
        if(currentIndex<animationSprites.Count-1) 
        {
            StopAllCoroutines();
            StartCoroutine(GoUpwards(0));
        }
    }
    public void Exit()
    {
        if(currentIndex>0)
        {
            StopAllCoroutines();
            StartCoroutine(GoDownwards(0));
        }
    }
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        Enter();
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        Exit();
    }

    IEnumerator GoUpwards(float overTime)
    {
        float time = overTime;
        while(time<frameTime)
        {
            time+=Time.unscaledDeltaTime;
            yield return null;
        }
        currentIndex++;
        image.sprite = animationSprites[currentIndex];
        if(currentIndex<animationSprites.Count-1) StartCoroutine(GoUpwards(time-frameTime));
    }
    IEnumerator GoDownwards(float overTime)
    {
        float time = overTime;
        while(time<frameTime)
        {
            time+=Time.unscaledDeltaTime;
            yield return null;
        }
        currentIndex--;
        image.sprite = animationSprites[currentIndex];
        if(currentIndex>0) StartCoroutine(GoDownwards(time-frameTime));
    }

    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        Enter();
    }

    void IDeselectHandler.OnDeselect(BaseEventData eventData)
    {
        Exit();
    }
}
