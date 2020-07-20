using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
   public void LoadActiveScene()
    {
        int getActiveIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(getActiveIndex);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void LoadNextLevel()
    {

        StartCoroutine("LoadLevel");
    }
    public void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }
    IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(2f);
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }
}
