using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSwitchManager : MonoBehaviour {

    public GameObject startingKid;
    public List<GameObject> kids;

    private void Awake()
    {
        if(startingKid != null)
        {
            startingKid.GetComponent<TopDownKidsController>().enabled = true;

            var ai = startingKid.GetComponent<AIKid>();

            if(ai != null)
            {
                ai.enabled = false;
                ai.EnableDisableAgent(false);
            }
        }

        foreach (var kid in kids)
        {
            if(kid != startingKid)
            {
                DelegateKidToAI(kid);
            }
        }
    }

    public void SwitchToNextKid()
    {

    }

    public void DelegateKidToAI(GameObject kid)
    {
        var kidToModify = kids.FirstOrDefault( k => k == kid);

        if(kidToModify != null)
        {
            var controller = kidToModify.GetComponent<TopDownKidsController>();
            controller.controlledByAI = true;

            //drop any object held by the ai
            if(controller.objectCarried != null)
            {
                controller.objectCarried = null;
                controller.DropCarriedObject(false);
            }
            

            var ai = kidToModify.GetComponent<AIKid>();
            if (ai != null)
            {
                ai.enabled = true;
                ai.EnableDisableAgent(true);
            }
        }
    }

    public void DelegateKidToPlayer(GameObject kid)
    {
        var kidToModify = kids.FirstOrDefault(k => k == kid);

        if (kidToModify != null)
        {
            kidToModify.GetComponent<TopDownKidsController>().controlledByAI = false;
            var ai = kidToModify.GetComponent<AIKid>();

            if (ai != null)
            {
                ai.enabled = false;
                ai.EnableDisableAgent(false);
            }
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
