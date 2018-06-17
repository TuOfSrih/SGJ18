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

	public Animator animator;
	public Animator fadeInAnim;
	private bool test;

	public AudioClip[] audio;
	private AudioSource[] source;
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
	private RayLight2D flashlight;

	private MusicManager music;

	void Start () {
		instance = this;
		rigidbody = GetComponent<Rigidbody2D>();
		collider = GetComponent<Collider2D>();
		diaryIsRead = false;
		nearCloset = false;
		nearDiary = false;
		isHidden = false;
		isReading = true;
		music = GameObject.FindObjectOfType<MusicManager>();
		wallbump = 0;
		source = GetComponents<AudioSource>();
		test = true;
		realDistance = (boss.transform.position - transform.position).magnitude;
		distance = 0;
		rad = 0.0f;
		gyro = new Vector3(0, 0, 0);
		accel = new Vector3(0, 0, 0);
		// get the public Joycon array attached to the JoyconManager in scene
		joycons = JoyconManager.Instance.j;
		j = joycons [jc_ind];

        flashlight = GetComponent<RayLight2D>();
		StartCoroutine("HeartBeat");
	}
	
	void Update () {
		if (test && (j.GetButtonDown(Joycon.Button.DPAD_DOWN)
		             || j.GetButtonDown(Joycon.Button.DPAD_UP)
		             || j.GetButtonDown(Joycon.Button.DPAD_RIGHT)
		             || j.GetButtonDown(Joycon.Button.DPAD_LEFT)))
		{
			Debug.Log("hö");
			fadeInAnim.SetBool("isTriggered", true);
			isReading = false;
			test = false;
		}
		
		realDistance = (boss.transform.position - transform.position).magnitude;
		if (realDistance > radius)
		{
			distance = 0;
			music.source.volume = 0.05f;
		}
		else
		{
			distance = 1 - (realDistance / radius) * 0.5f;
			music.source.volume = 0.015f;
		}

		if (diaryIsRead)
		{
			distance = 0;
		}
		
		if (j.GetButtonDown(Joycon.Button.SHOULDER_2))
		{
			j.Recenter();
		}



		if (!isReading)
		{
			if (j.GetStick()[0] != 0 || j.GetStick()[1] != 0)
			{
				animator.SetBool("isMoving", true);
			}
			else
			{
				animator.SetBool("isMoving", false);
			}

			if (j.isLeft)
			{
				Vector2 targetDir = new Vector2(-j.GetStick()[1], j.GetStick()[0]);
				Vector2 target = targetDir * movementSpeed;
				rigidbody.velocity = Vector2.MoveTowards(rigidbody.velocity, target, acceleration * Time.deltaTime);
				//facing = (Vector2) UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition) -
				//(Vector2) transform.position;
				accel = j.GetAccel();
				facingLerp = facing;
				facing = Vector2.Lerp(facingLerp, new Vector2(accel.x, -accel.y), 0.1f);
			
				//facing = new Vector2(Mathf.Sin(rad / 180 * Mathf.PI), Mathf.Cos(rad / 180 * Mathf.PI)) * 3 + (Vector2) transform.position;
				//Debug.DrawLine(facing, transform.position, Color.green);

				
				targetDir = targetDir.normalized;
				animator.SetFloat("x", targetDir.x);
				animator.SetFloat("y", targetDir.y);
				
			}
			else
			{
				accel = j.GetAccel();
				Vector2 targetDir = new Vector2(j.GetStick()[1], -j.GetStick()[0]);
				Vector2 target = targetDir * movementSpeed;
				rigidbody.velocity = Vector2.MoveTowards(rigidbody.velocity, target, acceleration * Time.deltaTime);
				//facing = (Vector2) UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition) -
				//(Vector2) transform.position;
				facingLerp = facing;
				facing = Vector2.Lerp(facingLerp, new Vector2(-accel.x, accel.y), 0.1f);
				//facing = new Vector2(Mathf.Sin(rad / 180 * Mathf.PI), Mathf.Cos(rad / 180 * Mathf.PI)) * 3;
				//Debug.DrawLine(facing, transform.position, Color.green);
				targetDir = targetDir.normalized;
				animator.SetFloat("x", targetDir.x);
				animator.SetFloat("y", targetDir.y);
				
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
                    flashlight.enabled = true;
				}
				else
				{
					isHidden = true;
					transform.position = closet.transform.position + Vector3.forward;
					rigidbody.simulated = false;
                    flashlight.enabled = false;
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
					animator.SetBool("isMoving", false);
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
		
		Debug.Log("hö");
		if (coll.CompareTag("boss"))
		{
			if (diaryIsRead)
			{
				//TODO: übergang und animation
				music.level1 = true;
				StopAllCoroutines();
				StartCoroutine("waitASec");
			}
			else
			{
				isReading = true;
				//TODO: animation
				StopAllCoroutines();
				StartCoroutine("waitASec");
			}
		}
		if (!coll.CompareTag("closet")) return;
		nearCloset = true;
		closet = coll.gameObject;
	}

	IEnumerator waitASec()
	{
		yield return new WaitForSeconds(10);
		j.SetRumble(0, 0, 0, 0);
		Application.LoadLevel(0);
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
			source[1].Play();
			source[1].pitch = 1 + (distance * 0.5f);
			yield return new WaitForSeconds(0.30f- (distance * 0.05f));
			j.SetRumble(200, 250, 0.2f + (distance * 0.1f), 30 - (int)(distance * 20f));
			yield return new WaitForSeconds(0.15f - (distance * 0.03f));
			j.SetRumble(100, 150, 0.2f + (distance * 0.1f), 30 - (int)(distance * 20f));
			yield return new WaitForSeconds(0.15f - (distance * 0.03f));
			j.SetRumble(200, 250, 0.05f + (distance * 0.3f), 50);
			if (distance == 0)
			{
				yield return new WaitForSeconds(1.0f);
			}
			else
			{
				yield return new WaitForSeconds(1f - (distance * 0.8f));
			}
		}
	}

	private int wallbump;

	void OnCollisionEnter2D(Collision2D other)
	{
		source[0].clip = audio[wallbump];
		source[0].Play();
		wallbump = (wallbump + 1) % 5;
	}
}