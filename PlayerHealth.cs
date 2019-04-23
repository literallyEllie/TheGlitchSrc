using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handles the health of the player.
/// </summary>
public class PlayerHealth : MonoBehaviour {

    private int lives = DefaultValues.PLAYER_LIVES;
    private int health = DefaultValues.PLAYER_HEALTH;
        
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            RenderHealthBar();
        }
    }
    public int Lives
    {
        get
        {
            return lives;
        }
        set
        {
            lives = value;
            RenderLives();
        }
    }

    private PlayerController playerController;

    // Graphics
    public RectTransform healthbarOverlay;
    public Slider healthbarRender;
    public Text healthbarLives;
    public Text healthDeductionText;

    public Sprite deadHeathbar;

    private float deductionTextSpawnTime;
    private bool fading;

    /// <summary>
    /// Renders the lives and health bar.
    /// </summary>
    void Start()
    {
        this.playerController = GetComponent<PlayerController>();
        RenderHealthBar();
        RenderLives();
    }

    /// <summary>
    /// Method to detuct health from a player.
    /// </summary>
    public void DetuctHealth(int damage)
    {
        int newHealth = health - damage;
        if (newHealth <= 0)
        {
            // respawn.
                
            if (lives - 1 <= 0)
            {
                // kill
                lives = 0;
                RenderLives();
                healthbarRender.value = 0;

                // Change heart to dead heart.

                healthbarOverlay.GetComponent<Image>().sprite = deadHeathbar;
                playerController.PlayerDeath();

                StartCoroutine(DeathTransition());
                return;
            }

            Lives -= 1;
            Health = DefaultValues.MAX_PLAYER_HEALTH;
            playerController.collectedCodes = 0;
            playerController.playerGameSession.UpdateSession(playerController, "Level1");
            SceneManager.LoadScene(playerController.playerGameSession.respawnLevel);

        } else
        {
            Health = newHealth;

            if (fading)
            {
                ResetHealthReductionText();
            }
            else if (!String.IsNullOrEmpty(healthDeductionText.text))
            {
                int alreadyMinusHealth;
                if (!Int32.TryParse(healthDeductionText.text, out alreadyMinusHealth))
                {
                    Debug.LogError("Failed to parse deducated health amount from: " + healthDeductionText.text);
                    return;
                }
                alreadyMinusHealth = Mathf.Abs(alreadyMinusHealth);

                damage += alreadyMinusHealth;
            }
            healthDeductionText.text = "-" + damage;
            deductionTextSpawnTime = Time.time;
        }
    }

    /// <summary>
    /// Handles the health reduction text.
    /// </summary>
    void Update()
    {
  
        if (fading && healthDeductionText.canvasRenderer.GetAlpha() == 0f)
        {
            ResetHealthReductionText();
        }

        if (deductionTextSpawnTime == 0f) return;

        if (Time.time - deductionTextSpawnTime >= 2f)
        {
            fading = true;
            healthDeductionText.CrossFadeAlpha(0, 1.5f, false);
            deductionTextSpawnTime = 0f;
        }
    }

    /// <summary>
    /// Displays the health bar.
    /// </summary>
    void RenderHealthBar()
    {
        healthbarRender.value = (float) health / (float) DefaultValues.MAX_PLAYER_HEALTH;
    }

    /// <summary>
    /// Displays how many lives the player has left.
    /// </summary>
    void RenderLives()
    {
        healthbarLives.text = "X" + lives;
    }

    /// <summary>
    /// Resets how much health the player has lost very recently.
    /// </summary>
    private void ResetHealthReductionText()
    {
        fading = false;
        healthDeductionText.text = "";
        healthDeductionText.CrossFadeAlpha(1, 0, false);
    }

    /// <summary>
    /// The series of events when a player dies.
    /// </summary>
    IEnumerator DeathTransition()
    {
        playerController.playerGameSession.Reset();
        yield return new WaitForSeconds(1);

        switch (SceneManager.GetActiveScene().name)
        {
            // Level 1
            case "Level1":
                SceneManager.LoadScene("Level1_Death");
                break;
            case "Level2":
                SceneManager.LoadScene("Level2_Death");
                break;       
        }

    }

}
