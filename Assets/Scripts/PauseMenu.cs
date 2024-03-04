using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public charactercontroller PlayerCharacter;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PlayerCharacter.ActiveState = charactercontroller.PlayerState.Active;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            this.gameObject.SetActive(false);
        }
    }
}
