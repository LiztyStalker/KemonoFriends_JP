using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FirebaseAPI;

public class UIFirebaseManager : MonoBehaviour {
	// Use this for initialization
	void Awake () {
        DontDestroyOnLoad(this);

        FirebaseApi.GetInstance.initInstance();



	}
	
}
