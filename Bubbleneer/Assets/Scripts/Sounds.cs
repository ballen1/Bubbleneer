using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour {

	public AudioSource audio;
	public AudioClip BubbleConfirm;
	public AudioClip ButtonClick;
	public AudioClip ButtonClickCancel;
	public AudioClip Snip;
	public AudioClip SellChaChing;
	public AudioClip AirHorn;
	public AudioClip PlaceBait;
	public AudioClip Sand01;
	public AudioClip Sand02;
	public AudioClip Sand03;
	public AudioClip Star00;
	public AudioClip Star01;
	public AudioClip Star02;
    public AudioClip BuildMusic;
    public AudioClip OperationMusic;

    AudioSource bgmAudioSource;
   

	void Start() {
		audio = GetComponent<AudioSource>();

        bgmAudioSource = gameObject.AddComponent<AudioSource>();
        bgmAudioSource.loop = true;
    }

    public void PlayPipeConfirm() {
		audio.PlayOneShot(BubbleConfirm, 1.0f);
	}

	public void PlayButtonClick() {
		audio.PlayOneShot(ButtonClick, 1.0f);
	}

	public void PlayButtonClickCancel() {
		audio.PlayOneShot(ButtonClickCancel, 1.0f);
	}

	public void PlaySnip()
	{
		audio.PlayOneShot(Snip, 1.0f);
	}

	public void PlaySellChaChing()
	{
		audio.PlayOneShot(SellChaChing, 1.0f);
	}

	public void PlayAirHorn()
	{
		audio.PlayOneShot (AirHorn, 1.0f);
	}

	public void PlayPlaceBait()
	{
		audio.PlayOneShot (PlaceBait, 1.0f);
	}
		
	public void PlayTouchedSand()
	{
		int value = Random.Range(1,3);

		switch (value) {

		case 1:
			audio.PlayOneShot (Sand01, 1.0f);
			break;
		case 2:
			audio.PlayOneShot (Sand02, 1.0f);
			break;
		case 3:
			audio.PlayOneShot (Sand03, 1.0f);
			break;

		}

	}

	public void PlayStarSound(int value)
	{
		value++;

		switch (value) {

		case 1:
			audio.PlayOneShot (Star02, 1.0f);
			break;
		case 2:
			audio.PlayOneShot (Star01, 1.0f);
			break;
		case 3:
			audio.PlayOneShot (Star00, 1.0f);
			break;

		}
  
	}

    public void PlayBuildMusic()
    {
        bgmAudioSource.clip = BuildMusic;
        bgmAudioSource.volume = 0.65f;
        bgmAudioSource.Play();
    }
    public void StopBuildMusic()
    {
        bgmAudioSource.Stop();
    }

    public void PlayOperationMusic()
    {
        bgmAudioSource.clip = OperationMusic;
        bgmAudioSource.volume = 0.65f;
        bgmAudioSource.Play();
    }
    public void StopOperationMusic()
    {
        bgmAudioSource.Stop();
    }

}
