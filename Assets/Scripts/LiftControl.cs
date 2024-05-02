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

    private void Start()
    {
        PlayerCharacter = GameObject.FindWithTag("Player").GetComponent<charactercontroller>();
        subFloorPasscode = "274";
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
    }

    private void Update()
    {
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
        }
    }

    private void closeWindow()
    {
        PlayerCharacter.ActiveState = charactercontroller.PlayerState.Active;
        PasscodePanel.SetActive(false);
        floorButtonPanel.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
