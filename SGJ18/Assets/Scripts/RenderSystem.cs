using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderSystem : MonoBehaviour {

    public Shader replacement;
    public RenderTexture normalTexture;
    public Camera normalCam;
    public Shader combine;

	RenderTexture lightTexture;


	Material clearMat;

	void Start() {
		lightTexture = new RenderTexture(Screen.width, Screen.height, 0);
		//normalTexture = new RenderTexture(Screen.width, Screen.height, 0);
		clearMat = new Material(Shader.Find("Unlit/Color"));
		clearMat.SetColor("_Color", Color.black);
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        RenderTexture albedo = source;


		if (normalCam != null) {
            normalCam.SetReplacementShader(replacement, "");
			normalCam.Render();
		}

		Graphics.SetRenderTarget(lightTexture);
		//Clear Current texture
		Graphics.Blit(lightTexture, lightTexture, clearMat, 0);

		//Set material to light render material
		Material mat = new Material(Shader.Find("Lighting")); //TODO
		mat.SetPass(0);

		//Render Lights
		foreach (Light2D l in Light2D.lights) {
			l.Render(mat);         
		}
        Material combineMat = new Material(combine);
        combineMat.SetTexture("_MainTex", albedo);
        combineMat.SetTexture("_LightingTex", lightTexture);
        combineMat.SetTexture("_Normals", normalTexture);

        Graphics.Blit(source, destination, combineMat);

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
