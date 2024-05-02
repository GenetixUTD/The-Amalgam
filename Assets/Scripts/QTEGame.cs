using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class QTEGame : MonoBehaviour
{
    [SerializeField] private Canvas canvasReference;

    [SerializeField] private GameObject QTEItem;

    public enum QTEState
    {
        Idle,
        Undergoing,
        Success,
        Failed
    }

    private Color redColor;
    private Color greenColor;

    public QTEState gameState;
    private float gameTime;
    private float QTETime;
    private bool QTESpawn;

    private int QTEButton; //0 = E, 1 = F

    [SerializeField] private GameObject EButton;
    [SerializeField] private GameObject FButton;

    private void Start()
    {
        gameState = QTEState.Idle;
        QTESpawn = false;
        QTETime = 2.0f;
        redColor = Color.red;
        greenColor = Color.green;
    }

    public void gameStart()
    {
        gameTime = 0;
        QTESpawn = false;
        gameState = QTEState.Undergoing;

        QTEItem.GetComponent<Image>().color = Color.green;
        QTEItem.SetActive(false);

        FButton.SetActive(false);
        EButton.SetActive(false);

        StartCoroutine(QTEEvent());
        StartCoroutine(gameWin());
    }

    private void nextQTE()
    {
        gameTime = 0;
        QTESpawn = false;
        QTEItem.SetActive(false);
        QTEItem.GetComponent<Image>().color = Color.green;
        StartCoroutine(QTEEvent());
    }

    private void Update()
    {
        
        if(QTESpawn && gameState == QTEState.Undergoing)
        {
            QTEItem.SetActive(true);
            gameTime += Time.deltaTime;
            QTEItem.GetComponent<Image>().color = Color.Lerp(greenColor, redColor, gameTime / QTETime);
            switch (QTEButton)
            {
                case 0:
                    EButton.SetActive(true); FButton.SetActive(false); break;
                case 1:
                    FButton.SetActive(true); EButton.SetActive(false); break;

            }

            if(QTEItem.GetComponent<Image>().color.Equals(Color.red))
            {
                gameState = QTEState.Failed;
                QTEItem.SetActive(false);
            }
            else if((QTEButton == 0 && Input.GetKeyDown(KeyCode.E)) || (QTEButton == 1 && Input.GetKeyDown(KeyCode.F)))
            {
                nextQTE();
            }
        }
    }

    public void gameStop()
    {
        gameState = QTEState.Idle;
        QTEItem.SetActive(false);
        FButton.SetActive(false);
        EButton.SetActive(false);
        StopAllCoroutines();
    }

    IEnumerator gameWin()
    {
        yield return new WaitForSeconds(15f);
        gameState = QTEState.Success; 
        QTEItem.SetActive(false);
        FButton.SetActive(false);
        EButton.SetActive(false);
    }

    IEnumerator QTEEvent()
    {
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        QTESpawn = true;
        QTEItem.GetComponent<Image>().color = Color.green;
        QTEButton = Random.Range(0, 2);
    }
}
