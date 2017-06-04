using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Pipe manager.  Manages the construction of pipes on the game map.
/// </summary>
public class PipeManager : MonoBehaviour
{
	// Used for 3D pipe array
	public int dirIndex = 0;
	public int rotateAmount = 0;

	// Direction of pipe stored in the 3D array
	private string[] direction = { "S", "W", "N", "E" };

	private ModelManager modelManager;
	private ToggleManager toggleManager;
	private NodeManager nodeManager;
    private GroundManager groundManager;

    // Needs to be awake
    // Otherwise highLight will instaniated as null
    void Awake ()
	{
		modelManager = gameObject.GetComponent<ModelManager> ();
		nodeManager = gameObject.GetComponent<NodeManager> ();
        groundManager = gameObject.GetComponent<GroundManager>();
        toggleManager = GameObject.Find ("Toggles").GetComponent<ToggleManager> ();
	}
		

	/// <summary>
	/// The player has toggled on the ability to drop bait
	/// </summary>
	/// <param name="state">If set to <c>true</c> state.</param>
	public void spawnPipe (Transform hit)
	{
        groundManager.removeGround(hit);
        // + Destory the gameobject below when the pipe is being built
        Destroy (hit.gameObject);

		GameObject newPipe = Instantiate (modelManager.getCurrentModel (), hit.position, 
			          					  Quaternion.Euler (0, modelManager.getCurrentModel ().transform.eulerAngles.y + rotateAmount, 0));

		string pipeType = toggleManager.currentSelection.name.Substring (10).ToLower();
		nodeManager.insertPipe (newPipe, pipeType, direction [dirIndex]);
	}

}

