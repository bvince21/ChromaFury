using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 50f;

    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log(gameObject.name + " took " + amount + " damage. Health: " + health);

        if (health <= 0f)
        {
            Debug.Log(gameObject.name + " destroyed!");
            Destroy(gameObject);
        }
    }
}
