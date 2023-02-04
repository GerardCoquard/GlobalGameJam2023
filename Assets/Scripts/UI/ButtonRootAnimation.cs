using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonRootAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,ISelectHandler,IDeselectHandler
{
    public List<Sprite> animationSprites;
    public float timeBetweenSprites;
    public Image image;
    int currentIndex;
    private void Start() {
        image.sprite = animationSprites[currentIndex];
    }
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if(currentIndex<animationSprites.Count-1) 
        {
            StopAllCoroutines();
            StartCoroutine(GoUpwards());
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if(currentIndex>0)
        {
            StopAllCoroutines();
            StartCoroutine(GoDownwards());
        }
    }

    IEnumerator GoUpwards()
    {
        float time = 0;
        while(time<timeBetweenSprites)
        {
            time+=Time.unscaledDeltaTime;
            yield return null;
        }
        currentIndex++;
        image.sprite = animationSprites[currentIndex];
        if(currentIndex<animationSprites.Count-1) StartCoroutine(GoUpwards());
    }
    IEnumerator GoDownwards()
    {
        yield return new WaitForSeconds(timeBetweenSprites);
        currentIndex--;
        image.sprite = animationSprites[currentIndex];
        if(currentIndex>0) StartCoroutine(GoDownwards());
    }

    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        if(currentIndex<animationSprites.Count-1) 
        {
            StopAllCoroutines();
            StartCoroutine(GoUpwards());
        }
    }

    void IDeselectHandler.OnDeselect(BaseEventData eventData)
    {
        if(currentIndex>0)
        {
            StopAllCoroutines();
            StartCoroutine(GoDownwards());
        }
    }
}
