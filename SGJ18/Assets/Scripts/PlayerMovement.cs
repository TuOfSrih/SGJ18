using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public float movementSpeed;
	public float acceleration;

	Rigidbody2D rigidbody;
	private Collider2D collider;

	private bool nearCloset;

	public bool isHidden;

	private GameObject closet;

	void Start () {
		rigidbody = GetComponent<Rigidbody2D>();
		collider = GetComponent<Collider2D>();
		nearCloset = false;
		isHidden = false;
	}
	
	void Update () {
		Vector2 target = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * movementSpeed;
		rigidbody.velocity = Vector2.MoveTowards(rigidbody.velocity, target, acceleration * Time.deltaTime);

		if (nearCloset)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				if (isHidden)
				{
					isHidden = false; 
					transform.position = closet.transform.position + Vector3.down * 5;
					rigidbody.simulated = true;
				}
				else
				{
					isHidden = true;
					transform.position = closet.transform.position + Vector3.forward;
					rigidbody.simulated = false;
				}
				
			}
		}
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (!coll.CompareTag("closet")) return;
		nearCloset = true;
		closet = coll.gameObject;
	}
	
	void OnTriggerExit2D(Collider2D coll)
	{
		if (!rigidbody.simulated || coll.CompareTag("closet")) return;
		nearCloset = false;
		closet = null;
	}
}
