﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuInteraction : MonoBehaviour
{
    public GameObject MenuCanvas;
    public GameObject PlayerConfirmCanvas;
    private MenuState menuState;
    public GameObject p1ReadyLabel;
    public GameObject p2ReadyLabel;

    public string p1ReadyTextDefault = "P1 not ready";
    public string p1ReadyText = "P1 ready";
    public string p2ReadyTextDefault = "P2 not ready";
    public string p2ReadyText = "P2 ready";
    public bool p1Ready, p2Ready;

    public string ButtonName_Start = "_Start";
    public string ButtonName_Back= "_Back";
    public string ButtonName_ExitKB= "ExitKB";

    private enum MenuState
    {
        MainScreen,
        PlayerReadyScreen
    }

    // Use this for initialization
    void Start()
    {
        menuState = MenuState.MainScreen;
        p1Ready = false;
        p2Ready = false;
    }

    public void PressPlayCallback()
    {
        MenuCanvas.SetActive(false);
        PlayerConfirmCanvas.SetActive(true);
        menuState = MenuState.PlayerReadyScreen;
    }

    public void PressScoresCallback()
    {
        //Open scores scene
    }

    // Update is called once per frame
    void Update()
    {
        if (menuState == MenuState.MainScreen)
        {
            if (Input.GetButtonDown("P1"+ ButtonName_Start) || Input.GetButtonDown("P2"+ ButtonName_Start))
            {
                Debug.Log("press start");
                PressPlayCallback();
            }

            if (Input.GetButtonDown("P1"+ ButtonName_Back) || Input.GetButtonDown("P2"+ ButtonName_Back))
            {
                Debug.Log("press scores");
                PressScoresCallback();
            }

            if (Input.GetButtonDown(ButtonName_ExitKB))
            {
                Debug.Log("press quit");
                Application.Quit();
            }
        }

        if (menuState == MenuState.PlayerReadyScreen)
        {
            if (Input.GetButtonDown("P1"+ ButtonName_Start))
            {
                p1Ready = !p1Ready;

                if (p1Ready)
                {
                    p1ReadyLabel.GetComponent<Text>().text = p1ReadyText;
                }
                else
                {
                    p1ReadyLabel.GetComponent<Text>().text = p1ReadyTextDefault;
                }
            }

            if (Input.GetButtonDown("P2"+ ButtonName_Start) || Input.GetButtonDown("Submit"))
            {
                p2Ready = !p2Ready;

                if (p2Ready)
                {
                    p2ReadyLabel.GetComponent<Text>().text = p2ReadyText;
                }
                else
                {
                    p2ReadyLabel.GetComponent<Text>().text = p2ReadyTextDefault;
                }
            }
        }

        if(p1Ready && p2Ready)
        {
            SceneManager.LoadScene(2);
        }
    }
}
