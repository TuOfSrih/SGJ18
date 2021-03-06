﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayLight2D : Light2D {

	public int coneAngle = 15;
	public float rayStartOffset = 1.5f;
	public int raysPerDeg = 2;
    public float minDistance;
   

	public override void Render(Material mat) {
        mat.SetFloat("_MinDistance", minDistance);
        mat.SetFloat("_Angle", coneAngle);
		base.Render(mat);
		//Debug.Log("Rendering");
		Graphics.DrawMeshNow(assembleMesh(), Vector3.zero, Quaternion.identity);
	}

	private Mesh assembleMesh() {
		Vector2 forward = facing;
		float angle = Mathf.Atan2(forward.y, forward.x);
		Vector3[] vertices = new Vector3[coneAngle * raysPerDeg * 2 + 2];
        //Vector2[] uv = new Vector2[coneAngle * raysPerDeg * 2 + 2];
		for (int i = 0; i <= coneAngle * 2 * raysPerDeg; i++) {
			float ang = angle + ((float)i / raysPerDeg - coneAngle) * Mathf.Deg2Rad;
			Vector3 cast = new Vector2(Mathf.Cos(ang), Mathf.Sin(ang));

			//-513 = alles - physicalnolight layer
			RaycastHit2D hit = Physics2D.Raycast(position + rayStartOffset * cast.normalized, cast, maxDistance - rayStartOffset, -513);
			if (hit.collider == null) {
				vertices[i] = position + cast.normalized * maxDistance;
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
        //mesh.u
		mesh.triangles = triangles;
		mesh.RecalculateBounds();
		return mesh;
	}
}
