using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EDirection { LEFT, UP, RIGHT, DOWN};

public enum ERotation {LEFT, RIGHT};

public enum EGameState	{ START, GAME, FEED_EAT, BONUS_APPEAR, BONUS_EAT, END};

public enum EBonusType { NONE = -1, SCORE = 0, SNAKE = 1, COUNT = 2};

public enum EButtonsType {PAUSE, NULL};

public enum EGUIState {SCORE, SPEED, HIGHSCORES};

public static class CUtility
{
	static public EDirection DirectionRotateRight(EDirection direction)
	{
		switch (direction) {
		case EDirection.LEFT:
			return direction = EDirection.UP;
		case EDirection.UP:
			return direction = EDirection.RIGHT;
		case EDirection.RIGHT:
			return direction = EDirection.DOWN;
		case EDirection.DOWN:
			return direction = EDirection.LEFT;
		}
		
		return direction;
	}
	
	static public EDirection DirectionRotateLeft(EDirection direction)
	{
		switch(direction)
		{
		case EDirection.LEFT: return direction = EDirection.DOWN;
		case EDirection.UP: return direction = EDirection.LEFT;
		case EDirection.RIGHT: return direction = EDirection.UP;
		case EDirection.DOWN: return direction = EDirection.RIGHT;
		}
		
		return direction;
	}
}

public class CPair
{
	public int elem1 {get; set;}
	public int elem2 {get; set;}
	
	public CPair(int aElem1, int aElem2)
	{
		elem1 = aElem1;
		elem2 = aElem2;
	}
};

public class CSnakeCell
{
	public CPair position {get; set;}
	public EDirection direction {get; set;}
	
	public CSnakeCell(CPair aPosition, EDirection aDirection)
	{
		position = aPosition;
		direction = aDirection;
	}
	
	public CSnakeCell Copy()
	{
		return new CSnakeCell (new CPair (position.elem1, position.elem2), direction);
	}
};

public class CBonus
{
	public CPair position {get; set;}
	public int size {get; set;}
	public bool appear {get; set;}
	public EBonusType type {get; set;}
	
	public CBonus (CPair aPosition, int nSize, bool bAppear, EBonusType ebtType)
	{
		position = aPosition;
		size = nSize;
		appear = bAppear;
		type = ebtType;
	}
};

public class CSnake
{
	public const int cnFail = -1;
	
	public EGameState 			meGameState 		{get; set;}
	public List<CSnakeCell> 	mlSnake 			{get; set;}
	public CPair 				mpArea				{get; set;}
	public CPair 				mpFeed				{get; set;}
	public CBonus 				mbBonus				{get; set;}
	public int 					mnSnakeMaxSize		{get; set;}
	public long 				mnScore				{get; set;}
	public long 				mnScorePerFeed		{get; set;}
	public long 				mnScorePerBonusSize	{get; set;}
	public int 					mnBonusMaxSize		{get; set;}
	public int 					mnBonusCellSize		{get; set;}
	public int 					mnFeedPerBonusNow	{get; set;}
	public int 					mnFeedPerBonus		{get; set;}
	
	
	public bool mbRotateComplete;
	
	public CSnake()
	{
		
		Random.seed = (int)System.DateTime.Now.Ticks;
		
		meGameState = EGameState.START;
		
		mlSnake = new List<CSnakeCell> ();
		
		mpArea = new CPair (cnFail, cnFail);
		mpFeed = new CPair (cnFail, cnFail);
		mbBonus = new CBonus (new CPair (cnFail, cnFail), cnFail, false, EBonusType.NONE);
		
		mnSnakeMaxSize = cnFail;
		mnScore = 0;
		mnScorePerFeed = cnFail;
		mnScorePerBonusSize = cnFail;
		mnBonusMaxSize = cnFail;
		mnBonusCellSize = cnFail;
		mnFeedPerBonusNow = cnFail;
		mnFeedPerBonus = cnFail;
		
		
		mbRotateComplete = true;
	}
	
