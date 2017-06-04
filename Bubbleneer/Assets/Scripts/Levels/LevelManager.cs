using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	private MapBuilder MB;

	private int CurrentLevel;

	public Sprite DefaultPreviewImage;

	private BubbleLevel[] BubbleLevels;

	void Awake() {
		DontDestroyOnLoad (this);

		SceneManager.sceneLoaded += SceneSwitch;
	
		TextAsset TextFile = (TextAsset)Resources.Load("MapFiles/LevelOrder") as TextAsset;
		string LevelText = TextFile.text;

		char[] DelimiterChars = {'\n'};
		string[] LevelNames = LevelText.Split(DelimiterChars);

		BubbleLevels = new BubbleLevel[LevelNames.Length];

		CurrentLevel = 0;

		for (int i = 0; i < LevelNames.Length; i++) {
			string BubbleLevelFile = LevelNames [i];

			TextAsset DescriptionFile = (TextAsset)Resources.Load ("MapFiles/" + BubbleLevelFile.TrimEnd() + "_description") as TextAsset;

			string LevelDesc;
			if (DescriptionFile) {
				LevelDesc = DescriptionFile.text;
			} else {
				LevelDesc = "No description";
			}

			Texture2D PreviewImage = Resources.Load<Texture2D> ("MapFiles/" + BubbleLevelFile.Trim () + "_preview");
		
			Sprite PreviewSprite;

			if (PreviewImage) {
				PreviewSprite = Sprite.Create (PreviewImage, new Rect (0, 0, PreviewImage.width, PreviewImage.height),
					new Vector2 (0.5f, 0.5f));
			} else {
				PreviewSprite = DefaultPreviewImage;
			}

			string FormattedName = "";
			string[] Name = LevelNames [i].Split ('_');
			for (int j = 0; j < Name.Length; j++) {
				FormattedName += Name [j] + ' ';
			}
			FormattedName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase (FormattedName);
			BubbleLevels [i] = new BubbleLevel (BubbleLevelFile.TrimEnd(), FormattedName, i + 1, LevelDesc, PreviewSprite);
		}
	}

	public void SetCurrentLevel(string LevelName) {
		for (int i = 0; i < BubbleLevels.Length; i++) {
			if (BubbleLevels [i].GetLevelName() == LevelName) {
				CurrentLevel = i;
			}
		}
	}

	public void SetCurrentLevel(int LevelNumber) {
		CurrentLevel = LevelNumber-1;
	}
		
	public bool DoesNextLevelExist() {
		return (CurrentLevel+1 < BubbleLevels.Length);
	}

	private void SceneSwitch(Scene NewScene, LoadSceneMode Mode) {
		if (NewScene.name == "GameScene") {
			MB = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<MapBuilder> ();
			MB.ParseMap (BubbleLevels[CurrentLevel]);
		}
		if (NewScene.name == "MainMenu") {
			MB = null;
		}
	}

	public BubbleLevel[] GetBubbleLevelList() {
		return BubbleLevels;
	}

	public void RestartLevel() {
		SceneManager.LoadScene ("GameScene");
	}

	public void PlayNextLevel() {
		if (DoesNextLevelExist ()) {
			CurrentLevel++;
			SceneManager.LoadScene ("GameScene");
		} else {
			SceneManager.LoadScene ("MainMenu");
		}
	}

	public void ReturnToMainMenu() {
		SceneManager.LoadScene ("MainMenu");
	}

	public void PlayGame() {
		SceneManager.LoadScene ("GameScene");
	}

	public void MarkCurrentLevelComplete() {
		BubbleLevels [CurrentLevel].CompleteLevel ();
	}

	public bool IsCurrentLevelComplete() {
		return BubbleLevels [CurrentLevel].IsComplete ();
	}
}
