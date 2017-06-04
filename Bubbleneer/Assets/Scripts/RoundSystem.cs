using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RoundSystem : MonoBehaviour {

	private bool RoundStarted;
	private bool RoundEnded;

	private bool SimulationStarted;
	private bool SimulationEnded;

	private bool EndGameActivated;

	public Text RoundCounter;

	public Text RoundScore;
	public Text EndRoundScore;
	public Text TextStageName;

	private int Score;

	private bool PlayerCanProgress;

	private float RoundDuration_Seconds;
	private float SimulationDuration_Seconds;

	public float TickerTapeSpeed;
	public RectTransform BuildText;
	public RectTransform RoundEndText;

	public GameObject BubbleScore;
	public float BubbleFadeDuration;
	public float BubbleDeltaY;
	public Transform InGameCanvas;
	public Camera Cam;

	private string MapName;

	private int MinScore;

    // + Added by Karsten
    public bool canPause = true;
	public GameObject RoundTicker;
    public GameObject Toggle;
    public GameObject RoundTimer;
    public GameObject CashInfo;
    public GameObject score;
	private LevelAnimations animations;
	private Sounds sounds;
	private CameraScript cameraScript;
	private float timeElapsed = 0.0f;
	public GameObject quitMenu;

	private LevelManager LM;

	// Use this for initialization
	void Start () {

		LM = GameObject.FindGameObjectWithTag ("LevelManager").GetComponent<LevelManager> ();

		PlayerCanProgress = false;
		EndGameActivated = false;
		RoundStarted = false;
		RoundEnded = false;

		SimulationStarted = false;
		SimulationEnded = false;

		TimeSpan span = TimeSpan.FromSeconds (RoundDuration_Seconds);
		RoundCounter.text = String.Format ("{0:0}:{1:00}", span.Minutes, span.Seconds);

		BuildText.position = new Vector3 (-400f, Screen.height/2.0f, 0.0f);
		StartCoroutine (TickerTape (2, BuildText, 400));

		RoundEndText.position = new Vector3 (-350f, Screen.height / 2.0f, 0.0f);

		Score = 0;
		RoundScore.text = Score.ToString ("D6");

		// + Added by Karsten
		animations = GetComponent<LevelAnimations> ();
		sounds = GameObject.Find ("AudioManager").GetComponent<Sounds> ();
		cameraScript = GameObject.Find ("Main Camera").GetComponent<CameraScript> ();
        //

        // added by kirk
        sounds.PlayBuildMusic();
	}

	void Update() {

		RoundScore.text = Score.ToString ("D6");

		if (SimulationEnded) {

			TextStageName.text = MapName;

			if ( !EndGameActivated && (Score < MinScore)) {
				Debug.Log ("You lost.");
				if (Score <= 0) {
					Debug.Log ("0 Stars");
					animations.SetStarCount (0);
				} else if (Score <= MinScore / 2) {
					Debug.Log ("1 Stars");
					animations.SetStarCount (1);
				} else {
					Debug.Log ("2 Stars");
					animations.SetStarCount (2);
				}
			} else if (!EndGameActivated && Score >= MinScore) {
				Debug.Log ("You won.");
				LM.MarkCurrentLevelComplete ();
				PlayerCanProgress = true;
				animations.SetStarCount (3);
			}

            canPause = false;
			// + Added by Karsten.
			cameraScript.enabled = false;

			animations.ShiftOutElements ();
			animations.ActivateEndGameAnimation ();

			// + Zoom camera out
			if (timeElapsed < 1.0f) {
				Cam.orthographicSize += Time.deltaTime;
				timeElapsed += Time.deltaTime;
			}

			EndRoundScore.text = Score.ToString ();
			EndGameActivated = true;

            sounds.StopOperationMusic();
		}
	}

	public void SetRoundTimes(float BuildTime, float SimulationTime) {
		RoundDuration_Seconds = BuildTime;
		SimulationDuration_Seconds = SimulationTime;
	}
		
	public bool IsRoundStarted() {
		return RoundStarted;
	}

	public void StartRound() {
		RoundStarted = true;
		StartCoroutine (RoundTimerCountdown (RoundDuration_Seconds));
	}

	public bool IsRoundOver() {
		return RoundEnded;
	}

	public void StartSimulation() {
		SimulationStarted = true;
		StartCoroutine (TickerTape (1, RoundEndText, 350));
		StartCoroutine (RoundTimerCountdown (SimulationDuration_Seconds));

        // added by kirk
        sounds.StopBuildMusic();
        sounds.PlayOperationMusic();
    }

	public bool IsSimulationStarted() {
		return SimulationStarted;
	}

	public bool IsSimulationOver() {
		return SimulationEnded;
	}

	public void AddBubbleScore(Transform EndPipe) {
		if (!SimulationEnded) {
			Score += 100;
			StartCoroutine (BubbleTextFade (EndPipe, BubbleFadeDuration, BubbleDeltaY));
		}
	}

	private IEnumerator RoundTimerCountdown(float Duration) {
		while (Duration > 0) {
			Duration -= Time.deltaTime;
			TimeSpan span = TimeSpan.FromSeconds (Duration);
			RoundCounter.text = String.Format ("{0:0}:{1:00}", span.Minutes, span.Seconds);
			yield return new WaitForEndOfFrame();
		}

		if (!RoundEnded) {
			RoundEnded = true;
			StartSimulation ();
			yield break;
		}

		if (RoundEnded && !SimulationEnded) {
			SimulationEnded = true;
			yield break;
		}
			
	}

	private IEnumerator TickerTape(int Passes, RectTransform TextTransform, int Width) {
		TextTransform.gameObject.SetActive (true);
		int Count = 0;
		while (Count < Passes) {
			while (TextTransform.position.x <= Screen.width) {
				TextTransform.Translate (new Vector3 (TickerTapeSpeed*Time.deltaTime, 0f, 0f));
				yield return null;
			}
			Count++;
			TextTransform.position = new Vector2 (-Width, TextTransform.position.y);
		}
		TextTransform.gameObject.SetActive (false);
	}

	private IEnumerator BubbleTextFade(Transform Target, float Duration, float DeltaY) {
		float TotalDuration = Duration;
		Vector3 ScreenTarget = Cam.WorldToScreenPoint (Target.position + Vector3.up*4);
		GameObject BubbleText = Instantiate (BubbleScore, InGameCanvas);
		Text BubbleTextLayer = BubbleText.GetComponent<Text> ();
		BubbleText.transform.position = ScreenTarget;

		BubbleTextLayer.CrossFadeAlpha (0, TotalDuration, false);

		while (Duration > 0) {
			Duration -= Time.deltaTime;
			BubbleText.transform.position = Cam.WorldToScreenPoint(Target.position + Vector3.up*(5 + 1-((Duration/TotalDuration)*DeltaY)));
			yield return new WaitForEndOfFrame ();
		}

		Destroy (BubbleText);
	}
		
	public void SetMinimumSucceedScore(int Score) {
		MinScore = Score;
	}

	public void SetMapName(string Name) {
		MapName = Name;
	}

	public void StopStartGame()
	{
		if (quitMenu.activeInHierarchy) {
			UnpauseGame ();
		} else {
			PauseGame ();
		}
	}

	public void PauseGame()
	{
        if(canPause)
        {
            sounds.PlayButtonClickCancel();
            RoundTicker.SetActive(false);
            Toggle.SetActive(false);
            CashInfo.SetActive(false);
            score.SetActive(false);
            RoundTimer.SetActive(false);
            quitMenu.SetActive(true);
            Time.timeScale = 0.0f;
        }
	}

	public void UnpauseGame()
	{
        if (canPause)
        {
            Time.timeScale = 1.0f;
            sounds.PlayButtonClickCancel();
            RoundTicker.SetActive(true);
            Toggle.SetActive(true);
            CashInfo.SetActive(true);
            score.SetActive(true);
            RoundTimer.SetActive(true);
            quitMenu.SetActive(false);
            Highlight highLight = GetComponent<Highlight>();
            highLight.setCurrentObject(highLight.getPreviousObject());
        }
	}

	public bool CanPlayerProgress() {
		return PlayerCanProgress;
	}

	public void PauseMenuRestart() {
		Time.timeScale = 1.0f;
		LM.RestartLevel ();
	}

	public void PauseMenuQuit() {
		Time.timeScale = 1.0f;
		LM.ReturnToMainMenu ();
	}

}