	public void NewGame(bool bCreateFeed, int nFirstCount, CPair pFirst, EDirection edDirection)
	{
		meGameState = EGameState.START;
		
		if(mlSnake.Count > 0)
			mlSnake.Clear();
		
		if(nFirstCount>0)
		{
			//CPair spPair = new CPair(mpArea.elem1 / 2, mpArea.elem2 / 2);
			//EDirection edDirection = EDirection.RIGHT;
			CSnakeCell sscCell = new CSnakeCell(pFirst, edDirection);
			mlSnake.Add(sscCell);
			nFirstCount--;
			
			for(int i = 0; i < nFirstCount; i++)
				IncreaseSnake();
		}
		
		if(bCreateFeed)
			GenerateFeed();
		
		mnScore = 0;
		mnFeedPerBonusNow = 0;
		mbBonus.appear = false;
		
		mbRotateComplete = true;
	}
	
	public EGameState Next()
	{
		if(meGameState != EGameState.END)
		{
			meGameState = EGameState.GAME;
			
			mlSnake.RemoveAt(mlSnake.Count - 1);
			
			CSnakeCell sscHead = mlSnake[0].Copy();
			
			switch(sscHead.direction)
			{
			case EDirection.LEFT:
				sscHead.position.elem1--;
				
				if(sscHead.position.elem1 < 0)
					sscHead.position.elem1 = mpArea.elem1 - 1;
				
				break;
				
			case EDirection.RIGHT:
				sscHead.position.elem1++;
				
				if(sscHead.position.elem1 == mpArea.elem1)
					sscHead.position.elem1 = 0;
				
				break;
				
			case EDirection.UP:
				sscHead.position.elem2--;
				
				if(sscHead.position.elem2 < 0)
					sscHead.position.elem2 = mpArea.elem2 - 1;
				
				break;
				
			case EDirection.DOWN:
				sscHead.position.elem2++;
				
				if(sscHead.position.elem2 == mpArea.elem2)
					sscHead.position.elem2 = 0;
				
				break;
			}
			
			if(IsSnake(sscHead.position))
			{
				meGameState = EGameState.END;
				
				return meGameState;
			}
			
			mlSnake.Insert(0, sscHead);
			
			mbRotateComplete = true;
			
			if(sscHead.position.elem1 == mpFeed.elem1 &&
			   sscHead.position.elem2 == mpFeed.elem2)
			{
				IncreaseSnake();
				GenerateFeed();
				
				meGameState = EGameState.FEED_EAT;
				mnScore += mnScorePerFeed;
				
				mnFeedPerBonusNow++;
				
				if(mnFeedPerBonusNow == mnFeedPerBonus)
				{
					mnFeedPerBonusNow = 0;
					GenerateBonus();
					
					meGameState = EGameState.BONUS_APPEAR;
				}
			}
			
			if(IsBonus(sscHead.position))
			{
				switch(mbBonus.type)
				{
				case EBonusType.SCORE:
				{
					IncreaseSnake();
					
					mnScore += mbBonus.size * mnScorePerBonusSize;
				}
					break;
					
				case EBonusType.SNAKE:
				{
					for(int i = 0; i < mnFeedPerBonus; i++)
						DecreaseSnake();
				}
					break;
				}
				
				mbBonus.appear = false;
				meGameState = EGameState.BONUS_EAT;
			}
		}
		
		if(mbBonus.appear)
		{
			mbBonus.size--;
			
			if(mbBonus.size == 0)
				mbBonus.appear = false;
		}
		
		return meGameState;
	}
	
	public bool Rotate(ERotation erRotate)
	{
		if(mbRotateComplete)
		{
			if(erRotate == ERotation.LEFT)
				mlSnake[0].direction = CUtility.DirectionRotateLeft(mlSnake[0].direction);
			else
				mlSnake[0].direction = CUtility.DirectionRotateRight(mlSnake[0].direction);
			
			mbRotateComplete = false;
			
			return true;
		}
		
		return false;
	}
	
