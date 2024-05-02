using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndOfGame : MonoBehaviour
{
    public Image blackScreen;
    private bool fade;
    private float fadeAmount;

    private void Start()
    {
        fade = false;
        fadeAmount = .5f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            fade = true;
        }

        if(fade)
        {
            FadeOut();
        }

        Debug.Log(blackScreen.color.a);
        if(blackScreen.color.a >= 1)
        {
            SceneManager.LoadScene("StartMenu");
        }
    }

    private void FadeOut()
    {
        blackScreen.color = new Color(0, 0, 0, (blackScreen.color.a + (fadeAmount * Time.deltaTime)));
    }
}
