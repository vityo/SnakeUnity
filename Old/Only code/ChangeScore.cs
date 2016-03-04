using UnityEngine;
using System.Collections;

public class ChangeScore : MonoBehaviour {

	// Use this for initialization
	void Start () {

		if(Model.snake != null)
			guiText.text = Model.snake.mnScore.ToString();
	}
	
	// Update is called once per frame
	void Update () {

	}
}
