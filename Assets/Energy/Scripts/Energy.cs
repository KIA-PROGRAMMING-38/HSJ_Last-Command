using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Energy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Player>() != null)
        {
            Player player = collision.gameObject.GetComponent<Player>();
            player.EarnEnergy();
            gameObject.SetActive(false);
        }
    }
}
