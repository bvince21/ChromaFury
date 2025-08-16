using UnityEngine;

public class BarrierMover : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float lifeTime = 7.5f;
    

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

    }
}

