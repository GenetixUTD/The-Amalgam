using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using static UnityEditor.Progress;


public class LevelManagerSaveTest : MonoBehaviour
{

    public GameObject[] ammoBoxes;

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
        XElement el = new XElement("root");
        for(int i = 0; i < ammoBoxes.Length; i++)
        {
            el.Add(new XElement("A", i));
            el.Add(new XElement("B", ammoBoxes[i].activeSelf));
        }

        string saveLocation = Application.persistentDataPath + "/saveDataTest.xml";

        Debug.Log("Saving: " +  el + " to: " + saveLocation);

        el.Save(saveLocation);

    }

    public void LoadData()
    {
        string saveLocation = Application.persistentDataPath + "/saveDataTest.xml";
        XElement rootElement = XElement.Load(saveLocation);
        List<XElement> loadedData = rootElement.Elements().ToList();


        Debug.Log(loadedData.Count());
        
        for(int i = 0;i < loadedData.Count;i+=2)
        {
            ammoBoxes[(int)loadedData[i]].SetActive((bool)loadedData[i + 1]);
        }
    }
}
