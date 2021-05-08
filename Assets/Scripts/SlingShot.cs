using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingShot : MonoBehaviour
{
    public LineRenderer[] lineRenderers;
    public Transform[] stripPositions;
    public Transform center;
    public Transform idlePos;
    public GameObject birdPrefab;

    public float maxLength;
    public float bottomBoundary;
    public float birdPosOffset;
    public float force;

    private Rigidbody2D birdRb;
    private Collider2D birdColl;

    private Vector3 currentPos;

    private bool isMouseDown;

    void Start()
    {
        CreateBird();
        lineRenderers[0].positionCount = 2;                                     // each lineRenderer have 2 point
        lineRenderers[1].positionCount = 2;
        lineRenderers[0].SetPosition(0, stripPositions[0].position);            // link LR pos to the strip pos
        lineRenderers[1].SetPosition(0, stripPositions[1].position);
    }

    
    void Update()
    {
		if (isMouseDown)
		{
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10;
            currentPos = Camera.main.ScreenToWorldPoint(mousePos);
            currentPos = center.position + Vector3.ClampMagnitude(currentPos - center.position, maxLength);         // clamp the max length slingshot can screcth
            currentPos = ClampBoundary(currentPos);                                                                 // clmap the pos to not pass bottomBoundray

            SetStrips(currentPos);

			if (birdColl)
			{
                birdColl.enabled = true;
			}
		}
		else
		{
            ResetStrips();      // keep the strip at idle pos
		}
    }

    private void CreateBird()
	{
        birdRb = Instantiate(birdPrefab).GetComponent<Rigidbody2D>();
        birdColl = birdRb.GetComponent<Collider2D>();
        birdColl.enabled = false;                                           // stop bird from collide with the slingshot when instantiate
        
    }

	private Vector3 ClampBoundary(Vector3 vector)
	{
        vector.y = Mathf.Clamp(vector.y, bottomBoundary, 1000);
        return vector;
	}

	private void OnMouseDown()
	{
        isMouseDown = true;
        birdRb.isKinematic = true;
    }

	private void OnMouseUp()
	{
        isMouseDown = false;
        birdRb.isKinematic = false;
        Shoot();
        
	}

	private void Shoot()
	{
        
        Vector3 birdForce = (currentPos - center.position) * force * -1;            // add force direct toward center pos
        birdRb.velocity = birdForce;

        birdRb = null;
        birdColl = null;
        Invoke("CreateBird", 2);                                                    // reset and create new bird

	}

	void ResetStrips()
	{
        currentPos = Vector3.Slerp(currentPos,idlePos.position,50f * Time.deltaTime);
        SetStrips(currentPos);
	}

    void SetStrips(Vector3 pos)
	{
        lineRenderers[0].SetPosition(1, pos);
        lineRenderers[1].SetPosition(1, pos);

		if (birdRb)
		{
            Vector3 dir = pos - center.position;
            birdRb.transform.position = pos + dir.normalized * birdPosOffset;           // keep the bird pos base on the offset
            birdRb.transform.right = -dir.normalized;                                   // keep the bird to alway face the center
        }
	}
}
