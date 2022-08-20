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
    //    UnityWebRequest request = UnityWebRequest.Get(url);//Ҫʹ�����ַ�ʽ��ȡ��Դ,���ܷ��ʵ������ֽ���

    //    yield return request.SendWebRequest();

    //    //�������ػ������ļ�
    //    if (request.isDone)
    //    {
    //        //�����ļ���
    //        FileStream fs = File.Create(Application.streamingAssetsPath + "/" + assetName);
    //        //���ֽ���д���ļ���,request.downloadHandler.data���Ի�ȡ��������Դ���ֽ���
    //        fs.Write(request.downloadHandler.data, 0, request.downloadHandler.data.Length);

    //        fs.Flush();     //�ļ�д��洢��Ӳ��
    //        fs.Close();     //�ر��ļ�������
    //        fs.Dispose();   //�����ļ�����

    //    }
    //}

    //IEnumerator LoadMyData(string assetName)
    //{
    //    AssetBundle assetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + assetName);
    //    yield return assetBundle;

    //}


}
