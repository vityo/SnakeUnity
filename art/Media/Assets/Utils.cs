using UnityEngine;
using System.Collections;


using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using System;
using System.Runtime.Serialization;
using System.Reflection;


public class Utils : MonoBehaviour {

	static public void Vibrate()
	{
		if(Model.bVibra)
		{
			Handheld.Vibrate ();
		}
	}

	static public void CameraReset()
	{
		if (Screen.width > Screen.height)
			Camera.main.orthographicSize = (float)Constants.nAreaCellHeight / 2.0f;
		else
			Camera.main.orthographicSize = ((float)Constants.nAreaCellWidth / 2.0f) * (float) Screen.height / (float)Screen.width;
	}

	static public void PlayMiniSound(GameObject goSound)
	{
		if(Model.bMiniSounds)
		{
			goSound.audio.PlayOneShot(goSound.audio.clip);
		}
	}

	static public float GetTimerPeriodSnakeBySpeed (int nSpeed)
	{
		//return Constants.timerPeriodSnakeMin + Constants.timerPeriodPerSnakeSpeed * (Constants.nSnakeSpeedMax - nSpeed);
		return Constants.timerPeriodSnakeKoef / (float)nSpeed;
	}

	static public void Save () {
		PlayerPrefs.SetString (Constants.sVibrationKey, Model.bVibra ? Constants.sTrue : Constants.sFalse);
		PlayerPrefs.SetString (Constants.sMiniSoundsKey, Model.bMiniSounds ? Constants.sTrue : Constants.sFalse);
		PlayerPrefs.SetString (Constants.sMusicKey, Model.bMusic ? Constants.sTrue : Constants.sFalse);
		PlayerPrefs.SetInt (Constants.sSpeedKey, Model.nSnakeSpeedCurrent);
		PlayerPrefs.SetString (Constants.sHelpAtStartKey, Model.bHelpAtStart ? Constants.sTrue : Constants.sFalse);

		for(int i = 0; i < Constants.nHighScoresCount; i++)
		{
			PlayerPrefs.SetInt (Constants.sHighScoreKey + (i+1).ToString(), Model.arHighScores[i]);
		}

		PlayerPrefs.SetString (Constants.sTotalKey, Model.lTotal.ToString());

		PlayerPrefs.Save ();
	}
	
	static public void Load () {
		if(PlayerPrefs.HasKey(Constants.sVibrationKey))
			Model.bVibra = PlayerPrefs.GetString(Constants.sVibrationKey) == Constants.sTrue;
		else
			Model.bVibra = true;

		if(PlayerPrefs.HasKey(Constants.sMiniSoundsKey))
			Model.bMiniSounds = PlayerPrefs.GetString(Constants.sMiniSoundsKey) == Constants.sTrue;
		else
			Model.bMiniSounds = true;

		if(PlayerPrefs.HasKey(Constants.sMusicKey))
			Model.bMusic = PlayerPrefs.GetString(Constants.sMusicKey) == Constants.sTrue;
		else
			Model.bMusic = true;

		if(PlayerPrefs.HasKey(Constants.sHelpAtStartKey))
			Model.bHelpAtStart = PlayerPrefs.GetString(Constants.sHelpAtStartKey) == Constants.sTrue;
		else
			Model.bHelpAtStart = true;

		if(PlayerPrefs.HasKey(Constants.sSpeedKey))
		{
			Model.nSnakeSpeedCurrent = PlayerPrefs.GetInt(Constants.sSpeedKey);

			if(Model.nSnakeSpeedCurrent < Constants.nSnakeSpeedMin || 
			   Model.nSnakeSpeedCurrent > Constants.nSnakeSpeedMax)
				Model.nSnakeSpeedCurrent = Constants.nSnakeSpeedFirst;
		}
		else
			Model.nSnakeSpeedCurrent = Constants.nSnakeSpeedFirst;

		for(int i = 0; i < Constants.nHighScoresCount; i++)
		{
			string sHighScoreKey = Constants.sHighScoreKey + (i+1).ToString();

			if(PlayerPrefs.HasKey(sHighScoreKey))
				Model.arHighScores[i] = PlayerPrefs.GetInt(sHighScoreKey);
			else
				Model.arHighScores[i] = 0;
		}

		if(PlayerPrefs.HasKey(Constants.sTotalKey))
		{
			long l = 0;

			if(long.TryParse(PlayerPrefs.GetString(Constants.sTotalKey), out l))
				Model.lTotal = l;
		}
		else
			Model.lTotal = 0;

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}



















