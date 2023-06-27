using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class URPResolution : MonoBehaviour
{
    private void OnEnable()
    {
        RenderPipelineManager.beginCameraRendering += OnPreCullCustom;
    }

    private void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= OnPreCullCustom;
    }

    void OnPreCullCustom(ScriptableRenderContext context, Camera camera)
    {
        GL.Clear(true, true, Color.black);
    }

    void Start()
    {
        onSetting();
    }
    public void onSetting()
    {

        Camera camera = GetComponent<Camera>();

        Rect rect = camera.rect;

        float scaleheight = ((float)Screen.width / Screen.height) / ((float)9 / 16); // (가로 / 세로)

        float scalewidth = 1f / scaleheight;

        if (scaleheight < 1)

        {

            rect.height = scaleheight;

            rect.y = (1f - scaleheight) / 2f;

        }

        else

        {

            rect.width = scalewidth;

            rect.x = (1f - scalewidth) / 2f;

        }

        camera.rect = rect;
    }

    public void OnReset()
    {
        Camera camera = GetComponent<Camera>();

        camera.rect = new Rect(0, 0, 1, 1);
    }
    private void RenderPipelineManager_endCameraRendering(ScriptableRenderContext context, Camera camera)
    {

        GL.Clear(true, true, Color.black);
    }

    void OnPreCull()
    {
        GL.Clear(true, true, Color.black);
    }

}