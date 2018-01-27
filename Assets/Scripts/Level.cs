using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VisibilityPolygonCSharp;

public class Vector2Adapter : PointAdapter<Vector2>
{
	/// <summary>
	/// Supply interface for our position struct.
	/// Note the float -> double conversion.
	/// </summary>
	/// <returns></returns>
	public override double GetX(Vector2 v)
	{
		return v.x;
	}
	public override double GetY(Vector2 v)
	{
		return v.y;
	}

	public override Vector2 Create(double x, double y)
	{
		return new Vector2((float)x, (float)y);
	}
}

public class Level : MonoBehaviour
{
	private List<Wall> walls;
	private Player player;
	
	private VisibilityPolygonCSharp<Vector2> visibilityPolygonCalculator;
	
	void Start ()
	{
		walls = new List<Wall>(FindObjectsOfType<Wall>());
		player = FindObjectOfType<Player>();
		
		visibilityPolygonCalculator = new VisibilityPolygonCSharp<Vector2>(new Vector2Adapter());
	}

	void Update()
	{
		var polygons = walls.Select(wall => wall.points).ToArray();
		var segments = VisibilityPolygonCSharp<Vector2>.ConvertToSegments(polygons);
		Debug.Log(player.transform.position);
		var visibility = visibilityPolygonCalculator.Compute(player.transform.position, segments);
		// Revert relative positioning
		visibility = visibility.Select(point => (Vector2)player.transform.InverseTransformPoint(point)).ToList();
		// Triangulate to render. Too low level for my taste, honestly.
		Triangulator tr = new Triangulator(visibility.ToArray());
		int [] triangles = tr.Triangulate();
		player.mesh.Clear();
		player.mesh.vertices = visibility.Select(v => (Vector3)v).ToArray();
		player.mesh.triangles = triangles;
		player.mesh.RecalculateNormals();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
	}
}
