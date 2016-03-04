using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Constants : MonoBehaviour {

	static public string sGame = "Game";
	static public string sHighScore = "HighScore";
	static public string sHighScores = "HIGH SCORES";

	static public string sYourScore = "YOUR SCORE";
	static public string sLevel = "LEVEL ";
	static public string sHighScoreMid = ". ";
	static public string sHelpAtStartKey = "helpatstart";
	static public string sVibrationKey = "vibration";
	static public string sMiniSoundsKey = "minisounds";
	static public string sMusicKey = "music";
	static public string sSpeedKey = "speed";
	static public string sHighScoreKey = "highscore";
	static public string sTotalKey = "total";
	static public string sTrue = "true";
	static public string sFalse = "false";
	static public string sTotal = "TOTAL ";
	static public Rect rRectPause;
	static public Rect rRectMiniSounds;
	static public Rect rRectMusic;
	static public Rect rRectPlay;
	//static public Rect rRectAds;
	static public Rect rRectSpeed;
	static public Rect rRectExit;
	static public Rect rRectLike;
	static public Rect rRectVibra;
	static public Rect rRectHigh;
	static public Rect rRectOk;
	static public Rect rRectTouch;
	static public Rect rRectTouchLeft;
	static public Rect rRectTapRotate;
	static public Rect rRectQuestion;

	static public int nSnakeHelpGo = 15;
	static public int nSnakeFirstCount = 3;
	static public int nSnakeLeftNextFirst = 0;
	static public EDirection edSnakeLeftDirection = EDirection.UP;
	static public CPair pSnakeHelpRect = new CPair (10, 6);
	static public CPair pSnakeHelp = new CPair (6, 9);
	static public int nSnakeHelpSize = 10;
	static public float fHighScoreLeft = 0.08f;
	static public float fHighScoreTopFirst = 0.65f;
	static public float fHighScoreDist = 0.1f;
	static public int nHighScoresCount = 5;
	static public float fGUITopLabel = 0.6f;
	static public float fGUITopLabelHigh = 0.75f;
	static public float fBonusSnakeTimeEmitFly = 1.0f;
	static public int nSnakeMaxSize = 100;
	public const int nAreaCellWidth = 16;
	public const int nAreaCellHeight = 13;
	public const float fCameraDistance = -10.0f;
	public const float fCameraAway = 100.0f;
	static public int nScorePerFeedPerSpeed = 1;
	static public int nScorePerBonusSizePerSpeed = 1;
	static public int nBonusMaxSize = 30;
	static public int nBonusCellSize = 2;
	static public int nFeedPerBonus = 5;

	static public float fFirstStartLifeTimeBonusScore = 1.0f;

	static public int nSnakePositionFail = -100;
	static public float timerPeriodPerSnakeSpeed = 0.1f;
	static public float timerPeriodSnakeKoef = 1.0f;
	static public float timerPeriodSnake;
	static public float timerHelpPeriodSnake;
	static public float timerPeriodLogo = 1.5f;
	static public float timerPeriodArrows = 1.0f;
	static public float timerPeriodDead = 3.0f;
	static public float timerPeriodGlow = 3.0f;

	static public float fArrowVerticalPosition = -9.0f;
	static public float fArrowLeftHorizontalPositionStart = 5.5f;
	static public float fArrowLeftHorizontalPositionEnd = 0.0f;
	static public float fArrowRightHorizontalPositionStart = 10.5f;
	static public float fArrowRightHorizontalPositionEnd = 16.0f;

	static public float fSnakeTransparentKoef = 0.97f;
	static public float fButtonCellSize = 2.0f;

	static public float fFontScale = 10.0f;
	static public float fYourScoreSize = 0.1f;
	static public float fScoreSize = 0.2f;

	public const int nButtonsPixelSize = 128;

	static public int nOriginalWidth = nButtonsPixelSize * nAreaCellWidth;
	static public int nOriginalHeight = nButtonsPixelSize * nAreaCellHeight;
	
	static public int nSnakeCellPixelSize;
	static private int nButtonPixelSize;

	static public int nSnakeSpeedFirst = 5;
	static public int nSnakeSpeedMin = 1;
	static public int nSnakeSpeedMax = 10;

	static public float fTranslate = 0.5f;

	static public void Init()
	{
		float fScreen = (float) Screen.width / (float) Screen.height;	
		float fGame = (float) nAreaCellWidth / (float) nAreaCellHeight;
		
		if (fScreen > fGame) 
			nSnakeCellPixelSize = Screen.height / nAreaCellHeight;
		else
			nSnakeCellPixelSize = Screen.width / nAreaCellWidth;

		rRectPlay = new Rect(0, 0, Constants.nButtonsPixelSize, Constants.nButtonsPixelSize);
		rRectPause = new Rect(0, 0, Constants.nButtonsPixelSize, Constants.nButtonsPixelSize);
		rRectMiniSounds = new Rect(Constants.nButtonsPixelSize, 0, Constants.nButtonsPixelSize, Constants.nButtonsPixelSize);
		rRectMusic = new Rect(Constants.nButtonsPixelSize * 2, 0, Constants.nButtonsPixelSize, Constants.nButtonsPixelSize);
		rRectVibra = new Rect(Constants.nButtonsPixelSize * 3, 0, Constants.nButtonsPixelSize, Constants.nButtonsPixelSize);
		//rRectAds = new Rect(Constants.nButtonsPixelSize * 3, 0, Constants.nButtonsPixelSize, Constants.nButtonsPixelSize);
		rRectSpeed = new Rect(Constants.nButtonsPixelSize * 4, 0, Constants.nButtonsPixelSize, Constants.nButtonsPixelSize);
		rRectHigh = new Rect(Constants.nButtonsPixelSize * 5, 0, Constants.nButtonsPixelSize, Constants.nButtonsPixelSize);
		rRectExit = new Rect(Constants.nButtonsPixelSize * 7, 0, Constants.nButtonsPixelSize, Constants.nButtonsPixelSize);
		rRectLike = new Rect(0, 0, Constants.nButtonsPixelSize * 3, Constants.nButtonsPixelSize);
		rRectOk = new Rect(0, 0, Constants.nButtonsPixelSize, Constants.nButtonsPixelSize);
		rRectTouch = new Rect(0, 0, Constants.nButtonsPixelSize * 2, Constants.nButtonsPixelSize * 3);
		rRectTouchLeft = new Rect(0, 0, Constants.nButtonsPixelSize * 2, Constants.nButtonsPixelSize * 3);
		rRectTapRotate = new Rect(0, 0, Constants.nButtonsPixelSize * 4, Constants.nButtonsPixelSize);
		rRectQuestion = new Rect(0, 0, Constants.nButtonsPixelSize, Constants.nButtonsPixelSize);

		Model.egsState = EGUIState.SCORE;

		timerPeriodSnake = Utils.GetTimerPeriodSnakeBySpeed (Model.nSnakeSpeedCurrent);
		timerHelpPeriodSnake = Utils.GetTimerPeriodSnakeBySpeed (Constants.nSnakeSpeedFirst);
	}

	static public int GetSnakeCellPixelSize()
	{
		float fScreen = (float) Screen.width / (float) Screen.height;	
		float fGame = (float) nAreaCellWidth / (float) nAreaCellHeight;
		
		if (fScreen > fGame) 
			nSnakeCellPixelSize = Screen.height / nAreaCellHeight;
		else
			nSnakeCellPixelSize = Screen.width / nAreaCellWidth;

		return nSnakeCellPixelSize;
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
