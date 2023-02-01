using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowShaderCode : MonoBehaviour
{
    [Range(0f, 1f)]
    public float m_MinGrow = 0f;
    [Range(0f, 1f)]
    public float m_MaxGrow = 1f;

    private List<Material> m_GrowMaterials = new List<Material>();
    void Awake()
    {
        SetGrowMaterials(GetComponent<MeshRenderer>());
    }

    public void SetGrowMaterials(MeshRenderer mesh)
    {
        for (int j = 0; j < mesh.materials.Length; j++)
        {
            if (mesh.materials[j].HasProperty("Grow_"))
            {
                mesh.materials[j].SetFloat("Grow_", m_MinGrow);
                m_GrowMaterials.Add(mesh.materials[j]);
            }
        }
    }
   
    public void SetGrowValue(float amount)
    {
        for (int i = 0; i < m_GrowMaterials.Count; i++)
        {
            m_GrowMaterials[i].SetFloat("Grow_",amount);
        }
    }
}
