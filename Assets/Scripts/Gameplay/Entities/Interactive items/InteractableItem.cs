using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : MonoBehaviour {

    public bool isInteractable;
    public GameObject kidThatCanInteract;
    public Enum.InteractableType interactableType;


    public void Awake()
    {
        isInteractable = true;
        kidThatCanInteract = null;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter(Collider col)
    {
        DebugLogger.Log("trigger enter interactable object", Enum.LoggerMessageType.Important);
        if (col.gameObject.tag == "kid" && kidThatCanInteract == null)
        {
            kidThatCanInteract = col.gameObject;
            col.gameObject.GetComponent<TopDownKidsController>().interactableObjectInRange = gameObject;
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "kid" && kidThatCanInteract == col.gameObject)
        {
            col.gameObject.GetComponent<TopDownKidsController>().interactableObjectInRange = null;
            kidThatCanInteract = null;
        }
    }

    //To be overrided by child classes
    public virtual void TriggerActionOnCombo(Enum.ComboAnimType animType)
    {

    }
}
