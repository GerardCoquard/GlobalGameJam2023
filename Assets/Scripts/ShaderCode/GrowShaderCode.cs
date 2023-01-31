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
    public BoxCollider m_MyCollider;
    public BoxCollider m_MyTrigger;

    private Vector3 m_ColliderCenter;
    private Vector3 m_MaxColliderSize;


    private Vector3 m_TriggerCenter;

    public bool m_Grown;
    
     
    void Awake()
    {
        SetGrowMaterials(GetComponent<MeshRenderer>());
       

        m_MaxColliderSize = m_MyCollider.size;
        m_ColliderCenter = m_MyCollider.center;

        m_TriggerCenter = m_MyTrigger.center;
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

    public bool IsCompleted()
    {
        return m_Grown;
    }
   
    public void SetGrowValue(float amount)
    {
        float l_newSize = amount * m_MaxColliderSize.y;
        float l_newCenter = (m_MaxColliderSize.y - l_newSize) / 2;
        m_MyCollider.center = new Vector3(m_ColliderCenter.x, l_newCenter, m_ColliderCenter.z);
        m_MyCollider.size = new Vector3(m_MaxColliderSize.x,l_newSize, m_MaxColliderSize.z);

        m_MyTrigger.center = new Vector3(m_TriggerCenter.x, -((-m_MaxColliderSize.y / 2f) + l_newSize), m_TriggerCenter.z);
       
        
        for (int i = 0; i < m_GrowMaterials.Count; i++)
        {
            m_GrowMaterials[i].SetFloat("Grow_",amount);
        }

        m_Grown = amount >= 1;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.transform.SetParent(transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.SetParent(null);
        }
    }


}
