using Assets.Scripts.Controllers.StateMachine;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public GameObject target;
    public float radius = 10.0f;
    public float speed = 3.0F;
    public Vector3 moveVelocity;
    public Vector3 moveRotation;
    public BoxCollider armCollider;
    public int attackPower = 5;
    public int MaxLife = 10;
    public int currentLife;

    private Vector3 _moveInput;
    private Rigidbody _rigidBody;
    private ZombieStateMachine _stateMachine;
    protected const string BULLET_TAG = "Bullet";

    void Start()
    {
        currentLife = MaxLife;
        _rigidBody = GetComponent<Rigidbody>();
        _stateMachine = GetComponent<ZombieStateMachine>();
    }

    void Update()
    {
        _stateMachine.currentState.Update(gameObject);
    }

    void OnTriggerStay(Collider other)
    {
        _stateMachine.currentState.OnTriggerStay(gameObject, other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == BULLET_TAG)
        {
            GetDamage(5);
            Destroy(collision.collider.gameObject);
        }
    }

    private void FixedUpdate()
    {
        _rigidBody.velocity = new Vector3(moveVelocity.x, _rigidBody.velocity.y, moveVelocity.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void GetDamage(int damage)
    {
        currentLife -= damage;
        if (currentLife < 1)
        {
            Destroy(gameObject);
        }
    }
}
