using UnityEngine;
using System.Collections;

public class quitOnClick : MonoBehaviour {
	public void quit(){
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}


}
