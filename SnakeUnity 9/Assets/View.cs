using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class View : MonoBehaviour {


    private Model model = null;
    private GameObject[] argoSnake;
	private bool bUpdate = false;
	
	static public float fGUIScale;
	static private Vector3 scale;
	//static public Rect rRectPause;
	//static public Rect rRectMiniSounds;
	//static public Rect rRectMusic;
	
	//public GUISkin skinAds = null;
	//public GUISkin skinExit = null;
	public GUISkin skinOk = null;
	public GUISkin skinQuestion = null;
    //public GameObject goPause = null;
	//public GameObject goLike = null;
	//public GameObject goArrowLeft = null;
	
	//public GameObject goArrowRight = null;
	public GameObject goBonusScore = null;
	public GameObject goBonusSnake = null;
	public GameObject goSnakeCell = null;
	public GameObject goSnakeHead = null;
	public GameObject goFeed = null;
    public GameObject goScore = null;
    public GameObject goSnakeDead = null;
	public GameObject goGlowFeed = null;
	public GameObject goGlowDead = null;
	//public GameObject goLine = null;
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
	//private bool bUpdateWas = false;

    //private bool bAudioAfterArrows = true;
    private bool bParticlePlayingOnce = true;
	
	void OnGUI()
	{

		fGUIScale = (float)model.GetSnakeCellPixelSize() * model.fButtonCellSize / (float)model.nButtonsPixelSize;
		
		scale.x = fGUIScale;
		scale.y = fGUIScale;
		scale.z = 1;
		
		Matrix4x4 svMat = GUI.matrix;
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, scale);
		
		
		if(model.state != State.HELP)
		{
			
			
			if(model.state == State.PAUSE)
			{				
				/*			bool bAdsNew = GUI.Toggle (model.rRectAds, model.bAds, "", skinAds.toggle);
				
				if(model.bAds != bAdsNew)
				{
					model.bAds = bAdsNew;
					model.PlaySound(goAudioMenuButton);	
				}
	*/
				
			/*	
				model.rRectExit = new Rect(Screen.width / fGUIScale - model.nButtonsPixelSize, model.rRectExit.y, model.rRectExit.width, model.rRectExit.height);
				
				if (GUI.Button (model.rRectExit, "", skinExit.button)) 
				{
                    model.PlaySound(model.goAudioMenuButton);
					
					Control.CalculateHighScores((int)model.snake.mnScore);
					
					model.Save();

                    SceneManager.LoadScene(model.sHighScore);
				}
				*/
                /*

                */
				
		/*		model.rRectQuestion = new Rect(0, Screen.height / fGUIScale - model.rRectQuestion.height, model.rRectQuestion.width, model.rRectQuestion.height);
				
				if (GUI.Button (model.rRectQuestion, "", skinQuestion.button)) 
				{
                    model.PlaySound(goAudioMenuButton);
					
					bUpdateWas = false;
					model.bHelp = true;
					model.bPause = true;
					
				}
                */
			}
			
			//Control.GamePause (bPauseNew);
			
		}
		
		if (model.state == State.HELP) 
		{
			model.rRectOk = new Rect((Screen.width / fGUIScale- model.rRectOk.width) / 2.0f, Screen.height / fGUIScale - model.nButtonsPixelSize, model.rRectOk.width, model.rRectOk.height);
			
			if (GUI.Button (model.rRectOk, "", skinOk.button)) 
			{
                model.PlaySound(Sound.MENU);
				model.bHelpAtStart = false;
				model.Save();
				RemoveFromSceneAll();
                model.state = State.GAME;
            }
			
			model.rRectTouch = new Rect(Screen.width / fGUIScale- model.rRectTouch.width, Screen.height / fGUIScale - model.rRectTouch.height, model.rRectTouch.width, model.rRectTouch.height);
			
			if(model.bRightTouchHover)
				GUI.DrawTexture(model.rRectTouch, textureTouchHover);
			else
				GUI.DrawTexture(model.rRectTouch, textureTouch);
			
			model.rRectTouchLeft = new Rect(0, Screen.height / fGUIScale - model.rRectTouchLeft.height, model.rRectTouchLeft.width, model.rRectTouchLeft.height);
			
			if(model.bLeftTouchHover)
				GUI.DrawTexture(model.rRectTouchLeft, textureTouchHoverLeft);
			else
				GUI.DrawTexture(model.rRectTouchLeft, textureTouchLeft);
			
			model.rRectTapRotate = new Rect((Screen.width / fGUIScale) / 4.0f - model.rRectTapRotate.width / 2.0f, 0, model.rRectTapRotate.width, model.rRectTapRotate.height);
			
			GUI.DrawTexture(model.rRectTapRotate, textureTapRotateLeft);
			
			model.rRectTapRotate = new Rect((Screen.width / fGUIScale) * 3.0f / 4.0f - model.rRectTapRotate.width / 2.0f, 0, model.rRectTapRotate.width, model.rRectTapRotate.height);
			
			GUI.DrawTexture(model.rRectTapRotate, textureTapRotateRight);
		}
		
		GUI.matrix = svMat;
		
		//goPause.GetComponent<GUIText>().enabled = model.bPause && !model.bHelp;
		goScore.GetComponent<GUIText>().enabled = model.state != State.PAUSE;
		//goLine.GetComponent<Renderer>().enabled = model.bHelp;
		//		goLike.guiText.enabled = model.bPause;
		
		//goLike.guiText.transform.position = new Vector3((Screen.width - model.nButtonsPixelSize * fGUIScale) / (float)Screen.width, 
		//goLike.guiText.transform.position.y,
		//goLike.guiText.transform.position.z);
	}
	
	void OnTimerDead()
	{
        model.state = State.RESULT;
    }
	
	void OnTimer()
	{
		model.SnakeNext();
		
		bUpdate = true;
		
		if(model.snake.meGameState == EGameState.END && !bTimerDead)
		{
            model.PlaySound(Sound.DEAD);
			model.Vibrate();
			bTimerDead = true;
		}
		
		if(model.snake.meGameState == EGameState.FEED_EAT)
		{
            model.PlaySound(Sound.FEED);
		}
		
		if(model.snake.meGameState == EGameState.BONUS_APPEAR)
		{
            model.PlaySound(Sound.FEED);
            model.PlaySound(Sound.BONUS_APPEAR);
		}
		
		if(model.snake.meGameState == EGameState.BONUS_EAT)
		{
            model.PlaySound(Sound.BONUS_EAT);
			model.Vibrate();
		}
		
		goScore.GetComponent<GUIText>().text = model.snake.mnScore.ToString();
	}
	
	/*void SetCameraGamePausePosition(bool bAway)
	{
		if(bAway)
			Camera.main.transform.position = new Vector3 (model.fCameraAway,
			                                              model.fCameraAway,
			                                              model.fCameraDistance);
		else
			Camera.main.transform.position = new Vector3 ((float)model.nAreaCellWidth / 2.0f,
			                                              -(float)model.nAreaCellHeight / 2.0f,
			                                              model.fCameraDistance);
	}
	*/

	void Help ()
	{
		if (model.state == State.HELP) 
		{
			timerHelpCurrent += Time.deltaTime;
			
			while (timerHelpCurrent >= model.timerHelpPeriodSnake) 
			{
				model.HelpGo(1);
				
				bUpdate = true;
				
				timerHelpCurrent -= model.timerHelpPeriodSnake;
			}
		}
	}
	/*void Arrows()
	{
		if (model.bArrows) 
		{
			timerCurrentArrows += Time.deltaTime;
			
			if(timerCurrentArrows >= model.timerPeriodArrows)
				timerCurrentArrows = 0.0f;
			
			float fTimerArrows = timerCurrentArrows / model.timerPeriodArrows;
			
			if(fTimerArrows < 0.5f)
			{
				goArrowLeft.transform.position = new Vector3(
					model.fArrowLeftHorizontalPositionStart + 
					fTimerArrows *(model.fArrowLeftHorizontalPositionEnd-model.fArrowLeftHorizontalPositionStart),
					model.fArrowVerticalPosition, 0.0f);
				
				goArrowRight.transform.position = new Vector3(
					model.fArrowRightHorizontalPositionStart + 
					fTimerArrows *(model.fArrowRightHorizontalPositionEnd-model.fArrowRightHorizontalPositionStart),
					model.fArrowVerticalPosition, 0.0f);
				
			}
			else
			{
				goArrowLeft.transform.position = new Vector3(
					model.fArrowLeftHorizontalPositionStart + 
					(1.0f - fTimerArrows)*(model.fArrowLeftHorizontalPositionEnd-model.fArrowLeftHorizontalPositionStart),
					model.fArrowVerticalPosition, 0.0f);
				
				goArrowRight.transform.position = new Vector3(
					model.fArrowRightHorizontalPositionStart + 
					(1.0f - fTimerArrows)*(model.fArrowRightHorizontalPositionEnd-model.fArrowRightHorizontalPositionStart),
					model.fArrowVerticalPosition, 0.0f);
			}
		}
		else
		{
			goArrowLeft.renderer.enabled = false;
			goArrowRight.renderer.enabled = false;
			
			if(bAudioAfterArrows)
			{
				model.PlaySound(goAudioFeed);
				bAudioAfterArrows = false;
			}
		}
	}
*/
	void ContinuousObjects ()
	{
		if (model.state != State.PAUSE && model.state != State.HELP) 
		{
			timerCurrent += Time.deltaTime;
			timerCurrentGlow += Time.deltaTime;
			
			if(timerCurrentGlow >= model.timerPeriodGlow)
				timerCurrentGlow = 0.0f;
			
			float fTimerGlow = timerCurrentGlow/ model.timerPeriodGlow;
			
			if(fTimerGlow < 0.5f)
				goGlowFeed.transform.localScale = new Vector3(0.75f + fTimerGlow/2.0f, 0.75f + fTimerGlow/2.0f, 1.0f);
			else
				goGlowFeed.transform.localScale = new Vector3(1.25f - fTimerGlow/2.0f, 1.25f - fTimerGlow/2.0f, 1.0f);
			
			
			
			if(model.snake.mbBonus.appear)
			{
				float fBonusScale = ((float)model.snake.mbBonus.size - (float)timerCurrent / (float)model.timerPeriodSnake)/ (float)model.snake.mnBonusMaxSize;
				
				if(model.snake.mbBonus.type == EBonusType.SCORE)
					goBonusScore.transform.localScale = new Vector3(fBonusScale, fBonusScale, 1.0f);
				
				if(model.snake.mbBonus.type == EBonusType.SNAKE)
					if(((float)model.snake.mbBonus.size - (float)timerCurrent / (float)model.timerPeriodSnake)* model.timerPeriodSnake < model.fBonusSnakeTimeEmitFly)
						goBonusSnake.GetComponent<ParticleEmitter>().emit = false;
				//goBonusSnake.transform.localScale = new Vector3(fBonusScale, fBonusScale, 1.0f);
				
			}
			else
			{
				//goBonusSnake.transform.localScale = new Vector3(0.0f,0.0f, 1.0f);
				if(model.snake.mbBonus.type == EBonusType.SCORE)
					goBonusScore.transform.localScale = new Vector3(0.0f,0.0f, 1.0f);
				
				if(model.snake.mbBonus.type == EBonusType.SNAKE)
					goBonusSnake.GetComponent<ParticleEmitter>().emit = true;
			}
		}
	}
	
	void Dead()
	{
		if(model.state != State.PAUSE && bTimerDead)
		{
			timerCurrentDead += Time.deltaTime;
			
			float fTimerGlow = timerCurrentDead/ model.timerPeriodGlow;
			
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
			
			if (timerCurrentDead >= model.timerPeriodDead)
				OnTimerDead ();
		}
	}
	
	void modelCalculate()
	{
		while (timerCurrent >= model.timerPeriodSnake) 
		{
			OnTimer();
			
			timerCurrent -= model.timerPeriodSnake;
		}
	}
	
	void RemoveFromScenePart()
	{
		goSnakeHead.transform.position = new Vector3 (model.nSnakePositionFail + model.fTranslate, -model.nSnakePositionFail - model.fTranslate, goSnakeHead.transform.position.z);
		goSnakeDead.transform.position = new Vector3 (model.nSnakePositionFail + model.fTranslate, -model.nSnakePositionFail - model.fTranslate, goSnakeDead.transform.position.z);
		goGlowDead.transform.position = new Vector3 (goSnakeDead.transform.position.x,goSnakeDead.transform.position.y, goGlowDead.transform.position.z);
		goFeed.transform.position = new Vector3 (model.nSnakePositionFail + model.fTranslate, -model.nSnakePositionFail - model.fTranslate, goFeed.transform.position.z);
		goGlowFeed.transform.position = goFeed.transform.position;
		
		bParticlePlayingOnce = true;
		
		goBonusScore.GetComponent<ParticleSystem>().Stop();
		
		goBonusScore.transform.position = new Vector3(model.nSnakePositionFail,
		                                              -model.nSnakePositionFail,
		                                              goBonusScore.transform.position.z);
		
		goBonusSnake.transform.position = new Vector3(model.nSnakePositionFail,
		                                              -model.nSnakePositionFail,
		                                              goBonusSnake.transform.position.z);
	}
	
	void RemoveFromSceneAll()
	{
		for (int i = 0; i < model.nSnakeMaxSize; i++) 
			argoSnake [i].transform.position = new Vector3 (model.nSnakePositionFail + model.fTranslate, -model.nSnakePositionFail - model.fTranslate, argoSnake [i].transform.position.z);
		
		RemoveFromScenePart ();
		
		
	}
	
	void GraphicsCalculate()
	{
		if (bUpdate) 
		{
			if(model.state == State.HELP)
			{
				for (int i = 1; i < model.snakeHelp.mlSnake.Count; i++) 
					argoSnake [i].transform.position = new Vector3 (model.snakeHelp.mlSnake[i].position.elem1 + model.fTranslate, -model.snakeHelp.mlSnake[i].position.elem2 - model.fTranslate, argoSnake [i].transform.position.z);
				
				for (int i = model.snakeHelp.mlSnake.Count; i < model.nSnakeMaxSize; i++) 
					argoSnake [i].transform.position = new Vector3 (model.nSnakePositionFail + model.fTranslate, -model.nSnakePositionFail - model.fTranslate, argoSnake [i].transform.position.z);
				
				RemoveFromScenePart();
			}
			else
			{
				if(model.snake.meGameState == EGameState.END)
				{
					if(model.snake.mlSnake.Count > 0)
						goSnakeHead.transform.position = new Vector3 (model.nSnakePositionFail + model.fTranslate, -model.nSnakePositionFail - model.fTranslate, goSnakeHead.transform.position.z);
					
					goSnakeDead.transform.position = new Vector3 (model.snake.mlSnake[0].position.elem1 + model.fTranslate, -model.snake.mlSnake[0].position.elem2  - model.fTranslate, goSnakeDead.transform.position.z);
					
					goGlowDead.transform.position = new Vector3 (goSnakeDead.transform.position.x,goSnakeDead.transform.position.y, goGlowDead.transform.position.z);
				}
				else
				{
					if(model.snake.mlSnake.Count > 0)
						goSnakeHead.transform.position = new Vector3 (model.snake.mlSnake[0].position.elem1 + model.fTranslate, -model.snake.mlSnake[0].position.elem2 - model.fTranslate, goSnakeHead.transform.position.z);
				}
				
				for (int i = 1; i < model.snake.mlSnake.Count; i++) 
					argoSnake [i].transform.position = new Vector3 (model.snake.mlSnake[i].position.elem1 + model.fTranslate, -model.snake.mlSnake[i].position.elem2 - model.fTranslate, argoSnake [i].transform.position.z);
				
				for (int i = model.snake.mlSnake.Count; i < model.nSnakeMaxSize; i++) 
					argoSnake [i].transform.position = new Vector3 (model.nSnakePositionFail + model.fTranslate, -model.nSnakePositionFail - model.fTranslate, argoSnake [i].transform.position.z);
				
				goFeed.transform.position = new Vector3 (model.snake.mpFeed.elem1 + model.fTranslate, -model.snake.mpFeed.elem2 - model.fTranslate, goFeed.transform.position.z);
				goGlowFeed.transform.position = goFeed.transform.position;
				
				if(model.snake.mbBonus.appear)
				{
					
					
					if(model.snake.mbBonus.type == EBonusType.SCORE)
					{
						if(!goBonusScore.GetComponent<ParticleSystem>().isPlaying && bParticlePlayingOnce)
						{
							goBonusScore.GetComponent<ParticleSystem>().Play();
							//goBonusScore.particleEmitter.emit = true;
							bParticlePlayingOnce = false;
						}
						
						goBonusScore.transform.position = new Vector3 (model.snake.mbBonus.position.elem1+1, 
						                                               -model.snake.mbBonus.position.elem2-1, 
						                                               goBonusScore.transform.position.z);
						
						goBonusScore.GetComponent<ParticleSystem>().startLifetime = (float)model.snake.mbBonus.size * model.fFirstStartLifeTimeBonusScore / (float)model.nBonusMaxSize;
						
						//if((float)model.snake.mbBonus.size * model.timerPeriodSnake < goBonusScore.particleSystem.startLifetime)
						//goBonusScore.particleSystem = false;
						//	goBonusScore.particleSystem.Stop();
					}
					
					if(model.snake.mbBonus.type == EBonusType.SNAKE)
					{
						/*
					if(!goBonusSnake.particleSystem.isPlaying && bParticlePlayingOnce)
					{
						goBonusSnake.particleSystem.Play();
						bParticlePlayingOnce = false;
					}
*/
						
						goBonusSnake.transform.position = new Vector3 (model.snake.mbBonus.position.elem1+1, 
						                                               -model.snake.mbBonus.position.elem2-1, 
						                                               goBonusSnake.transform.position.z);
						
					}
				}
				else
				{
					bParticlePlayingOnce = true;
					
					goBonusScore.GetComponent<ParticleSystem>().Stop();
					//goBonusSnake.particleSystem.Stop();
					
					goBonusScore.transform.position = new Vector3(model.nSnakePositionFail,
					                                              -model.nSnakePositionFail,
					                                              goBonusScore.transform.position.z);
					
					goBonusSnake.transform.position = new Vector3(model.nSnakePositionFail,
					                                              -model.nSnakePositionFail,
					                                              goBonusSnake.transform.position.z);
					
				}
			}
			
			
			
			bUpdate = false;
			//bUpdateWas = true;
		}
	}
	
