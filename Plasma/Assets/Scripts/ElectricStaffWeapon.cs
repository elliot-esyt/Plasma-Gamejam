using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ElectricStaffWeapon : MonoBehaviour
{
    // variables
    public float damage = 3f;
    public int chainCount = 1;
    public float chainRange = 5f;
    public float attackRate = 1f;
    private float cooldown = 0f;

    private void Update()
    {
        if (cooldown > 0f) cooldown -= Time.deltaTime; // cooldwn 
    }

    public Vector2 TryAttack() 
    {
        if (cooldown > 0f) return Vector2.zero; // check cooldown
        cooldown = attackRate;

        Enemy[] allEnemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None); // find all enemies
        if (allEnemies.Length == 0) return Vector2.zero; // if no enemies dont do anything 

        Enemy nearest = null;
        float nearestDist = float.MaxValue;
        foreach (Enemy e in allEnemies) // loop for all enemies 
        {
            float dist = Vector2.Distance(transform.position, e.transform.position); // calculate distance from player 
            if (dist < nearestDist) // calcs nearest enemy
            {
                nearestDist = dist;
                nearest = e;
            }
        }

        if (nearest == null) return Vector2.zero; // if no enemies dont do anything

        if (AudioManager.Instance != null) AudioManager.Instance.PlayStaffUse();

        List<Enemy> hitList = new List<Enemy>();
        List<Vector3> linePoints = new List<Vector3>(); // creates list of enemies to make the line

        hitList.Add(nearest); // adds the first target 
        linePoints.Add(transform.position);
        linePoints.Add(nearest.transform.position);

        Vector2 dirToFirst = ((Vector2)nearest.transform.position - (Vector2)transform.position).normalized; // direction to first guy 
        Vector3 lastPos = nearest.transform.position;
        nearest.TakeDamage(damage); // dmgs them

        for (int i = 0; i < chainCount; i++) // chain setup 
        {
            Enemy next = null; // find next target 
            float nextDist = float.MaxValue;

            foreach (Enemy e in allEnemies) // checks all enemies 
            {
                if (e == null || hitList.Contains(e)) continue; // skips if it doesnt exist or is hit
                float dist = Vector2.Distance(lastPos, e.transform.position); // checks range
                if (dist < chainRange && dist < nextDist) // checks closest target
                {
                    nextDist = dist;
                    next = e;
                }
            }

            if (next == null) break; // stop if nobody there 
            hitList.Add(next); // do next enemy
            linePoints.Add(next.transform.position);
            lastPos = next.transform.position;
            next.TakeDamage(damage);
        }

        StartCoroutine(DrawChainLines(linePoints)); // creates beam
        return dirToFirst;
    }

    private IEnumerator DrawChainLines(List<Vector3> points) // ok we need lines 
    {
        GameObject lineObj = new GameObject("ChainLine"); // create a gameobject for the line
        LineRenderer lr = lineObj.AddComponent<LineRenderer>(); // add a line renderer 

        Material mat = new Material(Shader.Find("Sprites/Default")); // creates material
        lr.material = mat; // m,material stats 
        lr.startColor = Color.white;
        lr.endColor = Color.white;
        lr.startWidth = 0.06f;
        lr.endWidth = 0.06f;
        lr.positionCount = points.Count;
        lr.SetPositions(points.ToArray());
        lr.useWorldSpace = true;
        lr.sortingOrder = 10;

        yield return new WaitForSeconds(0.15f); // wait 

        Destroy(mat); // handles cleanup
        Destroy(lineObj);
    }
}