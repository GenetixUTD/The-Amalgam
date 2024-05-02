using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static charactercontroller;

public class ItemPickup : Interactable
{
    public Outline ItemGlow;
    public enum PickupType
    {
        Ammo,
        Box,
        Other,
        Log,
        Keycard,
        Monitor,
        Research,
        Jacob
    }
    public PickupType InteractableType;
    public int AmmoBoxSize;
    public int AudioLogIndex;
    public int keycardFloor;
    public GameObject monitor;

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
            Destroy(this.gameObject);
        }
        else if(InteractableType == PickupType.Keycard)
        {
            PlayerCharacter.unlockFloor(keycardFloor);
            Destroy(this.gameObject);
        }
        else if(InteractableType == PickupType.Monitor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            PlayerCharacter.ActiveState = PlayerState.Paused;
            monitor.SetActive(true);
        }
        else if(InteractableType == PickupType.Research)
        {
            PlayerCharacter.storyResearchGathered = true;
            this.gameObject.SetActive(false);
        }
        else if(InteractableType == PickupType.Jacob)
        {
            PlayerCharacter.goodEnding();
        }
    }
}
