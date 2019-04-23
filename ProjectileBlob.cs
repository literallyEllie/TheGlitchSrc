using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Phyics for the blob projectile.
/// </summary>
public class ProjectileBlob : MonoBehaviour
{

    public Vector2 targetLocation;

    private float animation;

    public Vector2 startLocation;
    public bool dealSuperDamage;

    /// <summary>
    /// Init.
    /// </summary>
    void Start()
    {
        startLocation = transform.position;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    /// <summary>
    /// Moves the projectile in a curved motion.
    /// </summary>
    void Update()
    {
        animation += Time.deltaTime;
        transform.position = Parabola(startLocation, targetLocation, 2f, animation);
    }
    
    /// <summary>
    /// Collision handling.
    /// </summary>
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// A method to get the next place which the projectile to still be in the constraint of a parabola.
    /// </summary>
    public Vector2 Parabola(Vector2 start, Vector2 end, float height, float t)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x;
        var mid = Vector2.Lerp(start, end, t);
        return new Vector2(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t));
    }

}
