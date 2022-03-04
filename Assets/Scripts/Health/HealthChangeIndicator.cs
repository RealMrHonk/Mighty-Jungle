using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthChangeIndicator : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private GameObject indicator;
    [SerializeField] private Bounds randomSpawnBounds;

    private void OnEnable()
    {
        health.OnDamaged += CreateDamageIndicator;
    }

    private void CreateDamageIndicator()
    {
        Vector2 position = GenerateRandomPositionWithinBounds();

        GameObject createdIndicator = Instantiate(indicator, (Vector2)transform.position + position, Quaternion.identity);
        createdIndicator.GetComponent<TextMeshPro>().SetText((health.PreviousHealth - health.CurrentHp).ToString());
    }

    private Vector2 GenerateRandomPositionWithinBounds()
    {
        float xPos = Random.Range(-(randomSpawnBounds.size.x / 2), randomSpawnBounds.size.x / 2);
        float yPos = Random.Range(-(randomSpawnBounds.size.y / 2), randomSpawnBounds.size.y / 2);

        return new Vector2(xPos, yPos);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(randomSpawnBounds.center, randomSpawnBounds.size);
    }

    private void OnDisable()
    {
        health.OnDamaged -= CreateDamageIndicator;
    }
}
