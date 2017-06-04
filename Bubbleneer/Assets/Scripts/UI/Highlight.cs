using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Handles the highlighting of gameobjects on the map.
/// </summary>
public class Highlight : MonoBehaviour
{

	// + Highlight colors
	public Color normalColor;
	public Color cannotInteract;
	public Color buildSuccessful;
	public Color highlightColor;

	// + Tranform information of the current square the cursor is over
	private Transform prevObj = null;

	// + Transform information of the last square the cursor was over
	private Transform curObj = null;

	// + A reference to the PipeManager Class
	private PipeManager pipeManager;

	public Transform selection;

	private ToggleManager toggleManager;
	private GameObject tempObject;
	private ModelManager modelManager;
	private RoundSystem roundSystem;
	private string context = "";

	void Awake ()
	{
		// + Set the pipeManager to an instance of the PipeManager class
		toggleManager = GameObject.Find ("Toggles").GetComponent<ToggleManager> ();
		pipeManager = GameObject.Find ("GameManager").GetComponent<PipeManager> ();
		modelManager = GetComponent<ModelManager> ();
		roundSystem = GetComponent<RoundSystem> ();

	}

	void Update ()
	{
		// + Handle the highlighting of game objects
		if (!roundSystem.IsSimulationOver ())
			highLightObjects ();
		else
			curObj.GetComponent<Renderer> ().material.SetColor ("_MainTexColor", normalColor);

		if (tempObject != null) {
			Destroy (tempObject);
		}
			
		if (toggleManager.currentSelection != null) {
			ColorContext ();
		}

		if (curObj != null && modelManager.getCurrentModel () != null) {
			objectPreview (curObj, modelManager.getCurrentModel ());
		}

	}

	/// <summary>
	/// Highs the light objects.
	/// </summary>
	private void highLightObjects ()
	{
		// + Use a ray cast from the mouse cursor to determine what
		//   what square is under the cursor
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, 100)) {

			Debug.DrawLine (ray.origin, hit.point);

			// + General highlight code for when the player is moving the cursor around the map
			// + Will just highlight the current square the player's cursor is hovering over
			if (curObj != hit.transform) {

				prevObj = curObj;
				curObj = hit.transform;

				// + Set the current square to the high light colour
				if (!curObj.tag.Equals ("Menu")) {

					curObj.GetComponent<Renderer> ().material.SetColor ("_MainTexColor", highlightColor);

					if (hit.transform.tag.Equals (context) || context.Equals("") && toggleManager.currentSelection != null) {
						curObj.GetComponent<Renderer> ().material.SetColor ("_MainTexColor", highlightColor);
					} else if (!hit.transform.tag.Equals (context) && toggleManager.currentSelection != null) {
						curObj.GetComponent<Renderer> ().material.SetColor ("_MainTexColor", cannotInteract);
					}

				}
				// + Set the previous square back to its original colour
				if (prevObj != null && prevObj != selection) {
					prevObj.GetComponent<Renderer> ().material.SetColor ("_MainTexColor", normalColor);

				}
					
			}

		}

	}

	/// <summary>
	/// Creates a 'preview' pipe that represents what a pipe will look like
	/// before it is placed
	/// </summary>
	/// <param name="hit">Hit.</param>
	public void objectPreview (Transform hit, GameObject objectPrefab)
	{

		// + Create a temporary preview pipe
		if (toggleManager.currentSelection.name.Contains ("Pipe")) {
			tempObject = Instantiate (objectPrefab, hit.transform.position, Quaternion.Euler (0, objectPrefab.transform.eulerAngles.y + pipeManager.rotateAmount, 0));
		} else if (toggleManager.currentSelection.name.Contains ("Cut")) {

			float x = hit.transform.position.x;
			float y = hit.transform.position.y;
			float z = hit.transform.position.z;

			tempObject = Instantiate (objectPrefab, new Vector3 (x - 0.8f, y + 2.0f, z - 0.8f), Quaternion.Euler (0, objectPrefab.transform.eulerAngles.y, 0));
		} else {
			tempObject = Instantiate (objectPrefab, hit.transform.position, Quaternion.Euler (0, objectPrefab.transform.eulerAngles.y, 0));
		}

		tempObject.layer = LayerMask.NameToLayer ("Ignore Raycast");

		// + The player is hovering over a buildable square
		if (hit.transform.tag.Equals (context)) {
			// + Turn the preview pipe the highlight color to show the player can build there
			tempObject.GetComponent<Renderer> ().material.SetColor ("_MainTexColor", highlightColor);
		} 

		// + Otherwise the player is over a non-buildable square
		else {
			// + Turn the preview pipe a colour indicating the player can not build there
			tempObject.GetComponent<Renderer> ().material.SetColor ("_MainTexColor", cannotInteract);
		}
			

	}

	/// <summary>
	/// Gets the current object.
	/// </summary>
	/// <returns>The current object.</returns>
	public Transform getCurrentObject ()
	{
		return curObj;
	}

	public Transform getPreviousObject ()
	{
		return prevObj;
	}

	public void setCurrentObject (Transform other)
	{
		curObj = other;
	}

	private void ColorContext ()
	{
		print (toggleManager.currentSelection.name);

		switch (toggleManager.currentSelection.name)
		{

		case "ButtonSell":
			context = "Sellable";
			break;

		case "ButtonBait":
			context = "Buildable";
			break;


		case "ButtonCut":
			context = "Clearable";
			break;

		case "ButtonPipeStraight":
			context = "Buildable";
			break;

		case "ButtonPipeCurve":
			context = "Buildable";
			break;

		case "ButtonPipeUp":
			context = "Buildable";
			break;
		
		default:
			context = "";
			break;
		
		}
	}

}

