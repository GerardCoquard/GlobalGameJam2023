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
    public LayerMask m_CollisionDetectionLayers;

    public GameObject m_RootPrefab;
    public GameObject m_NodePrefab;
    public GameObject m_FirstNodePrefab;
    public float m_RootLength;
    public bool growing;
    public bool decreasing;
    

    List<RootGrowController> m_CurrentRoots = new List<RootGrowController>();
    List<RootGrowController> m_TotalRoots = new List<RootGrowController>();

    Vector3 m_NextPosition;
    public void StopGrow()
    {
        if(!growing) return;
        growing = false;
        Vector3 l_DirectioNode = (Mathf.CeilToInt(m_Distance / m_RootLength) - (m_Distance / m_RootLength)) * m_RootLength * m_GrowthDirection ;
        Vector3 l_NodePosition = m_NextPosition - l_DirectioNode;
        GameObject l_node = Instantiate(m_NodePrefab, l_NodePosition, Quaternion.identity);
        l_node.transform.SetParent(transform);

        m_TotalDistance += m_Distance;
        m_CurrentRoots = new List<RootGrowController>();
        m_Distance = 0;
        StopAllCoroutines();
    }
    public void Reset()
    {
        StopAllCoroutines();
        m_Distance = 0;
        m_TotalDistance = 0;
        growing = false;
        decreasing = false;
        m_CurrentRoots = new List<RootGrowController>();
        m_TotalRoots = new List<RootGrowController>();

        for(int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        GameObject l_node = Instantiate(m_FirstNodePrefab, transform.position, Quaternion.identity);
        l_node.transform.SetParent(transform);
    }
    public bool FullyGrown()
    {
        return m_TotalDistance >= m_MaxDistance;
    }
    void SetRootsAmount()
    {
        int newSegments = Mathf.CeilToInt(m_Distance / m_RootLength);
        int neededSegments = newSegments - m_CurrentRoots.Count;
        if (neededSegments > 0) AddRoots();
    }
    void SetRootsFills()
    {
        float currentAmount = (m_RootLength - (m_CurrentRoots.Count * m_RootLength - m_Distance)) / m_RootLength;
        m_CurrentRoots[m_CurrentRoots.Count-1].SetGrowValue(currentAmount);
    }
    void SetDecreaseFills(float decreaseAmount)
    {
        if(m_TotalRoots.Count < 1) return;
        float lastRootAmount = m_TotalRoots[m_TotalRoots.Count-1].GrowAmount();
        if( lastRootAmount < decreaseAmount)
        {
            RemoveRoot();
            SetDecreaseFills(decreaseAmount-lastRootAmount);
            return;
        }
        m_TotalRoots[m_TotalRoots.Count-1].SetGrowValue(lastRootAmount-decreaseAmount);
    }
    public void StrartGrowing(Vector3 target)
    {
        if(growing || decreasing) return;
        if (CheckMaxDistance()) StartCoroutine(Grow(target));
    }
    public void StartDecreasing()
    {
        if(growing || decreasing) return;
        if(m_TotalDistance > 0) StartCoroutine(Decrease());
    }
    public void StopDecreasing()
    {
        if(!decreasing) return;
        decreasing = false;
        Transform lastNode = transform.GetChild(transform.childCount-1);
        SetLastNode(lastNode);
        StopAllCoroutines();
    }

    IEnumerator Grow(Vector3 target)
    {
        growing = true;
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
    IEnumerator Decrease()
    {
        decreasing = true;
        Transform lastNode = transform.GetChild(transform.childCount-1);
        while (m_TotalDistance > 0)
        {
            float distanceLost = m_GrowthSpeed * Time.deltaTime;
            m_TotalDistance -= distanceLost;
            m_TotalDistance = Mathf.Clamp(m_TotalDistance,0,m_MaxDistance);
            SetDecreaseFills(distanceLost/m_RootLength);
            if(m_TotalRoots.Count > 0) SetLastNode(lastNode);
            yield return null;
        }
        DestroyImmediate(lastNode.gameObject);
        decreasing = false;
    }
    void SetLastNode(Transform node)
    {
        RootGrowController lastRoot = m_TotalRoots[m_TotalRoots.Count-1];
        Vector3 desiredDir = lastRoot.transform.forward * lastRoot.GrowAmount() * m_RootLength;
        Vector3 desiredPosition = lastRoot.transform.position + desiredDir;

        node.transform.position = desiredPosition;
    }
    public bool CheckMaxDistance()
    {
        return m_TotalDistance + m_Distance < m_MaxDistance;
    }
    public bool CheckCollision(Vector3 initialPosition)
    {
        if(Physics.Raycast(initialPosition,m_GrowthDirection,m_Distance + m_CollisionOffset, m_CollisionDetectionLayers))
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
        m_CurrentRoots.Add(l_Root.GetComponent<RootGrowController>());
        m_TotalRoots.Add(l_Root.GetComponent<RootGrowController>());
        m_NextPosition = l_Root.transform.position + (m_GrowthDirection * m_RootLength);
    }
    void RemoveRoot()
    {
        RootGrowController rootToRemove = m_TotalRoots[m_TotalRoots.Count-1];
        m_TotalRoots.Remove(rootToRemove);
        DestroyImmediate(rootToRemove.gameObject);
        if(transform.GetChild(transform.childCount-2).tag == "Node") DestroyImmediate(transform.GetChild(transform.childCount-2).gameObject);
    }
}
