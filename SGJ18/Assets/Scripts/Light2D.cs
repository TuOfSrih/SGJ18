using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light2D : MonoBehaviour {

	public Vector3 position;
	public Vector3 facing;
    public float maxDistance = 5;

	public static List<Light2D> lights;

	void Start() {
		if (lights == null) {
			lights = new List<Light2D>();
		}
		lights.Add(this);
	}

	public virtual void Render(Material mat) {
        mat.SetFloat("_MaxLength", maxDistance);
        mat.SetVector("_LightingPos", position);
        mat.SetPass(0);
	}

}
