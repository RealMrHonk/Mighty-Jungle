using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MeleeAttack : MonoBehaviour
{
    public event Action OnAttack;
    [SerializeField] private float damage = 1;
    public float Damage { get { return damage; } set { damage = value; } }
    [SerializeField] private LayerMask attackLayer;
    
    public void Attack() => OnAttack?.Invoke();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (1<<collision.gameObject.layer == attackLayer.value)
        {
            print("hursens");
            collision.GetComponent<IDamageable>().TakeDamage(damage);
        }
    }
}
