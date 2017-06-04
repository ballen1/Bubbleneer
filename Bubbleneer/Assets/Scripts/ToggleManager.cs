using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// Toggle manager.
/// </summary>
public class ToggleManager : MonoBehaviour
{
	private ToggleGroup toggleGroupInstance;

	// + An array of all the toggle elements in the toggle group
	private Toggle[] toggles;

	private Sounds sounds;
	private RoundSystem roundManager;

	private int currentID = -1;

	// Use this for initialization
	void Start ()
	{
		toggleGroupInstance = GetComponent<ToggleGroup> ();

		// + Gets all toggles in the toggle group
		toggles = toggleGroupInstance.GetComponentsInChildren<Toggle> ();

		sounds = GameObject.Find ("AudioManager").GetComponent<Sounds> ();
		roundManager = GameObject.Find ("GameManager").GetComponent <RoundSystem> ();

	}
		
	void Update(){

		if (roundManager.IsRoundOver() && (toggles.Length > 0)) {
			if (toggles [0].interactable) {
				foreach (Toggle element in toggles) {
					element.interactable = false;
				}
			}
		}
	
	}

	/// <summary>
	/// Gets the currently selected toggle element
	/// </summary>
	/// <value>The current selection.</value>
	public Toggle currentSelection {
		get { return toggleGroupInstance.ActiveToggles ().FirstOrDefault(); }
	}

	/// <summary>
	/// Selects the toggle, and sets the color
	/// </summary>
	/// <param name="id">Identifier.</param>
	public void SelectToggle (int id)
	{
		if (!(id > toggles.Length-1)) {
			
			if (currentID != id) {
				currentID = id;
				toggles [id].isOn = true;
			} else if (currentID == id) {
				toggles [id].isOn = false;
				currentID = -1;
			}

		}
			
	}

	/// <summary>
	/// Unselects all toggles in togglegroup
	/// </summary>
	public void UnselectCurrentToggle ()
	{
		if (currentSelection != null) {
			currentSelection.isOn = false;
			currentID = -1;
		}
			
	}

	public void ToggleSounds()
	{
		sounds.PlayButtonClick ();
	}

}

	
