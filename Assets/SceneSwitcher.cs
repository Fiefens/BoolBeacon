using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public TMP_InputField inputField;
    public static TMP_InputField input;

    private void Awake()
    {
        input = inputField;
    }

    public static void SwitchGameplayScene()
    {
        GameManager.CurrentStrikes = 0;
        SceneManager.LoadScene("Gameplay");
    }
    public static void SwitchTutorialScene()
    {
        GameManager.Name = input.text;
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
