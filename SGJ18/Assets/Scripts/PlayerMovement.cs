using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public float movementSpeed;
	public float acceleration;

	Rigidbody2D rigidbody;

	void Start () {
		rigidbody = GetComponent<Rigidbody2D>();
	}
	
	void Update () {
		Vector2 target = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * movementSpeed;
		rigidbody.velocity = Vector2.MoveTowards(rigidbody.velocity, target, acceleration * Time.deltaTime);

	}
}
