using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    [SerializeField]
    private EventSystem _es;

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
        _es = FindObjectOfType<EventSystem>();
        if (_es != null) _es.SetSelectedGameObject(_es.firstSelectedGameObject);
    }

    public void PressPlayCallback()
    {
        MenuCanvas.SetActive(false);
        PlayerConfirmCanvas.SetActive(true);
        menuState = MenuState.PlayerReadyScreen;
    }

    public void Exit()
    {
        Debug.Log("User has pressed exit");
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        if (menuState == MenuState.MainScreen)
        {
            if (Input.GetButtonDown("P1" + ButtonName_Start) || Input.GetButtonDown("P2" + ButtonName_Start))
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
