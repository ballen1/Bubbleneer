using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuAnimations : MonoBehaviour {

	private Animator animator;
	public GameObject quitMenu;

	// Use this for initialization
	void Awake () {
		animator = GameObject.Find ("CanvasMainMenu").GetComponent<Animator> ();
	}


	void Update()
	{
		KeyPressd ();
	}

	public void KeyPressd()
	{
		
		if (Input.anyKeyDown && animator.GetInteger ("AnimationState") == 0) {
			animator.SetBool ("PressAnyKey", true);
			animator.SetInteger ("AnimationState", 1);
		}

		if (Input.GetKeyDown(KeyCode.Escape) && animator.GetInteger ("AnimationState") == 1) {
			animator.SetBool ("PressAnyKey", false);
			animator.SetInteger ("AnimationState", 0);
		}
			
	}

	public void SelectLevelClicked()
	{
		animator.SetBool ("SelectLevel", true);
	}


	public void BackSelectLevelClicked()
	{
		animator.SetBool ("SelectLevel", false);
	}


	public void AboutClicked(){
		animator.SetBool ("About", true);
	}

	public void BackAboutClicked()
	{
		animator.SetBool ("About", false);
	}

	public void OpenQuitMenu()
	{
		quitMenu.gameObject.SetActive (true);
	}

	public void QuitYes()
	{
		Application.Quit ();
	}

	public void QuitNo()
	{
		quitMenu.gameObject.SetActive (false);
	}


}
