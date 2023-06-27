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

        //SetResolution(); // �ʱ⿡ ���� �ػ� ����

        //SetResolutionMobile();
    }

    /* �ػ� �����ϴ� �Լ� */
    public void SetResolution()
    {
        int setWidth = 1080; // ����� ���� �ʺ�
        int setHeight = 1920; // ����� ���� ����

        int deviceWidth = Screen.width; // ��� �ʺ� ����
        int deviceHeight = Screen.height; // ��� ���� ����

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution �Լ� ����� ����ϱ�

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // ����� �ػ� �� �� ū ���
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // ���ο� �ʺ�
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // ���ο� Rect ����
        }
        else // ������ �ػ� �� �� ū ���
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // ���ο� ����
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // ���ο� Rect ����
        }
    }

    public void SetResolutionMobile()
    {
        Camera camera = GetComponent<Camera>();
        Rect rect = camera.rect;
        float scaleheight = ((float)Screen.width / Screen.height) / ((float)9 / 16); // (���� / ����)
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
        Debug.Log("������");

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
        Debug.Log("��");

        GL.Clear(true, true, Color.black);

    }

}
