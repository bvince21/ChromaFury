using UnityEngine;

public class ColorNode : MonoBehaviour
{
    public enum NodeColor { Red, Blue, Yellow }
    public NodeColor nodeColor;

    [Header("Settings")]
    public float respawnTime = 5f;

    [Header("Visuals")]
    public GameObject visualObject; // Mesh or child object to show/hide
    public Light glowLight; // Optional glow
    public Color redColor = Color.red;
    public Color blueColor = Color.blue;
    public Color yellowColor = Color.yellow;

    private bool isAvailable = true;

    void Start()
    {
        ApplyColor();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isAvailable) return;

        if (other.CompareTag("Player"))
        {
            GivePlayerCharge(other.gameObject);
            StartCoroutine(RespawnTimer());
        }
    }

    void GivePlayerCharge(GameObject player)
    {
        // Example: player has a PlayerChargeManager script
        PlayerChargeManager pcm = player.GetComponent<PlayerChargeManager>();
        if (pcm != null)
        {
            pcm.AddCharge(nodeColor);
        }
    }

    System.Collections.IEnumerator RespawnTimer()
    {
        isAvailable = false;
        if (visualObject != null) visualObject.SetActive(false);
        if (glowLight != null) glowLight.enabled = false;

        yield return new WaitForSeconds(respawnTime);

        if (visualObject != null) visualObject.SetActive(true);
        if (glowLight != null) glowLight.enabled = true;
        isAvailable = true;
    }

    void ApplyColor()
    {
        Color chosenColor = Color.white;

        switch (nodeColor)
        {
            case NodeColor.Red:
                chosenColor = redColor;
                break;
            case NodeColor.Blue:
                chosenColor = blueColor;
                break;
            case NodeColor.Yellow:
                chosenColor = yellowColor;
                break;
        }

        if (visualObject != null)
        {
            Renderer r = visualObject.GetComponent<Renderer>();
            if (r != null)
            {
                r.material.color = chosenColor;
                r.material.SetColor("_EmissionColor", chosenColor * 2f);
            }
        }

        if (glowLight != null)
        {
            glowLight.color = chosenColor;
        }
    }
}
