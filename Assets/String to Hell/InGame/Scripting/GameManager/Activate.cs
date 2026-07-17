using UnityEngine;

public class Activate : MonoBehaviour
{
    public GameObject targetObject; // The object to activate
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var other = collision.gameObject;
        if (other.CompareTag("Player"))
        {
            targetObject.SetActive(true);
        }

    }
}
