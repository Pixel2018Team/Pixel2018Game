﻿using UnityEngine;
using Global;

public class TopDownKidsController : MonoBehaviour
{
    public float speed = 6.0F;
    public InputMapping.PlayerTag playerTag;
    public GameObject objectCarried;
    public int lockActionTime; //seconds
    public float currentlockActionTime;
    public float minInteractionDistance = 3f;
    public float facingAngleMargin = 5f;
    public float facingRotationSpeed = 5f;
    public GameObject interactableObjectInRange;
    public GameObject interactableObjectReceiverInRanger;
    public float tossForce = 5f;
    private Rigidbody _rigidBody;
    private Vector3 _moveInput;
    private Vector3 _moveVelocity;
    private Animator _animator;
    private bool isTagged;
    private bool controlsLocked;
    private bool isRotatingTowardsObject;
    private State state;

    private enum State
    {
        Normal,
        RotatingTowardsObject,
        InAction,
        CarryingObject,
        Catched,
        IA_Wandering,
        IA_PlayingAnim
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
                //Interact with an object (if we're not holding one)
                if(interactableObjectInRange != null && objectCarried == null)
                {
                    InteractWithObject(interactableObjectInRange);
                }

                //Dropping a held object
                else if (objectCarried != null)
                {
                    var dropOnReceiver = interactableObjectReceiverInRanger != null ? true : false;
                    DropCarriedObject(dropOnReceiver);
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
            var interactable = obj.GetComponent<InteractableItem>();

            if(interactable != null && interactable.isInteractable)
            {
                if (interactable.interactableType == Enum.InteractableType.SingleAction)
                {
                    state = State.RotatingTowardsObject;
                    _moveInput = Vector3.zero;
                    _moveVelocity = Vector3.zero;
                    controlsLocked = true;
                    currentlockActionTime = 0f;
                }

                else if (interactable.interactableType == Enum.InteractableType.ComboCarriable)
                {
                    state = State.CarryingObject;
                    objectCarried = obj;
                    objectCarried.transform.position = transform.position + transform.forward * 0.5f;
                    objectCarried.transform.parent = transform;
                    objectCarried.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    objectCarried.GetComponent<Rigidbody>().useGravity = false;
                    objectCarried.GetComponent<Collider>().enabled = false;
                }
            }
        }
    }

    public void DropCarriedObject(bool dropOnReceiver)
    {
        if (objectCarried != null)
        {
            //If combo receiver
            if (dropOnReceiver)
            {
                interactableObjectReceiverInRanger.GetComponent<ComboReceiver>().ReceiveObject(objectCarried);
                objectCarried.GetComponent<InteractableItem>().isInteractable = false;
            }

            else
            {
                //toss object forward
                objectCarried.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                objectCarried.GetComponent<Rigidbody>().useGravity = true;
                objectCarried.GetComponent<Collider>().enabled = true;
                objectCarried.GetComponent<Rigidbody>().AddForce(transform.forward.normalized * tossForce, ForceMode.Impulse);
                objectCarried.transform.parent = null;
            }

            state = State.Normal;
            objectCarried = null;
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