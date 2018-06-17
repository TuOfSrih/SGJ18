using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour {

	public AnimationCurve spriteOpacity;
	public AnimationCurve ambient;
	public RenderSystem rs;
	float baseAmbient;
	public float lightningTime = 30;
	public float lightningTimer = 3;
	public float lightningDuration = .5f;
	SpriteRenderer sRen;

	private AudioSource source;

	private bool isThunder;
	
	void Start () {
		sRen = GetComponent<SpriteRenderer>();
		if (rs == null) {
			Debug.LogError("No RenderSystem connected!");
		}
		baseAmbient = rs.ambient;
		source = GetComponent<AudioSource>();
		isThunder = true;
	}
	
	void Update () {

		lightningTimer -= Time.deltaTime;
		if (lightningTimer > lightningDuration) {
			rs.ambient = baseAmbient;
			sRen.color = new Color(1, 1, 1, 0);
			isThunder = true;
		} else {
			float t = 1 - lightningTimer / lightningDuration;
			rs.ambient = ambient.Evaluate(t);
			sRen.color = new Color(1, 1, 1, spriteOpacity.Evaluate(t));
			if (isThunder)
			{
				source.Play();
				isThunder = false;
			}
			
		}
		if (lightningTimer < 0) {
			lightningTimer = lightningTime * Random.Range(.8f,1.2f);
		}

	}
}
