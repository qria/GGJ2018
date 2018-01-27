using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float speed = 100000.0F;
	public GameObject fieldOfView;
	public MeshFilter meshFilter;
	public Mesh mesh;
	private Rigidbody2D rigidbody;
	void Start ()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		mesh = new Mesh();
		meshFilter.mesh = mesh;
	}

	void Update()
	{
		if (Input.GetKeyDown("space"))
		{
			Debug.Log("Phew Phew!");
		}
	}
	
	void FixedUpdate () 
	{		
		// Moving
		var horizontal = Input.GetAxis("Horizontal") * speed;
		var vertical = Input.GetAxis("Vertical") * speed;
		rigidbody.velocity = new Vector2(horizontal, vertical);
		
		// Look at Mouse
		Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		float AngleRad = Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x);
		float AngleDeg = 180 / Mathf.PI * AngleRad;
		transform.rotation = Quaternion.Euler(0, 0, AngleDeg);
	}
}
