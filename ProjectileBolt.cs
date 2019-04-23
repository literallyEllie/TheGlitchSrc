using UnityEngine;

/// <summary>
/// Handling the movement of the gun bullets.
/// </summary>
public class ProjectileBolt : MonoBehaviour
{

    public int bulletSpeed = 10;
    public float maxTravelDistance = 12.5f;

    private float startDistanceX;

    /// <summary>
    /// Init.
    /// </summary>
    void Start()
    {
        startDistanceX = transform.position.x;
    }

    /// <summary>
    /// Moves the bullet and destroys if it goes too far.
    /// </summary>
    void Update()
    {
        transform.position += transform.right * bulletSpeed * Time.deltaTime;

        if (transform.position.x - startDistanceX >= maxTravelDistance)
        {
            Object.Destroy(gameObject);
        }

    }

}
