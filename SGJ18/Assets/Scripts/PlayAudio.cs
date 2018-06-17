using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour {
    AudioSource source;

	// Use this for initialization
	void OnEnable () {
        source = GetComponent<AudioSource>();
        source.Play();
	}
}
