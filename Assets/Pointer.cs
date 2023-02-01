using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pointer : MonoBehaviour
{
    Image pointer;
    private void Awake() {
        pointer = GetComponent<Image>();
    }
    private void OnEnable() {
        PlantMode.OnCursorChanged += SetCursor;
    }
    private void OnDisable() {
        PlantMode.OnCursorChanged -= SetCursor;
    }
    void SetCursor(Sprite newCursor)
    {
        pointer.sprite = newCursor;
    }
}
