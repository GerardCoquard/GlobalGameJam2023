using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    State currentState;

    public List<State> nextStates;
    private void Awake()
    {
        nextStates = new List<State>(GetComponents<State>());
    }
    void Start()
    {
        currentState = nextStates[0];
        currentState.OnEnter();
    }
    void Update()
    {
        currentState.OnUpdate();
    }

    public void ChangeState<T>() where T : Component
    {
        currentState.OnExit();
        currentState = GetState<T>();
        currentState.OnEnter();
    }
    State GetState<T>() where T : Component
    {
        foreach (var item in nextStates)
        {
            if (item.GetType() == typeof(T))
            {
                return item;
            }
        }
        return null;
    }
    private void OnCollisionEnter(Collision collision)
    {
        currentState.OnCollisionEnter(collision);
    }
    public void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(other);
    }
    public void OnTriggerStay(Collider other)
    {
        currentState.OnTriggerStay(other);
    }
    public void OnTriggerExit(Collider other)
    {
        currentState.OnTriggerExit(other);
    }
    
}
