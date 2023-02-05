using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    bool pressed;
    bool blocked;

    public Transform openPos;
    public Transform closePos;

    public float distOffset;

    public float speed;

    public List<GameObject> pressers = new List<GameObject>();

    public Button button;
    private void Start()
    {
        
    }
    private void Update()
    {
        if (blocked) return;
        pressed = button.GetPressed();
        if (pressed )
        {
            OpenRock();
        }
        if (!pressed )
        {
            Close();
        }
    }

    void Close()
    {
        
        Vector3 l_Direction = closePos.position - transform.position;
        l_Direction.Normalize();
        if (Vector3.Distance(transform.position, closePos.position) >= distOffset)
        {
            transform.position += l_Direction * speed * Time.deltaTime;
        }
        else return;
    }

    public void OpenRock()
    {

        Vector3 l_Direction = openPos.position - transform.position;
        l_Direction.Normalize();

        if (Vector3.Distance(transform.position, openPos.position) >= distOffset)
        {

            transform.position += l_Direction * speed * Time.deltaTime;

        }
        else return;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
        if (other.gameObject.layer == LayerMask.NameToLayer("Root"))
        {
            CheckList();
            if (pressers.Contains(other.gameObject)) return;
            pressers.Add(other.gameObject);
            if (pressers.Count == 1)
            {
                blocked = true;

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger) return;
        if (other.gameObject.layer == LayerMask.NameToLayer("Root"))
        {
            if (pressers.Contains(other.gameObject)) pressers.Remove(other.gameObject);
            if (!GetPressed()) blocked = false;
        }
    }

    public bool GetPressed()
    {
        CheckList();
        return pressers.Count > 0;
    }

    void CheckList()
    {
        List<int> temp = new List<int>();
        for (int i = 0; i < pressers.Count; i++)
        {
            if (pressers[i] == null) temp.Add(i);
        }
        int idxOffset = 0;
        foreach (int t in temp)
        {
            pressers.RemoveAt(t - idxOffset);
            idxOffset++;
        }
    }
}