using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public Guid playerId;
    public bool IsDead;
    public int playerScore;
    public bool canPlay;

    private void Awake()
    {
        playerId = Guid.NewGuid();
        canPlay = true;
    }

    // Use this for initialization
    void Start () {
        IsDead = false;
        playerScore = 0;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
