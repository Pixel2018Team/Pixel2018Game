using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTest : MonoBehaviour {

	public bool GonzuelaLeads ;

	void Start(){
		Debug.Log ("Test sound is playing");
		AkSoundEngine.PostEvent ("Bbq_Start", gameObject);

	}

	void Update(){
		if (GonzuelaLeads == true) {
			AkSoundEngine.SetState ("MUS_Progression", "MUS_GonzuelaLeads");
		} else {
			AkSoundEngine.SetState ("MUS_Progression", "MUS_KidsLeads");
		}
	}
}
