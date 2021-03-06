﻿using UnityEngine;
using System.Collections;

public class HighScore : MonoBehaviour {

	private GameObject[] argoSnakeSpeed;
	private GameObject[] argoHighScoresNum;
	private GameObject[] argoHighScores;
	static public float fGUIScale;
	static private Vector3 scale;
	public GUISkin skinPlay = null;
	public GUISkin skinMiniSounds = null;
	public GUISkin skinMusic = null;
	//public GUISkin skinAds = null;
	public GUISkin skinSpeed = null;
	public GUISkin skinLike = null;
	public GUISkin skinVibra = null;
	public GUISkin skinHigh = null;
//	public GameObject goLike = null;
	public GameObject goAudioMenuButton = null;
	public GameObject goAudioBackground = null;
	public GameObject goTopLabel = null;
	public GameObject goScore = null;
	public GameObject goHighScore = null;
	public GameObject goTotal = null;
	public GameObject goSnakeCell = null;
	public GameObject goSpeedGlow = null;
	public GameObject goFrame = null;
	public Vector3 v3Fail;
	bool bUpdate = true;
	bool bHoldInSpeedRect = false;

	private float timerCurrentGlow = 0.0f;

	public GameObject goAudioFeed = null;

	public GUISkin skinExit = null;

	public void MusicPlayPause()
	{
		if(goAudioBackground.audio.isPlaying)
			goAudioBackground.audio.Pause();
		else
			goAudioBackground.audio.Play();
	}

	void OnGUI()
	{
		/*if(Screen.width > Screen.height)
			fGUIScale = (float)Screen.height/(float)Constants.nOriginalHeight;
		else
			fGUIScale = (float)Screen.width/(float)Constants.nOriginalHeight;
*/
		fGUIScale = (float)Constants.GetSnakeCellPixelSize() * Constants.fButtonCellSize / (float)Constants.nButtonsPixelSize;

		scale.x = fGUIScale;
		scale.y = fGUIScale;
		scale.z = 1;
		
		Matrix4x4 svMat = GUI.matrix;
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, scale);
		
		if (GUI.Button (Constants.rRectPlay, "", skinPlay.button)) 
		{
			Utils.PlayMiniSound(goAudioMenuButton);

			Control.ModelInit ();
			Utils.Save();
			Application.LoadLevel (Constants.sGame);
		}

		bool bMiniSoundsNew = GUI.Toggle (Constants.rRectMiniSounds, Model.bMiniSounds, "", skinMiniSounds.toggle);
		
		if(Model.bMiniSounds != bMiniSoundsNew)
		{
			Model.bMiniSounds = bMiniSoundsNew;
			Utils.PlayMiniSound(goAudioMenuButton);				
			Utils.Save();
		}
		
		bool bMusicNew = GUI.Toggle (Constants.rRectMusic, Model.bMusic, "", skinMusic.toggle);
		
		if(Model.bMusic != bMusicNew)
		{
			Model.bMusic = bMusicNew;
			Utils.PlayMiniSound(goAudioMenuButton);	
			
			MusicPlayPause();
			Utils.Save();
		}
		/*
		bool bAdsNew = GUI.Toggle (Constants.rRectAds, Model.bAds, "", skinAds.toggle);
		
		if(Model.bAds != bAdsNew)
		{
			Model.bAds = bAdsNew;
			Utils.PlayMiniSound(goAudioMenuButton);	

			bUpdate = true;
		}
*/
		bool bVibraNew = GUI.Toggle (Constants.rRectVibra, Model.bVibra, "", skinVibra.toggle);
		
		if(Model.bVibra != bVibraNew)
		{
			Model.bVibra = bVibraNew;
			Utils.PlayMiniSound(goAudioMenuButton);	
			Utils.Vibrate();
			Utils.Save();
		}

		bool bStateSpeed = GUI.Toggle (Constants.rRectSpeed, Model.egsState == EGUIState.SPEED, "", skinSpeed.toggle);
		
		if(bStateSpeed && Model.egsState != EGUIState.SPEED)
		{
			Model.egsState = EGUIState.SPEED;
			Utils.PlayMiniSound(goAudioMenuButton);	

			bUpdate = true;
		}

