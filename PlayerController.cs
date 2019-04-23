using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Everything about the player is here.
/// </summary>
public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    public Sprite superGus;

    public Vector3 minPos, maxPos;

    public float speed = DefaultValues.PLAYER_SPEED;
    public PlayerHealth playerHealth;
    public PlayerGameSession playerGameSession;
    public GameSettings gameSettings;

    public bool allowJump = true;
    private bool isJumping, preventInteract = false;

    private float lastDamageTime;

    public int collectedCodes;
    public Text txtCollectedCodes;

    public Transform pauseMenu;

    public Transform gunBullet;
    public Text gunReloadingText;
    public float fireDelay = 2.5f;
    private bool hasUpgradedGun;
    private bool canShootGun = true;
    private float lastGunFire;

    public AudioSource gameSound;

    /// <summary>
    /// Init and reads from the PlayerGameSession and sets the personal sound level.
    /// </summary>
    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        spriteRenderer = GetComponent<SpriteRenderer>();

        this.playerGameSession = new PlayerGameSession();
        this.playerGameSession.Load();

        this.playerHealth = GetComponent<PlayerHealth>();

        this.gameSound = GetComponent<AudioSource>();

        if (playerGameSession.validData)
        {
            playerHealth.Lives = playerGameSession.lives;
            playerHealth.Health = playerGameSession.health;
            collectedCodes = playerGameSession.collectedCodes;

            RenderCollectedCodes();
        }

        if (gameSettings.validData)
        {
            gameSound.volume = (float) gameSettings.soundLevel / 100f;
        }

    }
	
    /// <summary>
    /// Manages the player input, jumping and gun shooting. 
    /// </summary>
	void FixedUpdate ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnPauseToggle();
            return;
        }

        if (preventInteract) return;
        /* Input management */

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            spriteRenderer.flipX = false;
            Vector3 targetPos = this.transform.position + Vector3.right * speed * Time.deltaTime;

            if (CanMoveTo(targetPos))
            {
                this.transform.Translate(targetPos - this.transform.position);
            }
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            spriteRenderer.flipX = true;
            Vector3 targetPos = this.transform.position + Vector3.left * speed * Time.deltaTime;

            if (CanMoveTo(targetPos))
            {
                this.transform.Translate(targetPos - this.transform.position);
            }
        }

        /* Jumping */
        if (allowJump && !isJumping && Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            this.rb.AddForce(Vector3.up * DefaultValues.PLAYER_JUMP_VELOCITY);
        }

        /* Gun shooting */
        if (hasUpgradedGun && Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (canShootGun)
            {
                lastGunFire = Time.time;
                GameObject.Instantiate(gunBullet, new Vector2(spriteRenderer.flipX ? transform.position.x + 0.2f : transform.position.x + 1, transform.position.y - 0.1f), new Quaternion(transform.rotation.x, spriteRenderer.flipX ? 180 : transform.rotation.y, transform.rotation.z, transform.rotation.w));
                gunReloadingText.text = "RELOADING GUN...";
                canShootGun = false;
            }
        }

    }

    /// <summary>
    /// Displays the gun reload status and sets when they can use it again.
    /// </summary>
    void Update()
    {
        if (hasUpgradedGun && !canShootGun)
        {
            if (lastGunFire != 0 && Time.time - lastGunFire >= fireDelay)
            {
                gunReloadingText.text = "";
                canShootGun = true;
            }
        }
        
    }

    /// <summary>
    /// Called when player dies.
    /// </summary>
    public void PlayerDeath()
    {
        preventInteract = true;
    }

    /// <summary>
    /// Called when the pause menu is turned on or off.
    /// </summary>
    public void OnPauseToggle()
    {
        preventInteract = !preventInteract;
        pauseMenu.gameObject.SetActive(preventInteract);
    }

    /// <summary>
    /// Called when the player decides to quit to the main menu.
    /// </summary>
    public void QuitToMainMenu()
    {
        gameSettings.Save();
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Called when the player exits the application.
    /// </summary>
    void OnApplicationQuit()
    {
        gameSettings.Save();
        playerGameSession.Reset();    
    }

    /// <summary>
    /// Registers when a player hits the ground from jumping and also take damage from the boss.
    /// </summary>
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (preventInteract) return;

        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }    

        if (collision.gameObject.CompareTag("SlugProjectile"))
        {
            playerHealth.DetuctHealth(collision.gameObject.GetComponent<ProjectileBlob>().dealSuperDamage ? DefaultValues.SUPER_BOSS_DAMAGE : DefaultValues.BOSS_DAMAGE);
        }

    }

    /// <summary>
    /// Handles being interacting and interacting with.
    /// </summary>
    void OnTriggerEnter2D(Collider2D collision)
    {
    
        if (collision.gameObject.CompareTag("SpannerPickup"))
        {

            BoxCollider2D collider = GetComponent<BoxCollider2D>();

            spriteRenderer.sprite = superGus;
            transform.localScale = new Vector3(1.284f, 1.284f, 1f);

            collider.offset = new Vector2(0, 0);
            collider.size = new Vector3(spriteRenderer.bounds.size.x / transform.lossyScale.x,
                                         spriteRenderer.bounds.size.y / transform.lossyScale.y,
                                         spriteRenderer.bounds.size.z / transform.lossyScale.z);

            Object.Destroy(collision.gameObject);
            hasUpgradedGun = true;   
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            playerHealth.DetuctHealth(DefaultValues.ENEMY_DAMAGE);
            lastDamageTime = Time.time;
        }

        if (collision.gameObject.CompareTag("Portal"))
        {
            playerGameSession.UpdateSession(this, null);
            SceneManager.LoadScene("Level2");
        }

        if (collision.gameObject.CompareTag("Level2Portal"))
        {
            playerGameSession.UpdateSession(this, null);
            SceneManager.LoadScene("EndLevel");
        }

        if (collision.gameObject.CompareTag("Collectable"))
        {
            collectedCodes += 1;
            Destroy(collision.gameObject);
            RenderCollectedCodes();
        }

    }

    /// <summary>
    /// Repetetive damage handling.
    /// </summary>
    void OnTriggerStay2D(Collider2D collision)
    {
        if (preventInteract) return;

        if (Time.time - lastDamageTime < 1)  {
            // Buffered damage, don't let take damage more than 1 time in a second.
            return;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            playerHealth.DetuctHealth(DefaultValues.ENEMY_DAMAGE);
            lastDamageTime = Time.time;
        }

        if (collision.gameObject.CompareTag("Acid"))
        {
            playerHealth.DetuctHealth(DefaultValues.ACID_DAMAGE);
            lastDamageTime = Time.time;
        }

    }

    /// <summary>
    /// Checks if the targetPosition is in the allowed bounds.
    /// </summary>
    bool CanMoveTo(Vector3 targetPosition)
    {
        return (targetPosition.x <= maxPos.x && targetPosition.x >= minPos.x);
    }

    /// <summary>
    /// Updates the collected codes.
    /// </summary>
    void RenderCollectedCodes()
    {
        txtCollectedCodes.text = "COLLECTED CODES: " + collectedCodes;
    }

}
