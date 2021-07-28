using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
    {
    // This class will be used on each enemy doing damage.
    // Will be used to get the damage each enemy and player is doing.
    // Should check if possible to use attack Speed in this script.
     [SerializeField] int damage = 10;

    public int GetDamage()
    {
        return damage;
    }

    public void SetDamage(int DMG)
    {
        damage = DMG;
    }

    public void Hit()
    {
        Destroy(gameObject);
    }


}