	private void GenerateFeed()
	{
		CPair pFeed = new CPair(cnFail, cnFail);
		
		do
		{
			pFeed.elem1 = Random.Range(0, mpArea.elem1);
			pFeed.elem2 = Random.Range(0, mpArea.elem2);
		}while(IsSnake(pFeed));
		
		mpFeed.elem1 = pFeed.elem1;
		mpFeed.elem2 = pFeed.elem2;
	}
	
	private void GenerateBonus()
	{
		CPair pBonus = new CPair(cnFail, cnFail);
		bool bBonusCreate = true;
		
		do
		{
			bBonusCreate = true;
			
			pBonus.elem1 = Random.Range(0, mpArea.elem1 - mnBonusCellSize);
			pBonus.elem2 = Random.Range(0, mpArea.elem2 - mnBonusCellSize);
			
			for(int i = 0; i < mnBonusCellSize && bBonusCreate; i++)
				for(int j = 0; j < mnBonusCellSize && bBonusCreate; j++)
			{
				CPair spBonusPart = new CPair(pBonus.elem1 + i, pBonus.elem2 + j);
				
				if(IsSnake(spBonusPart) ||
				   (mpFeed.elem1 == spBonusPart.elem1 &&
				 mpFeed.elem2 == spBonusPart.elem2))
					bBonusCreate = false;
			}
		}while(!bBonusCreate);
		
		mbBonus.position.elem1 = pBonus.elem1;
		mbBonus.position.elem2 = pBonus.elem2;
		mbBonus.appear = true;
		mbBonus.size = mnBonusMaxSize;
		mbBonus.type = (EBonusType)(Random.Range(0, (int)EBonusType.COUNT));
	}
	
	private void IncreaseSnake()
	{
		if(mlSnake.Count < mnSnakeMaxSize)
		{
			if(mlSnake.Count > 0)
			{
				mlSnake.Add(mlSnake[0]);
			}
		}
	}
	
	private void DecreaseSnake()
	{
		if(mlSnake.Count > 1)
			mlSnake.RemoveAt(mlSnake.Count - 1);
	}
	
	private bool IsSnake(CPair position)
	{
		for (int i = 0; i < mlSnake.Count; i++) 
			if (mlSnake [i].position.elem1 == position.elem1 &&
			    mlSnake [i].position.elem2 == position.elem2)
				return true;
		
		return false;
	}
	
	private bool IsBonus(CPair position)
	{
		if(mbBonus.appear)
			for(int i = 0; i < mnBonusCellSize; i++)
				for(int j = 0; j < mnBonusCellSize; j++)
			{
				CPair spBonusPart = new CPair(mbBonus.position.elem1 + i, mbBonus.position.elem2 + j);
				
				if(position.elem1 == spBonusPart.elem1 &&
				   position.elem2 == spBonusPart.elem2)
					return true;
			}
		
		return false;
	}
	
	
}


public enum State { GAME, PAUSE, RESULT, HELP };
public enum Sound { MENU, DEAD, FEED, BONUS_APPEAR, BONUS_EAT };

public class Model : MonoBehaviour
{
    public string sGame = "Game";
    public string sHighScore = "HighScore";
    public string sHighScores = "HIGH SCORES";

    public string sYourScore = "YOUR SCORE";
    public string sLevel = "LEVEL ";
    public string sHighScoreMid = ". ";
    public string sHelpAtStartKey = "helpatstart";
    public string sVibrationKey = "vibration";
    public string sMiniSoundsKey = "minisounds";
    public string sMusicKey = "music";
    public string sSpeedKey = "speed";
    public string sHighScoreKey = "highscore";
    public string sTotalKey = "total";
    public string sTrue = "true";
    public string sFalse = "false";
    public string sTotal = "TOTAL ";
    public Rect rRectPause;
    public Rect rRectMiniSounds;
    public Rect rRectMusic;
    public Rect rRectPlay;
    //public Rect rRectAds;
    public Rect rRectSpeed;
    public Rect rRectExit;
    public Rect rRectLike;
    public Rect rRectVibra;
    public Rect rRectHigh;
    public Rect rRectOk;
    public Rect rRectTouch;
    public Rect rRectTouchLeft;
    public Rect rRectTapRotate;
    public Rect rRectQuestion;
    public Rect rectOptions;

