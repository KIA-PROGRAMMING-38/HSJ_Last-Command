using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Missile : MonoBehaviour
{
    protected IObjectPool<Missile> _currentPool;
    protected Vector3 _direction;
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
    public void SetPool(IObjectPool<Missile> pool)
    {
        _currentPool = pool;
    }
    protected abstract void ReturnMissle();
    public void SetDirection(Vector3 direction)
    {
        _direction = direction;
    }
}
