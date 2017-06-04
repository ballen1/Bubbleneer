using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAnimations : MonoBehaviour {

	private LevelAnimations levelAnimations;

	public int maxTickerTapeLoops = 1;

	private int tickerTapeLoops = 0;

	// Use this for initialization
	void Start () {
		levelAnimations = GameObject.Find ("GameManager").GetComponent<LevelAnimations> ();
	}
	
	public void ActivateEndGameAnimation()
	{
		levelAnimations.ActivateEndGameAnimation ();
	}
		
	public void PauseAnimation()
	{
		levelAnimations.PauseAnimation ();

		levelAnimations.DecrementStarCount();

		if (levelAnimations.GetStarCount() >= 0) {
			UnPauseAnimation ();
		} else if (levelAnimations.GetStarCount() < 0) {
			levelAnimations.StopLevelEndAnimation ();
		}

	}

	public void UnPauseAnimation()
	{
		levelAnimations.UnPauseAnimation ();
	}

	public void TickerTapePlays()
	{
		tickerTapeLoops++;

		if (tickerTapeLoops >= maxTickerTapeLoops) {
			levelAnimations.StopLevelAnimation ();
		}
	}

	public int GetStarCount() {
		return levelAnimations.GetStarCount ();
	}
}
