using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuInteraction : MonoBehaviour
{
    public GameObject MenuCanvas;
    public GameObject PlayerConfirmCanvas;
    private MenuState menuState;

    public string ButtonName_Start = "_Start";
    public string ButtonName_Back= "_Back";
    public string ButtonName_ExitKB= "ExitKB";

    [SerializeField]
    private EventSystem _es;

    private enum MenuState
    {
        MainScreen,
        PlayerReadyScreen
    }

    // Use this for initialization
    private void Start()
    {
        menuState = MenuState.MainScreen;
        _es = FindObjectOfType<EventSystem>();
        if (_es != null) _es.SetSelectedGameObject(_es.firstSelectedGameObject);
    }

    public void PressPlayCallback()
    {
        MenuCanvas.SetActive(false);
        PlayerConfirmCanvas.SetActive(true);
        menuState = MenuState.PlayerReadyScreen;
        //SceneManager.LoadScene(2);
    }

    public void Exit()
    {
        Debug.Log("User has pressed exit");
        Application.Quit();
    }

    // Update is called once per frame
    private void Update()
    {
        if (menuState == MenuState.MainScreen)
        {
            if (Input.GetButtonDown("P1" + ButtonName_Start) || Input.GetButtonDown("P1_A") || Input.GetButtonDown("P2" + ButtonName_Start) || Input.GetButtonDown("P2_A"))
            {
                if(_es != null)
                {
                    if (_es.currentSelectedGameObject.name == "PlayButton") PressPlayCallback();
                    else if (_es.currentSelectedGameObject.name == "ExitButton") Exit();
                }
            }
        }

        if (menuState == MenuState.PlayerReadyScreen)
        {
            if (Input.GetButtonDown("P1_X")) SceneManager.LoadScene(2);

            if (Input.GetButtonDown("P2_X")) SceneManager.LoadScene(2);
        }
    }
}