//	void SetCamera()
	//{
		//model.CameraReset ();
		
		/*if (Screen.width > Screen.height)
			goLine.transform.position = new Vector3 (goLine.transform.position.x, -((float)model.nAreaCellHeight)+2.0f, goLine.transform.position.z);
		else
			goLine.transform.position = new Vector3 (goLine.transform.position.x, 
			                                         -((float)model.nAreaCellHeight)+2.0f
			                                         -(((float)model.nAreaCellWidth / 2.0f) * (float) Screen.height / (float)Screen.width
			  - (float)model.nAreaCellHeight / 2.0f),
			                                         goLine.transform.position.z);
                                                     */
		/*Pause Help
		1	1	0
		1	0	1
		0	1	
		0	0	0
*/
	//	if(bUpdateWas)
	//		SetCameraGamePausePosition (model.state == State.PAUSE && model.state != State.HELP);
	//}
	
	// Use this for initialization
	void Start ()
    {
        model = GetComponent<Model>();
        // model.CameraReset();

        model.Load();
			
			//model.bPreLoad = true;
		
		model.ConstantsInit ();
        model.Init ();
        model.HelpInit ();
		
		argoSnake = new GameObject[model.nSnakeMaxSize];
		
		for (int i = 0; i < model.nSnakeMaxSize; i++) 
		{
			argoSnake[i] = (GameObject)Instantiate (goSnakeCell, new Vector3 (model.nSnakePositionFail, -model.nSnakePositionFail, 0), Quaternion.identity);
		}		
		
		for (int i = 1; i < model.nSnakeMaxSize; i++) 
		{
			argoSnake[i].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 
			                                                              argoSnake[i-1].GetComponent<SpriteRenderer>().color.a * model.fSnakeTransparentKoef);
		}
		
		//SetCameraGamePausePosition (model.state == State.PAUSE);
		
		//goLine.GetComponent<Renderer>().enabled = false;
		bUpdate = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
	//	SetCamera ();
		//Arrows ();
		
		Help ();
		
		ContinuousObjects ();
		
		Dead ();
		
		modelCalculate ();
		
		GraphicsCalculate ();
		
		
	}
}
