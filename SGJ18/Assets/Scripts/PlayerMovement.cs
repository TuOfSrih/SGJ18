using System.Collections;
using System.Collections.Generic;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	
	private List<Joycon> joycons;

	// Values made available via Unity
	public float[] stick;
	public Vector3 gyro;
	public Vector3 accel;
	public int jc_ind = 0;
	public Quaternion orientation;
	public Vector3 or;
	public GameObject boss;
	public float distance;
	public float realDistance;
	public float radius;
	
	
	

	public static PlayerMovement instance;

	public float movementSpeed;
	public float acceleration;

	Rigidbody2D rigidbody;
	private Collider2D collider;

	private bool nearCloset;
	private bool nearDiary;
	public bool isReading;
	private float rad;
	

	public bool isHidden;
	public bool diaryIsRead;

	private GameObject closet;
	

	public DialogueTrigger diary;
	public DialogueManager dialogue;

	[HideInInspector]
	public Vector2 facing;
	Vector2 facingLerp;

	public RayLight2D rayLight;

	private Joycon j;

	void Start () {
		instance = this;
		rigidbody = GetComponent<Rigidbody2D>();
		collider = GetComponent<Collider2D>();
		diaryIsRead = false;
		nearCloset = false;
		nearDiary = false;
		isHidden = false;
		realDistance = (boss.transform.position - transform.position).magnitude;
		distance = 0;
		rad = 0.0f;
		
		gyro = new Vector3(0, 0, 0);
		accel = new Vector3(0, 0, 0);
		// get the public Joycon array attached to the JoyconManager in scene
		joycons = JoyconManager.Instance.j;
		if (joycons.Count < jc_ind+1){
			Destroy(gameObject);
		}
		j = joycons [jc_ind];
		StartCoroutine("HeartBeat");
	}
	
	void Update () {
		realDistance = (boss.transform.position - transform.position).magnitude;
		if (realDistance > radius)
		{
			distance = 0;
		}
		else
		{
			distance = 1 - (realDistance / radius) * 0.5f;
		}

		if (diaryIsRead)
		{
			distance = 0;
		}
		
		if (j.GetButtonDown(Joycon.Button.SHOULDER_2))
		{
			j.Recenter();
		}

		if (j.GetButton(Joycon.Button.SL))
		{
			rad -= Time.deltaTime * 200;
		}

		if (j.GetButton(Joycon.Button.SR))
		{
			rad += Time.deltaTime * 200;
		}
		if (!isReading)
		{
			if (j.isLeft)
			{
				Vector2 target = new Vector2(j.GetStick()[0], -j.GetStick()[1]) * movementSpeed;
				rigidbody.velocity = Vector2.MoveTowards(rigidbody.velocity, target, acceleration * Time.deltaTime);
				//facing = (Vector2) UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition) -
				//(Vector2) transform.position;
				accel = j.GetAccel();
				facing = new Vector2(accel.x, -accel.y) * 2;

				//facing = new Vector2(Mathf.Sin(rad / 180 * Mathf.PI), Mathf.Cos(rad / 180 * Mathf.PI)) * 3 + (Vector2) transform.position;
				//Debug.DrawLine(facing, transform.position, Color.green);
			}
			else
			{
				accel = j.GetAccel();
				Vector2 target = new Vector2(j.GetStick()[1], -j.GetStick()[0]) * movementSpeed;
				rigidbody.velocity = Vector2.MoveTowards(rigidbody.velocity, target, acceleration * Time.deltaTime);
				//facing = (Vector2) UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition) -
				//(Vector2) transform.position;
				facingLerp = facing;
				facing = Vector2.Lerp(facingLerp, new Vector2(-accel.x, accel.y) * 20000, 0.1f);
				facing = new Vector2(Mathf.Sin(rad / 180 * Mathf.PI), Mathf.Cos(rad / 180 * Mathf.PI)) * 3;
				//Debug.DrawLine(facing, transform.position, Color.green);
			}
		}

		if (rayLight != null) {
			rayLight.position = transform.position;
			rayLight.facing = facing;
		}

		
		if (nearCloset)
		{
			if (Input.GetKeyDown(KeyCode.Space) || j.GetButtonDown (Joycon.Button.DPAD_DOWN) 
			                                    || j.GetButtonDown (Joycon.Button.DPAD_UP) 
			                                    || j.GetButtonDown (Joycon.Button.DPAD_RIGHT) 
			                                    || j.GetButtonDown (Joycon.Button.DPAD_LEFT))
			{
				rigidbody.velocity = Vector3.zero;
				if (isHidden)
				{
					isHidden = false; 
					transform.position = closet.transform.position + Vector3.down * 0.5f;
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
			if (Input.GetKeyDown(KeyCode.Space) || j.GetButtonDown (Joycon.Button.DPAD_DOWN) 
			                                    || j.GetButtonDown (Joycon.Button.DPAD_UP) 
			                                    || j.GetButtonDown (Joycon.Button.DPAD_RIGHT) 
			                                    || j.GetButtonDown (Joycon.Button.DPAD_LEFT))
			{
				rigidbody.velocity = Vector3.zero;
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
		if (coll.CompareTag("diary"))
		{
			nearDiary = false;
		}

		if (rigidbody.simulated && coll.CompareTag("closet"))
		{
			nearCloset = false;
			closet = null;
		}
	}

	IEnumerator HeartBeat()
	{
		while (true)
		{
			j.SetRumble(200, 250, 0.2f + (distance * 0.1f), 30 - (int)(distance * 20f));
			yield return new WaitForSeconds(0.15f - (distance * 0.03f));
			j.SetRumble(100, 150, 0.2f + (distance * 0.1f), 30 - (int)(distance * 20f));
			yield return new WaitForSeconds(0.15f - (distance * 0.03f));
			j.SetRumble(200, 250, 0.05f + (distance * 0.3f), 50);
			if (distance == 0)
			{
				yield return new WaitForSeconds(1.5f);
			}
			else
			{
				yield return new WaitForSeconds(1f - (distance * 0.8f));
			}
		}
	}
}