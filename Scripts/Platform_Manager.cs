using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Platform_Manager : MonoBehaviour {

    void Awake() {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WebGLPlayer)
        {
            SceneManager.LoadScene("Lobby_Mobile", LoadSceneMode.Single);
            return;
        }
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer ||
            Application.platform == RuntimePlatform.LinuxPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
            return;
        }

    }
}
