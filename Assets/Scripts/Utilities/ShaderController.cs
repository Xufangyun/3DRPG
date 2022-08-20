using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderController : MonoBehaviour
{
    public float dissolveTime;

    private SkinnedMeshRenderer _render;

    private void Start()
    {
        _render =GetComponentInChildren<SkinnedMeshRenderer>();
    }

    private void Update()
    {
    }

    private void OnEnable()
    {
        EventHandler.SelectedEnemyShaderChangeEvent += OnSelectedEnemyShaderChangeEvent;
    }

    private void OnDisable()
    {
        EventHandler.SelectedEnemyShaderChangeEvent -= OnSelectedEnemyShaderChangeEvent;
    }

    public void SetDissolve()
    {
        StartCoroutine(Dissolve());
    }

    IEnumerator Dissolve()
    {
        yield return new WaitForSeconds(2.0f);
        int shaderID = Shader.PropertyToID("_ClipRate");
        float time = 0f;
        while (time < dissolveTime)
        {
            time += Time.deltaTime;
            _render.material.SetFloat(shaderID, time/dissolveTime);
            yield return null;
        }
    }
    

    /// <summary>
    /// 更改Shader参数，被选中状态
    /// </summary>
    /// <param name="target"></param>
    public void OnSelectedEnemyShaderChangeEvent(GameObject target)
    {
        if (gameObject == target)//判断是否是点击的目标
        {
            _render.material.SetFloat("_RimPower", 2);
        }
        else
        {
            _render.material.SetFloat("_RimPower", 200);
        }
    }

}
