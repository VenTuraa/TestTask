using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFieldController : MonoBehaviour
{
	private float speed = 2.5f;
	private int endPoint = -55;

	[HideInInspector]
	public List<Vector3> listDirections = new List<Vector3>()
	{
		new Vector3(0,0,1),     //left
		new Vector3(0,0,-1),    //right
		new Vector3(1,0,0),     // up
		new Vector3(-1,0,0)     //down
	};

	public static GameFieldController GetInstance { get; private set; }

	private static int _levelLength = 61;  //Length of field

	Cube[,] listCubes = new Cube[_levelLength, 5];  // all cubes

	public GameObject defaultCube;                  // cube prefab

	[Range(0, 100)]
	public int _complexity = 85; // complexity

	public Material   // cube materials
		matDefault,
		matUpperCube;

	private Vector3 startPosition; //start position of field

	System.Random rnd = new System.Random();
	private void Awake()
	{
		if (GetInstance != null && GetInstance != this)
			Destroy(this.gameObject);
		else
			GetInstance = this;

		for (int i = 0; i < _levelLength; i++)
			for (int j = 0; j < 5; j++)
				listCubes[i, j] = Instantiate(defaultCube, new Vector3(i, 0, j), Quaternion.identity, this.transform).GetComponent<Cube>();
	}

	private void Start()
	{
		startPosition = transform.position;
		setupCubes();
	}

	public void setupCubes()
	{
		for (int i = 0; i < _levelLength; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				if (i > 5 && i < _levelLength - 3)
				{
					int random = rnd.Next(0, 100);

					if (random > _complexity && !listCubes[i, j].covered && !listCubes[i, j].upper)
					{
						int rndDirection = 0;
						while (!listCubes[i, j].upper)
						{
							rndDirection = rnd.Next(0, listDirections.Count);
							switch (rndDirection)
							{
								case 0:
									{
										if (j != 4)
										{
											if (listCubes[i, j].transform.position + listDirections[rndDirection] ==
												listCubes[i, j + 1].transform.position && !listCubes[i, j + 1].covered)
											{
												listCubes[i, j].upper = true;
												listCubes[i, j + 1].covered = true;
											}
										}
									}
									break;
								case 1:
									{
										if (j != 0)
										{
											if (listCubes[i, j].transform.position + listDirections[rndDirection] ==
												listCubes[i, j - 1].transform.position && !listCubes[i, j - 1].covered)
											{
												listCubes[i, j].upper = true;
												listCubes[i, j - 1].covered = true;
											}
										}
									}
									break;
								case 2:
									{
										if (listCubes[i, j].transform.position + listDirections[rndDirection] == listCubes[i + 1, j].transform.position && !listCubes[i + 1, j].covered)
										{
											listCubes[i, j].upper = true;
											listCubes[i + 1, j].covered = true;
										}
									}
									break;
								case 3:
									{
										if (listCubes[i, j].transform.position + listDirections[rndDirection] == listCubes[i - 1, j].transform.position && !listCubes[i - 1, j].covered)
										{
											listCubes[i, j].upper = true;
											listCubes[i - 1, j].covered = true;
										}
									}
									break;
							}
						}

						listCubes[i, j].transform.position = new Vector3(listCubes[i, j].transform.position.x, 1, listCubes[i, j].transform.position.z) + listDirections[rndDirection];
						listCubes[i, j].GetComponent<Renderer>().material = matUpperCube;
						listCubes[i, j].preferVector = -listDirections[rndDirection];
					}
				}
			}
		}
	}

	public IEnumerator StartMove()
	{
		while (transform.position.x > endPoint)
		{
			transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
			yield return new WaitForEndOfFrame();
		}
		GameController.GetInstance.btnRestart.gameObject.SetActive(true);
		GameController.GetInstance.txtCountDownTime.gameObject.SetActive(true);
		GameController.GetInstance.txtCountDownTime.text = "COMPLETE!";
		yield return 0;
	}

	public void Refresh()
	{
		for (int i = 0; i < _levelLength; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				listCubes[i, j].covered = false;
				listCubes[i, j].GetComponent<Renderer>().material = matDefault;
				listCubes[i, j].upper = false;
				listCubes[i, j].transform.position = new Vector3(i, 0, j);
			}
		}
		setupCubes();
		transform.position = startPosition;
	}
}
