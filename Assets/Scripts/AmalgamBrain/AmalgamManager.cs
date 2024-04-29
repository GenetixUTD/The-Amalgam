using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmalgamManager : MonoBehaviour
{
    private int tensionMeter;

    public Transform[] extryExitPoints;

    private GameObject theAmalgam;

    private void Start()
    {
        theAmalgam = GameObject.FindGameObjectWithTag("amalgam");
    }

    public void toggleAI()
    {
        theAmalgam.GetComponent<AmalgamFSM>().enabled = !theAmalgam.GetComponent<AmalgamFSM>().enabled;
    }
}
