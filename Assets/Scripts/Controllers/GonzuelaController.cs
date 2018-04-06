using UnityEngine;

public class GonzuelaController : MonoBehaviour
{
    private string KID_TAG = "kid";

    public float radius = 10.0f;

    void Start()
    {
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    void Update()
    {
        ScanKidsInReach();
    }

    private void ScanKidsInReach()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].tag == KID_TAG)
            {
                var kidController = colliders[i].GetComponent<KidController>();
                if (kidController.isBad && !kidController.isTag)
                {
                    kidController.GetTagged();
                }
            }
        }
    }
}
