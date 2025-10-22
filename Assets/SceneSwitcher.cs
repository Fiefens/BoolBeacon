using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public static void SwitchGameplayScene()
    {
        GameManager.CurrentStrikes = 0;
        SceneManager.LoadScene("Gameplay");
    }
    public static void SwitchTutorialScene()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public static void SwitchTitleScene()
    {
        SceneManager.LoadScene("Title");
    }
    public static void SwitchEndScene()
    {
        SceneManager.LoadScene("End");
    }
}
