using System;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    #region variables
    public GameObject playButton;
    public GameObject optionsButton;
    public GameObject quitButton;
    public GameObject quitDialogue;
    public GameObject optionsMenu;
    public GameObject backButton;
    public GameObject option1;
    public Text currentLetter;
    public Text averageSpeed;
    public double avgSpeed;
    public int matchCount;
    public int errors;

    DateTime time;
    TimeSpan delta;
    string gameState;
    string prevGameState;
    const string ALPHA = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    const string NUMERAL = "0123456789";
    const string ALPHANUM = ALPHA + NUMERAL;
    const string menu = "menu";
    const string play = "play";
    const string pause = "pause";
    const string quit = "quit";
    const string options = "options";
    Vector2[] quitButtonAnchorsMenu = new Vector2[2];
    Vector2[] quitButtonAnchorsPlay = new Vector2[2];
    #endregion

    void Awake()
    {
        quitButtonAnchorsMenu[0] = quitButton.transform.GetComponent<RectTransform>().anchorMin;
        quitButtonAnchorsMenu[1] = quitButton.transform.GetComponent<RectTransform>().anchorMax;
        quitButtonAnchorsPlay[0] = new Vector2(0, 1);
        quitButtonAnchorsPlay[1] = new Vector2(0, 1);
        gameState = menu;
        prevGameState = menu;
        ChangeGameState(menu);
    }

    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            if (gameState == pause) ChangeGameState(play);
            if (gameState == play) ChangeGameState(pause);
            if (gameState == menu) ChangeGameState(quit);
            if (gameState == quit) ChangeGameState(prevGameState);
            if (gameState == options) ChangeGameState(menu);
        }
        if (Input.GetKeyDown("return"))
        {
            if (gameState == menu || gameState == pause) ChangeGameState(play);
        }
        if (gameState != play) return;

        if (Input.GetKeyDown(currentLetter.text.ToLower()))
        {
            matchCount++;
            DateTime tempTime = DateTime.UtcNow;
            delta = tempTime - time;
            time = tempTime;
            double matchTime = delta.TotalMilliseconds;
            if (matchCount < 2) avgSpeed = matchTime;
            else
                avgSpeed = ((avgSpeed / (matchCount - 1)) + delta.TotalMilliseconds) / matchCount;
            averageSpeed.text = "Average Speed: " + avgSpeed.ToString("0") + " ms";
            string tempLetter = currentLetter.text;
            while (currentLetter.text.ToLower() == tempLetter.ToLower())
                tempLetter = string.Format("{0}", ALPHA[UnityEngine.Random.Range(0, 52)]);
            currentLetter.text = tempLetter;
        }
    }

    void MainMenu()
    {
        playButton.SetActive(true);
        optionsButton.SetActive(true);
        ToggleQuitPosition(menu);
        quitButton.SetActive(true);
        avgSpeed = 0;
        matchCount = 0;
        errors = 0;
    }

    void OptionsMenu()
    {
        ToggleQuitPosition(play);
        backButton.SetActive(true);
        optionsMenu.SetActive(true);
    }

    void StartGame()
    {
        ToggleQuitPosition(play);
        quitButton.SetActive(true);
        currentLetter.text = string.Format("{0}", ALPHA[UnityEngine.Random.Range(0, 52)]);
        currentLetter.enabled = true;
        averageSpeed.text = "Average Speed: " + avgSpeed.ToString("0") + " ms";
        averageSpeed.enabled = true;
        time = DateTime.UtcNow;
    }

    void PauseGame()
    {
        playButton.GetComponentInChildren<Text>().text = "RESUME GAME";
        playButton.SetActive(true);
        optionsButton.SetActive(true);
        ToggleQuitPosition(menu);
        quitButton.SetActive(true);
    }

    void QuitMenu()
    {
        quitDialogue.SetActive(true);
    }

    public void QuitGame()
    {
        if (prevGameState == pause) ChangeGameState(menu);
        if (prevGameState == play) ChangeGameState(menu);
        if (prevGameState == menu)
        {
            //if (Debug.isDebugBuild) UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }
    }

    public void CancelQuit()
    {
        ChangeGameState(prevGameState);
    }

    public void ChangeGameState(string state)
    {
        prevGameState = gameState;
        gameState = state;
        playButton.GetComponentInChildren<Text>().text = "START GAME";
        playButton.SetActive(false);
        optionsButton.SetActive(false);
        quitButton.SetActive(false);
        quitDialogue.SetActive(false);
        optionsMenu.SetActive(false);
        backButton.SetActive(false);
        currentLetter.enabled = false;
        averageSpeed.enabled = false;
        if (state == menu) MainMenu();
        if (state == options) OptionsMenu();
        if (state == pause) PauseGame();
        if (state == play) StartGame();
        if (state == quit) QuitMenu();
    }

    void ToggleQuitPosition(string state)
    {
        if (state == play)
        {
            quitButton.GetComponentInChildren<Text>().text = "MENU";
            quitButton.transform.GetComponent<RectTransform>().anchorMin = quitButtonAnchorsPlay[0];
            quitButton.transform.GetComponent<RectTransform>().anchorMax = quitButtonAnchorsPlay[1];
        }
        else
        {
            quitButton.GetComponentInChildren<Text>().text = "QUIT";
            quitButton.transform.GetComponent<RectTransform>().anchorMin = quitButtonAnchorsMenu[0];
            quitButton.transform.GetComponent<RectTransform>().anchorMax = quitButtonAnchorsMenu[1];
        }
    }
}
