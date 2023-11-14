using UnityEngine;

public class BlackBlockBehavior : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("family"))
        {
            collider.gameObject.SendMessage("ChangeToBlack", SendMessageOptions.DontRequireReceiver);
        }
    }
}