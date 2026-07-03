using UnityEngine;

public class CameraFixUI : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    private float targetAspect = 16f / 9f;

    private void Start()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;

        ApplyViewport();
    }

    private void Update()
    {
        ApplyViewport();    //!后续修改为只在更改分辨率时调用
    }

    private void ApplyViewport()
    {
        float screenAspect = (float)Screen.width / Screen.height;

        // 如果比例正好是 16:9，全屏
        if (Mathf.Approximately(screenAspect, targetAspect))
        {
            targetCamera.rect = new Rect(0, 0, 1, 1);
            return;
        }

        if (screenAspect < targetAspect)    //固定16:9
        {
            float height = screenAspect / targetAspect;
            float y = (1f - height) / 2f;
            targetCamera.rect = new Rect(0, y, 1, height);
        }
        else if (screenAspect > targetAspect)
        {
            float width =  targetAspect / screenAspect;
            float x = (1f - width) / 2f;
            targetCamera.rect = new Rect(x, 0, width, 1);
        }
    }
}