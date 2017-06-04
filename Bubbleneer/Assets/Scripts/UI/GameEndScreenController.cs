using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEndScreenController : MonoBehaviour {

	public Button RestartButton;
	public Button LevelSelectButton;
	public Button ContinueButton;

	private LevelManager LM;
	private RoundSystem RM;

	void Start() {
		RestartButton.onClick.AddListener (RestartButtonClick);
		LevelSelectButton.onClick.AddListener (LevelSelectButtonClick);
		ContinueButton.onClick.AddListener (ContinueButtonClick);

		LM = GameObject.FindGameObjectWithTag ("LevelManager").GetComponent<LevelManager> ();
		RM = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<RoundSystem> ();

	}

	void Update() {
		if (!RM.CanPlayerProgress ()) {
			ContinueButton.interactable = false;
		} else {
			ContinueButton.interactable = true;
		}
	}

	private void RestartButtonClick() {
		LM.RestartLevel ();
	}

	private void LevelSelectButtonClick() {
		LM.ReturnToMainMenu ();
	}

	private void ContinueButtonClick() {
		LM.PlayNextLevel ();
	}

}
