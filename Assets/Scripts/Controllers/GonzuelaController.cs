using UnityEngine;

public class GonzuelaController : MonoBehaviour
{
    private string BREAK_TAG = "breakable";

    public float radius = 10.0f;
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
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
            if (colliders[i].tag == BREAK_TAG)
            {
                var controller = colliders[i].GetComponent<RepaireController>();
                if (controller.acting && !controller.isBroken)
                {
                    controller.CatchActing();
                    _animator.SetTrigger("angry");
                }
            }
        }
    }
}
