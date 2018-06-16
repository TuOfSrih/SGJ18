using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

	public GameObject player;
	public Vector3 offset = Vector3.back * 3;
	public float speed = 30;

	// Update is called once per frame
	void Update ()
	{
		Vector3 target = player.transform.position + offset;
		transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
	}
}
