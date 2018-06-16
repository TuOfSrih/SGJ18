using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Renderer : MonoBehaviour {

    Shader replacement;
    RenderTexture normalTexture;
    public PlayerMovement playermov;
    public Camera normalCam;

    public int coneAngle = 15;
    public float length = 1;


    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        RenderTexture albedo = source;
        Mesh mesh = assembleMesh();
        normalCam.Render();

    }
    private Mesh assembleMesh() {
        Vector2 forward = playermov.gameObject.transform.forward;
        float angle = Mathf.Atan2(forward.y, forward.x);
        Vector3[] vertices = new Vector3[coneAngle * 2 + 2];
        for (int i = -coneAngle; i <= coneAngle; i++) {
            Vector3 cast = new Vector2(Mathf.Cos(angle + i), Mathf.Sin(angle + i));
            RaycastHit2D hit = Physics2D.Raycast(playermov.gameObject.transform.position, cast, length);
            if (hit.collider == null) vertices[i + coneAngle] = playermov.gameObject.transform.position + cast.normalized * length;
            else {
                vertices[i + coneAngle] = new Vector2(hit.transform.position.x, hit.transform.position.y);
            }
        }
        vertices[vertices.Length - 1] = playermov.gameObject.transform.position;
        int[] triangles = new int[2 * coneAngle];
        for (int i = 0; i < 2 * coneAngle; i++) {
            triangles[3 * i] = i;
            triangles[3 * i + 1] = i + 1;
            triangles[3 * i + 2] = vertices.Length - 1;
        }
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        return mesh;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
