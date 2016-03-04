using UnityEngine;
using System.Collections;

public class GUIPlay : MonoBehaviour {
    public GUISkin skinPlay = null;
    private Model model = null;

    // Use this for initialization
    void Start ()
    {
        model = GetComponent<Model>();
    }
	
	// Update is called once per frame
	void Update () {

    }

    void OnGUI()
    {
        if (model.state == State.PAUSE || model.state == State.RESULT)
        {
            Matrix4x4 mat = GUI.matrix;
            GUI.matrix = model.guiMatrix;

            if (GUI.Button(model.rRectPlay, "", skinPlay.button))
            {
                if (model.state == State.PAUSE)
                {
                    model.state = State.GAME;
                    
                }

                if(model.state == State.RESULT)
                {
                    model.state = State.GAME;
                    model.Init();
                }

                model.PlaySound(Sound.MENU);
            }

            GUI.matrix = mat;
        }
    }
}
