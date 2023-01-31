using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowShaderCode : MonoBehaviour
{

    public List<MeshRenderer> m_GrowMeshes;

    [Range(0f, 1f)]
    public float m_MinGrow = 0f;
    [Range(0f, 1f)]
    public float m_MaxGrow = 1f;

    private List<Material> m_GrowMaterials = new List<Material>();


    void Start()
    {
        SetGrowMaterials(m_GrowMeshes);
    }

    public void SetGrowMaterials(List<MeshRenderer> meshes)
    {
        for (int i = 0; i < meshes.Count; i++)
        {
            for (int j = 0; j < meshes[i].materials.Length; j++)
            {
                if (meshes[i].materials[j].HasProperty("Grow_"))
                {
                    meshes[i].materials[j].SetFloat("Grow_", m_MinGrow);
                    m_GrowMaterials.Add(meshes[i].materials[j]);
                }
            }
        }
        

    }

    public bool IsCompleted()
    {
        return m_GrowMaterials[0].GetFloat("Grow_") >= 1;
    }

   
    public void SetGrowValue(float amount)
    {
        for (int i = 0; i < m_GrowMaterials.Count; i++)
        {
            m_GrowMaterials[i].SetFloat("Grow_",amount);
        }
    }

   
}
