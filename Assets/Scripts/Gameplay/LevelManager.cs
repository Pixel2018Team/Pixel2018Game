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
    private Image
        _chaosBar,
        _consuelaHappy,
        _consuelaSad,
        _kidsHappy,
        _kidsSad;

    public bool _consuelaLeads = true;

    private int _chaos = 0;
    private List<DoorScript> _doors;
    private int _totalChaos = 0;
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

        _startingChaos = (_totalChaos / 2);
    }

    // Update is called once per frame
    private void Update()
    {
        _gameTimer -= Time.deltaTime;
        if (_gameTimer <= 0) FinishLevel();
        if (_chaos >= _totalChaos) FinishLevel();
    }

    private void Reset()
    {
        _gameTimer = 300f;
        _chaos = 0;
        _totalChaos = 0;

        // Hide the game over panel
        _gameOverPanel.SetActive(false);
    }

    private void FinishLevel()
    {
        // Show the gameover screen
        _gameOverPanel.SetActive(true);
    }

    public void OpenAllDoors()
    {
        foreach(DoorScript door in _doors)
        {
            door.Open();
        }
    }

    /// <summary>
    /// Method to modify the actual level of chaos
    /// </summary>
    /// <param name="amount">The amount by which to modify the chaos</param>
    public void AddChaos(int amount)
    {
        _chaos += amount;

        _chaosBar.fillAmount = (float)(_chaos / _totalChaos);

        if (_chaos > _startingChaos && _consuelaLeads)
        {
            _consuelaLeads = false;

            _consuelaHappy.gameObject.SetActive(false);
            _consuelaSad.gameObject.SetActive(true);
            _kidsHappy.gameObject.SetActive(true);
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
    }

    /// <summary>
    /// Method used to add to the total chaos at the start of the game
    /// </summary>
    /// <param name="amount">The amount to add to the total level of chaos</param>
    public void TotalChaos(int amount)
    {
        this._totalChaos += amount;
    }
}
