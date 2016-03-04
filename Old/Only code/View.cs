
using UnityEngine;
using System.Collections;

public class View : MonoBehaviour {


	private GameObject[] argoSnake;
	private bool bUpdate = false;

	static public float fGUIScale;
	static private Vector3 scale;
	//static public Rect rRectPause;
	//static public Rect rRectMiniSounds;
	//static public Rect rRectMusic;

	public GUISkin skinPause = null;
	public GUISkin skinMiniSounds = null;
	public GUISkin skinMusic = null;
	//public GUISkin skinAds = null;
	public GUISkin skinExit = null;
	public GUISkin skinLike = null;
	public GUISkin skinVibra = null;
	public GUISkin skinOk = null;
	public GUISkin skinQuestion = null;
	public GameObject goAudioFeed = null;
	public GameObject goAudioBonusAppear = null;
	public GameObject goAudioBonusEat = null;
	public GameObject goAudioDead = null;
	public GameObject goAudioMenuButton = null;
	public GameObject goAudioBackground = null;
	public GameObject goPause = null;
	//public GameObject goLike = null;
	public GameObject goScore = null;
	//public GameObject goArrowLeft = null;

	//public GameObject goArrowRight = null;
	public GameObject goBonusScore = null;
	public GameObject goBonusSnake = null;
	public GameObject goSnakeCell = null;
	public GameObject goSnakeHead = null;
	public GameObject goFeed = null;
	public GameObject goSnakeDead = null;
	public GameObject goGlowFeed = null;
	public GameObject goGlowDead = null;
	public GameObject goLine = null;
	public Texture textureTouch = null;
	public Texture textureTouchHover = null;
	public Texture textureTouchLeft = null;
	public Texture textureTouchHoverLeft = null;
	public Texture textureTapRotateLeft = null;
	public Texture textureTapRotateRight = null;
	private float timerCurrent = 0.0f;
	//private float timerCurrentArrows = 0.0f;
	private float timerCurrentDead = 0.0f;
	private float timerCurrentGlow = 0.0f;
	private float timerHelpCurrent = 0.0f;
	private bool bTimerDead = false;
	private bool bUpdateWas = false;

	//private bool bAudioAfterArrows = true;
	private bool bParticlePlayingOnce = true;

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
			fGUIScale = (float)Screen.height/(float)Constants.nOriginalWidth;
		else
			fGUIScale = (float)Screen.width/(float)Constants.nOriginalWidth;
*/
		fGUIScale = (float)Constants.GetSnakeCellPixelSize() * Constants.fButtonCellSize / (float)Constants.nButtonsPixelSize;

		scale.x = fGUIScale;
		scale.y = fGUIScale;
		scale.z = 1;
		
