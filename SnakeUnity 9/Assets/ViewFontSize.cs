using UnityEngine;
using System.Collections;

public enum EFontType {YOURSCORE, SCORE, NULL}

public class ViewFontSize : MonoBehaviour {
    private Model model = null;
    public float fSize = 0.0f;

	// Use this for initialization
	void Start () {
        model = Camera.main.GetComponent<Model>();
    }
	
	// Update is called once per frame
	void Update () {

    }
    void OnGUI()
    {
        int nKoef = Screen.width > Screen.height ?
            (int)(model.fFontScale * (float)Screen.height / (float)model.nAreaCellHeight) :
                (int)(model.fFontScale * (float)Screen.width / (float)model.nAreaCellWidth);

        GetComponent<GUIText>().fontSize = (int)(nKoef * fSize);
    }
}













