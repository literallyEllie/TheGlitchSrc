using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Class to do the introduction display.
/// </summary>
public class StoryTeller : MonoBehaviour
{

    public Sprite[] images;
    public Image storyView;
    private int lastImage;

    private float startFade;

    private bool fadingIn, fadingOut, skip;

    public float fadeInOutDuration = 3;
    public Text skipText;

    /// <summary>
    /// Prepares te first image.
    /// </summary>
    void Start()
    {
        storyView.color = new Color(255, 255, 255, 0);
        storyView.sprite = images[0];
        lastImage++;
       
        startFade = Time.time;
        fadingIn = true;
    }

    /// <summary>
    /// Handles the fading in and out of images.
    /// </summary>
    void Update()
    {

        // Skip.
        if (!skip && Input.GetKeyDown(KeyCode.Return))
        {
            skip = true;
            fadingIn = false;
            fadingOut = true;
        }

        if (fadingIn)
        {
            storyView.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(0, 1, (Time.time - startFade) / fadeInOutDuration));

            if (storyView.color.a == 1)
            {
                fadingIn = false;
                StartCoroutine(PostImageTransitionIn());
            }
        }

        else if (fadingOut)
        {
            storyView.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1, 0, (Time.time - startFade) / fadeInOutDuration));
            if (skip) skipText.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1, 0, (Time.time - startFade) / fadeInOutDuration));

            if (storyView.color.a == 0)
            {

                if (skip)
                {
                    SceneManager.LoadScene("StartLevel");
                    return;
                }

                fadingOut = false;
                StartCoroutine(PostImageTransitionOut());
            }
        }

    }

    /// <summary>
    /// Shows the next photo, if all of them are shown, start the game.
    /// </summary>
    void ShowNextPhoto()
    {
        if (lastImage == images.Length)
        {
            SceneManager.LoadScene("StartLevel");
            return;
        }

        storyView.sprite = images[lastImage++];

        startFade = Time.time;
        fadingIn = true;
    }

    /// <summary>
    /// Pause whilst the image is showing.
    /// </summary>
    IEnumerator PostImageTransitionIn()
    {
        yield return new WaitForSeconds(1);
        startFade = Time.time;
        fadingOut = true;
        yield return null;
    }

    /// <summary>
    /// Show the next photo.
    /// </summary>
    IEnumerator PostImageTransitionOut()
    {
        yield return new WaitForSeconds(1);
        ShowNextPhoto();
        yield return null;
    }


}
