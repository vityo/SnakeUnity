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

public class Model
{
	static public bool bPreLoad = false;

	static public bool bHelpAtStart;
	static public int nTouchHoverCurrent;
	static public int nTouchHoverMax;
	static public bool bRightTouchHover;
	static public bool bLeftTouchHover;
	static public int nDist;
	static public int nDistMax;
	static public int nDistCurrentEvery;
	static public int nDistEvery;
	static public int nStepVertical;
	static public int nStepHorizontal;
	static public int nSnakeHelpCurrentRotateCount;
	static public ERotation erSnakeHelpRotation;
	static public int nSnakeHelpRotateCountMax;
	static public CSnake snakeHelp;

	static public CSnake snake;
	static public bool bHelp;
	static public bool bPause;
	//static public bool bArrows;
	static public bool bMiniSounds;
	static public bool bMusic;
	//static public bool bAds;
	static public int nSnakeSpeedCurrent;
	static public EGUIState egsState;
	static public bool bVibra;

	static public int[] arHighScores;
	static public long lTotal;
}

