    public int nSnakeHelpGo = 15;
    public int nSnakeFirstCount = 3;
    public int nSnakeLeftNextFirst = 0;
    public EDirection edSnakeLeftDirection = EDirection.UP;
    public CPair pSnakeHelpRect = new CPair(10, 6);
    public CPair pSnakeHelp = new CPair(6, 9);
    public int nSnakeHelpSize = 10;
    public float fHighScoreLeft = 0.08f;
    public float fHighScoreTopFirst = 0.65f;
    public float fHighScoreDist = 0.1f;
    public int nHighScoresCount = 5;
    public float fGUITopLabel = 0.6f;
    public float fGUITopLabelHigh = 0.75f;
    public float fBonusSnakeTimeEmitFly = 1.0f;
    public int nSnakeMaxSize = 100;
    public int nAreaCellWidth = 16;
    public int nAreaCellHeight = 13;
    public float fCameraDistance = -10.0f;
    public float fCameraAway = 100.0f;
    public int nScorePerFeedPerSpeed = 1;
    public int nScorePerBonusSizePerSpeed = 1;
    public int nBonusMaxSize = 30;
    public int nBonusCellSize = 2;
    public int nFeedPerBonus = 5;

    public float fFirstStartLifeTimeBonusScore = 1.0f;

    public int nSnakePositionFail = -100;
    public float timerPeriodPerSnakeSpeed = 0.1f;
    public float timerPeriodSnakeKoef = 1.0f;
    public float timerPeriodSnake;
    public float timerHelpPeriodSnake;
    public float timerPeriodLogo = 1.5f;
    public float timerPeriodArrows = 1.0f;
    public float timerPeriodDead = 3.0f;
    public float timerPeriodGlow = 3.0f;

    public float fArrowVerticalPosition = -9.0f;
    public float fArrowLeftHorizontalPositionStart = 5.5f;
    public float fArrowLeftHorizontalPositionEnd = 0.0f;
    public float fArrowRightHorizontalPositionStart = 10.5f;
    public float fArrowRightHorizontalPositionEnd = 16.0f;

    public float fSnakeTransparentKoef = 0.97f;
    public float fButtonCellSize = 2.0f;

    public float fFontScale = 10.0f;
    public float fYourScoreSize = 0.1f;
    public float fScoreSize = 0.2f;

    public int nButtonsPixelSize = 128;

    public int nSnakeCellPixelSize;

    public int nSnakeSpeedFirst = 5;

    public int buttonPlayPixels = 192;

    public float fTranslate = 0.5f;
    public Matrix4x4 guiMatrix = Matrix4x4.zero;

    public float fGUIScale = 1.0f;


   // public bool bPreLoad = false;
	
	public bool bHelpAtStart;
	public int nTouchHoverCurrent;
	public int nTouchHoverMax;
	public bool bRightTouchHover;
	public bool bLeftTouchHover;
	public int nDist;
	public int nDistMax;
	public int nDistCurrentEvery;
	public int nDistEvery;
	public int nStepVertical;
	public int nStepHorizontal;
	public int nSnakeHelpCurrentRotateCount;
	public ERotation erSnakeHelpRotation;
	public int nSnakeHelpRotateCountMax;
	public CSnake snakeHelp;
	
	public CSnake snake;

	public bool bMiniSounds;
	public bool bMusic;
    public bool bVibra;
    private _options;
    public bool options {
    get
        {
            return _options;
        }
    set
        {

        }
    } = true;
    
    public int nSnakeSpeedCurrent;
	
	public int[] arHighScores;
	public long lTotal;
    private GameObject goSoundBackground = null;
    public GameObject goSoundBackgroundGame = null;
    public GameObject goSoundBackgroundResult = null;
    public GameObject goSoundMenu = null;
    public GameObject goSoundDead = null;
    public GameObject goSoundFeed = null;
    public GameObject goSoundBonusAppear = null;
    public GameObject goSoundBonusEat = null;
    public GameObject goTopLabel = null;
    public GameObject goScoreResult = null;

