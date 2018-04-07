using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LightNetwork : InteractableItem {

    public List<GameObject> lights;
    public bool isOn = true;
    public GameObject button;
    private Animator buttonAnimator;

    public void Awake()
    {
        buttonAnimator = button.GetComponent<Animator>();
    }

    public override void TriggerActionOnInteract()
    {
        Debug.Log("INTERACTED");

        if (isOn)
        {
            //buttonAnimator.SetBool("isOn", true);
            SwitchAllState(false);
        }
        else
        {
            //buttonAnimator.SetBool("isOn", false);
            SwitchAllState(true);
        }
    }

    // Use this for initialization
    void Start () {
        isOn = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SwitchAllState(bool on)
    {
        isOn = on;
        Debug.Log("Lights on = "+isOn);
        if (lights.Count() > 0)
        {
            lights.ForEach(l => l.GetComponent<Light>().enabled = isOn);
        }
    }

    public void SwitchLight(GameObject light, bool on)
    {
        lights.FirstOrDefault(l => l == light).SetActive(on);
    }
}
