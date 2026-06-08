using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // variables
    public float moveSpeed = 5f;

    private void Update() 
    {
        // handles player movement
        var kb = Keyboard.current;
        if (kb == null) return;

        Vector2 move = Vector2.zero;

        if (kb.wKey.isPressed || kb.upArrowKey.isPressed) move.y += 1;
        if (kb.sKey.isPressed || kb.downArrowKey.isPressed) move.y -= 1;
        if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) move.x += 1;
        if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) move.x -= 1;

        transform.Translate(move.normalized * moveSpeed * Time.deltaTime);
    }
}
