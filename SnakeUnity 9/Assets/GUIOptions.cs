using UnityEngine;
using System.Collections;

public class GUIOptions : MonoBehaviour {
    public GUISkin skinOptions = null;
    public GUISkin skinMiniSounds = null;
    public GUISkin skinMusic = null;
    public GUISkin skinVibra = null;
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

            bool optionsNew = GUI.Toggle(model.rectOptions, model.options, "", skinOptions.toggle);

            if (model.options != optionsNew)
            {
                model.options = optionsNew;
                model.PlaySound(Sound.MENU);
            }

            if (!model.options)
            {
                bool bMiniSoundsNew = GUI.Toggle(model.rRectMiniSounds, model.bMiniSounds, "", skinMiniSounds.toggle);

                if (model.bMiniSounds != bMiniSoundsNew)
                {
                    model.bMiniSounds = bMiniSoundsNew;
                    model.PlaySound(Sound.MENU);
                    model.Save();
                }

                bool bMusicNew = GUI.Toggle(model.rRectMusic, model.bMusic, "", skinMusic.toggle);

                if (model.bMusic != bMusicNew)
                {
                    model.bMusic = bMusicNew;
                    model.PlaySound(Sound.MENU);

                    model.MusicPlayPause();
                    model.Save();
                }

                bool bVibraNew = GUI.Toggle(model.rRectVibra, model.bVibra, "", skinVibra.toggle);

                if (model.bVibra != bVibraNew)
                {
                    model.bVibra = bVibraNew;
                    model.PlaySound(Sound.MENU);
                    model.Vibrate();
                    model.Save();
                }
            }

            GUI.matrix = mat;
        }
    }
}
