using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Outline ItemGlow;
    public enum PickupType
    {
        Ammo,
        Box,
        Other
    }
    public PickupType InteractableType;
    public int AmmoBoxSize;

    private void Start()
    {
        ItemGlow = GetComponent<Outline>();
    }

    public void EnableGlow()
    {
        ItemGlow.enabled = true;
    }

    public void DisableGlow()
    {
        ItemGlow.enabled = false;
    }

    public void InteractedWith(Gun playergun)
    {
        if(InteractableType == PickupType.Ammo)
        {
            playergun.IncreaseReserves(AmmoBoxSize);
            Destroy(this.gameObject);
            playergun.UpdateReserveCount();
        }
    }
}
