using UnityEngine;
using UnityEngine.InputSystem;

public class DebugKill : MonoBehaviour
{
    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            if (enemies.Length == 0) return;
            Enemy target = enemies[Random.Range(0, enemies.Length)];
            target.TakeDamage(9999f);
        }
    }
}