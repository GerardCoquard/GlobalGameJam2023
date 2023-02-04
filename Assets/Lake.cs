using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lake : MonoBehaviour
{
    Renderer _renderer;
    [ColorUsage(true, true)]
    public Color hdr_col;
    public Material waterMat;
    private void Start() {
        _renderer = GetComponent<Renderer>();
    }
    public void Purify()
    {
        _renderer.material = waterMat;
    }
}
