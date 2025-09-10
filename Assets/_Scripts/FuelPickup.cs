using UnityEngine;

public class FuelPickup : MonoBehaviour
{
    [SerializeField] private float fuelAmount;
    public float GetFuelAmount() => fuelAmount;
    public void DestroySelf() => Destroy(gameObject);
}
