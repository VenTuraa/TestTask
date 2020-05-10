using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cube : MonoBehaviour
{
	public bool upper = false;   //that cube is over
	public bool covered = false; //that cube is under
	public Vector3 preferVector; //which way to swipe

	private RaycastHit hit;
	private Vector3
		startPosition = Vector3.zero,
		endPosition,
		swipeVector = Vector3.zero;

	void FixedUpdate()
	{
		MouseButtonDown();
		MouseButotnUp();
	}

	private void MouseButtonDown()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit))
			{
				if (transform == hit.collider.transform && upper)
				{
					startPosition = Input.mousePosition;
				}
			}
		}
	}

	private void MouseButotnUp()
	{
		if (startPosition != Vector3.zero)
		{
			if (Input.GetMouseButtonUp(0))
			{
				endPosition = Input.mousePosition;
				Vector3 directionVector = endPosition - startPosition;

				directionVector.Normalize();
				if (startPosition != endPosition)
				{
					if (Mathf.Abs(directionVector.x) > Mathf.Abs(directionVector.y))
					{
						if (directionVector.x > 0)
						{
							swipeVector = GameFieldController.GetInstance.listDirections[1];

							Debug.Log("right");
						}
						else
						{
							swipeVector = GameFieldController.GetInstance.listDirections[0];
							Debug.Log("left");
						}
					}
					else
					{
						if (directionVector.y < 0)
						{
							swipeVector = GameFieldController.GetInstance.listDirections[3];
							Debug.Log("down");
						}
						else
						{
							swipeVector = GameFieldController.GetInstance.listDirections[2];
							Debug.Log("up");
						}
					}

					if (swipeVector == preferVector)
						transform.position = new Vector3(transform.position.x + swipeVector.x, 0, transform.position.z + swipeVector.z);
				}
				startPosition = Vector3.zero;
				hit = new RaycastHit();
			}
		}
	}
}
