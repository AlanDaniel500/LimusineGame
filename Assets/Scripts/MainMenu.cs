using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play(string escena)
    {
        SceneManager.LoadScene(escena);
    }

    public void Leave()
    {
        Application.Quit();
    }

    public void Return()
    {
        SceneManager.LoadScene(0);
    }
}
