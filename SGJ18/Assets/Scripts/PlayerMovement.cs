using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public static PlayerMovement instance;

	public float movementSpeed;
	public float acceleration;

	Rigidbody2D rigidbody;
	private Collider2D collider;

	private bool nearCloset;
	private bool nearDiary;
	private bool isReading;
	
	

	public bool isHidden;
	public bool diaryIsRead;

	private GameObject closet;

	public DialogueTrigger diary;
	public DialogueManager dialogue;

	[HideInInspector]
	public Vector2 facing;

	public RayLight2D rayLight;

	void Start () {
		instance = this;
		rigidbody = GetComponent<Rigidbody2D>();
		collider = GetComponent<Collider2D>();
		diaryIsRead = false;
		nearCloset = false;
		nearDiary = false;
		isHidden = false;
	}
	
	void Update () {
		Vector2 target = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * movementSpeed;
		rigidbody.velocity = Vector2.MoveTowards(rigidbody.velocity, target, acceleration * Time.deltaTime);
		facing = (Vector2)UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
		if (rayLight != null) {
			rayLight.position = transform.position;
			rayLight.facing = facing;
		}


		if (nearCloset)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				if (isHidden)
				{
					isHidden = false; 
					transform.position = closet.transform.position + Vector3.down * 4.5f;
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
		
		if (nearDiary)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				if (isReading)
				{
					dialogue.DisplayNextSentence();
					isReading = false;
				}
				else
				{
					diary.TriggerDialogue();
					diaryIsRead = true;
					isReading = true;
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.CompareTag("diary"))
		{
			nearDiary = true;
		}
		
		
		if (!coll.CompareTag("closet")) return;
		nearCloset = true;
		closet = coll.gameObject;
	}
	
	void OnTriggerExit2D(Collider2D coll)
	{
		Debug.Log("first");
		if (coll.CompareTag("diary"))
		{
			nearDiary = false;
		}

		if (rigidbody.simulated && coll.CompareTag("closet"))
		{
			Debug.Log("second");
			nearCloset = false;
			closet = null;
		}
	}
}