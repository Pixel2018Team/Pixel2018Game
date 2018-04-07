using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSwitchManager : MonoBehaviour {

    public GameObject currentKid;
    public List<GameObject> kids;
    private int arrayIndex;

    private void Start()
    {
        if(currentKid != null)
        {
            currentKid.GetComponent<TopDownKidsController>().enabled = true;

            var ai = currentKid.GetComponent<AIKid>();

            if(ai != null)
            {
                ai.enabled = false;
                ai.EnableDisableAgent(false);
            }
        }

        foreach (var kid in kids)
        {
            if(kid != currentKid)
            {
                DelegateKidToAI(kid);
            }

            if(kid == currentKid)
            {
                arrayIndex = kids.IndexOf(currentKid);
            }
        }
    }

    public void SwitchToNextKid()
    {
        arrayIndex++;
        if(arrayIndex > kids.Count() - 1)
        {
            arrayIndex = 0;
        }

        //DelegateKidToPlayer(kids[arrayIndex]);

        foreach (var k in kids)
        {
            if (kids.IndexOf(k) != arrayIndex)
            {
                DelegateKidToAI(k);
            }

            else
            {
                DelegateKidToPlayer(k);
            }
        }    
    }

    public void DelegateKidToAI(GameObject kid)
    {
        var kidToModify = kids.FirstOrDefault( k => k == kid);

        kidToModify.GetComponent<Rigidbody>().velocity = Vector3.zero;

        if (kidToModify != null)
        {
            var controller = kidToModify.GetComponent<TopDownKidsController>();
            controller.controlledByAI = true;
            //drop any object held by the ai
            if (controller.objectCarried != null)
            {
                //controller.releaseOwnershipOnUsedObject();
                controller.DropCarriedObject(false);
            }
            
            var ai = kidToModify.GetComponent<AIKid>();
            if (ai != null)
            {
                ai.enabled = true;
                ai.EnableDisableAgent(true);
                ai.StartIdle();
            }
        }
    }

    public void DelegateKidToPlayer(GameObject kid)
    {
        var kidToModify = kids.FirstOrDefault(k => k == kid);

        if (kidToModify != null)
        {
            var controller = kidToModify.GetComponent<TopDownKidsController>();
            controller.controlledByAI = false;
            var ai = kidToModify.GetComponent<AIKid>();
            kidToModify.GetComponent<Rigidbody>().AddForce(Vector3.up * 0.1f);

            if (ai != null)
            {
                ai.enabled = false;
                ai.EnableDisableAgent(false);
            }
        }

        currentKid = kidToModify;
    }

	
	// Update is called once per frame
	void Update () {

        if (currentKid != null && currentKid.GetComponent<TopDownKidsController>().requestedSwitchKid == true)
        {
            currentKid.GetComponent<TopDownKidsController>().requestedSwitchKid = false;
            SwitchToNextKid();
        }
	}
}
