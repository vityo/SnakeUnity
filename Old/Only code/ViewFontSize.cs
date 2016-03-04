using UnityEngine;
using System.Collections;

public enum EFontType {YOURSCORE, SCORE, NULL}

public class ViewFontSize : MonoBehaviour {

	public float fSize = 0.0f;

	void OnGUI()
	{
		int nKoef = Screen.width > Screen.height ? 
			(int)(Constants.fFontScale * (float) Screen.height / (float)Constants.nAreaCellHeight) : 
			(int)(Constants.fFontScale * (float) Screen.width / (float)Constants.nAreaCellWidth);

		guiText.fontSize = (int) (nKoef * fSize);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}













