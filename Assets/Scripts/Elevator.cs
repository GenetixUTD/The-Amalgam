using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public GameObject DoorL;
    public GameObject DoorR;

    public float speed;

    public GameObject playerParent;
    public GameObject playerActual;

    public float floor2y;
    public float floor1y;
    public float floor0y;
    public float floorsub1y;

    public bool inMotion;

    public int targetFloor;

    private bool playerChild;

    private void Start()
    {
        floor2y = -50;
        floor1y = -100;
        floor0y = -150;
        floorsub1y = -200;
        playerChild = false;

    }
    private void Update()
    {
        if(targetFloor != playerActual.GetComponent<charactercontroller>().currentFloor)
        {
            inMotion = true;
            float temp = -50;
            switch(targetFloor)
            {
                case 2:
                    temp = floor2y;
                    break;
                case 1:
                    temp = floor1y;
                    break;
                case 0: 
                    temp = floor0y; 
                    break;
                case -1 : 
                    temp = floorsub1y; 
                    break;
            }
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(0,temp,0), speed *Time.deltaTime);
        }
        else if(targetFloor == playerActual.GetComponent<charactercontroller>().currentFloor)
        {
            inMotion = false;
        }


        if(inMotion)
        {
            CloseDoors();
            playerActual.GetComponent<charactercontroller>().currentFloor = 100;
        }
        else if(!inMotion && playerChild == true)
        {
            OpenDoors();
        }


        if (Mathf.Abs(transform.position.y - floor2y) < 1f && targetFloor == 2)
        {
            playerActual.GetComponent<charactercontroller>().currentFloor = 2;
            transform.position = new Vector3(0, floor2y, 0);
            inMotion = false;
        }
        else if(Mathf.Abs(transform.position.y - floor1y) < 1f && targetFloor == 1)
        {
            playerActual.GetComponent<charactercontroller>().currentFloor = 1;
            transform.position = new Vector3(0, floor1y, 0);
            inMotion = false;
        }
        else if (Mathf.Abs(transform.position.y - floor0y) < 1f && targetFloor == 0)
        {
            playerActual.GetComponent<charactercontroller>().currentFloor = 0;
            transform.position = new Vector3(0, floor0y, 0);
            inMotion = false;
        }
        else if (Mathf.Abs(transform.position.y - floorsub1y) < 1f && targetFloor == -1)
        {
            playerActual.GetComponent<charactercontroller>().currentFloor = -1;
            transform.position = new Vector3(0, floorsub1y, 0);
            inMotion = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            playerActual.transform.SetParent(this.transform);
            playerChild = true;
            if(!inMotion)
            {
                OpenDoors();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            playerChild = false;
            playerActual.transform.SetParent(playerParent.transform);
            CloseDoors();
        }
    }

    public void OpenDoors()
    {
        DoorL.GetComponent<Animator>().SetBool("open", true);
        DoorL.GetComponent<Animator>().SetBool("close", false);
        DoorR.GetComponent<Animator>().SetBool("open", true);
        DoorR.GetComponent<Animator>().SetBool("close", false);
    }

    public void CloseDoors()
    {
        DoorL.GetComponent<Animator>().SetBool("close", true);
        DoorL.GetComponent<Animator>().SetBool("open", false);
        DoorR.GetComponent<Animator>().SetBool("close", true);
        DoorR.GetComponent<Animator>().SetBool("open", false);
    }

}
