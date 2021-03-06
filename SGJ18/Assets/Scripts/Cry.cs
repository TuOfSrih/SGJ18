﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cry : MonoBehaviour
{

	public AudioClip[] clips;
	private AudioSource source;
	private int number;
	private MusicManager music;

	public PlayerMovement player;

	void Start()
	{
		music = GameObject.FindObjectOfType<MusicManager>();
		number = 0;
		source = GetComponent<AudioSource>();
		StartCoroutine("cry");
	}

	void Update()
	{
		if (player.diaryIsRead && (player.j.GetButtonDown(Joycon.Button.DPAD_DOWN)
		                           || player.j.GetButtonDown(Joycon.Button.DPAD_UP)
		                           || player.j.GetButtonDown(Joycon.Button.DPAD_RIGHT)
		                           || player.j.GetButtonDown(Joycon.Button.DPAD_LEFT)))
		{
			//TODO: animation, load level
			music.levelCount++; 
			SceneManager.LoadScene(1);
		}
	}

	IEnumerator cry()
	{
		while (true)
		{
			source.clip = clips[number];
			number = (number + 1) % 6;
			yield return new WaitForSeconds(Random.Range(1.0f, 5.0f));
		}
	}
}
