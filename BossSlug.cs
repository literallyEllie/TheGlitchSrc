using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The class handling the Slug boss found in level 2
/// </summary>
public class BossSlug : MonoBehaviour
{

    private bool bossActive = false, isSuper = false;
    private float lastAttack;

    public SpriteRenderer slugRender;
    public Sprite bossIdle, bossPrepAttack, bossAttack, bossDying;
    public Sprite superBossIdle, superBossAttack, bossDead;
    public Sprite bossProjectileLaunch, bossProjetileLaunched;
    public Transform projectileBlob;

    public Slider healthSlider;

    public PlayerController playerController;
    public int delayBetweenAttacks = 2;

    private float bossHp;
    private Vector2 targetPosition;

    /// <summary>
    /// Init.
    /// </summary>
    void Start()
    {
        bossHp = DefaultValues.BOSS_HEALTH;
        slugRender.sprite = bossIdle;
    }

    /// <summary>
    /// Animates the boss and sends attacks.
    /// </summary>
    void Update()
    {
        if (!bossActive) return;

        if (lastAttack != 0f && Time.time - lastAttack >= delayBetweenAttacks)
        {
            slugRender.sprite = isSuper ? superBossAttack : bossAttack;
            BossAttack();
        } else if (lastAttack == 0f || (Time.time - lastAttack < 1.5 && Time.time - lastAttack > 1))
        {
            slugRender.sprite = isSuper ? superBossIdle : bossPrepAttack;
        }

    }

    /// <summary>
    /// Handles the event when the player interacts with the boss.
    /// </summary>
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            if (!isSuper && !bossActive)
            {
                ActivateBoss();
            }

        }
    }

    /// <summary>
    /// Handles the event when a player sends a bullet.
    /// </summary>
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BoltProjectile"))
        {
            if (bossHp <= 0) return;
            DamageBoss(DefaultValues.BOLT_GUN_DAMAGE);
            Object.Destroy(collision.gameObject); 
        }
    }

    /// <summary>
    /// Method initilising the boss being activated.
    /// </summary>
    void ActivateBoss()
    {
        bossActive = true;
        lastAttack = Time.time;
        slugRender.sprite = bossPrepAttack;
    }

    /// <summary>
    /// Method handling the boss when they are killed.
    /// </summary>
    void KillBoss()
    {
        slugRender.sprite = bossDying;

        bossActive = false;

        if (!isSuper)
        {
            StartCoroutine(DeathAnimation());
        } else
        {
            slugRender.sprite = bossDead;

            GetComponent<BoxCollider2D>().enabled = false;
            transform.position = new Vector2(transform.position.x, transform.position.y - 1);
        }
    }

    /// <summary>
    /// Method handling the boss they respawn.
    /// </summary>
    void RespawnBoss()
    {
        bossActive = true;
        isSuper = true;
        lastAttack = Time.time;
        slugRender.sprite = superBossIdle;

        bossHp = DefaultValues.SUPER_BOSS_HEALTH;
        RenderHealthBar();
    }

    /// <summary>
    /// Method to handle the damaging of the boss.
    /// </summary>
    void DamageBoss(int damage)
    {
        if (!bossActive)
        {
            ActivateBoss();
            return;
        }

        bossHp -= damage;
        RenderHealthBar();

        if (bossHp <= 0)
        {
            KillBoss();
        }

    }

    /// <summary>
    /// Method to handle the shooting of the bullets.
    /// </summary>
    void BossAttack()
    {
        lastAttack = Time.time;
        targetPosition = playerController.gameObject.transform.position;

        Transform projectile = GameObject.Instantiate(projectileBlob);

        ProjectileBlob projectileProps = projectile.GetComponent<ProjectileBlob>();
        projectileProps.targetLocation = targetPosition;
        projectileProps.dealSuperDamage = isSuper;
    }

    /// <summary>
    /// Updates the healthbar of the boss.
    /// </summary>
    void RenderHealthBar()
    {
        healthSlider.value = ((float) bossHp / (isSuper ? (float) DefaultValues.SUPER_BOSS_HEALTH : (float) DefaultValues.BOSS_HEALTH));
    }

    IEnumerator DeathAnimation()
    {
        yield return new WaitForSeconds(2);

        RespawnBoss();
        yield return null;
    }


}
