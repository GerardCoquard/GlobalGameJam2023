using UnityEngine;

public class PopUpTrigger : MonoBehaviour
{
    public Sprite sprite;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            InterfaceManager.instance.OpenPopUp(sprite);
            Destroy(gameObject);
        }
    }
}
