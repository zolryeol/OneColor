using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
public class CameraSizeSetter : MonoBehaviour
{
    [SerializeField] Camera camera;
    [SerializeField] CanvasScaler scaler;

    private void Awake()
    {
        camera = Camera.main;

        scaler = FindObjectOfType<CanvasScaler>();

        //SetResolution(); // 초기에 게임 해상도 고정

        //SetResolutionMobile();
    }

    /* 해상도 설정하는 함수 */
    public void SetResolution()
    {
        int setWidth = 1080; // 사용자 설정 너비
        int setHeight = 1920; // 사용자 설정 높이

        int deviceWidth = Screen.width; // 기기 너비 저장
        int deviceHeight = Screen.height; // 기기 높이 저장

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution 함수 제대로 사용하기

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
        }
        else // 게임의 해상도 비가 더 큰 경우
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // 새로운 높이
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
        }
    }

    public void SetResolutionMobile()
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
    void OnPreCull()
    {
        Debug.Log("프리컬");

        GL.Clear(true, true, Color.black);

    }
    void OnEnable()

    {

#if !UNITY_EDITOR

RenderPipelineManager.beginCameraRendering += RenderPipelineManager_endCameraRendering;

#endif

    }
    void OnDisable()
    {

    #if !UNITY_EDITOR

    RenderPipelineManager.beginCameraRendering -= RenderPipelineManager_endCameraRendering;

    #endif

    }

    private void RenderPipelineManager_endCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        Debug.Log("컬");

        GL.Clear(true, true, Color.black);

    }

}
