using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public float health = 4f;
	public GameObject deadEffect;

	private void OnCollisionEnter2D(Collision2D collInfo)
	{
		if (collInfo.relativeVelocity.magnitude > health)        // RelativeVelocity compare speed of 1 object to another in x & y
		{                                                       // magnitude then turn this x & y into a single number
			Die();
		}
	}

	private void Die()
	{
		Instantiate(deadEffect, transform.position, Quaternion.identity);

		Destroy(gameObject);
	}
}
