using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public int stage;
    // Start is called before the first frame update
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }

    public void loadGame()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        stage = data.stage;

        SceneManager.LoadScene(stage);
    }
}
