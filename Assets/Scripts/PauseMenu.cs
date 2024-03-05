using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public charactercontroller PlayerCharacter;

    public GameObject SettingsTabGroup;
    public GameObject InventoryTabGroup;

    public GameObject SettingsButton;
    public GameObject InventoryButton;

    public TMP_Dropdown ResolutionDropdown;

    private Resolution[] Resolutions;

    private void Start()
    {
        Resolutions = Screen.resolutions;
        ResolutionDropdown.ClearOptions();

        List<string> optionsList = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < Resolutions.Length; i++)
        {
            string option = Resolutions[i].width + " x " + Resolutions[i].height;
            optionsList.Add(option);

            if (Resolutions[i].width == Screen.currentResolution.width && Resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        ResolutionDropdown.AddOptions(optionsList);
        ResolutionDropdown.value = currentResolutionIndex;
        ResolutionDropdown.RefreshShownValue();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Unpause();
        }
    }

    public void Unpause()
    {
        PlayerCharacter.ActiveState = charactercontroller.PlayerState.Active;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        this.gameObject.SetActive(false);
    }

    public void SetVolume(float volume)
    {
        //Wwise integration here
    }

    public void SetGraphicsQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void UpdateResolution(int resolutionIndex)
    {
        Resolution resolutionToBe = Resolutions[resolutionIndex];
        Screen.SetResolution(resolutionToBe.width, resolutionToBe.height, Screen.fullScreen);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SettingsTab()
    {
        SettingsTabGroup.SetActive(true);
        SettingsButton.SetActive(false);
        InventoryTabGroup.SetActive(false);
        InventoryButton.SetActive(true);
    }

    public void InventoryTab()
    {
        SettingsTabGroup.SetActive(false);
        SettingsButton.SetActive(true);
        InventoryTabGroup.SetActive(true);
        InventoryButton.SetActive(false);
    }
}
