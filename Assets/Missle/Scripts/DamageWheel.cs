using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageWheel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.gameObject.GetComponent<Player>().Damaged();
        }
    }
}
