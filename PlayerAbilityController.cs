using UnityEngine;

public class PlayerAbilityController : MonoBehaviour
{
    [Header("Ability Settings")]
    public GameObject barrierPrefab;       // Prefab for the moving barrier
    public Transform spawnPoint;           // Where the barrier spawns (in front of the player)
    public float barrierCooldown = 1f;    // Cooldown between uses change 2 10!!!!
    public float barrierSpeed = 3f;        // Speed the barrier moves forward

    private float lastAbilityTime = -999f;

    void Update()
    {
        // Left Mouse Button or Q to activate (customizable)
        if (Input.GetKeyDown(KeyCode.E) && Time.time >= lastAbilityTime + barrierCooldown)
        {
            ActivateBarrier();
            lastAbilityTime = Time.time;
        }
    }

    void ActivateBarrier()
    {
        // Spawn the barrier
        Quaternion spawnRotation = spawnPoint.rotation * Quaternion.Euler(90, 0, 90);
        GameObject barrier = Instantiate(barrierPrefab, spawnPoint.position, spawnPoint.rotation);

        // Add movement script to barrier
        BarrierMover mover = barrier.AddComponent<BarrierMover>();
        mover.moveSpeed = barrierSpeed;

        // Optional: Add damage, color abilities, etc. later
    }
}