		if(!bStateSpeed && Model.egsState == EGUIState.SPEED)
		{
			Model.egsState = EGUIState.SCORE;
			Utils.PlayMiniSound(goAudioMenuButton);

			bUpdate = true;
		}

		bool bStateHigh = GUI.Toggle (Constants.rRectHigh, Model.egsState == EGUIState.HIGHSCORES, "", skinHigh.toggle);
		
		if(bStateHigh && Model.egsState != EGUIState.HIGHSCORES)
		{
			Model.egsState = EGUIState.HIGHSCORES;
			Utils.PlayMiniSound(goAudioMenuButton);	
			
			bUpdate = true;
		}
		
		if(!bStateHigh && Model.egsState == EGUIState.HIGHSCORES)
		{
			Model.egsState = EGUIState.SCORE;
			Utils.PlayMiniSound(goAudioMenuButton);
			
			bUpdate = true;
		}

		Constants.rRectExit = new Rect(Screen.width / fGUIScale - Constants.nButtonsPixelSize, Constants.rRectExit.y, Constants.rRectExit.width, Constants.rRectExit.height);

		if (GUI.Button (Constants.rRectExit, "", skinExit.button)) 
		{
			Utils.PlayMiniSound(goAudioMenuButton);

			Utils.Save();

			Application.Quit ();
		}

		Constants.rRectLike = new Rect(Screen.width / fGUIScale - Constants.rRectLike.width, Screen.height / fGUIScale - Constants.nButtonsPixelSize, Constants.rRectLike.width, Constants.rRectLike.height);
		
		if (GUI.Button (Constants.rRectLike, "", skinLike.button)) 
		{
			Utils.PlayMiniSound(goAudioMenuButton);
		}

