using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderSystem : MonoBehaviour {

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


	RenderTexture lightTexture;

	Material clearMat;

	void Start() {
		lightTexture = new RenderTexture(Screen.width, Screen.height, 0);
		//normalTexture = new RenderTexture(Screen.width, Screen.height, 0);
		clearMat = new Material(Shader.Find("Unlit/Color"));
		clearMat.SetColor("_Color", Color.black);

        triggerTransition();
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        RenderTexture albedo = source;


		Graphics.SetRenderTarget(lightTexture);
		//Clear Current texture
		Graphics.Blit(lightTexture, lightTexture, clearMat, 0);

		//Set material to light render material
		Material mat = new Material(Shader.Find("Lighting")); //TODO
		mat.SetPass(0);
        mat.SetTexture("_MainTex", source);

		//Render Lights
		foreach (Light2D l in Light2D.lights) {
			l.Render(mat);         
		}
        Material combineMat = new Material(combine);
        combineMat.SetTexture("_MainTex", albedo);
        combineMat.SetTexture("_LightingTex", lightTexture);
        combineMat.SetTexture("_Normals", normalTexture);
        //combineMat.SetFloat("_Ambient", ambient);


        //Combine Lights
        RenderTexture fY = RenderTexture.GetTemporary(albedo.width, albedo.height);
        Graphics.Blit(source, fY, combineMat);

        //Blur
        RenderTexture next = RenderTexture.GetTemporary(albedo.width, albedo.height);
        Graphics.Blit(fY, next);

        //Add ambient
        RenderTexture ambientTex = RenderTexture.GetTemporary(albedo.width, albedo.height);
        ambientMat.SetTexture("_Albedo", albedo);
        ambientMat.SetFloat("_Ambient", ambient);
        Graphics.Blit(next, ambientTex, ambientMat);

        Transitionmaterial.SetFloat("_Magnitude", Magnitude);
        //Transitionmaterial.SetTexture("_MainTex", albedo);
        Graphics.Blit(ambientTex, destination, Transitionmaterial);
        RenderTexture.ReleaseTemporary(fY);
        RenderTexture.ReleaseTemporary(ambientTex);
        RenderTexture.ReleaseTemporary(next);

		//DEBUGGING STUFF REMOVE THIS!!!
		if (Input.GetKey(KeyCode.Y)) {
			Graphics.Blit(lightTexture, destination);
		}
	}

    public void triggerTransition() {
        StartCoroutine(transition());
    }
    private IEnumerator transition() {
        while(Magnitude <= 0.5) {
            Magnitude += Time.deltaTime * 0.5f / fadeTime;
            yield return null;
        }
        //Switch Map here

        while(Magnitude > 0) {
            Magnitude -= Time.deltaTime * 0.5f / fadeTime;
            yield return null;
        }
        Magnitude = 0;
        
        
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
