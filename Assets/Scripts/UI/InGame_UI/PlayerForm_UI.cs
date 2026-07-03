using UnityEngine;

public class PlayerForm_UI : MonoBehaviour
{
    [SerializeField] private RectTransform eyePos;
    [SerializeField] private float eyePosRateX = 0.02f;
    [SerializeField] private float eyePosRateY = 0.02f;
    [SerializeField] private float smoothSpeed = 5f;  // 平滑速度，越大越快

    private Vector2 originalPercentPos;
    private Vector2 currentPos;  // 当前实际位置

    private void Start()
    {
        Vector2 parentSize = GetParentSize();
        originalPercentPos = eyePos.anchoredPosition / parentSize;
        currentPos = eyePos.anchoredPosition;
    }

    private void Update()
    {
        Vector2 parentSize = GetParentSize();

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        int facingDir = PlayerManager.instance.player.facingDir;

        // 计算目标位置
        Vector2 targetPos = originalPercentPos * parentSize;

        // 限制范围
        x = Mathf.Clamp(x, targetPos.x - eyePosRateX * parentSize.x, targetPos.x + eyePosRateX * parentSize.x);
        y = Mathf.Clamp(y, targetPos.y - eyePosRateY * parentSize.y, targetPos.y + eyePosRateY * parentSize.y);

        targetPos += new Vector2(x * eyePosRateX * parentSize.x, y * eyePosRateY * parentSize.y);

        // 平滑过渡到目标位置
        currentPos = Vector2.Lerp(currentPos, targetPos, Time.deltaTime * smoothSpeed);

        eyePos.anchoredPosition = currentPos;
    }

    private Vector2 GetParentSize()
    {
        RectTransform parent = transform.parent as RectTransform;
        if (parent == null) return new Vector2(Screen.width, Screen.height);
        return parent.rect.size;
    }
}