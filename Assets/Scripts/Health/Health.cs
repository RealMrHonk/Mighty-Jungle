using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{

    [SerializeField] private float currentHp;
    public float CurrentHp => currentHp;
    public float PreviousHealth { get; private set; }

    [SerializeField] private float maxHp;
    public float MaxHp { get { return maxHp; } set { maxHp = value; } }

    public bool isAlive { get { return currentHp > 0; } }
    public bool isDead { get { return !isAlive; } }

    public event Action OnDamaged;
    public event Action OnDeath;  

    public void Kill()       
    {
        PreviousHealth = currentHp;
        currentHp = 0;
        OnDeath?.Invoke();
        print(gameObject.name + " died");
    }

    public void TakeDamage(float rawDamage)
    {
        PreviousHealth = currentHp;
        currentHp -= rawDamage;
        currentHp = Mathf.Clamp(CurrentHp, 0, MaxHp);
        OnDamaged?.Invoke();
        
        if(CurrentHp == 0)
        {
            Kill();
        }
    }
}
