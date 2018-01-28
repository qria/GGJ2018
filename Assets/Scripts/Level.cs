using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
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
	public List<Wall> walls;
	public Player player;
	
	private VisibilityPolygonCSharp<Vector2> visibilityPolygonCalculator;
	private Text gameOverText;
	
	void Start ()
	{
		walls = new List<Wall>(FindObjectsOfType<Wall>());
		player = FindObjectOfType<Player>();
		
		visibilityPolygonCalculator = new VisibilityPolygonCSharp<Vector2>(new Vector2Adapter());

		gameOverText = FindObjectOfType<Text>();
		gameOverText.enabled = false;
	}

	void Update()
	{
		// Render Player Field of View
		var polygons = walls.Select(wall => wall.points).ToArray();
		var segments = VisibilityPolygonCSharp<Vector2>.ConvertToSegments(polygons);
		// `player.transform.right` is the 'front' direction
		// Cheesy hack to restrict player's field of view
		segments.Add(new Segment<Vector2>(
			player.transform.position - player.transform.right * 0.001F,
			player.transform.position + player.transform.right + player.transform.up
		));
		segments.Add(new Segment<Vector2>(
			player.transform.position - player.transform.right * 0.001F,
			player.transform.position + player.transform.right - player.transform.up
		));
		
		segments = visibilityPolygonCalculator.BreakIntersections(segments);
		var visibility = visibilityPolygonCalculator.Compute(player.transform.position, segments);
		
		// Triangulate to render. Too low level for my taste, honestly.
		Triangulator tr = new Triangulator(visibility.ToArray());
		int [] triangles = tr.Triangulate();
		player.mesh.Clear();
		player.mesh.vertices = visibility.Select(v => (Vector3)v).ToArray();
		player.mesh.triangles = triangles;
		player.mesh.RecalculateNormals();
	}

	public void GameOver()
	{
		gameOverText.enabled = true;
	}
}
