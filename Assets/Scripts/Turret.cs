using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
	public GameObject fieldOfView;
	public MeshFilter meshFilter;
	public Mesh mesh;
	
	public SpriteRenderer renderer;

	// Use this for initialization
	void Start ()
	{
		mesh = new Mesh();
		meshFilter.mesh = mesh;
		
		renderer = GetComponent<SpriteRenderer>();
		renderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