    private State _state;
    public State state
    {
        get { return _state; }
        set
        {
            _state = value;

            goTopLabel.GetComponent<GUIText>().enabled = _state == State.RESULT;

            if (_state == State.RESULT)
            {
                CalculateHighScores((int)snake.mnScore);

                Save();

                goScoreResult.GetComponent<GUIText>().enabled = true;
                goScoreResult.GetComponent<GUIText>().text = snake.mnScore.ToString();

                if (goSoundBackground != goSoundBackgroundResult)
                {
                    goSoundBackground = goSoundBackgroundResult;
                    MusicPlay();
                }

            //    Ads.ShowAd();
            }
            else
            {
                goScoreResult.GetComponent<GUIText>().enabled = false;
            }

            if (_state == State.GAME)
            {
                Camera.main.transform.position = new Vector3((float)nAreaCellWidth / 2.0f, -(float)nAreaCellHeight / 2.0f, fCameraDistance);
                goSoundBonusAppear.GetComponent<AudioSource>().pitch = 3.0f / (11.0f - (((float)nSnakeSpeedCurrent - 10.0f) / 2.0f + 10.0f));
            }

            if (_state == State.PAUSE || _state == State.RESULT)
            {
                Camera.main.transform.position = new Vector3(fCameraAway, fCameraAway, fCameraDistance);
            }

            if (_state == State.GAME || _state == State.PAUSE)
            {
                if (goSoundBackground != goSoundBackgroundGame)
                {
                    if (goSoundBackground)
                    {
                        goSoundBackground.GetComponent<AudioSource>().Stop();
                    }

                    goSoundBackground = goSoundBackgroundGame;
                    MusicPlay();
                }
            }
        }
    }

    private void MusicPlay()
    {
        if(bMusic)
        {
            goSoundBackground.GetComponent<AudioSource>().Play();
        }
    }

    public void MusicPlayPause()
    {
        if (goSoundBackground)
        {
            if (goSoundBackground.GetComponent<AudioSource>().isPlaying)
                goSoundBackground.GetComponent<AudioSource>().Pause();
            else
                goSoundBackground.GetComponent<AudioSource>().Play();
        }
    }
    public Rect getPauseRectScaled()
    {
        return new Rect(rRectPause.x * fGUIScale, Screen.height - rRectPause.height * fGUIScale, rRectPause.width * fGUIScale, rRectPause.height * fGUIScale);
    }
    public int GetSnakeCellPixelSize()
    {
        float fScreen = (float)Screen.width / (float)Screen.height;
        float fGame = (float)nAreaCellWidth / (float)nAreaCellHeight;

        if (fScreen > fGame)
            nSnakeCellPixelSize = Screen.height / nAreaCellHeight;
        else
            nSnakeCellPixelSize = Screen.width / nAreaCellWidth;

        return nSnakeCellPixelSize;
    }
    public void Init()
    {
  /*      if (bHelpAtStart)
        {
            state = State.HELP;
        }
        else
        {
            state = State.PAUSE;
        }
        */
        snake.mpArea = new CPair(nAreaCellWidth, nAreaCellHeight);
        snake.mnSnakeMaxSize = nSnakeMaxSize;
        snake.mnScorePerFeed = nScorePerFeedPerSpeed * nSnakeSpeedCurrent;
        snake.mnScorePerBonusSize = nScorePerBonusSizePerSpeed * nSnakeSpeedCurrent;
        snake.mnBonusMaxSize = nBonusMaxSize;
        snake.mnBonusCellSize = nBonusCellSize;
        snake.mnFeedPerBonus = nFeedPerBonus;

        CPair spPair = new CPair(snake.mpArea.elem1 / 2, snake.mpArea.elem2 / 2);
        EDirection edDirection = EDirection.RIGHT;
        snake.NewGame(true, nSnakeFirstCount, spPair, edDirection);
    }

