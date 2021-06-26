using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour
{
    public void Again()
    {
        SceneManager.LoadScene(0);
    }

    public void EndScene()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit(0);
    }
}
