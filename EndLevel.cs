using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Plays out the end level. 
/// </summary>
public class EndLevel : MonoBehaviour
{

    private SpriteRenderer castleRender;
    public PlayerController playerController;
    public SpriteRenderer playerRender;

    public Text endText;
    public Rigidbody2D motherGus, babyGus;
    public SpriteRenderer motherGusRender, babyGusRender;

    private float startFade;
    private bool fadingPlayer;
    private bool done;

    /// <summary>
    /// Init.
    /// </summary>
    void Start()
    {
        castleRender = GetComponent<SpriteRenderer>();    
    }

    /// <summary>
    /// Method handling the event when the player interacts with the castle.
    /// </summary>
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        fadingPlayer = true;
        startFade = Time.time;
        playerController.PlayerDeath();

        motherGus.AddForce(Vector2.up * 150);
        babyGus.AddForce(Vector2.up * 150);

    }

    /// <summary>
    /// Plays the animation and fade out of the little family.
    /// </summary>
    void Update()
    {
        if (done || !fadingPlayer) return;

        playerRender.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1, 0, (Time.time - startFade) / 3));
        motherGusRender.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1, 0, (Time.time - startFade) / 3));
        babyGusRender.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1, 0, (Time.time - startFade) / 3));

        if (playerRender.color.a == 0f)
        {
            fadingPlayer = false;
            done = true;
            StartCoroutine(EndScene());
        }

    }

    /// <summary>
    /// Loads out of the level.
    /// </summary>
    IEnumerator EndScene()
    {
        yield return new WaitForSeconds(2);
        endText.text = "THE END";
        yield return new WaitForSeconds(1);

        SceneManager.LoadScene("MainMenu");
        yield return null;
    }


}
