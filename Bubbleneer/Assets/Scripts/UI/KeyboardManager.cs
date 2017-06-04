using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardManager : MonoBehaviour
{
	private RoundSystem roundManager;
	private ToggleManager toggleManager;
	private MacroManager macroManager;
	private Sounds sounds;

	public GameObject pauseMenu;

	// Use this for initialization
	void Awake ()
	{
		toggleManager = GameObject.Find ("Toggles").GetComponent<ToggleManager> ();
		sounds = GameObject.Find ("AudioManager").GetComponent<Sounds> ();
	
		macroManager = GetComponent<MacroManager> ();
		roundManager = GetComponent<RoundSystem> ();

	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update ()
	{

		if (Input.GetKeyDown (KeyCode.Escape)) {

			if (toggleManager.currentSelection != null) {
				toggleManager.UnselectCurrentToggle ();
				sounds.PlayButtonClickCancel ();
			} else if (!pauseMenu.activeInHierarchy && toggleManager.currentSelection == null) {
				roundManager.PauseGame ();
			} else if (pauseMenu.activeInHierarchy && toggleManager.currentSelection == null) {
				roundManager.UnpauseGame ();
			}

		}

		if (!roundManager.IsSimulationStarted () && !pauseMenu.activeInHierarchy) {

			if (Input.GetKeyDown (macroManager.SellMacroKey)) {
				toggleManager.SelectToggle (0);
			} else if (Input.GetKeyDown (macroManager.BaitToolMacroKey)) {
				toggleManager.SelectToggle (1);
			} else if (Input.GetKeyDown (macroManager.CutToolMacroKey)) {
				toggleManager.SelectToggle (2);
			} else if (Input.GetKeyDown (macroManager.StraightPipeMacroKey)) {
				toggleManager.SelectToggle (3);
			} else if (Input.GetKeyDown (macroManager.CurvePipeMacroKey)) {
				toggleManager.SelectToggle (4);
			} else if (Input.GetKeyDown (macroManager.UpPipeMacroKey)) {
				toggleManager.SelectToggle (5);
			} 

			 
		} else if (roundManager.IsSimulationStarted () && toggleManager.currentSelection != null) {
			toggleManager.currentSelection.isOn = false;

		}
	}



}