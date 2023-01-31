using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootController : MonoBehaviour
{

    private float m_Distance;
    public float m_MaxDistance = 100f;
    float m_TotalDistance;
    public float m_GrowthSpeed = 10f;
    Vector3 m_GrowthDirection;

    public float m_CollisionOffset = 0.5f;
    public LayerMask m_CollisionLayer;

    public GameObject m_RootPrefab;
    public GameObject m_NodePrefab;
    public float m_RootLength;

    List<GrowShaderCode> m_CurrentRoots = new List<GrowShaderCode>();

    Vector3 m_NextPosition;

    private void Update()
    {
        
        if (Input.GetKey(KeyCode.P))
        {
            m_Distance += m_GrowthSpeed * Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            m_Distance = 0;
            m_CurrentRoots = new List<GrowShaderCode>();
            //crear nodo
        }

    }
    public void StopGrow()
    {
        StopAllCoroutines();
        Vector3 l_DirectioNode = (Mathf.CeilToInt(m_TotalDistance / m_RootLength) - (m_TotalDistance / m_RootLength)) * m_GrowthDirection;
        Vector3 l_NodePosition = m_NextPosition - l_DirectioNode;
        GameObject l_node = Instantiate(m_NodePrefab, l_NodePosition, Quaternion.identity);
        l_node.transform.SetParent(transform);

        m_TotalDistance += m_Distance;
        m_CurrentRoots = new List<GrowShaderCode>();
        m_Distance = 0;
    }
    void SetRootsAmount()
    {
        int newSegments = Mathf.CeilToInt(m_Distance / m_RootLength);
        int neededSegments = newSegments - m_CurrentRoots.Count;
        if (neededSegments > 0) AddRoots();
       
    }

    IEnumerator Grow(Vector3 target)
    {
        m_Distance = 0;

        Vector3 l_InitialPosition = transform.GetChild(transform.childCount - 1).position;
        m_GrowthDirection = target - l_InitialPosition;
        m_GrowthDirection.Normalize();
        m_NextPosition = l_InitialPosition;

        AddRoots();

        while (CheckMaxDistance() && CheckCollision(l_InitialPosition))
        {
            m_Distance += m_GrowthSpeed * Time.deltaTime;
            SetRootsAmount();
        }
        StopGrow();
        yield return null;
    }

    bool CheckMaxDistance()
    {
        return m_TotalDistance + m_Distance < m_MaxDistance;
    }

    bool CheckCollision(Vector3 initialPosition)
    {
        Ray l_Ray = new Ray(initialPosition, m_GrowthDirection);
        RaycastHit l_RayHit;

        if(Physics.Raycast(l_Ray, out l_RayHit, m_CollisionOffset, m_CollisionLayer.value))
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
        m_CurrentRoots.Add(l_Root.GetComponent<GrowShaderCode>());
        m_NextPosition = l_Root.transform.position + (m_GrowthDirection * m_RootLength);
    }

    void RemoveRoots()
    {

    }

   

   


        
    

    


}
