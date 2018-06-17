using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
	public MusicManager music;
	

	void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log("hö");
		if (other.CompareTag("asdf"))
		{
			if (other.GetComponent<PlayerMovement>().diaryIsRead)
			{
				//TODO: übergang und animation
				//music.level1 = true;
				Application.LoadLevel(0);
			}
			else
			{
				other.GetComponent<PlayerMovement>().isReading = true;
				//TODO: animation
				Application.LoadLevel(0);
			}
		}
	}
}
