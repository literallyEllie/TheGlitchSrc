using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuHandle : MonoBehaviour
{

    public SoundSlider soundSlider;

    public void ClickStart()
    {
        UpdateSound();
        SceneManager.LoadScene("StoryBoard");
    }

    public void ClickInfo()
    {
        UpdateSound();
        SceneManager.LoadScene("MenuInfo");
    }

    public void ClickExit()
    {
        UpdateSound();
        Application.Quit();
    }

    public void ClickBack()
    {
        UpdateSound();
        SceneManager.LoadScene("MainMenu");
    }

    private void UpdateSound()
    {
        if (soundSlider == null) return;
        soundSlider.MenuExit();
    }

}
