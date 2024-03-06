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
        XElement el = new XElement("root");
        SaveDictionary.Clear();
        for(int i = 0; i < ammoBoxes.Length; i++)
        {
            el.Add(new XElement("A", i));
            el.Add(new XElement("B", ammoBoxes[i].activeSelf));
        }
        // Ex. string "0, false" "1, true" "2, true"
        

        /*foreach(var item in SaveDictionary)
        {
            el.Add(new XElement("A", item.Key));
            el.Add(new XElement("B", item.Value));
        }*/
        string saveLocation = Application.persistentDataPath + "/saveDataTest.xml";

        Debug.Log("Dictionary: " + SaveDictionary);
        Debug.Log("Saving: " +  el + " to: " + saveLocation);

        el.Save(saveLocation);

    }

    public void LoadData()
    {
        string saveLocation = Application.persistentDataPath + "/saveDataTest.xml";
        XElement rootElement = XElement.Load(saveLocation);
        List<XElement> loadedData = rootElement.Elements().ToList();
        //LoadDictionary = JsonUtility.FromJson<Dictionary<int, bool>>(loadingData);

        //Debug.Log(rootElement.Element("EntryIndex").Value);

        Debug.Log(loadedData.Count());
        
        for(int i = 0;i < loadedData.Count;i+=2)
        {
            ammoBoxes[(int)loadedData[i]].SetActive((bool)loadedData[i + 1]);
        }
        /*foreach(var item in LoadDictionary)
        {
            ammoBoxes[item.Key].SetActive(item.Value);
        }*/
    }
}