    public void PlaySound(Sound sound)
    {
        if (bMiniSounds)
        {
            GameObject goSound = null;

            switch (sound)
            {
                case Sound.MENU:
                    goSound = goSoundMenu;        
                    break;
                case Sound.DEAD:
                    goSound = goSoundDead;
                    break;
                case Sound.FEED:
                    goSound = goSoundFeed;
                    break;
                case Sound.BONUS_APPEAR:
                    goSound = goSoundBonusAppear;
                    break;
                case Sound.BONUS_EAT:
                    goSound = goSoundBonusEat;
                    break;
            }

            goSound.GetComponent<AudioSource>().PlayOneShot(goSound.GetComponent<AudioSource>().clip);
        }
    }

    public void CalculateHighScores(int nNewHighScore)
    {
        int nInsert = nNewHighScore;

        for (int i = 0; i < nHighScoresCount; i++)
        {
            if (arHighScores[i] <= nInsert)
            {
                int nTemp = arHighScores[i];
                arHighScores[i] = nInsert;
                nInsert = nTemp;
            }
        }

        lTotal += nNewHighScore;
    }

    public void SnakeNext()
    {
        snake.Next();
    }

    public void ConstantsInit()
    {
        float fScreen = (float)Screen.width / (float)Screen.height;
        float fGame = (float)nAreaCellWidth / (float)nAreaCellHeight;

        if (fScreen > fGame)
            nSnakeCellPixelSize = Screen.height / nAreaCellHeight;
        else
            nSnakeCellPixelSize = Screen.width / nAreaCellWidth;

        //rRectAds = new Rect(nButtonsPixelSize * 3, 0, nButtonsPixelSize, nButtonsPixelSize);
        rRectSpeed = new Rect(nButtonsPixelSize * 4, 0, nButtonsPixelSize, nButtonsPixelSize);
        rRectHigh = new Rect(nButtonsPixelSize * 5, 0, nButtonsPixelSize, nButtonsPixelSize);
        rRectExit = new Rect(nButtonsPixelSize * 7, 0, nButtonsPixelSize, nButtonsPixelSize);
        rRectOk = new Rect(0, 0, nButtonsPixelSize, nButtonsPixelSize);
        rRectTouch = new Rect(0, 0, nButtonsPixelSize * 2, nButtonsPixelSize * 3);
        rRectTouchLeft = new Rect(0, 0, nButtonsPixelSize * 2, nButtonsPixelSize * 3);
        rRectTapRotate = new Rect(0, 0, nButtonsPixelSize * 4, nButtonsPixelSize);
        rRectQuestion = new Rect(0, 0, nButtonsPixelSize, nButtonsPixelSize);

        timerPeriodSnake = GetTimerPeriodSnakeBySpeed(nSnakeSpeedCurrent);
        timerHelpPeriodSnake = GetTimerPeriodSnakeBySpeed(nSnakeSpeedFirst);

        fGUIScale = (float)GetSnakeCellPixelSize() * fButtonCellSize / (float)nButtonsPixelSize;
        Vector3 scale = new Vector3(fGUIScale, fGUIScale, 1);
        guiMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, scale);
        
        rRectPause = new Rect(Screen.width / fGUIScale - nButtonsPixelSize, 0, nButtonsPixelSize, nButtonsPixelSize);
        rRectPlay = new Rect(Screen.width / fGUIScale - buttonPlayPixels * 1.5f,
            (Screen.height / fGUIScale - buttonPlayPixels) * 0.5f, buttonPlayPixels, buttonPlayPixels);

        rRectLike = new Rect(nButtonsPixelSize, Screen.height / fGUIScale - nButtonsPixelSize, nButtonsPixelSize, nButtonsPixelSize);

