using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    string MENU_SCENE_NAME = "Menu Scene";
    string GAME_SCENE_NAME = "Game";

    public void LoadMenu()
    {
        SceneManager.LoadScene(MENU_SCENE_NAME);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(GAME_SCENE_NAME);
    }
}
