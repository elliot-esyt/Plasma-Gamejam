using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    // variables
    public float moveSpeed = 3f;

    private Transform _target;

    public void SetTarget(Transform target) // sets the target to the player
    {
        _target = target;
    }

    private void Update() // moves to player
    {
        if (_target == null) return;

        Vector3 direction = (_target.position - transform.position).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
    }
}