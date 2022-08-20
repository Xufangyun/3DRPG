using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;
using UnityEngine.Networking;

public class Hotfix : MonoBehaviour
{
    private LuaEnv luaEnv;
    public Transform [] parent;

    private string url;
    private void Awake()
    {
        url = @"http://localhost:7201/AssetBundles/build/";

        luaEnv = new LuaEnv();
        luaEnv.AddLoader(MyLoader);
        luaEnv.DoString("require'Test'");
    }


    private void Start()
    {
        
    }
    private byte[] MyLoader(ref string filePath)
    {
        string absPath = @"D:\Unity Project\3DRPG_xLua\Assets\LuaScripts\" + filePath + ".lua.txt";
        return System.Text.Encoding.UTF8.GetBytes(File.ReadAllText(absPath));
    }


    private void OnDisable()
    {
        luaEnv.DoString("require'Dispose'");
    }
    private void OnDestroy()
    {
        luaEnv.Dispose();
    }

    [LuaCallCSharp]
    public void LoadResource()
    {
        StartCoroutine(LoadAllResource());
    }

    IEnumerator LoadAllResource()
    {
        int i = 1;
        while(i<=5)
        {
            yield return StartCoroutine(Load("build0"+i, i));
            i++;
        }
    }
    IEnumerator Load(string resName,int i)
    {

        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url+resName + ".ab");

        yield return request.SendWebRequest();
        AssetBundle ab = DownloadHandlerAssetBundle.GetContent(request);
        Instantiate(ab.LoadAsset<GameObject>(resName), parent[i-1].position,parent[i-1].rotation);
    }

    //IEnumerator SaveMyData(string url, string assetName)
    //{
    //    UnityWebRequest request = UnityWebRequest.Get(url);//要使用这种方式获取资源,才能访问到他的字节流

    //    yield return request.SendWebRequest();

    //    //保存下载回来的文件
    //    if (request.isDone)
    //    {
    //        //构造文件流
    //        FileStream fs = File.Create(Application.streamingAssetsPath + "/" + assetName);
    //        //将字节流写入文件里,request.downloadHandler.data可以获取到下载资源的字节流
    //        fs.Write(request.downloadHandler.data, 0, request.downloadHandler.data.Length);

    //        fs.Flush();     //文件写入存储到硬盘
    //        fs.Close();     //关闭文件流对象
    //        fs.Dispose();   //销毁文件对象

    //    }
    //}

    //IEnumerator LoadMyData(string assetName)
    //{
    //    AssetBundle assetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + assetName);
    //    yield return assetBundle;

    //}


}
