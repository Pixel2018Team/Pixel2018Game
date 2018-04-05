using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Vector3 offset;

    public GameObject targetObject;

    void Start()
    {
        offset = transform.position - targetObject.transform.position;
    }

    void LateUpdate()
    {
        transform.position = targetObject.transform.position + offset;
    }
}
