using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{
    public GameObject playerPrefab;
    GameObject player;
    FadeUI fade;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        fade = FindObjectOfType<FadeUI>();
    }
    public void TransitionToDestination(TransitionPoint transitionPoint)
    {
        switch (transitionPoint.transitionType)
        {
            case TransitionPoint.TransitionType.SameScene:
                StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.destinationTag));
                break;
            case TransitionPoint.TransitionType.DifferentScene:
                StartCoroutine(Transition(transitionPoint.sceneName, transitionPoint.destinationTag));
                break;
        }
    }

    //��Ϸ����֮����л�
    IEnumerator Transition(string sceneName,TransitionDestination.DestinationTag destinationTag)
    {
        yield return StartCoroutine(fade.Fade(1));
        SaveManager.Instance.SavePlayerData();
        SaveManager.Instance.SaveBagData();
        if (SceneManager.GetActiveScene().name != sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName);
            yield return Instantiate(playerPrefab, GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            SaveManager.Instance.LoadPlayerData();
            SaveManager.Instance.LoadBagData();
        }
        else
        {
            player = GameManager.Instance.playerStats.gameObject;
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            player.GetComponent<NavMeshAgent>().enabled = true;
            yield return null;
        }
        yield return StartCoroutine(fade.Fade(0));

    }

    private TransitionDestination GetDestination(TransitionDestination.DestinationTag destinationTag)
    {
        var entrance = FindObjectsOfType<TransitionDestination>();

        for(int i = 0; i < entrance.Length; i++)
        {
            if (entrance[i].destinationTag == destinationTag)
            {
                return entrance[i];
            }
        }
        return null;
    }

    //�ɲ˵��л�����Ϸ����
    public void TransitionToGameScene(string sceneName)
    {
        StartCoroutine(LoadLevel(sceneName));
    }

    //����Ϸ�����л��ز˵�
    public void TransitionToMain()
    {
        StartCoroutine(LoadMain());
    }

    //
    IEnumerator LoadLevel(string scene)
    {
        if (scene != "")
        {
            yield return StartCoroutine(fade.Fade(1));
            yield return SceneManager.LoadSceneAsync(scene);
            SaveManager.Instance.LoadPlayerData();
            SaveManager.Instance.LoadBagData();
            yield return player = Instantiate(playerPrefab, GameManager.Instance.GetEntrance().position, GameManager.Instance.GetEntrance().rotation);
            
            yield return StartCoroutine(fade.Fade(0));
            yield break;
        }
    }

    IEnumerator LoadMain()
    {
        //��������
        SaveManager.Instance.SavePlayerData();
        SaveManager.Instance.SaveBagData();

        yield return StartCoroutine(fade.Fade(1));
        yield return SceneManager.LoadSceneAsync("MenuScene");
        
        yield return StartCoroutine(fade.Fade(0));
        yield break;
    }
}
