using UnityEngine;
using TMPro; // Use this if using TextMeshPro
// using UnityEngine.UI; // Uncomment if using regular UI Text

public class AmmoUI : MonoBehaviour
{
    public Gun gun;               // Reference to your gun script
    public TMP_Text ammoText;     // TextMeshPro text component
    // public Text ammoText;      // Use this if using regular UI Text

    void Update()
    {
        if (gun != null && ammoText != null)
        {
            ammoText.text = gun.ammoInMag + " / " + gun.magazineSize;
        }
    }
}
