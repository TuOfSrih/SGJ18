using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayLight2D : Light2D {

	public int coneAngle = 15;
	public float length = 5;
	public float rayStartOffset = 1.5f;
	public int raysPerDeg = 2;

	public override void Render() {
		base.Render();
		//Debug.Log("Rendering");
		Graphics.DrawMeshNow(assembleMesh(), Vector3.zero, Quaternion.identity);
	}

	private Mesh assembleMesh() {
		Vector2 forward = facing;
		float angle = Mathf.Atan2(forward.y, forward.x);
		Vector3[] vertices = new Vector3[coneAngle * raysPerDeg * 2 + 2];
		for (int i = 0; i <= coneAngle * 2 * raysPerDeg; i++) {
			float ang = angle + ((float)i / raysPerDeg - coneAngle) * Mathf.Deg2Rad;
			Vector3 cast = new Vector2(Mathf.Cos(ang), Mathf.Sin(ang));
			RaycastHit2D hit = Physics2D.Raycast(position + rayStartOffset * cast.normalized, cast, length - rayStartOffset);
			if (hit.collider == null) {
				vertices[i] = position + cast.normalized * length;
			} else {
				vertices[i] = hit.point;
			}
		}
		vertices[vertices.Length - 1] = position;

		int[] triangles = new int[6 * raysPerDeg * coneAngle];
		for (int i = 0; i < 2 * raysPerDeg * coneAngle; i++) {
			triangles[3 * i + 0] = i + 1;
			triangles[3 * i + 1] = i;
			triangles[3 * i + 2] = vertices.Length - 1;
		}
		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.RecalculateBounds();
		return mesh;
	}
}
