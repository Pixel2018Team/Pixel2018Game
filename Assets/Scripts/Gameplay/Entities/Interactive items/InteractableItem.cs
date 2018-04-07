using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : MonoBehaviour {

    public bool isInteractable;
    public GameObject kidThatCanInteract;
    public GameObject currentOwner; //entity that manipulated this object
    public Enum.InteractableType interactableType;
    public int chaosAmount = 10;
    public bool causesChaos;


    private void Awake()
    {
        isInteractable = true;
        kidThatCanInteract = null;
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "kid" && !col.gameObject.GetComponent<TopDownKidsController>().controlledByAI && kidThatCanInteract == null)
        {
            DebugLogger.Log("trigger enter interactable object", Enum.LoggerMessageType.Important);
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

    //To be overrided by child classes (combo)
    public virtual void TriggerActionOnCombo(Enum.ComboAnimType animType)
    {

    }

    //To be overrided by child classes (single action)
    public virtual void TriggerActionOnInteract()
    {
    }

    public void CheckProvokeChaos()
    {
        if (causesChaos && currentOwner.tag == "kid")
        {
            LevelManager.Instance.AddChaos(chaosAmount);
        }
    }

    public void ReleaseOwner()
    {
        currentOwner = null;
    }
}
