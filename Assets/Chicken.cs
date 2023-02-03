using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : MonoBehaviour
{
    public float acceleration;
    public float maxSpeed;
    float speed;
    public float timeKnocked;
    public float knockPower;
    public Transform centerPoint;
    bool stopped;
    List<GameObject> pressers = new List<GameObject>();
    private void LateUpdate() {
        if(stopped) return;
        speed+=acceleration*Time.deltaTime;
        speed = Mathf.Clamp(speed,0,maxSpeed);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x,transform.eulerAngles.y,transform.eulerAngles.z+speed);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.isTrigger) return;
        if(other.gameObject.layer == LayerMask.NameToLayer("Player") && !stopped)
        {
            Vector3 direction = other.transform.position - centerPoint.transform.position;
            other.GetComponent<MovementController>().StopMotion(direction,timeKnocked,knockPower);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Root"))
        {
            CheckList();
            if (pressers.Contains(other.gameObject)) return;
            pressers.Add(other.gameObject);
            if(pressers.Count == 1)
            {
                stopped = true;
                speed = 0;
            }
        }
    }
   
    private void OnTriggerExit(Collider other)
    {
        if(other.isTrigger) return;
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Root"))
        {
            if (pressers.Contains(other.gameObject)) pressers.Remove(other.gameObject);
            if(!GetPressed()) stopped = false;
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
            pressers.RemoveAt(t-idxOffset);
            idxOffset++;
        }
    }
}
