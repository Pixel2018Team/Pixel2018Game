﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    // Singleton Structure
    #region Singleton
    public static LevelManager _instance;

    public static LevelManager Instance
    {
        get
        {
            if (_instance == null)
            {
                if (GameObject.Find("LevelManager"))
                {
                    GameObject g = GameObject.Find("LevelManager");
                    if (g.GetComponent<LevelManager>())
                    {
                        _instance = g.GetComponent<LevelManager>();
                    }
                    else
                    {
                        _instance = g.AddComponent<LevelManager>();
                    }
                }
                else
                {
                    GameObject g = new GameObject { name = "LevelManager" };
                    _instance = g.AddComponent<LevelManager>();
                }
            }

            return _instance;
        }

        set
        {
            _instance = value;
        }
    }
    #endregion Singleton

    [SerializeField]
    private float _gameTimer = 300f;

    [SerializeField]
    private GameObject
        _ConsuelaGOPanel,
        _KidsGOPanel,
        _xButton;

    public int maxChaos;

    [SerializeField]
    private Slider _chaosBar;

    public bool _consuelaLeads = true;

    private int _chaos = 0;
    private List<DoorScript> _doors;
    private int _startingChaos = 0;
    private bool _gameover = false;

    private void Awake()
    {
        Reset();
    }

    // Use this for initialization
    private void Start()
    {
        _doors = new List<DoorScript>();

        // Populating the list of doors
        //foreach (var obj in FindObjectsOfType<DoorScript>())
        //{
        //    DoorScript dm = obj;
        //    if (dm != null) _doors.Add(dm);
        //}

        _startingChaos = (maxChaos / 2);
        _chaos = _startingChaos;
        if(_chaosBar != null)
        {
            _chaosBar.value = ((float)_chaos / (float)maxChaos);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (_gameTimer > 0) _gameTimer -= Time.deltaTime;
        if (_gameTimer <= 0) FinishLevel();
        if (_chaos >= maxChaos) FinishLevel();

        if (_gameTimer <= 0)
        {
            if (Input.GetButtonDown("P1_X") || Input.GetButtonDown("P2_X"))
            {
                Reset();
            }
        }

        if(_gameover)
        {
            if (Input.GetButtonDown("P1_X") || Input.GetButtonDown("P2_X")) SceneManager.LoadScene(2);
        }
    }

    private void Reset()
    {
        _gameTimer = 300f;
        _chaos = 0;
        _gameover = false;

        // Hide the game over panel
        if (_ConsuelaGOPanel != null) _ConsuelaGOPanel.SetActive(false);
        if (_KidsGOPanel != null) _KidsGOPanel.SetActive(false);
        if (_xButton != null) _xButton.SetActive(false);
    }

    private void FinishLevel()
    {
        // Show the gameover screen
        if (_ConsuelaGOPanel != null && _chaos <= _startingChaos)
        {
            _ConsuelaGOPanel.SetActive(true);
        }
        else if (_KidsGOPanel != null && _chaos > _startingChaos)
        {
            _KidsGOPanel.SetActive(true);
        }
        if (_xButton != null) _xButton.SetActive(true);

        _gameover = true;
    }

    public void OpenAllDoors()
    {
        foreach (DoorScript door in _doors)
        {
            door.OpenClose(true);
        }
    }

    /// <summary>
    /// Method to modify the actual level of chaos
    /// </summary>
    /// <param name="amount">The amount by which to modify the chaos</param>
    public void AddChaos(int amount)
    {
        _chaos += amount;

        if(_chaosBar != null)
        {
            _chaosBar.value = ((float)_chaos / (float)maxChaos);
        }

        if (_chaos > _startingChaos && _consuelaLeads)
        {
            _consuelaLeads = false;
            AkSoundEngine.PostEvent("Kids_Lead", gameObject);
        }
        else if (_chaos < _startingChaos && !_consuelaLeads)
        {
            _consuelaLeads = true;
            AkSoundEngine.PostEvent("Gonzuela_Leads", gameObject);
        }
    }
}