		GUI.matrix = svMat;

//		goLike.guiText.transform.position = new Vector3((Screen.width - Constants.rRectLike.width * fGUIScale) / (float)Screen.width, 
		                                                //goLike.guiText.transform.position.y,
		                                                //goLike.guiText.transform.position.z);
	}

	public void Action(Vector2 v2PositionScreen)
	{
		Vector3 v3Position = v2PositionScreen;
		v3Position.z = -10.0f;
		v3Position = Camera.main.ScreenToWorldPoint(v3Position);
		
		if(Mathf.Abs(v3Position.y - ((float)Constants.nAreaCellHeight * (goScore.transform.position.y - 1.0f) - Constants.fTranslate)) <= 0.5f &&
		   Mathf.Abs(v3Position.x - ((float)Constants.nAreaCellWidth / 2.0f)) <= ((float)Constants.nSnakeSpeedMax - (float)Constants.nSnakeSpeedMin + 1) / 2.0f)
		{
			bHoldInSpeedRect = true;
		}

		if(bHoldInSpeedRect)
		{
			int nSnakeSpeedCurrent = (int)(v3Position.x -((float)Constants.nAreaCellWidth / 2.0f - ((float)Constants.nSnakeSpeedMax - (float)Constants.nSnakeSpeedMin + 1) / 2.0f)) + 1;

			if(nSnakeSpeedCurrent != Model.nSnakeSpeedCurrent &&
			   nSnakeSpeedCurrent >= Constants.nSnakeSpeedMin &&
			   nSnakeSpeedCurrent <= Constants.nSnakeSpeedMax)
			{
				Control.SetSnakeSpeed(nSnakeSpeedCurrent);
				Utils.PlayMiniSound(goAudioFeed);
				bUpdate = true;
			}
		}
	}

	// Use this for initialization
	void Start () {
		Control.ModelDebugInit ();

		v3Fail = new Vector3 (Constants.nSnakePositionFail, -Constants.nSnakePositionFail, goSnakeCell.transform.position.z);

		Constants.Init ();

		if(Model.bMusic)
			goAudioBackground.audio.Play();

		argoSnakeSpeed = new GameObject[Constants.nSnakeSpeedMax - Constants.nSnakeSpeedMin + 1];
		
		for (int i = 0; i <= Constants.nSnakeSpeedMax - Constants.nSnakeSpeedMin; i++) 
		{
			argoSnakeSpeed[i] = (GameObject)Instantiate (goSnakeCell, new Vector3 (Constants.nSnakePositionFail, -Constants.nSnakePositionFail, goSnakeCell.transform.position.z), Quaternion.identity);

			if(i != 0)
				argoSnakeSpeed[i].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 
           			argoSnakeSpeed[i-1].GetComponent<SpriteRenderer>().color.a * Constants.fSnakeTransparentKoef);
		}

		goHighScore.guiText.enabled = false;
		argoHighScoresNum = new GameObject[Constants.nHighScoresCount];
		argoHighScores = new GameObject[Constants.nHighScoresCount];
		
		for (int i = 0; i < Constants.nHighScoresCount; i++) 
		{
			argoHighScoresNum[i] = (GameObject)Instantiate (goHighScore, new Vector3(0, 0, 0), Quaternion.identity);
			argoHighScores[i] = (GameObject)Instantiate (goHighScore, new Vector3(0, 0, 0), Quaternion.identity);


			argoHighScoresNum[i].guiText.transform.position = new Vector3(goHighScore.guiText.transform.position.x,
				Constants.fHighScoreTopFirst - i * Constants.fHighScoreDist,
           		goHighScore.guiText.transform.position.z);
			argoHighScores[i].guiText.transform.position = new Vector3(goHighScore.guiText.transform.position.x + Constants.fHighScoreLeft,
			                                                           Constants.fHighScoreTopFirst - i * Constants.fHighScoreDist,
			                                                           goHighScore.guiText.transform.position.z);

			argoHighScoresNum[i].guiText.text = (i+1).ToString() + Constants.sHighScoreMid;
     	}

		goTotal.guiText.enabled = false;
		goTotal.guiText.transform.position = new Vector3(goTotal.guiText.transform.position.x,
		                                                 Constants.fHighScoreTopFirst - Constants.nHighScoresCount * Constants.fHighScoreDist,
		                                                 goTotal.guiText.transform.position.z);


		goSpeedGlow.transform.position = v3Fail;

		goFrame.transform.position = v3Fail;
	}
	
	// Update is called once per frame
	void Update () {

		Utils.CameraReset ();

		timerCurrentGlow += Time.deltaTime;

		if(timerCurrentGlow >= Constants.timerPeriodGlow)
			timerCurrentGlow = 0.0f;
		
		float fTimerGlow = timerCurrentGlow/ Constants.timerPeriodGlow;
		
		if(fTimerGlow < 0.5f)
			goSpeedGlow.transform.localScale = new Vector3(0.75f + fTimerGlow/2.0f, 0.75f + fTimerGlow/2.0f, 1.0f);
		else
			goSpeedGlow.transform.localScale = new Vector3(1.25f - fTimerGlow/2.0f, 1.25f - fTimerGlow/2.0f, 1.0f);
		
		if(bUpdate)
		{


			switch(Model.egsState)
			{
			case EGUIState.SCORE:
				{
				//clear
					goSpeedGlow.transform.position = v3Fail;
					goFrame.transform.position = v3Fail;

					for(int i = 0; i <= Constants.nSnakeSpeedMax - Constants.nSnakeSpeedMin;i++)
					{
						argoSnakeSpeed[i].transform.position = v3Fail;
					}

					for (int k = 0; k < Constants.nHighScoresCount; k++) 
					{
						argoHighScores[k].guiText.enabled = false;
						argoHighScoresNum[k].guiText.enabled = false;
					}

					goTotal.guiText.enabled = false;

				//set
				goTopLabel.guiText.transform.position = new Vector3(goTopLabel.guiText.transform.position.x,
				                                                    Constants.fGUITopLabel,
				                                                    goTopLabel.guiText.transform.position.z);
					goTopLabel.guiText.text = Constants.sYourScore;
					goScore.guiText.enabled = true;

				
			}
				break;
				
			case EGUIState.SPEED:
				{
				//clear
					goScore.guiText.enabled = false;

					for (int k = 0; k < Constants.nHighScoresCount; k++) 
					{
						argoHighScores[k].guiText.enabled = false;
						argoHighScoresNum[k].guiText.enabled = false;
					}

					goTotal.guiText.enabled = false;

				//set
					goTopLabel.guiText.transform.position = new Vector3(goTopLabel.guiText.transform.position.x,
				                                                    Constants.fGUITopLabel,
				                                                    goTopLabel.guiText.transform.position.z);
					goTopLabel.guiText.text = Constants.sLevel + Model.nSnakeSpeedCurrent.ToString();
					int i = 0; 
					
					for (i = 0; i <= Model.nSnakeSpeedCurrent - Constants.nSnakeSpeedMin; i++) 
					{
						argoSnakeSpeed[Model.nSnakeSpeedCurrent - Constants.nSnakeSpeedMin - i].transform.position = new Vector3 (
						Constants.fTranslate + (float)Constants.nAreaCellWidth / 2.0f - ((float)Constants.nSnakeSpeedMax - (float)Constants.nSnakeSpeedMin + 1) / 2.0f + i,
						- Constants.fTranslate + (float)Constants.nAreaCellHeight * (goScore.transform.position.y - 1.0f), 
							argoSnakeSpeed[i].transform.position.z);
					}
					
					goSpeedGlow.transform.position = argoSnakeSpeed[0].transform.position;

					for(i = Model.nSnakeSpeedCurrent - Constants.nSnakeSpeedMin + 1; i <= Constants.nSnakeSpeedMax - Constants.nSnakeSpeedMin;i++)
					{
						argoSnakeSpeed[i].transform.position = new Vector3 (
						Constants.fTranslate + Constants.nSnakePositionFail, 
						- Constants.fTranslate - Constants.nSnakePositionFail, 
						argoSnakeSpeed[i].transform.position.z);
					}
					
					goFrame.transform.position = new Vector3 (
					(float)Constants.nAreaCellWidth / 2.0f,
					- Constants.fTranslate + (float)Constants.nAreaCellHeight * (goScore.transform.position.y - 1.0f),
						goFrame.transform.position.z);
				}
				break;

			case EGUIState.HIGHSCORES:
				{
				//clear
					goScore.guiText.enabled = false;

					goSpeedGlow.transform.position = v3Fail;
					goFrame.transform.position = v3Fail;

					for(int i = 0; i <= Constants.nSnakeSpeedMax - Constants.nSnakeSpeedMin;i++)
					{
						argoSnakeSpeed[i].transform.position = v3Fail;
					}

				//set
					for (int k = 0; k < Constants.nHighScoresCount; k++) 
					{
						argoHighScoresNum[k].guiText.enabled = true;
						argoHighScores[k].guiText.enabled = true;						
						
						if(Model.arHighScores[k] > 0)
							argoHighScores[k].guiText.text = Model.arHighScores[k].ToString();
					}

				//argoHighScores[0].guiText.text = "4212";
				//argoHighScores[1].guiText.text = "322";

					goTopLabel.guiText.transform.position = new Vector3(goTopLabel.guiText.transform.position.x,
				                                                    Constants.fGUITopLabelHigh,
				                                                    goTopLabel.guiText.transform.position.z);

					goTopLabel.guiText.text = Constants.sHighScores;

					goTotal.guiText.enabled = true;
					goTotal.guiText.text = Constants.sTotal + Model.lTotal.ToString();
				}
				break;
			}
			
			bUpdate = false;
		}

		if(Model.egsState == EGUIState.SPEED)
		{
			for (int i = 0; i < Input.touchCount; i++) 
			{
				if(Input.GetTouch (i).phase == TouchPhase.Began ||
				   Input.GetTouch (i).phase == TouchPhase.Moved)
				{
					//Model.bArrows = false;
					
					Action(Input.GetTouch (i).position);
				}

				if(Input.GetTouch (i).phase == TouchPhase.Ended)
				{
					bHoldInSpeedRect = false;
				}
			}
			
			if(Input.GetMouseButtonDown(0) ||
			   Input.GetMouseButton(0))
			{
				Action(Input.mousePosition);
			}

			if(Input.GetMouseButtonUp(0))
		 	{
				bHoldInSpeedRect = false;
			}


		}
	}
}
