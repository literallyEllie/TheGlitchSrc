using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the Level one.
/// </summary>
public class LevelOne : MonoBehaviour
{

    public GameObject player;
    private PlayerController playerController;

    private bool transition;

    /// <summary>
    /// Init.
    /// </summary>
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        playerController.allowJump = false;
        playerController.playerGameSession.Reset();
    }

    /// <summary>
    /// Checks if they fell down the hole and starts the transition to the next level.
    /// </summary>
    void FixedUpdate()
    {
        if (transition) return;

        if (playerController.gameObject.transform.position.y <= -5)
        {
            transition = true;
            playerController.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

            TransitionFinished();
        }
    }

    /// <summary>
    /// Called when the next level is ready to be loaded.
    /// </summary>
    void TransitionFinished()
    {
        playerController.playerGameSession.UpdateSession(playerController, "Level1");
        SceneManager.LoadScene("Level1");
    }

}
