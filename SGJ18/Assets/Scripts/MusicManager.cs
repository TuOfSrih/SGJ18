using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

	public AudioClip[] clips;
	public AudioSource source;

	public bool level1;
	public bool level2;
	public bool level3;
	
	// Use this for initialization
	void Awake ()
	{
		MusicManager[] objects = FindObjectsOfType<MusicManager>();
		if (objects.Length > 1)
		{
			Destroy(gameObject);
		}
		source = GetComponent<AudioSource>();
		changeSong(1);
		level1 = false;
		level2 = false;
		level3 = false;
	}

	public void changeSong(int songCount)
	{
		source.clip = clips[songCount];
	}
	
}
