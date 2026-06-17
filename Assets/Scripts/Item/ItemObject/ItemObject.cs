using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;
    [SerializeField] private LayerMask whatIsGround;

    private float lastGroundedTime;

    private Vector2 targetPosition;

    private bool isPicked = false;  //防止多次拾取
    private bool isGrounded = false;

    private void Start()
    {
        SetupVisuals();
    }

    public void SetupItem(ItemData _itemData, Vector2 _velocity)
    {
        itemData = _itemData;
        rb.velocity = _velocity;
        SetupVisuals();
    }

    private void SetupVisuals()
    {
        if (itemData == null)
            return;

        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item Object - " + itemData.itemName;
    }

    public void PickupItem()
    {
        Inventory.instance.AddItem(itemData);
        isPicked = true;
        Destroy(gameObject);
    }

    public IEnumerator CheckGrounded(Vector2 _position, bool _isPositionGrounded, float _seconds)
    {
        targetPosition = _position;
        yield return new WaitForSeconds(_seconds);
        if (!isGrounded || Vector2.Distance(transform.position, targetPosition) > 10f)
        {
            if (!_isPositionGrounded)
            {
                Player player = PlayerManager.instance.player;
                yield return new WaitUntil(() => player.IsGroundDetected());
                targetPosition = player.transform.position;
            }
            isGrounded = true;
            transform.position = targetPosition;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (whatIsGround == (whatIsGround | 1 << collision.gameObject.layer))
        {
            isGrounded = true;
            lastGroundedTime = Time.time;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) => lastGroundedTime = -1;

    public bool canPicked() => isGrounded && lastGroundedTime > 0 && Time.time - lastGroundedTime > 1f
        && !isPicked && Inventory.instance.CanAddItem(itemData);   //包装成方法给ItemObject_Trigger调用
}
