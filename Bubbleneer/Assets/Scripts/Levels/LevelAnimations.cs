using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAnimations : MonoBehaviour {

	public Animator levelAnimator;
	public Animator levelEndAnimator;

	private int scoreRating = 3;

	public void ActivateEndGameAnimation()
	{
		levelEndAnimator.SetBool ("GameOver", true);

	}

	public void ActivateStarScreen()
	{
		levelEndAnimator.SetBool ("GameOver", true);
	}


	public void PauseAnimation()
	{
		levelEndAnimator.speed = 0;
	}

	public void UnPauseAnimation()
	{
		levelEndAnimator.speed = 1;
	}

	public void StopLevelEndAnimation()
	{
		levelEndAnimator.Stop ();
	}

	public void StopLevelAnimation()
	{
		levelAnimator.Stop ();
	}


	public void ShiftOutElements()
	{
		levelAnimator.SetBool ("GameOver", true);
	}

	public void TickerTapeAnimation()
	{
		levelAnimator.SetBool ("PlayTickerTape", true);
	}

	public void SetStarCount(int Stars) {
		scoreRating = Stars;
	}

	public void DecrementStarCount() {
		scoreRating--;
	}

	public int GetStarCount() {
		return scoreRating;
	}

}
