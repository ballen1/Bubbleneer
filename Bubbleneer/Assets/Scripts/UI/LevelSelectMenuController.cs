using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelSelectMenuController : MonoBehaviour {

	public GameObject LevelSelectGrid;

	public Text Info_LevelName;
	public Text LevelDescription;
	public Button PlayButton;
	public Image LevelPreview;

	private LevelManager LM;

	private BubbleLevel[] Levels;
	public Button LevelButtonPrefab;
	private Button[] LevelButtons;

	private int SelectedLevel;

	// Use this for initialization
	void Start () {
		SelectedLevel = 0;

		LM = GameObject.FindGameObjectWithTag ("LevelManager").GetComponent<LevelManager> ();

		Levels = LM.GetBubbleLevelList ();

		LevelButtons = new Button[Levels.Length];

		for (int i = 0; i < Levels.Length; i++) {
			LevelButtons [i] = Instantiate (LevelButtonPrefab, LevelSelectGrid.transform);
			LevelButtons [i].transform.localScale = new Vector3 (1, 1, 1);
			if (Levels [i].IsComplete ()) {
				LevelButtons [i].GetComponentInChildren<Text> ().text = Levels [i].GetLevelName () + "\n(COMPLETE)";
			} else {
				LevelButtons [i].GetComponentInChildren<Text> ().text = Levels [i].GetLevelName ();
			}
			LevelButtons [i].onClick.AddListener (LevelButtonClick);
		}

		PlayButton.onClick.AddListener (PlayButtonClick);

		Info_LevelName.text = Levels [SelectedLevel].GetLevelName ();
		LevelDescription.text = Levels [SelectedLevel].GetLevelDescription ();
		LevelPreview.sprite = Levels [SelectedLevel].GetPreviewSprite ();
		LM.SetCurrentLevel (Levels [SelectedLevel].GetLevelNumber ());
	}

	void LevelButtonClick() {
		Button Selected = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();

		for (int i = 0; i < LevelButtons.Length; i++) {
			if (LevelButtons [i].GetInstanceID() == Selected.GetInstanceID()) {
				SelectedLevel = i;
				LM.SetCurrentLevel (Levels [SelectedLevel].GetLevelNumber ());
				Info_LevelName.text = Levels [SelectedLevel].GetLevelName ();
				LevelDescription.text = Levels [SelectedLevel].GetLevelDescription ();
				LevelPreview.overrideSprite = Levels [SelectedLevel].GetPreviewSprite ();
			}
		}
	}

	void PlayButtonClick() {
		Debug.Log ("Play Button");
		LM.PlayGame ();
	}

}
