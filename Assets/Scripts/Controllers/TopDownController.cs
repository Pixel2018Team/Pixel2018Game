using UnityEngine;
using Global;

public class TopDownController : MonoBehaviour
{
    public float speed = 6.0F;
    public InputMapping.PlayerTag playerTag;

    private Rigidbody _rigidBody;
    private Vector3 _moveInput;
    private Vector3 _moveVelocity;
    private Animator _animator;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        //_animator = GetComponent<Animator>();
    }

    void Update()
    {
        Debug.DrawLine(transform.position, transform.position + transform.rotation * Vector3.forward * 3, Color.red);

        _moveInput = new Vector3(Input.GetAxisRaw(InputMapping.GetInputName(playerTag, InputMapping.Input.Horizontal)), 0f,
            Input.GetAxisRaw(InputMapping.GetInputName(playerTag, InputMapping.Input.Vertical)));
        _moveVelocity = _moveInput * speed;

        Vector3 newDirection = Vector3.right * Input.GetAxisRaw(InputMapping.GetInputName(playerTag, InputMapping.Input.RHorizontal))
            + Vector3.forward * Input.GetAxisRaw(InputMapping.GetInputName(playerTag, InputMapping.Input.RVertical));
        if(newDirection.sqrMagnitude > 0.0f)
        {
            transform.rotation = Quaternion.LookRotation(newDirection, Vector3.up);
        }
    }

    private void FixedUpdate()
    {
        //TODO: can remove the gravity by removing the up vector
        //_animator.SetFloat("speed", _moveVelocity.magnitude);
        _rigidBody.velocity = _moveVelocity + _rigidBody.velocity.y * Vector3.up;
    }
}