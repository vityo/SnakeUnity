using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour {



	static public void CalculateHighScores(int nNewHighScore)
	{
		int nInsert = nNewHighScore;

		for(int i = 0; i < Constants.nHighScoresCount; i++)
		{
			if(Model.arHighScores[i] <= nInsert)
			{
				int nTemp = Model.arHighScores[i];
				Model.arHighScores[i] = nInsert;
				nInsert = nTemp;
			}
		}

		Model.lTotal += nNewHighScore;
	}

	static public void SetSnakeSpeed(int nSnakeSpeedCurrent)
	{
		if(nSnakeSpeedCurrent >= Constants.nSnakeSpeedMin && 
		   nSnakeSpeedCurrent <= Constants.nSnakeSpeedMax) 
		{
			Model.nSnakeSpeedCurrent = nSnakeSpeedCurrent;
		}
	}

	static public void SnakeNext()
	{
		Model.snake.Next ();
	}

	static public void ConstantsInit()
	{
		Constants.Init ();
	}

	static public void ModelDebugInit()
	{
		if(Model.snake == null)
			Model.snake = new CSnake();

		if(Model.arHighScores == null)
			Model.arHighScores = new int[Constants.nHighScoresCount];
	}

	static public void ModelPreInit()
	{
		Model.arHighScores = new int[Constants.nHighScoresCount];
		Model.egsState = EGUIState.SCORE;
		Model.snake = new CSnake();

	}

	static public void ModelInit()
	{
		if (Model.bHelpAtStart) 
		{
			Model.bHelp = true;
			//Model.bArrows = true;
			Model.bPause = true;
		}
		else
		{
			Model.bHelp = false;
			//Model.bArrows = true;
			Model.bPause = false;
		}

		//Model.bMiniSounds = true;
		//Model.bMusic = true;

		//Model.bVibra = true;
		//Model.bAds = false;

		Model.snake.mpArea 				= new CPair(Constants.nAreaCellWidth, Constants.nAreaCellHeight);
		Model.snake.mnSnakeMaxSize  	= Constants.nSnakeMaxSize;
		Model.snake.mnScorePerFeed 		= Constants.nScorePerFeedPerSpeed * Model.nSnakeSpeedCurrent;
		Model.snake.mnScorePerBonusSize = Constants.nScorePerBonusSizePerSpeed * Model.nSnakeSpeedCurrent;
		Model.snake.mnBonusMaxSize 		= Constants.nBonusMaxSize;
		Model.snake.mnBonusCellSize 	= Constants.nBonusCellSize;
		Model.snake.mnFeedPerBonus 		= Constants.nFeedPerBonus;

		CPair spPair = new CPair(Model.snake.mpArea.elem1 / 2, Model.snake.mpArea.elem2 / 2);
		EDirection edDirection = EDirection.RIGHT;
		Model.snake.NewGame(true, Constants.nSnakeFirstCount, spPair, edDirection);
	}

	static public void ModelHelpInit()
	{
		Model.nTouchHoverCurrent = 0;
		Model.nTouchHoverMax = 3;
		Model.bRightTouchHover = false;
		Model.bLeftTouchHover = false;
		Model.nDist = 0;
		Model.nDistMax = 3;
		Model.nDistCurrentEvery = 0;
		Model.nDistEvery = 3;
		Model.nStepVertical = 0;
		Model.nStepHorizontal = 0;
		Model.nSnakeHelpCurrentRotateCount = 0;
		Model.erSnakeHelpRotation = ERotation.LEFT;
		Model.nSnakeHelpRotateCountMax = 4;

		Model.snakeHelp = new CSnake();
		Model.snakeHelp.mpArea 				= new CPair(Constants.nAreaCellWidth, Constants.nAreaCellHeight);
		Model.snakeHelp.mnSnakeMaxSize  	= Constants.nSnakeMaxSize;
		Model.snakeHelp.mnScorePerFeed 		= Constants.nScorePerFeedPerSpeed * Model.nSnakeSpeedCurrent;
		Model.snakeHelp.mnScorePerBonusSize = Constants.nScorePerBonusSizePerSpeed * Model.nSnakeSpeedCurrent;
		Model.snakeHelp.mnBonusMaxSize 		= Constants.nBonusMaxSize;
		Model.snakeHelp.mnBonusCellSize 	= Constants.nBonusCellSize;
		Model.snakeHelp.mnFeedPerBonus 		= Constants.nFeedPerBonus;

		Model.snakeHelp.NewGame(false,Constants.nSnakeHelpSize, Constants.pSnakeHelp, EDirection.UP);

		ModelHelpGo (Constants.nSnakeHelpSize);
		ModelHelpGo (Constants.nSnakeHelpGo);

	}

	static public void ModelHelpGo (int nStep)
    {
		for(int i = 0; i < nStep; i++)
		{
			if(Model.nSnakeHelpCurrentRotateCount == Model.nDistEvery)
			{
				Model.nDist = Model.nDistMax;
				Model.nDistCurrentEvery = 0;
			}
			else
				Model.nDist = 0;

			if(Model.nStepVertical == Constants.pSnakeHelpRect.elem2 ||
			   Model.nStepHorizontal == Constants.pSnakeHelpRect.elem1 / 2 + Model.nDist)
			{
				Model.nStepVertical = 0;
				Model.nStepHorizontal = 0;

				if(Model.nSnakeHelpCurrentRotateCount == Model.nSnakeHelpRotateCountMax)
				{
					Model.erSnakeHelpRotation = Model.erSnakeHelpRotation == ERotation.LEFT ? ERotation.RIGHT : ERotation.LEFT;
					Model.nSnakeHelpCurrentRotateCount = 0;
				}
					
				Model.snakeHelp.Rotate(Model.erSnakeHelpRotation);

				Model.bLeftTouchHover = Model.erSnakeHelpRotation == ERotation.LEFT;
				Model.bRightTouchHover = Model.erSnakeHelpRotation == ERotation.RIGHT;

				Model.nSnakeHelpCurrentRotateCount++;
			}

		   	Model.snakeHelp.Next();

			if(Model.bLeftTouchHover || Model.bRightTouchHover)
			{
				Model.nTouchHoverCurrent++;

				if(Model.nTouchHoverCurrent == Model.nTouchHoverMax)
				{
					Model.nTouchHoverCurrent = 0;
					Model.bLeftTouchHover = Model.bRightTouchHover = false;
				}
			}

			if(Model.snakeHelp.mlSnake[0].direction == EDirection.UP ||
			   Model.snakeHelp.mlSnake[0].direction == EDirection.DOWN)
				Model.nStepVertical++;

			if(Model.snakeHelp.mlSnake[0].direction == EDirection.LEFT ||
			   Model.snakeHelp.mlSnake[0].direction == EDirection.RIGHT)
				Model.nStepHorizontal++;
		}
	}

	void SnakeRotateLeft()
	{
		Model.snake.Rotate (ERotation.LEFT);
	}
	
	void SnakeRotateRight()
	{
		Model.snake.Rotate (ERotation.RIGHT);
	}

	void Action(Vector2 action)
	{
		if(action.y <= Screen.height - Constants.nButtonsPixelSize * View.fGUIScale ||
		   action.x >= Constants.nButtonsPixelSize * View.fGUIScale)
		{
			if (action.x < Screen.width / 2)
				SnakeRotateLeft ();
			else
				SnakeRotateRight ();
		}
	}

	// Use this for initialization
	void Start () {
	
	}

	void Update () {

		if (!Model.bPause) 
		{
			for (int i = 0; i < Input.touchCount; i++) 
			{
				if(Input.GetTouch (i).phase == TouchPhase.Began)
				{
					//Model.bArrows = false;

					Action(Input.GetTouch (i).position);
				}
			}

			if(Input.GetMouseButtonDown(0))
			{
				//Model.bArrows = false;

				Action(Input.mousePosition);
			}
		}
	}
}










