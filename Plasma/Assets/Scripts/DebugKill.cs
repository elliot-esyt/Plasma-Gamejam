using UnityEngine;
using UnityEngine.InputSystem;

public class DebugKill : MonoBehaviour // DEBUG SCRIPT TO INSTA KILL RANDOM ENEMY
{
    private void Update()
    {
        var kb = Keyboard.current;

        if (kb.qKey.wasPressedThisFrame) // gets the Q key and see if it was pressed that frame
        {
            Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None); // finds an enemy
            if (enemies.Length == 0) return; // if theres no enemies then dont do anything
            Enemy target = enemies[Random.Range(0, enemies.Length)]; // pick a random enemy
            target.TakeDamage(9999f); // slime them
        }
    }
}