using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   
  public void NewGame()
    {
        SceneManager.LoadScene("Opening");

    }


    public void Load()
    {
        SceneManager.LoadScene("");

    }

    public void Quit()
    {
        Application.Quit();
    }
}
