using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LiftControl : MonoBehaviour
{
    public Elevator thisElevator;
    public charactercontroller PlayerCharacter;

    public GameObject floorButtonPanel;
    public GameObject PasscodePanel;

    public TMP_InputField PasscodeEntryField;

    public string subFloorPasscode;

    public GameObject floor1button;
    public GameObject floor0button;
    public GameObject roofButton;

    private void Start()
    {
        PlayerCharacter = GameObject.FindWithTag("Player").GetComponent<charactercontroller>();
        subFloorPasscode = "Kingston";
    }

    public void buttonPressAttemptMove(int Floor)
    {
        if (Floor == -1)
        {
            floorButtonPanel.SetActive(false);
            PasscodePanel.SetActive(true);
        }
        else if(thisElevator.verifyPlayerAuth(Floor))
        {
            thisElevator.targetFloor = Floor;
            closeWindow();
        }
        else
        {

        }
        
        
        if(Floor == 3)
        {
            roofButtonPressed();
        }
    }

    private void Update()
    {
        if(PlayerCharacter.storyResearchGathered)
        {
            roofButton.SetActive(true);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            closeWindow();
        }

        if(PasscodeEntryField.text.Length == 3)
        {
            if (string.Equals(PasscodeEntryField.text.ToString(), subFloorPasscode))
            {
                thisElevator.targetFloor = -1;
                closeWindow();
            }
            else
            {
                PasscodeEntryField.text = "";
                closeWindow();
            }
        }

        if (thisElevator.verifyPlayerAuth(1))
        {
            floor1button.SetActive(true);
        }
        if (thisElevator.verifyPlayerAuth(0))
        {
            floor0button.SetActive(true);
        }
    }

    private void closeWindow()
    {
        PlayerCharacter.ActiveState = charactercontroller.PlayerState.Active;
        PasscodePanel.SetActive(false);
        floorButtonPanel.SetActive(true);
        this.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void roofButtonPressed()
    {
        PlayerCharacter.badEnding();
    }
}
