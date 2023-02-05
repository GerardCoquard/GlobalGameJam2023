using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootController : MonoBehaviour
{
    [SerializeField] float m_MaxDistance = 100f;
    [SerializeField] float m_GrowthSpeed = 10f;
    [SerializeField] GameObject m_RootPrefab;
    [SerializeField] GameObject m_NodePrefab;
    [SerializeField] float m_CollisionOffset = 0.5f;
    [SerializeField] float m_RootLength;
    [SerializeField] LayerMask m_CollisionDetectionLayers;
    [SerializeField] float resetTime = 20;
    
    List<RootGrowController> m_CurrentRoots = new List<RootGrowController>();
    List<RootGrowController> m_TotalRoots = new List<RootGrowController>();
    float m_Distance;
    float m_TotalDistance;
    Vector3 m_GrowthDirection;
    Vector3 m_NextPosition;
    bool growing;
    bool decreasing;
    public bool picked;
    float timeOffline;
    int id;
    private void Start() {
        id = Random.Range(0,5000);
    }
    public void StrartGrowing(Vector3 target)
    {
        timeOffline = 0;
        if(growing || decreasing) return;
        if (CheckMaxDistance()) StartCoroutine(Grow(target));
    }
    public void StartDecreasing()
    {
        timeOffline = 0;
        if(growing || decreasing) return;
        if(m_TotalDistance > 0) StartCoroutine(Decrease());
    }
    IEnumerator Grow(Vector3 target)
    {
        growing = true;
        Vector3 l_InitialPosition = transform.GetChild(transform.childCount - 1).position;
        m_GrowthDirection = target - l_InitialPosition;
        m_GrowthDirection.Normalize();
        m_NextPosition = l_InitialPosition;
        AddRoot();
        if(picked)AudioManager.instance.PlaySound("RaizesCreciendo"+id.ToString(),"RaizesCreciendo", 0.5f, true);
        while (CheckMaxDistance() && CheckCollision(l_InitialPosition))
        {
            if(picked)CameraShake.instance.shake();
            m_Distance += m_GrowthSpeed * Time.deltaTime;
            m_Distance = Mathf.Clamp(m_Distance,0,m_MaxDistance - m_TotalDistance);
            CheckIfNeedRoots();
            SetGrowFill();
            yield return null;
        }
        if(m_TotalRoots[m_TotalRoots.Count-1].GetGrowAmount() == 0)
        {
            RemoveRoot();
        }
        StopGrow();
    }
    IEnumerator Decrease()
    {
        decreasing = true;
        m_Distance = 0;
        Transform lastNode = transform.GetChild(transform.childCount-1);
        if(picked)AudioManager.instance.PlaySound("RaizesCreciendo"+id.ToString(),"RaizesCreciendo", 0.5f, true);
        while (m_TotalDistance > 0)
        {
            if(picked)CameraShake.instance.shake();
            float distanceLost = m_GrowthSpeed * Time.deltaTime;
            m_TotalDistance -= distanceLost;
            m_TotalDistance = Mathf.Clamp(m_TotalDistance,0,m_MaxDistance);
            SetDecreaseFill(distanceLost/m_RootLength);
            if(m_TotalRoots.Count > 0) SetLastNode(lastNode);
            yield return null;
        }
        DestroyImmediate(lastNode.gameObject);
        CameraShake.instance.StopShake();
        decreasing = false;
        AudioManager.instance.StopSound("RaizesCreciendo"+id.ToString());
    }
    public void StopGrow()
    {
        if(!growing) return;
        growing = false;

        AudioManager.instance.StopSound("RaizesCreciendo"+id.ToString());
        CameraShake.instance.StopShake();
        RootGrowController lastRoot = m_TotalRoots[m_TotalRoots.Count-1];
        Vector3 l_DirectioNode = lastRoot.GetGrowAmount()*m_RootLength * lastRoot.transform.forward;
        Vector3 l_NodePosition = lastRoot.transform.position + l_DirectioNode;
        GameObject l_node = Instantiate(m_NodePrefab, l_NodePosition, lastRoot.transform.rotation);
        l_node.transform.SetParent(transform);

        m_TotalDistance += m_Distance;
        m_CurrentRoots = new List<RootGrowController>();
        m_Distance = 0;

        RemoveDuplicatedNodes();

        StopAllCoroutines();
    }
    public void StopDecreasing()
    {
        if(!decreasing) return;
        AudioManager.instance.StopSound("RaizesCreciendo"+id.ToString());
        decreasing = false;
        CameraShake.instance.StopShake();
        Transform lastNode = transform.GetChild(transform.childCount-1);
        SetLastNode(lastNode);
        StopAllCoroutines();
    }
    void CheckIfNeedRoots()
    {
        int newSegments = Mathf.CeilToInt(m_Distance / m_RootLength);
        int neededSegments = newSegments - m_CurrentRoots.Count;
        if (neededSegments > 0) AddRoot();
    }
    void AddRoot()
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
    }
    void RemoveLastNode()
    {
        if(transform.GetChild(transform.childCount-2).tag == "Node") DestroyImmediate(transform.GetChild(transform.childCount-2).gameObject);
    }
    void RemoveDuplicatedNodes()
    {
        if(transform.childCount < 2) return;
        GameObject node1 = transform.GetChild(transform.childCount-1).gameObject;
        GameObject node2 = transform.GetChild(transform.childCount-2).gameObject;
        if(node1.tag == "Node" && node2.tag == "Node") DestroyImmediate(node1);
    }
    void SetGrowFill()
    {
        float currentAmount = (m_RootLength - (m_CurrentRoots.Count * m_RootLength - m_Distance)) / m_RootLength;
        m_CurrentRoots[m_CurrentRoots.Count-1].SetGrowValue(currentAmount);
    }
    void SetDecreaseFill(float decreaseAmount)
    {
        if(m_TotalRoots.Count < 1) return;
        float lastRootAmount = m_TotalRoots[m_TotalRoots.Count-1].GetGrowAmount();
        if( lastRootAmount < decreaseAmount)
        {
            RemoveRoot();
            RemoveLastNode();
            SetDecreaseFill(decreaseAmount-lastRootAmount);
            return;
        }
        m_TotalRoots[m_TotalRoots.Count-1].SetGrowValue(lastRootAmount-decreaseAmount);
    }
    void SetLastNode(Transform node)
    {
        RootGrowController lastRoot = m_TotalRoots[m_TotalRoots.Count-1];
        Vector3 desiredDir = lastRoot.transform.forward * lastRoot.GetGrowAmount() * m_RootLength;
        Vector3 desiredPosition = lastRoot.transform.position + desiredDir;

        node.transform.position = desiredPosition;
    }
    public void SetUnselected()
    {
        timeOffline = 0;
        picked = false;
        StopAllCoroutines();
        StartCoroutine(ResetTheTimer(id));
    }
    IEnumerator ResetTheTimer(int _id)
    {
        if(_id != id) yield break;
        while(timeOffline < resetTime && !picked)
        {
            timeOffline+=Time.deltaTime;
            yield return null;
        }
        if(!picked) StartDecreasing();
    }
    public void SetSelected()
    {
        picked = true;
        timeOffline = 0;
    }
    public bool CheckCollision(Vector3 initialPosition)
    {
        return !Physics.Raycast(initialPosition,m_GrowthDirection,m_Distance + m_CollisionOffset, m_CollisionDetectionLayers);
    }
    public bool CheckMaxDistance()
    {
        return m_TotalDistance + m_Distance < m_MaxDistance;
    }
    public bool FullyGrown()
    {
        return m_TotalDistance >= m_MaxDistance;
    }
    public float GetCurrentDistance()
    {
        return m_Distance;
    }
    public float GetDistance()
    {
        return m_TotalDistance;
    }
    public float GetMaxDistance()
    {
        return m_MaxDistance;
    }
    public bool GetGrowing()
    {
        return growing;
    }
    public bool GetDecreasing()
    {
        return decreasing;
    }
}
