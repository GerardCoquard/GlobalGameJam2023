using UnityEngine;

public class PopUpTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PopUp.instance.OpenPopUp();
        }
    }
}
