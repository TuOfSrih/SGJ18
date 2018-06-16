using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamReplaceShader : MonoBehaviour {

    public Shader replace;

	// Use this for initialization
	void Enable () {
		if(replace != null) {
            GetComponent<Camera>().SetReplacementShader(replace, "");
        }
	}
	
	// Update is called once per frame
	void Disable () {
        GetComponent<Camera>().ResetReplacementShader();
	}
}
