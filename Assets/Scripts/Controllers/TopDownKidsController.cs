using UnityEngine;
using Global;

public class TopDownKidsController : MonoBehaviour
{
    public float speed = 6.0F;
    public InputMapping.PlayerTag playerTag;

    private Rigidbody _rigidBody;
    private Vector3 _moveInput;
    private Vector3 _moveVelocity;
    private Animator _animator;
    private bool isTagged;
    private bool controlsLocked;
    private bool isRotatingTowardsObject;
    private State state;
    public GameObject objectCarried;
    public int lockActionTime; //seconds
    public float currentlockActionTime;
    public float minInteractionDistance = 3f;
    public float facingAngleMargin = 5f;
    public float facingRotationSpeed = 5f;
    public GameObject interactableObjectInRange;
    private Vector3 carriedObjectSnapPosition;

    private enum State
    {
        Normal,
        RotatingTowardsObject,
        InAction,
        CarryingObject,
        Catched
    };

    private void Awake()
    {
        controlsLocked = false;
        isTagged = false;
        state = State.Normal;
    }

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        Debug.DrawLine(transform.position, transform.position + transform.rotation * Vector3.forward * 3, Color.red);

        //If not interacting with anything nor catched by gonzuela, move normally
        if (!controlsLocked)
        {
            _moveInput = new Vector3(Input.GetAxisRaw(InputMapping.GetInputName(playerTag, InputMapping.Input.Horizontal)), 0f,
                Input.GetAxisRaw(InputMapping.GetInputName(playerTag, InputMapping.Input.Vertical)));
            _moveVelocity = _moveInput * speed;

            Vector3 newDirection = Vector3.right * Input.GetAxisRaw(InputMapping.GetInputName(playerTag, InputMapping.Input.Horizontal))
                + Vector3.forward * Input.GetAxisRaw(InputMapping.GetInputName(playerTag, InputMapping.Input.Vertical));
            if (newDirection.sqrMagnitude > 0.0f)
            {
                transform.rotation = Quaternion.LookRotation(newDirection, Vector3.up);
            }

            if (Input.GetButtonDown(InputMapping.GetInputName(playerTag, InputMapping.Input.A)))
            {
                //TODO : single interactable objects
                if(interactableObjectInRange != null && objectCarried == null)
                {
                    InteractWithObject(interactableObjectInRange);
                    /*if (IsFacingAndCloseToObject(interactableObjectInRange))
                    {
                        DebugLogger.Log("Test Interaction", Enum.LoggerMessageType.Important);
                    }*/
                }

                //Combo objects
                else if (interactableObjectInRange != null && objectCarried != null)
                {
                    DropCarriedObject();
                }

                //Carriable object
                else if(objectCarried != null)
                {
                    DropCarriedObject();
                }
            }
        }

        if(state == State.RotatingTowardsObject)
        {
            if(interactableObjectInRange != null)
            {
                var objPosition = new Vector3(interactableObjectInRange.transform.position.x, transform.position.y, interactableObjectInRange.transform.position.z) ;
                var rotToObject = Quaternion.LookRotation(objPosition - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotToObject, Time.deltaTime * facingRotationSpeed);

                DebugLogger.Log("angle = " + Vector3.Angle(transform.forward, objPosition - transform.position), Enum.LoggerMessageType.Important);
                if (Vector3.Angle(transform.forward, objPosition - transform.position) <= facingAngleMargin)
                {
                    DebugLogger.Log("Rotation over", Enum.LoggerMessageType.Important);
                    state = State.InAction;
                    gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
                }
            }
        }

        if (state == State.InAction)
        {
            currentlockActionTime += Time.deltaTime;
            var elapsedSecs = currentlockActionTime % 60;

            if(elapsedSecs >= lockActionTime)
            {
                DebugLogger.Log("Interaction phase over", Enum.LoggerMessageType.Important);
                state = State.Normal;
                controlsLocked = false;
                gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
            }
        }

        if(state == State.CarryingObject)
        {

        }

        if (state == State.Catched)
        {

        }
    }

    private void InteractWithObject(GameObject obj)
    {
        if(obj != null)
        {

            //if(obj.GetComponent<TBD>().isSingleAction)
            /*state = State.RotatingTowardsObject;
            _moveInput = Vector3.zero;
            _moveVelocity = Vector3.zero;
            controlsLocked = true;
            currentlockActionTime = 0f;*/

            //else if(obj.GetComponent<TBD>().isCarriable)
            objectCarried = obj;
            objectCarried.transform.position = transform.position + transform.forward * 0.5f;
            objectCarried.transform.parent = transform;
        }
    }

    public void DropCarriedObject()
    {
        if (objectCarried != null)
        {
            objectCarried.transform.parent = null;
            objectCarried = null;

            //TODO drop carried object on another object
        }
    }


    private bool IsFacingAndCloseToObject(GameObject obj)
    {
        if (Vector3.Dot(transform.forward, obj.transform.position) >= 0 
            && Vector3.Distance(transform.position, obj.transform.position) <= minInteractionDistance)
        {
            return true;
        }

        return false;
    }

    public void CaughtByGonzuela()
    {
        if(state != State.Catched)
        {
            state = State.Catched;
        }
    }

    private void FixedUpdate()
    {
        //TODO: can remove the gravity by removing the up vector
        _animator.SetFloat("speed", _moveVelocity.magnitude);
        _rigidBody.velocity = _moveVelocity + _rigidBody.velocity.y * Vector3.up;
    }
}