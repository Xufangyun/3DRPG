using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.IO;

public class LoadLua : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Load());
    }

    IEnumerator Load()
    {
        UnityWebRequest request = UnityWebRequest.Get(@"http://localhost:28454/" + "Test.lua.txt");
        yield return request.SendWebRequest();
        string str = request.downloadHandler.text;
        if (str != null)
        {
            File.WriteAllText(@"D:\Unity Project\Xlua\Assets\LuaScripts\Test.lua.txt", str);
        }
        

        UnityWebRequest request1 = UnityWebRequest.Get(@"http://localhost:28454/" + "Dispose.lua.txt");
        yield return request1.SendWebRequest();
        string str1 = request1.downloadHandler.text;
        if (str != null)
        {
            File.WriteAllText(@"D:\Unity Project\Xlua\Assets\LuaScripts\Dispose.lua.txt", str1);
        }
        
        SceneManager.LoadScene(1);
    }
}
