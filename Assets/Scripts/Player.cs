using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float speed = 100000.0F;
	public MeshFilter meshFilter;
	public Mesh mesh;
	private Rigidbody2D rigidbody;
	void Start ()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		mesh = new Mesh();
		meshFilter.mesh = mesh;
	}
	
	void FixedUpdate () {
		var horizontal = Input.GetAxis("Horizontal") * speed;
		var vertical = Input.GetAxis("Vertical") * speed;
		rigidbody.velocity = new Vector2(horizontal, vertical);
	}
}
