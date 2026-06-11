using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 screenBounds;
    private float playerHalfWidth;
    private float playerHalfHeight;

    private void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        playerHalfWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
        playerHalfHeight = GetComponent<SpriteRenderer>().bounds.extents.y;
        GetComponent<Rigidbody2D>().sleepMode = RigidbodySleepMode2D.NeverSleep;
    }

    private void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        Vector2 move = Vector2.zero;

        if (kb.wKey.isPressed || kb.upArrowKey.isPressed) move.y += 1;
        if (kb.sKey.isPressed || kb.downArrowKey.isPressed) move.y -= 1;
        if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) move.x += 1;
        if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) move.x -= 1;

        transform.Translate(move.normalized * moveSpeed * Time.deltaTime);

        float clampedX = Mathf.Clamp(transform.position.x, -screenBounds.x + playerHalfWidth, screenBounds.x - playerHalfWidth);
        Vector2 posx = transform.position;
        posx.x = clampedX;
        transform.position = posx;

        float clampedY = Mathf.Clamp(transform.position.y, -screenBounds.y + playerHalfHeight, screenBounds.y - playerHalfHeight);
        Vector2 posy = transform.position;
        posy.y = clampedY;
        transform.position = posy;
    }
}