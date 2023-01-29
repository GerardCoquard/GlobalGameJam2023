using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    FSM fsm { get;}
    public virtual void OnEnter(){}
    public virtual void OnUpdate(){}
    public virtual void OnExit(){}
    public virtual void OnCollisionEnter(Collision collision){}
    public virtual void OnTriggerEnter(Collider other){}
    public virtual void OnTriggerStay(Collider other){}
    public virtual void OnTriggerExit(Collider other){}
}
