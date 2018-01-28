using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Timeline;

public class Player : MonoBehaviour {

	public float speed = 30000.0F;
	public GameObject fieldOfView;
	public MeshFilter meshFilter;
	public Mesh mesh;
	public LaserGun laserGun;
	
	private Rigidbody2D rigidbody;
	private SpriteRenderer renderer;
	private Level level;
	
	void Start ()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		renderer = GetComponent<SpriteRenderer>();
		mesh = new Mesh();
		meshFilter.mesh = mesh;

		level = FindObjectOfType<Level>();
	}

	void Update()
	{
		if (Input.GetButton("Fire1"))
		{
			// Note laser's path is ddetermined at the start of the shooting
			// to prevent "scrubbing" cheats
			var facingRadian = (float)(transform.rotation.eulerAngles.z / 180 * Math.PI);
			var facingDirection = new Vector2(Mathf.Cos(facingRadian), Mathf.Sin(facingRadian));
			laserGun.fire(facingDirection);
		}
	}
	
	void FixedUpdate () 
	{		
		// Moving
		var horizontal = Input.GetAxis("Horizontal") * speed;
		var vertical = Input.GetAxis("Vertical") * speed;
		if (Input.GetButton("Fire3")) 
		{
			// Press Shift to run
			horizontal *= 1.5f;
			vertical *= 1.5f;
		}
		rigidbody.velocity = new Vector2(horizontal, vertical);
		
		// Look at Mouse
		Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		float AngleRad = Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x);
		float AngleDeg = 180 / Mathf.PI * AngleRad;
		transform.rotation = Quaternion.Euler(0, 0, AngleDeg);
	}
	
	public void Die()
	{
		rigidbody.velocity = new Vector2();  // Stop dead body
		renderer.enabled = false;
		level.GameOver();
		enabled = false;
	}
}
