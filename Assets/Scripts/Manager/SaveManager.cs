using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : Singleton<SaveManager>
{
    string sceneName;
    public CharacterData_SO characterData;
    public string SceneName { get => PlayerPrefs.GetString(sceneName); }
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneController.Instance.TransitionToMain();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SavePlayerData();
            SaveBagData();


        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadPlayerData();
            LoadBagData();
        }
    }

    public void SavePlayerData()
    {
        Save(characterData, characterData.name);
    }

    public void SaveBagData()
    {
        Save(InventoryManager.Instance.bagData, InventoryManager.Instance.bagData.name);
    }

    public void LoadPlayerData()
    {
        Load(characterData, characterData.name);
    }

    public void LoadBagData()
    {
        Load(InventoryManager.Instance.bagData, InventoryManager.Instance.bagData.name);
    }

    public void Save(Object data,string key)
    {
        var jsonData = JsonUtility.ToJson(data,true);
        PlayerPrefs.SetString(key, jsonData);
        PlayerPrefs.SetString(sceneName, SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
    }

    public void Load(Object data, string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);
        }
    }
}
