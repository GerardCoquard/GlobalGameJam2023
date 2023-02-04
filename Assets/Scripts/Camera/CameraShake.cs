using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    [SerializeField] float shakeMagnitude;

    public static CameraShake instance;
    Vector3 originalPos;
    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        originalPos = transform.localPosition;
    }
    public void shake()
    {

        

        float x = Random.Range(-0.5f, 0.5f) * shakeMagnitude;
        float y = Random.Range(-0.5f, 0.5f) * shakeMagnitude;

        transform.localPosition = new Vector3(transform.localPosition.x + x, transform.localPosition.y + y, originalPos.z);

    }

    public void StopShake()
    {
        transform.localPosition = originalPos;
    }
}