        rectOptions = new Rect(0, Screen.height / fGUIScale - nButtonsPixelSize, nButtonsPixelSize, nButtonsPixelSize);
        rRectMiniSounds = new Rect(0, Screen.height / fGUIScale - nButtonsPixelSize * 2, nButtonsPixelSize, nButtonsPixelSize);
        rRectMusic = new Rect(0, Screen.height / fGUIScale - nButtonsPixelSize * 3, nButtonsPixelSize, nButtonsPixelSize);
        rRectVibra = new Rect(0, Screen.height / fGUIScale - nButtonsPixelSize * 4, nButtonsPixelSize, nButtonsPixelSize);

        Camera.main.orthographicSize = (float)nAreaCellHeight / 2.0f;

        snake = new CSnake();
        state = State.PAUSE;
    }

    public void DebugInit()
    {
        if (snake == null)
            snake = new CSnake();

        if (arHighScores == null)
            arHighScores = new int[nHighScoresCount];
    }

    public void HelpInit()
    {
        nTouchHoverCurrent = 0;
        nTouchHoverMax = 3;
        bRightTouchHover = false;
        bLeftTouchHover = false;
        nDist = 0;
        nDistMax = 3;
        nDistCurrentEvery = 0;
        nDistEvery = 3;
        nStepVertical = 0;
        nStepHorizontal = 0;
        nSnakeHelpCurrentRotateCount = 0;
        erSnakeHelpRotation = ERotation.LEFT;
        nSnakeHelpRotateCountMax = 4;

        snakeHelp = new CSnake();
        snakeHelp.mpArea = new CPair(nAreaCellWidth, nAreaCellHeight);
        snakeHelp.mnSnakeMaxSize = nSnakeMaxSize;
        snakeHelp.mnScorePerFeed = nScorePerFeedPerSpeed * nSnakeSpeedCurrent;
        snakeHelp.mnScorePerBonusSize = nScorePerBonusSizePerSpeed * nSnakeSpeedCurrent;
        snakeHelp.mnBonusMaxSize = nBonusMaxSize;
        snakeHelp.mnBonusCellSize = nBonusCellSize;
        snakeHelp.mnFeedPerBonus = nFeedPerBonus;

        snakeHelp.NewGame(false, nSnakeHelpSize, pSnakeHelp, EDirection.UP);

        HelpGo(nSnakeHelpSize);
        HelpGo(nSnakeHelpGo);
    }

    public void HelpGo(int nStep)
    {
        for (int i = 0; i < nStep; i++)
        {
            if (nSnakeHelpCurrentRotateCount == nDistEvery)
            {
                nDist = nDistMax;
                nDistCurrentEvery = 0;
            }
            else
                nDist = 0;

            if (nStepVertical == pSnakeHelpRect.elem2 ||
               nStepHorizontal == pSnakeHelpRect.elem1 / 2 + nDist)
            {
                nStepVertical = 0;
                nStepHorizontal = 0;

                if (nSnakeHelpCurrentRotateCount == nSnakeHelpRotateCountMax)
                {
                    erSnakeHelpRotation = erSnakeHelpRotation == ERotation.LEFT ? ERotation.RIGHT : ERotation.LEFT;
                    nSnakeHelpCurrentRotateCount = 0;
                }

                snakeHelp.Rotate(erSnakeHelpRotation);

                bLeftTouchHover = erSnakeHelpRotation == ERotation.LEFT;
                bRightTouchHover = erSnakeHelpRotation == ERotation.RIGHT;

                nSnakeHelpCurrentRotateCount++;
            }

            snakeHelp.Next();

            if (bLeftTouchHover || bRightTouchHover)
            {
                nTouchHoverCurrent++;

                if (nTouchHoverCurrent == nTouchHoverMax)
                {
                    nTouchHoverCurrent = 0;
                    bLeftTouchHover = bRightTouchHover = false;
                }
            }

            if (snakeHelp.mlSnake[0].direction == EDirection.UP ||
               snakeHelp.mlSnake[0].direction == EDirection.DOWN)
                nStepVertical++;

            if (snakeHelp.mlSnake[0].direction == EDirection.LEFT ||
               snakeHelp.mlSnake[0].direction == EDirection.RIGHT)
                nStepHorizontal++;
        }
    }

    void SnakeRotateLeft()
    {
        snake.Rotate(ERotation.LEFT);
    }

    void SnakeRotateRight()
    {
        snake.Rotate(ERotation.RIGHT);
    }

    void Action(Vector2 action)
    {
        if (!getPauseRectScaled().Contains(action))
        {
            if (action.x < Screen.width / 2)
                SnakeRotateLeft();
            else
            {
                SnakeRotateRight();
            }
        }
    }

    public void Vibrate()
    {
        if (bVibra)
        {
            Handheld.Vibrate();
        }
    }

  //  public void CameraReset()
   // {
    //    if (Screen.width > Screen.height)
   //         Camera.main.orthographicSize = (float)nAreaCellHeight / 2.0f;
    //    else
    //        Camera.main.orthographicSize = ((float)nAreaCellWidth / 2.0f) * (float)Screen.height / (float)Screen.width;
  //  }

    public float GetTimerPeriodSnakeBySpeed(int nSpeed)
    {
        //return timerPeriodSnakeMin + timerPeriodPerSnakeSpeed * (nSnakeSpeedMax - nSpeed);
        return timerPeriodSnakeKoef / (float)nSpeed;
    }

    public void Save()
    {
        PlayerPrefs.SetString(sVibrationKey, bVibra ? sTrue : sFalse);
        PlayerPrefs.SetString(sMiniSoundsKey, bMiniSounds ? sTrue : sFalse);
        PlayerPrefs.SetString(sMusicKey, bMusic ? sTrue : sFalse);
        PlayerPrefs.SetInt(sSpeedKey, nSnakeSpeedCurrent);
        PlayerPrefs.SetString(sHelpAtStartKey, bHelpAtStart ? sTrue : sFalse);

        for (int i = 0; i < nHighScoresCount; i++)
        {
            PlayerPrefs.SetInt(sHighScoreKey + (i + 1).ToString(), arHighScores[i]);
        }

        PlayerPrefs.SetString(sTotalKey, lTotal.ToString());

        PlayerPrefs.Save();
    }

    public void Load()
    {
        arHighScores = new int[nHighScoresCount];

        if (PlayerPrefs.HasKey(sVibrationKey))
            bVibra = PlayerPrefs.GetString(sVibrationKey) == sTrue;
        else
            bVibra = true;

        if (PlayerPrefs.HasKey(sMiniSoundsKey))
            bMiniSounds = PlayerPrefs.GetString(sMiniSoundsKey) == sTrue;
        else
            bMiniSounds = true;

        if (PlayerPrefs.HasKey(sMusicKey))
            bMusic = PlayerPrefs.GetString(sMusicKey) == sTrue;
        else
            bMusic = true;

        if (PlayerPrefs.HasKey(sHelpAtStartKey))
            bHelpAtStart = PlayerPrefs.GetString(sHelpAtStartKey) == sTrue;
        else
            bHelpAtStart = true;

        if (PlayerPrefs.HasKey(sSpeedKey))
        {
            nSnakeSpeedCurrent = PlayerPrefs.GetInt(sSpeedKey);
        }
        else
            nSnakeSpeedCurrent = nSnakeSpeedFirst;

        for (int i = 0; i < nHighScoresCount; i++)
        {
            string sHighScoreKeyLocal = sHighScoreKey + (i + 1).ToString();

            if (PlayerPrefs.HasKey(sHighScoreKeyLocal))
                arHighScores[i] = PlayerPrefs.GetInt(sHighScoreKeyLocal);
            else
                arHighScores[i] = 0;
        }

        if (PlayerPrefs.HasKey(sTotalKey))
        {
            long l = 0;

            if (long.TryParse(PlayerPrefs.GetString(sTotalKey), out l))
                lTotal = l;
        }
        else
            lTotal = 0;

    }

    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {

        if (state != State.PAUSE)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (Input.GetTouch(i).phase == TouchPhase.Began)
                {
                    //bArrows = false;

                    Action(Input.GetTouch(i).position);
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                //bArrows = false;

                Action(Input.mousePosition);
            }
        }
    }
}

























