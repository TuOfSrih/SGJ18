using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maggot : MonoBehaviour {

	public float stompDist = .3f;
	public Sprite stomped;

	void Start () {
		
	}
	
	void Update () {
		if (PlayerMovement.instance != null) {
			if (Vector2.Distance(transform.position, PlayerMovement.instance.transform.position) < stompDist) {
				GetComponent<Animator>().enabled = false;
				GetComponent<SpriteRenderer>().sprite = stomped;
				Debug.Log("Crushed the MADENs");
			}
		}
	}
}
