using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootController : MonoBehaviour
{

    public float m_Distance;
    public float m_MaxDistance = 100f;
    public float m_TotalDistance;
    public float m_GrowthSpeed = 10f;
    Vector3 m_GrowthDirection;

    public float m_CollisionOffset = 0.5f;
    public LayerMask m_CollisionLayer;

    public GameObject m_RootPrefab;
    public GameObject m_NodePrefab;
    public float m_RootLength;
    bool m_Growing;
    

    List<GrowShaderCode> m_CurrentRoots = new List<GrowShaderCode>();

    Vector3 m_NextPosition;
    public void StopGrow()
    {

        if (!m_Growing) return;
        Vector3 l_DirectioNode = (Mathf.CeilToInt(m_Distance / m_RootLength) - (m_Distance / m_RootLength)) * m_RootLength * m_GrowthDirection ;
        Vector3 l_NodePosition = m_NextPosition - l_DirectioNode;
        GameObject l_node = Instantiate(m_NodePrefab, l_NodePosition, Quaternion.identity);
        l_node.transform.SetParent(transform);

        m_TotalDistance += m_Distance;
        m_CurrentRoots = new List<GrowShaderCode>();
        m_Growing = false;
        m_Distance = 0;
        StopAllCoroutines();
    }
    public void Reset()
    {
        StopAllCoroutines();
        m_Distance = 0;
        m_TotalDistance = 0;
        m_Growing = false;
        m_CurrentRoots = new List<GrowShaderCode>();

        for(int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        GameObject l_node = Instantiate(m_NodePrefab, transform.position, Quaternion.identity);//poner nodo inicial personalizao si eso
        l_node.transform.SetParent(transform);
    }
    
    void SetRootsAmount()
    {
        int newSegments = Mathf.CeilToInt(m_Distance / m_RootLength);
        int neededSegments = newSegments - m_CurrentRoots.Count;
        if (neededSegments > 0) AddRoots();
    }
    void SetRootsFills()
    {

        /*for (int i = 1; i <= m_CurrentRoots.Count; i++)
        {
            if (m_CurrentRoots[i - 1].IsCompleted()) continue;
            if (i * m_RootLength < m_Distance)
            {
                m_CurrentRoots[i - 1].SetGrowValue(1);
            }
            else
            {
                float currentAmount = (m_RootLength - (i * m_RootLength - m_Distance)) / m_RootLength;
                m_CurrentRoots[i - 1].SetGrowValue(currentAmount);
            }
        }*/
        float currentAmount = (m_RootLength - (m_CurrentRoots.Count * m_RootLength - m_Distance)) / m_RootLength;
        m_CurrentRoots[m_CurrentRoots.Count-1].SetGrowValue(currentAmount);
    }
    public void StrartGrowing(Vector3 target)
    {
        if (CheckMaxDistance())
        {
            StartCoroutine(Grow(target));
        }
       
    }

    IEnumerator Grow(Vector3 target)
    {
       
        m_Growing = true;
        Vector3 l_InitialPosition = transform.GetChild(transform.childCount - 1).position;
        m_GrowthDirection = target - l_InitialPosition;
        m_GrowthDirection.Normalize();
        m_NextPosition = l_InitialPosition;

        AddRoots();

        while (CheckMaxDistance() && CheckCollision(l_InitialPosition))
        {
            m_Distance += m_GrowthSpeed * Time.deltaTime;
            m_Distance = Mathf.Clamp(m_Distance,0,m_MaxDistance - m_TotalDistance);
            SetRootsAmount();
            SetRootsFills();
            yield return null;
        }
        
        StopGrow();
    }

    public bool CheckMaxDistance()
    {
        return m_TotalDistance + m_Distance < m_MaxDistance;
    }
    public bool CheckCollision(Vector3 initialPosition)
    {
        if(Physics.Raycast(initialPosition,m_GrowthDirection,m_Distance + m_CollisionOffset, m_CollisionLayer))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    void AddRoots()
    {
        GameObject l_Root = Instantiate(m_RootPrefab, m_NextPosition, Quaternion.identity);
        l_Root.transform.forward = m_GrowthDirection;
        l_Root.transform.SetParent(transform);
        m_CurrentRoots.Add(l_Root.GetComponentInChildren<GrowShaderCode>());
        m_NextPosition = l_Root.transform.position + (m_GrowthDirection * m_RootLength);
    }
}
