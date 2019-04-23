using UnityEngine;

/// <summary>
/// BasicMovementPathing moves an object from a to b in a range.
/// It can also flip the sprite if necessary.
/// </summary>
public class BasicMovementPathing : MonoBehaviour
{

    public float speed, range;
    public bool flipOnRotation;
    public bool useY;
    private Vector3 spawnLocation;
    private Vector3? lastLocation;

    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// Init.
    /// </summary>
    void Awake()
    {
        spawnLocation = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// The main method that moves the spriteRenderer.
    /// </summary>
    void FixedUpdate()
    {

        if (useY)
        {
            transform.position = new Vector2(transform.position.x, spawnLocation.y + range * Mathf.Sin(Time.time * speed));
        } else
        {
            transform.position = new Vector2(spawnLocation.x + range * Mathf.Sin(Time.time * speed), transform.position.y);
        }

        if (flipOnRotation)
        {
            // If no last location, set.
            if (!lastLocation.HasValue)
            {
                lastLocation = transform.position;
                return;
            }

            // Whether the sprite is flipped is based on whether their last location x is greater than their current x.
            spriteRenderer.flipX = lastLocation.Value.x > transform.position.x;
            lastLocation = transform.position;
        }

    }

}