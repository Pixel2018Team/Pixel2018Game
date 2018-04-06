using UnityEngine;
using System.Collections.Generic;

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

    public int _chaos = 0;

    private List<DoorManager> _doors;
    private int _totalChaos = 0;

    // Use this for initialization
    private void Start()
    {
        Reset();

        // Populating the list of doors
        foreach(var obj in FindObjectsOfType<DoorManager>())
        {
            DoorManager dm = obj;
            if (obj != null) _doors.Add(dm);
        }
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
        foreach(DoorManager door in _doors)
        {
            door.Open();
        }
    }

    public void AddChaos(int amount)
    {
        this._totalChaos += amount;
    }
}
