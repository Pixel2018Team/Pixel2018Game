using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboReceiver : MonoBehaviour {

    public GameObject triggerObject; //object that should interact with this
    public bool isActivated;
    public SnapPositionForTrigger snapPositionForTrigger;
    public float offsetY = 1.0f;

    public enum SnapPositionForTrigger
    {
        Top,
        Inside
    };

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter(Collider col)
    {
        DebugLogger.Log("trigger enter", Enum.LoggerMessageType.Important);
        if (col.gameObject.tag == "kid")
        {
            col.gameObject.GetComponent<TopDownKidsController>().interactableObjectReceiverInRanger = gameObject;
            DebugLogger.Log("Combo object inbound", Enum.LoggerMessageType.Important);
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "kid")
        {
            col.gameObject.GetComponent<TopDownKidsController>().interactableObjectReceiverInRanger = null;
        }
    }


    public void ReceiveObject(GameObject obj)
    {
        if(obj == triggerObject)
        {
            DebugLogger.Log("Received object", Enum.LoggerMessageType.Important);
            isActivated = true;
            //GetComponent<Renderer>().material.color = Color.red;

            obj.transform.parent = transform;

            if(snapPositionForTrigger == SnapPositionForTrigger.Top)
            {
                obj.transform.position = transform.position + transform.up * (transform.localScale.y / 2);
            }
            
            else if (snapPositionForTrigger == SnapPositionForTrigger.Inside)
            {
                obj.transform.position = new Vector3(transform.position.x, transform.position.y + offsetY, transform.position.z);
            }

            triggerObject.GetComponent<InteractableItem>().TriggerActionOnCombo(Enum.ComboAnimType.StaticToAnimated);
        }
    }
}
