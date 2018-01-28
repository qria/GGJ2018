using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour
{

	public float shotDuration;
	public float cooldown;
	
	private LineRenderer lineRenderer;
	private bool isShooting;
	private float lastShotTime;

	void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();
		// I could not figure out how to set up line material in the
		// unity editor.
//		lineRenderer.material = new Material (Shader.Find("Particles/Additive"));  // This don't work in webgl
		lineRenderer.SetColors(Color.white, Color.white);

		isShooting = false;
		lineRenderer.enabled = false;
		lastShotTime = -9999F;
	}

	public void fire(Vector3 facingDirection)
	{
		if (isShooting)
		{
			return;
		}

		if (Time.time - lastShotTime < cooldown)
		{
			return;
		}
		
		// Pew pew!
		isShooting = true;
		lineRenderer.enabled = true;
		lastShotTime = Time.time;
		
		var laserPath = new List<Vector3>();
		
		// Note position is NOT player's position
		RaycastHit2D firstHit = Physics2D.Raycast(transform.position, facingDirection);
		RaycastHit2D lastHit = firstHit;
		
		laserPath.Add(transform.position);
		laserPath.Add(firstHit.point);
		
		if (firstHit.collider.gameObject.CompareTag("Wall"))
		{
			var reflectDirection = Vector2.Reflect(facingDirection, firstHit.normal);
			RaycastHit2D secondHit = Physics2D.Raycast(firstHit.point, reflectDirection);
			lastHit = secondHit;
			
			laserPath.Add(secondHit.point);
		}

		if (lastHit.collider.gameObject.CompareTag("Enemy"))
		{
			Enemy enemy = lastHit.collider.gameObject.GetComponent<Enemy>();
			enemy.Die();
		}
		
		
		lineRenderer.SetVertexCount(laserPath.Count);
		lineRenderer.SetPositions(laserPath.ToArray());
	}

	private void FixedUpdate()
	{
		// unfire after time
		if (Time.time - lastShotTime > shotDuration)
		{
			isShooting = false;
			lineRenderer.enabled = false;
		}
	}
}
