using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ElectricStaffWeapon : MonoBehaviour
{
    public float damage = 3f;
    public int chainCount = 1;
    public float chainRange = 5f;
    public float attackRate = 1f;
    private float cooldown = 0f;

    private void Update()
    {
        if (cooldown > 0f) cooldown -= Time.deltaTime;
    }

    public Vector2 TryAttack()
    {
        if (cooldown > 0f) return Vector2.zero;
        cooldown = attackRate;

        Enemy[] allEnemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        if (allEnemies.Length == 0) return Vector2.zero;

        Enemy nearest = null;
        float nearestDist = float.MaxValue;
        foreach (Enemy e in allEnemies)
        {
            float dist = Vector2.Distance(transform.position, e.transform.position);
            if (dist < nearestDist)
            {
                nearestDist = dist;
                nearest = e;
            }
        }

        if (nearest == null) return Vector2.zero;

        List<Enemy> hitList = new List<Enemy>();
        List<Vector3> linePoints = new List<Vector3>();

        hitList.Add(nearest);
        linePoints.Add(transform.position);
        linePoints.Add(nearest.transform.position);

        Vector2 dirToFirst = ((Vector2)nearest.transform.position - (Vector2)transform.position).normalized;
        Vector3 lastPos = nearest.transform.position;
        nearest.TakeDamage(damage);

        for (int i = 0; i < chainCount; i++)
        {
            Enemy next = null;
            float nextDist = float.MaxValue;

            foreach (Enemy e in allEnemies)
            {
                if (e == null || hitList.Contains(e)) continue;
                float dist = Vector2.Distance(lastPos, e.transform.position);
                if (dist < chainRange && dist < nextDist)
                {
                    nextDist = dist;
                    next = e;
                }
            }

            if (next == null) break;
            hitList.Add(next);
            linePoints.Add(next.transform.position);
            lastPos = next.transform.position;
            next.TakeDamage(damage);
        }

        StartCoroutine(DrawChainLines(linePoints));
        return dirToFirst;
    }

    private IEnumerator DrawChainLines(List<Vector3> points)
    {
        GameObject lineObj = new GameObject("ChainLine");
        LineRenderer lr = lineObj.AddComponent<LineRenderer>();

        Material mat = new Material(Shader.Find("Sprites/Default"));
        lr.material = mat;
        lr.startColor = Color.white;
        lr.endColor = Color.white;
        lr.startWidth = 0.06f;
        lr.endWidth = 0.06f;
        lr.positionCount = points.Count;
        lr.SetPositions(points.ToArray());
        lr.useWorldSpace = true;
        lr.sortingOrder = 10;

        yield return new WaitForSeconds(0.15f);

        Destroy(mat);
        Destroy(lineObj);
    }
}