using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelManagerSaveTest : MonoBehaviour
{

    public GameObject[] ammoBoxes;

    private Dictionary<int, bool> SaveDictionary = new Dictionary<int, bool>();
    private Dictionary<int, bool> LoadDictionary = new Dictionary<int, bool>();

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            ammoBoxes[0].SetActive(!ammoBoxes[0].activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ammoBoxes[1].SetActive(!ammoBoxes[1].activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ammoBoxes[2].SetActive(!ammoBoxes[2].activeSelf);
        }

        if(Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("SAVING");
            SaveData();
        }

        if(Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("LOADING");
            LoadData();
        }
    }

    public void SaveData()
    {
        SaveDictionary.Clear();
        for(int i = 0; i < ammoBoxes.Length; i++)
        {
            SaveDictionary.Add(i, ammoBoxes[i].activeSelf);
        }
        // Ex. string "0, false" "1, true" "2, true"
        string savingData = JsonUtility.ToJson(SaveDictionary);
        string saveLocation = Application.persistentDataPath + "/saveDataTest.json";

        Debug.Log("Dictionary: " + SaveDictionary);
        Debug.Log("Saving: " +  savingData + " to: " + saveLocation);

        System.IO.File.WriteAllText(saveLocation, savingData);

    }

    public void LoadData()
    {
        string saveLocation = Application.persistentDataPath + "/saveDataTest.json";
        string loadingData = System.IO.File.ReadAllText(saveLocation);

        LoadDictionary = JsonUtility.FromJson<Dictionary<int, bool>>(loadingData);

        foreach(var item in LoadDictionary)
        {
            ammoBoxes[item.Key].SetActive(item.Value);
        }
    }
}
