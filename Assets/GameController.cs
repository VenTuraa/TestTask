using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	public static GameController GetInstance { get; private set; }
	int _countdownTime = 3;
	public Text txtCountDownTime;

	public Button btnRestart;

	void Awake()
	{
		if (GetInstance ==null)
			GetInstance = this;
	}

	void Start()
	{btnRestart.onClick.AddListener(() =>
		{
			Restart();
		});
		StartCoroutine(Countdown());
	}

	IEnumerator Countdown()
	{
		while (_countdownTime > 0)
		{
			txtCountDownTime.text = _countdownTime.ToString();
			yield return new WaitForSeconds(1f);
			_countdownTime--;
		}

		txtCountDownTime.text = "GO!";
		yield return new WaitForSeconds(1f);
		txtCountDownTime.gameObject.SetActive(false);
		StartCoroutine(GameFieldController.GetInstance.StartMove());
	}

	public void Restart()
	{
		SceneManager.LoadScene(0);
	}
}
