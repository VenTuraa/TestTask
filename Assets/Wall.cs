using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
	void OnTriggerEnter(Collider col)
	{
		if (col.transform.tag == "Cube")
		{
			Time.timeScale = 0;
			GameController.GetInstance.txtCountDownTime.gameObject.SetActive(true);
			GameController.GetInstance.btnRestart.gameObject.SetActive(true);
			GameController.GetInstance.txtCountDownTime.text = "GAME OVER";
		}
	}
}
