using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public int _chaos = 0;

    // Use this for initialization
    private void Start()
    {
        Reset();
    }

    // Update is called once per frame
    private void Update()
    {
        _gameTimer -= Time.deltaTime;
        if (_gameTimer <= 0) FinishLevel();
    }

    private void Reset()
    {
        _gameTimer = 300f;
        _chaos = 0;
    }

    private void FinishLevel()
    {
        // Show the gameover screen
    }
}
