using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class Wall : MonoBehaviour {
	/// <summary>
	/// Converts Collider2D to Mesh
	/// Source: https://answers.unity.com/questions/835675/how-to-fill-polygon-collider-with-a-solid-color.html
	/// </summary>
	PolygonCollider2D pc2 ;

	public Vector2[] points
	{
		// returns all absolutes positions of points
		get
		{
			return pc2.points.Select(point => point + (Vector2)transform.position).ToArray();
		}
	}

	void Start () {
		pc2 = gameObject.GetComponent<PolygonCollider2D>();
		//Render thing
		int pointCount = 0;
		pointCount = pc2.GetTotalPointCount();
		MeshFilter mf = GetComponent<MeshFilter>();
		Mesh mesh = new Mesh();
		Vector2[] points = pc2.points;
		Vector3[] vertices = new Vector3[pointCount];
		Vector2[] uv = new Vector2[pointCount];
		for(int j=0; j<pointCount; j++){
			Vector2 actual = points[j];
			vertices[j] = new Vector3(actual.x, actual.y, 0);
			uv[j] = actual;
		}
		Triangulator tr = new Triangulator(points);
		int [] triangles = tr.Triangulate();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uv;
		mf.mesh = mesh;
		//Render thing
	}
#if UNITY_EDITOR
	void Update(){
		if (Application.isPlaying)
			return;
		if(pc2 != null){
			Start();
		}
	}
#endif
}