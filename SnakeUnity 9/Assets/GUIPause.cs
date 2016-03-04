using UnityEngine;
using System.Collections;

public class GUIPause : MonoBehaviour {
    public GUISkin skinPause = null;
    private Model model = null;

    // Use this for initialization
    void Start()
    {
        model = GetComponent<Model>();
    }

    // Update is called once per frame
    void Update() {

    }

    void OnGUI()
    {
        if (model.state == State.GAME)
        {
            Matrix4x4 mat = GUI.matrix;
            GUI.matrix = model.guiMatrix;

            if (GUI.Button(model.rRectPause, "", skinPause.button))
            {
                model.state = State.PAUSE;
                model.PlaySound(Sound.MENU);
            }

            GUI.matrix = mat;
        }
    }
}
