using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class to handle the sound slider and can also set the actual sound.
/// </summary>
public class SoundSlider : MonoBehaviour
{

    public GameSettings settings;

    private int initialLevel;

    public Slider soundSlider;
    public Text soundDisplay;
    public AudioSource audioSource;

    /// <summary>
    /// Sets the default value to 50 or the saved data.
    /// </summary>
    void Start()
    {
        soundSlider.value = settings.validData ? settings.soundLevel : 50;

        initialLevel = (int)soundSlider.value;
    }

    /// <summary>
    /// Event when the sound bar is changed.
    /// </summary>
    public void SoundSlideChange()
    {
        settings.soundLevel = (int) soundSlider.value;
        soundDisplay.text = settings.soundLevel.ToString();

        if (audioSource != null) { 
            audioSource.volume = soundSlider.value / 100f;
        }
    }

    /// <summary>
    /// When a menu is exited, it will be saved if it has changed.
    /// </summary>
    public void MenuExit()
    {
        if (settings == null || initialLevel == settings.soundLevel) return;
        settings.Save();
    }
    
}
