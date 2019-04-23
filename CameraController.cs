using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Method to handle the movement of the camera.
/// </summary>
public class CameraController : MonoBehaviour
{

    public Vector2 velocity;
    public float smoothTimeX, smoothTimeY;
    public GameObject toFollow;
    public Vector3 minPos, maxPos;

    /// <summary>
    /// Method to mvoe the camera and follow toFollow smoothly.
    /// </summary>
    void FixedUpdate()
    {
        float posX = Mathf.SmoothDamp(transform.position.x, toFollow.transform.position.x, ref velocity.x, smoothTimeX);
        float posY = Mathf.SmoothDamp(transform.position.y, toFollow.transform.position.y, ref velocity.y, smoothTimeY);

        transform.position = new Vector3(posX, posY, transform.position.z);
        
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minPos.x, maxPos.x),
            Mathf.Clamp(transform.position.y, minPos.y, maxPos.y),
            Mathf.Clamp(transform.position.z, minPos.z, maxPos.z));
    }

}
