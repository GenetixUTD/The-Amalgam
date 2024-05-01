using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    public Outline ItemGlow;
    public enum PickupType
    {
        Ammo,
        Box,
        Other,
        Log
    }
    public PickupType InteractableType;
    public int AmmoBoxSize;
    public int AudioLogIndex;

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

    public override void InteractedWith(charactercontroller PlayerCharacter)
    {
        base.InteractedWith(PlayerCharacter);
        if(InteractableType == PickupType.Ammo)
        {
            Gun playergun = PlayerCharacter.PlayerGun;
            playergun.IncreaseReserves(AmmoBoxSize);
            Destroy(this.gameObject);
            playergun.UpdateReserveCount();
        }
        else if(InteractableType == PickupType.Log)
        {
            PlayerCharacter.unlockLog(AudioLogIndex);
        }
    }
}
