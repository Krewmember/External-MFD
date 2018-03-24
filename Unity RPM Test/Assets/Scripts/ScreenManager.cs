using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenManager : MonoBehaviour {

	//Loads scene by scene number
	public void OpenScene(int sceneNumber){
		SceneManager.LoadScene (sceneNumber);
	}

}
