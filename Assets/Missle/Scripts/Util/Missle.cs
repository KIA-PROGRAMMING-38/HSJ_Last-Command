using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Missle : MonoBehaviour
{
    protected IObjectPool<Missle> _currentPool;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.gameObject.GetComponent<Player>().Damaged();
            gameObject.SetActive(false);
            ReturnMissle();
        }

        if(collision.gameObject.layer == LayerMask.NameToLayer("DespawnZone"))
        {
            ReturnMissle();
        }
    }
    public void SetPool(IObjectPool<Missle> pool)
    {
        _currentPool = pool;
    }
    protected abstract void ReturnMissle();
}
