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
    }
}
