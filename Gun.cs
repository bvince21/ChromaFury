using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("References")]
    public Camera fpsCam;                   // Main Camera
    public Transform muzzleFlashPoint;      // Where the flash spawns
    public GameObject muzzleFlashPrefab;    // Muzzle flash polygon prefab

    [Header("Gun Settings")]
    public float damage = 20f;
    public float range = 100f;
    public float fireRate = 10f;            // Bullets per second

    [Header("Audio")]
    public AudioSource gunAudioSource;      // AudioSource component
    public AudioClip shootClip;             // Sound for shooting
    public AudioClip reloadClip;            // Sound for reloading

    [Header("Aiming Settings")]
    public Vector3 hipPosition;             // Default local position
    public Vector3 aimPosition;             // ADS local position
    public float aimSpeed = 8f;

    [Header("Ammo Settings")]
    public int magazineSize = 30;
    public int ammoInMag;
    public float reloadTime = 2f;

    [Header("Recoil Settings")]
    public Vector2 verticalRecoilRange = new Vector2(1f, 3f);   // vertical recoil per shot
    public Vector2 horizontalRecoilRange = new Vector2(-1f, 1f); // horizontal recoil per shot
    public float recoilRecoverySpeed = 8f;                       // how fast it recovers
    public float verticalClamp = 10f;
    public float horizontalClamp = 5f;

    private Vector3 recoilRotation;                               // current recoil offset
    private Quaternion originalRotation;                          // gun's starting rotation

    private bool isReloading = false;
    private bool isAiming = false;
    private float nextTimeToFire = 0f;

    void Start()
    {
        hipPosition = transform.localPosition; // Record starting position
        ammoInMag = magazineSize;              // Start full
        originalRotation = transform.localRotation; // Record starting rotation
    }

    void Update()
    {
        if (isReloading) return;

        // Auto reload when empty
        if (ammoInMag <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        // Shooting (with fire rate control)
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }

        // Manual reload
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }

        // Aiming
        isAiming = Input.GetButton("Fire2");
        Vector3 targetPos = isAiming ? aimPosition : hipPosition;
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * aimSpeed);

        // Smoothly recover gun rotation after recoil
        recoilRotation = Vector3.Lerp(recoilRotation, Vector3.zero, Time.deltaTime * recoilRecoverySpeed);
        transform.localRotation = originalRotation * Quaternion.Euler(recoilRotation);
    }

    void Shoot()
    {
        ammoInMag--;

        // Apply recoil
        float verticalRecoil = Random.Range(verticalRecoilRange.x, verticalRecoilRange.y);
        float horizontalRecoil = Random.Range(horizontalRecoilRange.x, horizontalRecoilRange.y);

        recoilRotation.x = Mathf.Clamp(recoilRotation.x - verticalRecoil, -verticalClamp, verticalClamp);
        recoilRotation.y = Mathf.Clamp(recoilRotation.y + horizontalRecoil, -horizontalClamp, horizontalClamp);

        if (gunAudioSource && shootClip)
        {
            gunAudioSource.PlayOneShot(shootClip);
        }

        // Raycast hit detection
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log("Hit: " + hit.transform.name);
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
                target.TakeDamage(damage);
        }

        // Muzzle flash
        if (muzzleFlashPrefab && muzzleFlashPoint)
        {
            GameObject flash = Instantiate(muzzleFlashPrefab, muzzleFlashPoint.position, muzzleFlashPoint.rotation, muzzleFlashPoint);
            Destroy(flash, 0.05f);
        }
    }

    System.Collections.IEnumerator Reload()
    {
        isReloading = true;
        isAiming = false; // Cancel ADS during reload
        transform.localPosition = hipPosition; // Reset gun position
        recoilRotation = Vector3.zero; // Reset recoil
        Debug.Log("Reloading...");

        if (gunAudioSource && reloadClip)
        {
            gunAudioSource.PlayOneShot(reloadClip);
        }

        yield return new WaitForSeconds(reloadTime);

        ammoInMag = magazineSize;
        isReloading = false;
        Debug.Log("Reloaded!");


    }
}
