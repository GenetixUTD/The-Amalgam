using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorLock : MonoBehaviour
{
    public TMP_InputField PasscodeEntryField;

    public string subFloorPasscode;

    public charactercontroller PlayerController;

    public GameObject LDoor;
    public GameObject RDoor;

    private void Start()
    {
        PlayerController = GameObject.FindWithTag("Player").GetComponent<charactercontroller>();
        subFloorPasscode = "274";
    }
    private void Update()
    {
        if (PasscodeEntryField.text.Length == 3)
        {
            if (string.Equals(PasscodeEntryField.text.ToString(), subFloorPasscode))
            {
                openTheDoors();
                closeWindow();
            }
            else
            {
                PasscodeEntryField.text = "";
                closeWindow();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            closeWindow();
        }
    }

    public void openTheDoors()
    {
        LDoor.GetComponent<Animator>().SetBool("open", true);
        LDoor.GetComponent<Animator>().SetBool("close", false);
        RDoor.GetComponent<Animator>().SetBool("open", true);
        RDoor.GetComponent<Animator>().SetBool("close", false);
    }

    public void closeTheDoors()
    {
        LDoor.GetComponent<Animator>().SetBool("open", false);
        LDoor.GetComponent<Animator>().SetBool("close", true);
        RDoor.GetComponent<Animator>().SetBool("open", false);
        RDoor.GetComponent<Animator>().SetBool("close", true);
    }

    public void closeWindow()
    {
        PlayerController.ActiveState = charactercontroller.PlayerState.Active;
        PasscodeEntryField.text = "";
        this.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
