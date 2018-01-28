using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.XR.WSA;

public class Enemy : MonoBehaviour
{

	private SpriteRenderer renderer;
	private LineRenderer lineRenderer;  // enemy's laser
	private Level level;
	
	void Start ()
	{
		renderer = GetComponent<SpriteRenderer>();
		lineRenderer = GetComponent<LineRenderer>();
		// I could not figure out how to set up line material in the
		// unity editor.
		lineRenderer.material = new Material (Shader.Find("Particles/Additive"));
		lineRenderer.SetColors(Color.red, Color.red);
		lineRenderer.enabled = false;
		
		// This is a very bad pattern IMO
		// probably best to make level a singleton object;
		level = FindObjectOfType<Level>();
	}
	
	public void Die()
	{
		renderer.enabled = false;
	}

	private void FixedUpdate()
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position, level.player.transform.position - transform.position);

		if (hit.collider.gameObject.CompareTag("Player"))
		{
			Player player = hit.collider.gameObject.GetComponent<Player>();
			lineRenderer.enabled = true;
			lineRenderer.SetPositions(
				new []{transform.position, level.player.transform.position}
			);
			player.Die();
		}
	}
}
