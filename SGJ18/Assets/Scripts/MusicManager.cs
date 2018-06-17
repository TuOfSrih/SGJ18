using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

	public AudioClip[] clips;
	public AudioSource source;

	public int levelCount;
	
	// Use this for initialization
	void Awake ()
	{
		DontDestroyOnLoad(gameObject);
		MusicManager[] objects = FindObjectsOfType<MusicManager>();
		if (objects.Length > 1)
		{
			Destroy(gameObject);
		}
		source = GetComponent<AudioSource>();
		changeSong(1);
		levelCount = 0;

	}

	public void changeSong(int songCount)
	{
		source.clip = clips[songCount];
	}
	

	
}
