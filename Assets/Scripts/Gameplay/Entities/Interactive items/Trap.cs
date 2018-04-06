using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour {
    public float decayTime = 3f;
    public float currentTime;
    private bool trapActive = true;
    public GameObject owner;

	// Use this for initialization
	void Start () {
        trapActive = true;
    }
	
	// Update is called once per frame
	void Update () {

        if (trapActive)
        {
            var elapsedSecs = currentTime % 60;
            currentTime += Time.deltaTime;

            if (elapsedSecs >= decayTime)
            {
                trapActive = false;
                DebugLogger.Log("Trap decays", Enum.LoggerMessageType.Important);
                owner.GetComponent<TrapActionController>().ReallowTrap();
                Destroy(gameObject);
            }
        }
    }
}
