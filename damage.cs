using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damage : MonoBehaviour
{
    public float initialHealth = 10;
    public float currentHealth = 10;
    // Start is called before the first frame update
    private void Awake()
    {
        currentHealth = initialHealth;
        Debug.Log("Damage: Awake");
    }

    public void ApplyDamage(float damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
        }
        if (currentHealth <= 0)
        {
            Destruct();
        }

        Debug.Log("Damage: ApplyDamage");
    }


    private void Destruct()
    {
        Debug.Log("Damage: Destruct");
        Destroy(gameObject);
    }
   
}
