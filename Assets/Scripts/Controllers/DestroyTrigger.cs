using UnityEngine;

public class DestroyTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
