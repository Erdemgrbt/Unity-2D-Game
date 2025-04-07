using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverMenu : MonoBehaviour
{
    public void Home()
    {
        SceneManager.LoadScene("Main Menu");    
    }

    public void Restart()
    {
        SceneManager.LoadScene("GameplayScene");
    }
}
