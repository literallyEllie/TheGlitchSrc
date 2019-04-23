using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Respawn countdown.
/// </summary>
public class RespawnCountdown : MonoBehaviour
{

    public int timeLeft = 5;
    private Text countdown;

    /// <summary>
    /// Init.
    /// </summary>
    void Start()
    {
        this.countdown = GetComponent<Text>();

        StartCoroutine(Countdown());
        Time.timeScale = 1;
    }

    /// <summary>
    /// Countdown to respawn the player.
    /// </summary>
    IEnumerator Countdown()
    {
        while (true)
        {
            if (timeLeft <= 0)
            {
                SceneManager.LoadScene("Level1");
                yield break;
            }

            yield return new WaitForSeconds(1);
            timeLeft--;

            countdown.text = "RESPAWNING IN... " + timeLeft;
        }
    }

}