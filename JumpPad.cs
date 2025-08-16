using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [Header("Launch Settings")]
    public float launchPower = 15f; // Higher than normal jumpPower for a boost

    [Header("Player Detection")]
    public string playerTag = "Player"; // Make sure your player is tagged as "Player"

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            FPSController playerController = other.GetComponent<FPSController>();
            if (playerController != null)
            {
                // Boost the player's upward velocity
                var moveDirField = playerController.GetType().GetField("moveDirection",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                if (moveDirField != null)
                {
                    Vector3 moveDir = (Vector3)moveDirField.GetValue(playerController);
                    moveDir.y = launchPower;
                    moveDirField.SetValue(playerController, moveDir);
                }
            }
        }
    }
}