		Matrix4x4 svMat = GUI.matrix;
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, scale);


		if(!Model.bHelp)
		{
			bool bPauseNew = GUI.Toggle (Constants.rRectPause, Model.bPause, "", skinPause.toggle);

			if(Model.bPause != bPauseNew)
			{
				Model.bPause = bPauseNew;
				Utils.PlayMiniSound(goAudioMenuButton);
			}

			if(Model.bPause)
			{
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

	/*			bool bAdsNew = GUI.Toggle (Constants.rRectAds, Model.bAds, "", skinAds.toggle);
				
				if(Model.bAds != bAdsNew)
				{
					Model.bAds = bAdsNew;
					Utils.PlayMiniSound(goAudioMenuButton);	
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

				Constants.rRectExit = new Rect(Screen.width / fGUIScale - Constants.nButtonsPixelSize, Constants.rRectExit.y, Constants.rRectExit.width, Constants.rRectExit.height);

				if (GUI.Button (Constants.rRectExit, "", skinExit.button)) 
				{
					Utils.PlayMiniSound(goAudioMenuButton);

					Control.CalculateHighScores((int)Model.snake.mnScore);

					Utils.Save();				

					Application.LoadLevel (Constants.sHighScore);
				}

				Constants.rRectLike = new Rect(Screen.width / fGUIScale - Constants.rRectLike.width, Screen.height / fGUIScale - Constants.nButtonsPixelSize, Constants.rRectLike.width, Constants.rRectLike.height);

				if (GUI.Button (Constants.rRectLike, "", skinLike.button)) 
				{
					Utils.PlayMiniSound(goAudioMenuButton);
				}

				Constants.rRectQuestion = new Rect(0, Screen.height / fGUIScale - Constants.rRectQuestion.height, Constants.rRectQuestion.width, Constants.rRectQuestion.height);
				
				if (GUI.Button (Constants.rRectQuestion, "", skinQuestion.button)) 
				{
					Utils.PlayMiniSound(goAudioMenuButton);

					bUpdateWas = false;
					Model.bHelp = true;
					Model.bPause = true;

				}
			}

		//Control.GamePause (bPauseNew);

		}

		if (Model.bHelp) 
		{
			Constants.rRectOk = new Rect((Screen.width / fGUIScale- Constants.rRectOk.width) / 2.0f, Screen.height / fGUIScale - Constants.nButtonsPixelSize, Constants.rRectOk.width, Constants.rRectOk.height);
			
			if (GUI.Button (Constants.rRectOk, "", skinOk.button)) 
			{
				Utils.PlayMiniSound(goAudioMenuButton);
				Model.bHelpAtStart = false;
				Utils.Save();
				RemoveFromSceneAll();
				Model.bHelp = false;
				Model.bPause = false;
			}

			Constants.rRectTouch = new Rect(Screen.width / fGUIScale- Constants.rRectTouch.width, Screen.height / fGUIScale - Constants.rRectTouch.height, Constants.rRectTouch.width, Constants.rRectTouch.height);

			if(Model.bRightTouchHover)
				GUI.DrawTexture(Constants.rRectTouch, textureTouchHover);
			else
				GUI.DrawTexture(Constants.rRectTouch, textureTouch);

			Constants.rRectTouchLeft = new Rect(0, Screen.height / fGUIScale - Constants.rRectTouchLeft.height, Constants.rRectTouchLeft.width, Constants.rRectTouchLeft.height);

			if(Model.bLeftTouchHover)
				GUI.DrawTexture(Constants.rRectTouchLeft, textureTouchHoverLeft);
			else
				GUI.DrawTexture(Constants.rRectTouchLeft, textureTouchLeft);

			Constants.rRectTapRotate = new Rect((Screen.width / fGUIScale) / 4.0f - Constants.rRectTapRotate.width / 2.0f, 0, Constants.rRectTapRotate.width, Constants.rRectTapRotate.height);
			
			GUI.DrawTexture(Constants.rRectTapRotate, textureTapRotateLeft);

			Constants.rRectTapRotate = new Rect((Screen.width / fGUIScale) * 3.0f / 4.0f - Constants.rRectTapRotate.width / 2.0f, 0, Constants.rRectTapRotate.width, Constants.rRectTapRotate.height);
			
			GUI.DrawTexture(Constants.rRectTapRotate, textureTapRotateRight);
		}

		GUI.matrix = svMat;

		goPause.guiText.enabled = Model.bPause && !Model.bHelp;
		goScore.guiText.enabled = !Model.bHelp;
		goLine.renderer.enabled = Model.bHelp;
//		goLike.guiText.enabled = Model.bPause;

		//goLike.guiText.transform.position = new Vector3((Screen.width - Constants.nButtonsPixelSize * fGUIScale) / (float)Screen.width, 
		                                                //goLike.guiText.transform.position.y,
		                                                //goLike.guiText.transform.position.z);
	}

	void OnTimerDead()
	{
		Control.CalculateHighScores((int)Model.snake.mnScore);

		Utils.Save();				

		Application.LoadLevel (Constants.sHighScore);
	}
	
	void OnTimer()
	{
		Control.SnakeNext();

		bUpdate = true;

		if(Model.snake.meGameState == EGameState.END && !bTimerDead)
		{
			Utils.PlayMiniSound(goAudioDead);
			Utils.Vibrate();
			bTimerDead = true;
		}

		if(Model.snake.meGameState == EGameState.FEED_EAT)
		{
			Utils.PlayMiniSound(goAudioFeed);
		}

		if(Model.snake.meGameState == EGameState.BONUS_APPEAR)
		{
			Utils.PlayMiniSound(goAudioFeed);
			Utils.PlayMiniSound(goAudioBonusAppear);
		}

		if(Model.snake.meGameState == EGameState.BONUS_EAT)
		{
			Utils.PlayMiniSound(goAudioBonusEat);
			Utils.Vibrate();
		}

		goScore.guiText.text = Model.snake.mnScore.ToString();
	}

	void SetCameraGamePausePosition(bool bAway)
	{
		if(bAway)
			Camera.main.transform.position = new Vector3 (Constants.fCameraAway,
		                                              Constants.fCameraAway,
		                                              Constants.fCameraDistance);
		else
			Camera.main.transform.position = new Vector3 ((float)Constants.nAreaCellWidth / 2.0f,
			                                              -(float)Constants.nAreaCellHeight / 2.0f,
			                                              Constants.fCameraDistance);
	}

	void Help ()
	{
		if (Model.bHelp) 
		{
			timerHelpCurrent += Time.deltaTime;

			while (timerHelpCurrent >= Constants.timerHelpPeriodSnake) 
			{
				Control.ModelHelpGo(1);

				bUpdate = true;

				timerHelpCurrent -= Constants.timerHelpPeriodSnake;
			}
		}
	}
	/*void Arrows()
	{
		if (Model.bArrows) 
		{
			timerCurrentArrows += Time.deltaTime;
			
			if(timerCurrentArrows >= Constants.timerPeriodArrows)
				timerCurrentArrows = 0.0f;
			
			float fTimerArrows = timerCurrentArrows / Constants.timerPeriodArrows;
			
			if(fTimerArrows < 0.5f)
			{
				goArrowLeft.transform.position = new Vector3(
					Constants.fArrowLeftHorizontalPositionStart + 
					fTimerArrows *(Constants.fArrowLeftHorizontalPositionEnd-Constants.fArrowLeftHorizontalPositionStart),
					Constants.fArrowVerticalPosition, 0.0f);
				
				goArrowRight.transform.position = new Vector3(
					Constants.fArrowRightHorizontalPositionStart + 
					fTimerArrows *(Constants.fArrowRightHorizontalPositionEnd-Constants.fArrowRightHorizontalPositionStart),
					Constants.fArrowVerticalPosition, 0.0f);
				
			}
			else
			{
				goArrowLeft.transform.position = new Vector3(
					Constants.fArrowLeftHorizontalPositionStart + 
					(1.0f - fTimerArrows)*(Constants.fArrowLeftHorizontalPositionEnd-Constants.fArrowLeftHorizontalPositionStart),
					Constants.fArrowVerticalPosition, 0.0f);
				
				goArrowRight.transform.position = new Vector3(
					Constants.fArrowRightHorizontalPositionStart + 
					(1.0f - fTimerArrows)*(Constants.fArrowRightHorizontalPositionEnd-Constants.fArrowRightHorizontalPositionStart),
					Constants.fArrowVerticalPosition, 0.0f);
			}
		}
		else
		{
			goArrowLeft.renderer.enabled = false;
			goArrowRight.renderer.enabled = false;
			
			if(bAudioAfterArrows)
			{
				Utils.PlayMiniSound(goAudioFeed);
				bAudioAfterArrows = false;
			}
		}
	}
*/
	void ContinuousObjects ()
	{
		if (!Model.bPause && !Model.bHelp) 
		{
			timerCurrent += Time.deltaTime;
			timerCurrentGlow += Time.deltaTime;
			
			if(timerCurrentGlow >= Constants.timerPeriodGlow)
				timerCurrentGlow = 0.0f;
			
			float fTimerGlow = timerCurrentGlow/ Constants.timerPeriodGlow;
			
			if(fTimerGlow < 0.5f)
				goGlowFeed.transform.localScale = new Vector3(0.75f + fTimerGlow/2.0f, 0.75f + fTimerGlow/2.0f, 1.0f);
			else
				goGlowFeed.transform.localScale = new Vector3(1.25f - fTimerGlow/2.0f, 1.25f - fTimerGlow/2.0f, 1.0f);
			
			
			
			if(Model.snake.mbBonus.appear)
			{
				float fBonusScale = ((float)Model.snake.mbBonus.size - (float)timerCurrent / (float)Constants.timerPeriodSnake)/ (float)Model.snake.mnBonusMaxSize;
				
				if(Model.snake.mbBonus.type == EBonusType.SCORE)
					goBonusScore.transform.localScale = new Vector3(fBonusScale, fBonusScale, 1.0f);
				
				if(Model.snake.mbBonus.type == EBonusType.SNAKE)
					if(((float)Model.snake.mbBonus.size - (float)timerCurrent / (float)Constants.timerPeriodSnake)* Constants.timerPeriodSnake < Constants.fBonusSnakeTimeEmitFly)
						goBonusSnake.particleEmitter.emit = false;
				//goBonusSnake.transform.localScale = new Vector3(fBonusScale, fBonusScale, 1.0f);
				
			}
			else
			{
				//goBonusSnake.transform.localScale = new Vector3(0.0f,0.0f, 1.0f);
				if(Model.snake.mbBonus.type == EBonusType.SCORE)
					goBonusScore.transform.localScale = new Vector3(0.0f,0.0f, 1.0f);
				
				if(Model.snake.mbBonus.type == EBonusType.SNAKE)
					goBonusSnake.particleEmitter.emit = true;
			}
		}
	}

	void Dead()
	{
		if(!Model.bPause && bTimerDead)
		{
			timerCurrentDead += Time.deltaTime;
			
			float fTimerGlow = timerCurrentDead/ Constants.timerPeriodGlow;
			
			//while(fTimerGlow > 1.0f)
			//fTimerGlow -= 1.0f;
			
			Color cSnakeDead = goSnakeDead.GetComponent<SpriteRenderer>().color;
			
			if(fTimerGlow < 0.5f)
			{
				goGlowDead.transform.localScale = new Vector3(1.0f + fTimerGlow/2.0f, 1.0f + fTimerGlow/2.0f, 1.0f);
				goSnakeDead.GetComponent<SpriteRenderer>().color = new Color(cSnakeDead.r, 0.5f + fTimerGlow, 0.5f + fTimerGlow, cSnakeDead.a);
			}
			else
			{
				goGlowDead.transform.localScale = new Vector3(1.5f - fTimerGlow/2.0f, 1.5f - fTimerGlow/2.0f, 1.0f);
				goSnakeDead.GetComponent<SpriteRenderer>().color = new Color(cSnakeDead.r, 1.5f - fTimerGlow, 1.5f - fTimerGlow, cSnakeDead.a);
			}

			if (timerCurrentDead >= Constants.timerPeriodDead)
				OnTimerDead ();
		}
	}

	void ModelCalculate()
	{
		while (timerCurrent >= Constants.timerPeriodSnake) 
		{
			OnTimer();
			
			timerCurrent -= Constants.timerPeriodSnake;
		}
	}

	void RemoveFromScenePart()
	{
		goSnakeHead.transform.position = new Vector3 (Constants.nSnakePositionFail + Constants.fTranslate, -Constants.nSnakePositionFail - Constants.fTranslate, goSnakeHead.transform.position.z);
		goSnakeDead.transform.position = new Vector3 (Constants.nSnakePositionFail + Constants.fTranslate, -Constants.nSnakePositionFail - Constants.fTranslate, goSnakeDead.transform.position.z);
		goGlowDead.transform.position = new Vector3 (goSnakeDead.transform.position.x,goSnakeDead.transform.position.y, goGlowDead.transform.position.z);
		goFeed.transform.position = new Vector3 (Constants.nSnakePositionFail + Constants.fTranslate, -Constants.nSnakePositionFail - Constants.fTranslate, goFeed.transform.position.z);
		goGlowFeed.transform.position = goFeed.transform.position;
		
		bParticlePlayingOnce = true;
		
		goBonusScore.particleSystem.Stop();
		
		goBonusScore.transform.position = new Vector3(Constants.nSnakePositionFail,
		                                              -Constants.nSnakePositionFail,
		                                              goBonusScore.transform.position.z);
		
		goBonusSnake.transform.position = new Vector3(Constants.nSnakePositionFail,
		                                              -Constants.nSnakePositionFail,
		                                              goBonusSnake.transform.position.z);
	}

	void RemoveFromSceneAll()
	{
		for (int i = 0; i < Constants.nSnakeMaxSize; i++) 
			argoSnake [i].transform.position = new Vector3 (Constants.nSnakePositionFail + Constants.fTranslate, -Constants.nSnakePositionFail - Constants.fTranslate, argoSnake [i].transform.position.z);

		RemoveFromScenePart ();


	}

	void GraphicsCalculate()
	{
		if (bUpdate) 
		{
			if(Model.bHelp)
			{
				for (int i = 1; i < Model.snakeHelp.mlSnake.Count; i++) 
					argoSnake [i].transform.position = new Vector3 (Model.snakeHelp.mlSnake[i].position.elem1 + Constants.fTranslate, -Model.snakeHelp.mlSnake[i].position.elem2 - Constants.fTranslate, argoSnake [i].transform.position.z);

				for (int i = Model.snakeHelp.mlSnake.Count; i < Constants.nSnakeMaxSize; i++) 
					argoSnake [i].transform.position = new Vector3 (Constants.nSnakePositionFail + Constants.fTranslate, -Constants.nSnakePositionFail - Constants.fTranslate, argoSnake [i].transform.position.z);

				RemoveFromScenePart();
			}
			else
			{
				if(Model.snake.meGameState == EGameState.END)
				{
					if(Model.snake.mlSnake.Count > 0)
						goSnakeHead.transform.position = new Vector3 (Constants.nSnakePositionFail + Constants.fTranslate, -Constants.nSnakePositionFail - Constants.fTranslate, goSnakeHead.transform.position.z);
					
					goSnakeDead.transform.position = new Vector3 (Model.snake.mlSnake[0].position.elem1 + Constants.fTranslate, -Model.snake.mlSnake[0].position.elem2  - Constants.fTranslate, goSnakeDead.transform.position.z);
					
					goGlowDead.transform.position = new Vector3 (goSnakeDead.transform.position.x,goSnakeDead.transform.position.y, goGlowDead.transform.position.z);
				}
				else
				{
					if(Model.snake.mlSnake.Count > 0)
						goSnakeHead.transform.position = new Vector3 (Model.snake.mlSnake[0].position.elem1 + Constants.fTranslate, -Model.snake.mlSnake[0].position.elem2 - Constants.fTranslate, goSnakeHead.transform.position.z);
				}
				
				for (int i = 1; i < Model.snake.mlSnake.Count; i++) 
					argoSnake [i].transform.position = new Vector3 (Model.snake.mlSnake[i].position.elem1 + Constants.fTranslate, -Model.snake.mlSnake[i].position.elem2 - Constants.fTranslate, argoSnake [i].transform.position.z);
				
				for (int i = Model.snake.mlSnake.Count; i < Constants.nSnakeMaxSize; i++) 
					argoSnake [i].transform.position = new Vector3 (Constants.nSnakePositionFail + Constants.fTranslate, -Constants.nSnakePositionFail - Constants.fTranslate, argoSnake [i].transform.position.z);
				
				goFeed.transform.position = new Vector3 (Model.snake.mpFeed.elem1 + Constants.fTranslate, -Model.snake.mpFeed.elem2 - Constants.fTranslate, goFeed.transform.position.z);
				goGlowFeed.transform.position = goFeed.transform.position;
				
				if(Model.snake.mbBonus.appear)
				{
					
					
					if(Model.snake.mbBonus.type == EBonusType.SCORE)
					{
						if(!goBonusScore.particleSystem.isPlaying && bParticlePlayingOnce)
						{
							goBonusScore.particleSystem.Play();
							//goBonusScore.particleEmitter.emit = true;
							bParticlePlayingOnce = false;
						}
						
						goBonusScore.transform.position = new Vector3 (Model.snake.mbBonus.position.elem1+1, 
						                                               -Model.snake.mbBonus.position.elem2-1, 
						                                               goBonusScore.transform.position.z);
						
						goBonusScore.particleSystem.startLifetime = (float)Model.snake.mbBonus.size * Constants.fFirstStartLifeTimeBonusScore / (float)Constants.nBonusMaxSize;
						
						//if((float)Model.snake.mbBonus.size * Constants.timerPeriodSnake < goBonusScore.particleSystem.startLifetime)
						//goBonusScore.particleSystem = false;
						//	goBonusScore.particleSystem.Stop();
					}
					
					if(Model.snake.mbBonus.type == EBonusType.SNAKE)
					{
						/*
					if(!goBonusSnake.particleSystem.isPlaying && bParticlePlayingOnce)
					{
						goBonusSnake.particleSystem.Play();
						bParticlePlayingOnce = false;
					}
*/
						
						goBonusSnake.transform.position = new Vector3 (Model.snake.mbBonus.position.elem1+1, 
						                                               -Model.snake.mbBonus.position.elem2-1, 
						                                               goBonusSnake.transform.position.z);
						
					}
				}
				else
				{
					bParticlePlayingOnce = true;
					
					goBonusScore.particleSystem.Stop();
					//goBonusSnake.particleSystem.Stop();
					
					goBonusScore.transform.position = new Vector3(Constants.nSnakePositionFail,
					                                              -Constants.nSnakePositionFail,
					                                              goBonusScore.transform.position.z);
					
					goBonusSnake.transform.position = new Vector3(Constants.nSnakePositionFail,
					                                              -Constants.nSnakePositionFail,
					                                              goBonusSnake.transform.position.z);
					
				}
			}

			
			
			bUpdate = false;
			bUpdateWas = true;
		}
	}

	void SetCamera()
	{
		Utils.CameraReset ();

		if (Screen.width > Screen.height)
			goLine.transform.position = new Vector3 (goLine.transform.position.x, -((float)Constants.nAreaCellHeight)+2.0f, goLine.transform.position.z);
		else
			goLine.transform.position = new Vector3 (goLine.transform.position.x, 
			                                         -((float)Constants.nAreaCellHeight)+2.0f
			                                         -(((float)Constants.nAreaCellWidth / 2.0f) * (float) Screen.height / (float)Screen.width
			                                         - (float)Constants.nAreaCellHeight / 2.0f),
                 goLine.transform.position.z);
		/*Pause Help
		1	1	0
		1	0	1
		0	1	
		0	0	0
*/
		if(bUpdateWas)
			SetCameraGamePausePosition (Model.bPause && !Model.bHelp);
	}

	// Use this for initialization
	void Start () 
	{
		if(!Model.bPreLoad)
		{
			Control.ModelPreInit ();
			Utils.Load();

			Model.bPreLoad = true;
		}

		Control.ConstantsInit ();
		Control.ModelInit ();
		Control.ModelHelpInit ();
		goPause.guiText.enabled = Model.bPause;
		//goLike.guiText.enabled = Model.bPause;

		Screen.orientation = ScreenOrientation.AutoRotation;



		argoSnake = new GameObject[Constants.nSnakeMaxSize];

		for (int i = 0; i < Constants.nSnakeMaxSize; i++) 
		{
			argoSnake[i] = (GameObject)Instantiate (goSnakeCell, new Vector3 (Constants.nSnakePositionFail, -Constants.nSnakePositionFail, 0), Quaternion.identity);
		}		

		for (int i = 1; i < Constants.nSnakeMaxSize; i++) 
		{
			argoSnake[i].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 
			     argoSnake[i-1].GetComponent<SpriteRenderer>().color.a * Constants.fSnakeTransparentKoef);
		}

		SetCameraGamePausePosition (Model.bPause);

		if(Model.bMusic)
			goAudioBackground.audio.Play();

		//timerCurrent = Constants.timerPeriodSnake;

		goAudioBonusAppear.audio.pitch = 3.0f / (11.0f - (((float)Model.nSnakeSpeedCurrent - 10.0f) / 2.0f + 10.0f) );//(0.3f + 1.0f - 0.3f * (float)Constants.nSnakeSpeedMax / (float)Model.nSnakeSpeedCurrent)*3.0f;//0.2f + (float)Model.nSnakeSpeedCurrent * (3.0f - 0.2f) / (float)Constants.nSnakeSpeedMax;

		goLine.renderer.enabled = false;
		bUpdate = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		SetCamera ();
		//Arrows ();

		Help ();

		ContinuousObjects ();

		Dead ();

		ModelCalculate ();

		GraphicsCalculate ();


	}
}
