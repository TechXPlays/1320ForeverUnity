using UnityEngine;
using System.Collections;

#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2 
#else
using UnityEngine.SceneManagement;
#endif

public class RS_LevelLoader  {
	

	public static void LoadLevel(RS_Scene scene) {
		LoadLevel(scene.ToString());
	}




	private static void LoadLevel(string levelName) {
		#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
		Application.LoadLevel(levelName);
		#else
		SceneManager.LoadScene(levelName);
		#endif
	}


}
