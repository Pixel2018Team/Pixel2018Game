using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

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
    private GameObject _gameOverPanel;

    [SerializeField]
    private Text ScoreTextTest;

    public int maxChaos;

    [SerializeField]
    private Image
        _consuelaHappy,
        _consuelaSad,
        _kidsHappy,
        _kidsSad;

    [SerializeField]
    private Slider _chaosBar;

    public bool _consuelaLeads = true;

    private int _chaos = 0;
    private List<DoorScript> _doors;
    private int _startingChaos = 0;

    private void Awake()
    {
        Reset();
    }

    // Use this for initialization
    private void Start()
    {
        // Populating the list of doors
        foreach(var obj in FindObjectsOfType<DoorScript>())
        {
            DoorScript dm = obj;
            if (obj != null) _doors.Add(dm);
        }

        _startingChaos = (maxChaos / 2);
        _chaos = _startingChaos;
        _chaosBar.value = ((float)_chaos / (float)maxChaos);
        ScoreTextTest.text = _chaos.ToString();
    }

    // Update is called once per frame
    private void Update()
    {
        _gameTimer -= Time.deltaTime;
        if (_gameTimer <= 0) FinishLevel();
        if (_chaos >= maxChaos) FinishLevel();
    }

    private void Reset()
    {
        _gameTimer = 300f;
        _chaos = 0;

        // Hide the game over panel
        if (_gameOverPanel != null)
        {
            _gameOverPanel.SetActive(false);
        }
    }

    private void FinishLevel()
    {
        // Show the gameover screen
        if(_gameOverPanel != null)
        {
            _gameOverPanel.SetActive(true);
        }
    }

    public void OpenAllDoors()
    {
        foreach(DoorScript door in _doors)
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

        _chaosBar.value = ((float)_chaos / (float)maxChaos);

        if (_chaos > _startingChaos && _consuelaLeads)
        {
            _consuelaLeads = false;

            if(_consuelaHappy != null)
            _consuelaHappy.gameObject.SetActive(false);

            if (_consuelaSad != null)
                _consuelaSad.gameObject.SetActive(true);

            if (_kidsHappy != null)
                _kidsHappy.gameObject.SetActive(true);

            if (_kidsSad != null)
                _kidsSad.gameObject.SetActive(false);
        }
        else if (_chaos < _startingChaos && !_consuelaLeads)
        {
            _consuelaLeads = true;

            _consuelaHappy.gameObject.SetActive(true);
            _consuelaSad.gameObject.SetActive(false);
            _kidsHappy.gameObject.SetActive(false);
            _kidsSad.gameObject.SetActive(true);
        }

        ScoreTextTest.text = _chaos.ToString();
    }
}
