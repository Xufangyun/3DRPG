using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : Singleton<GameManager>
{
    public CharacterStats playerStats;

    private CinemachineController cmController;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        //告诉所有观察者，玩家死亡
        if (playerStats != null&&playerStats.CurrentHealth == 0) 
            EventHandler.CallEndGame();
    }
    public void RigisterPlayer(CharacterStats player)
    {
        playerStats = player;

        cmController = FindObjectOfType<CinemachineController>();

        var virtualCamera = cmController.transform.GetChild(0).GetComponent<CinemachineVirtualCamera>();
        var freeCamera = cmController.transform.GetChild(1).GetComponent<CinemachineFreeLook>();
        

        virtualCamera.Follow = player.transform.GetChild(3).transform;
        virtualCamera.LookAt = player.transform.GetChild(3).transform;

        freeCamera.Follow = player.transform.GetChild(3).transform;
        freeCamera.LookAt = player.transform.GetChild(3).transform;

    }

    public Transform GetEntrance()
    {
        foreach (var item in FindObjectsOfType<TransitionDestination>())
        {
            if (item.destinationTag == TransitionDestination.DestinationTag.ENTER)
            {
                return item.transform;
            }
        }
        return null;
    }
}
