using System;
using UnityEngine;

public class BubbleLevel
{

	private string LevelFile;
	private string LevelName;
	private int LevelNumber;
	private bool LevelComplete;
	private string LevelDescription;
	private Sprite LevelPreviewImage;

	public BubbleLevel (string TextFile, string Name, int Number, string Description, Sprite PreviewSprite)
	{
		LevelFile = TextFile;
		LevelName = Name;
		LevelNumber = Number;
		LevelComplete = false;
		LevelDescription = Description;
		LevelPreviewImage = PreviewSprite;
	}

	public string GetLevelFileName() {
		return LevelFile;
	}

	public string GetLevelName() {
		return LevelName;
	}

	public int GetLevelNumber() {
		return LevelNumber;
	}

	public void CompleteLevel() {
		LevelComplete = true;
	}

	public bool IsComplete() {
		return LevelComplete;
	}

	public string GetLevelDescription() {
		return LevelDescription;
	}

	public Sprite GetPreviewSprite() {
		return LevelPreviewImage;
	}
}

