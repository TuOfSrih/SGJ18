using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Closet : MonoBehaviour
{

	public Sprite first;
	public Sprite second;

	private SpriteRenderer sprite;

	private AudioSource audio;

	void Start()
	{
		sprite = GetComponent<SpriteRenderer>();
		audio = GetComponent<AudioSource>();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("asdf"))
		{
			sprite.sprite = second;
			audio.Play();
		}
	}

	void OnTriggerExit2D (Collider2D other)
	{
		if (other.CompareTag("asdf"))
		{
			sprite.sprite = first;
		}
	}
}
