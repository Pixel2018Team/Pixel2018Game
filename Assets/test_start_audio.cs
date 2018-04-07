using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_start_audio : MonoBehaviour
{
    void Start()
    {
        AkSoundEngine.PostEvent("Gamestart", gameObject);
    }

    void Update()
    {
        if (Input.GetButtonDown("P1_A"))
        {
            LevelManager.Instance.AddChaos(25);
        }
        if (Input.GetButtonDown("P1_X"))
        {
            LevelManager.Instance.AddChaos(-25);
        }
    }
}
