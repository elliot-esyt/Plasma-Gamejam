using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{

    // title buttons 
    public void OnPlayClicked()
    {
        SceneManager.LoadScene("GameScene");
 
    }

    public void OnQuitClicked() { Application.Quit(); }

}