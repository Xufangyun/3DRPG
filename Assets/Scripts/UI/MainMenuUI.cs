using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class MainMenuUI : MonoBehaviour
{
    Button newGameBtn;

    Button continueBtn;

    Button quitBtn;

    PlayableDirector playableDirector;

    private void Awake()
    {
        newGameBtn = transform.GetChild(1).GetComponent<Button>();
        continueBtn = transform.GetChild(2).GetComponent<Button>();
        quitBtn = transform.GetChild(3).GetComponent<Button>();
        playableDirector = FindObjectOfType<PlayableDirector>();

        newGameBtn.onClick.AddListener(PlayTimeline);
        continueBtn.onClick.AddListener(ContinueGame);
        quitBtn.onClick.AddListener(QuiteGame);
        playableDirector.stopped += NewGame;
    }

    void PlayTimeline()
    {
        playableDirector.Play();
    }

    void NewGame(PlayableDirector director)
    {
        PlayerPrefs.DeleteAll();
        SceneController.Instance.TransitionToGameScene("OutdoorScene");
    }

    void ContinueGame()
    {
        SceneController.Instance.TransitionToGameScene(SaveManager.Instance.SceneName);
    }

    void QuiteGame()
    {
        Application.Quit();
    }
}
