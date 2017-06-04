using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSounds : MonoBehaviour {

	private Sounds sounds;
	private GetAnimations getAnimations;

	// Use this for initialization
	void Start () {
		sounds = GameObject.Find ("AudioManager").GetComponent<Sounds> ();
		getAnimations = GetComponent<GetAnimations> ();
	}

	public void PlayAirHorn()
	{
		sounds.PlayAirHorn ();
	}

	public void PlayStarSounds()
	{
		sounds.PlayStarSound (getAnimations.GetStarCount());
	}
}
