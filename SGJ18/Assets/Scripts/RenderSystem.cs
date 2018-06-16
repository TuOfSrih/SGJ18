using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderSystem : MonoBehaviour {

    Shader replacement;
    RenderTexture normalTexture;
    public Camera normalCam;

	public Texture clearTexture;
	RenderTexture lightTexture;

	Material clearMat;

	void Start() {
		lightTexture = new RenderTexture(Screen.width, Screen.height, 0);
		normalTexture = new RenderTexture(Screen.width, Screen.height, 0);
		clearMat = new Material(Shader.Find("Unlit/Color"));
		clearMat.SetColor("_Color", Color.black);
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        RenderTexture albedo = source;



		if (normalCam != null) {
			normalCam.Render();
		}

		Graphics.SetRenderTarget(lightTexture);
		//Clear Current texture
		Graphics.Blit(lightTexture, lightTexture, clearMat, 0);

		//Set material to light render material
		Material mat = new Material(Shader.Find("Standard")); //TODO
		mat.SetPass(0);

		//Render Lights
		foreach (Light2D l in Light2D.lights) {
			l.Render();
		}

		Graphics.Blit(source, destination);

		//DEBUGGING STUFF REMOVE THIS!!!
		if (Input.GetKey(KeyCode.Y)) {
			Graphics.Blit(lightTexture, destination);
		}
	}


	// Update is called once per frame
	void Update () {
		
	}

	void OnDestroy() {
		Debug.Log("releasing render texutres");
		lightTexture.Release();
		normalTexture.Release();
	}
}
