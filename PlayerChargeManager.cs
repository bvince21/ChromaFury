using UnityEngine;

public class PlayerChargeManager : MonoBehaviour
{
    public int redCharges;
    public int blueCharges;
    public int yellowCharges;

    public void AddCharge(ColorNode.NodeColor color)
    {
        switch (color)
        {
            case ColorNode.NodeColor.Red:
                redCharges++;
                break;
            case ColorNode.NodeColor.Blue:
                blueCharges++;
                break;
            case ColorNode.NodeColor.Yellow:
                yellowCharges++;
                break;
        }

        Debug.Log($"Picked up {color} charge!");
    }
}

