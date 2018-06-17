using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderSystem : MonoBehaviour {

	public Shader fxBloom;
	public Shader lighting;
    public Shader replacement;
    public RenderTexture normalTexture;
    public Camera normalCam;
    public Shader combine;
    public Material ambientMat;
    public Material Transitionmaterial;
    public Material BlurMaterial;
    [Range(0, 0.5f)]
    public float Magnitude;
    [Range(0, 1)]
    public float ambient;
    public float fadeTime;

	public float blurSize = 10;
	private float blurIntensity = 1;

	RenderTexture lightTexture;

    private float[] kernel = { 0.015625f, 0.09375f, 0.234375f, 0.3125f };

	Material clearMat;

	void Start() {
		lightTexture = new RenderTexture(Screen.width, Screen.height, 0);
		//normalTexture = new RenderTexture(Screen.width, Screen.height, 0);

		clearMat = new Material(replacement);
		clearMat.SetColor("_Color", Color.black);

        startFadeIn();
    }

	private void OnRenderImage(RenderTexture source, RenderTexture destination) {
		

		RenderTexture albedo = source;


		Graphics.SetRenderTarget(lightTexture);
		//Clear Current texture
		Graphics.Blit(source, lightTexture, clearMat);

		//Set material to light render material
		Material mat = new Material(lighting); //TODO
		mat.SetPass(0);
        mat.SetTexture("_MainTex", source);

		//Render Lights
		foreach (Light2D l in Light2D.lights) {
            if(l != null && l.enabled) l.Render(mat);         
		}


		
		RenderTexture tempTex = RenderTexture.GetTemporary(albedo.width, albedo.height);
		
		//Blur
		Material blurMat = new Material(fxBloom);
		blurMat.SetFloat("_blurSize", blurSize);
		blurMat.SetFloat("_intensity", blurIntensity);

		blurMat.SetFloat("_XDir", 0);
		Graphics.Blit(lightTexture, tempTex, blurMat);

		blurMat.SetFloat("_XDir", 1);
		Graphics.Blit(tempTex, lightTexture, blurMat);




		Material combineMat = new Material(combine);
		combineMat.SetTexture("_MainTex", albedo);
		combineMat.SetTexture("_LightingTex", lightTexture);
		combineMat.SetTexture("_Normals", normalTexture);
		//combineMat.SetFloat("_Ambient", ambient);


		//Combine Lights
		RenderTexture fY = RenderTexture.GetTemporary(albedo.width, albedo.height);
		Graphics.Blit(source, fY, combineMat);



		//Add ambient
		ambientMat.SetTexture("_Albedo", albedo);
		ambientMat.SetFloat("_Ambient", ambient);
		Graphics.Blit(fY, tempTex, ambientMat);

		Transitionmaterial.SetFloat("_Magnitude", Magnitude);
		//Transitionmaterial.SetTexture("_MainTex", albedo);
		Graphics.Blit(tempTex, source, Transitionmaterial);
		RenderTexture.ReleaseTemporary(fY);
		RenderTexture.ReleaseTemporary(tempTex);
		Graphics.Blit(source, destination);
		//RenderTexture.ReleaseTemporary(next);
		//RenderTexture.ReleaseTemporary(next2);

		//DEBUGGING STUFF REMOVE THIS!!!
		if (Input.GetKey(KeyCode.Y)) {
			Graphics.Blit(lightTexture, destination);
		}
	}

    public void startFadeOut() {
        StartCoroutine(fadeOut());
    }
    public void startFadeIn() {
        StartCoroutine(fadeIn());
    }
    private IEnumerator fadeOut() {
        Magnitude = 0;
        while (Magnitude <= 0.5) {
            Magnitude += Time.deltaTime * 0.5f / fadeTime;
            yield return null;
        }
        Magnitude = 0.5f;
    }
    private IEnumerator fadeIn() {
        Magnitude = 0.5f;
        while (Magnitude > 0) {
            Magnitude -= Time.deltaTime * 0.5f / fadeTime;
            yield return null;
        }
        Magnitude = 0;
    }
        


	void OnDestroy() {
		Debug.Log("releasing render texutres");
		lightTexture.Release();
		normalTexture.Release();
	}
}
