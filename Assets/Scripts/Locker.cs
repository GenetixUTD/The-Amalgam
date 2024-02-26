using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locker : Interactable
{
    public Transform playerhideposition;

    public Outline LockerActual;
    public Outline LockerDoor;

    public GameObject RegisterArea;
    public Transform ExitArea;

    private GameObject Player;

    private bool HidingInside;

    private void Start()
    {
        HidingInside = false;
    }

    private void Update()
    {
        if(HidingInside && Input.GetKeyDown(KeyCode.E))
        {
            Player.transform.position = ExitArea.position;
            Player.GetComponent<charactercontroller>().ActiveState = charactercontroller.PlayerState.Active;
        }
    }

    public void EnableGlow()
    {
        LockerActual.enabled = true;
        LockerDoor.enabled = true;
    }

    public void DisableGlow()
    {
        LockerActual.enabled = false;
        LockerDoor.enabled = false;
    }

    public override void InteractedWith(charactercontroller PlayerCharacter)
    {
        base.InteractedWith(PlayerCharacter);
        HidingInside = true;
        PlayerCharacter.ActiveState = charactercontroller.PlayerState.Hiding;
        PlayerCharacter.gameObject.transform.SetPositionAndRotation(playerhideposition.position, playerhideposition.rotation);
        Player = PlayerCharacter.gameObject;
    }
}
