using UnityEngine;
using System.Collections;

public class GUILike : MonoBehaviour {
    public GUISkin skinLike = null;
    private Model model = null;

    // Use this for initialization
    void Start () {
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

            if (GUI.Button(model.rRectLike, "", skinLike.button))
            {
                model.PlaySound(Sound.MENU);
                Application.OpenURL("market://details?id=com.PinkGlasses.SnakeForever");
            }

            GUI.matrix = mat;
        }
    }
}
