using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootGrowController : MonoBehaviour
{
    public BoxCollider m_MyCollider;
    public BoxCollider m_MyTrigger;

    private Vector3 m_ColliderCenter;
    private Vector3 m_MaxColliderSize;
    bool m_Grown;


    private Vector3 m_TriggerCenter;
    public List<GrowShaderCode> roots = new List<GrowShaderCode>();

    void Awake()
    {
        m_MaxColliderSize = m_MyCollider.size;
        m_ColliderCenter = m_MyCollider.center;

        m_TriggerCenter = m_MyTrigger.center;
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

        foreach (GrowShaderCode item in roots)
        {
            item.SetGrowValue(amount);
        }

        m_Grown = amount >= 1;
    }
}
