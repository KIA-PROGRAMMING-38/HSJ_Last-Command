using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class Block : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            GameObject player = collision.gameObject;
            player.transform.position = player.GetComponent<PlayerMovement>().CollideWithBlock(player.transform.position);

            for(int i = 0;  i < player.transform.childCount; ++i)
            {
                player.transform.GetChild(i).GetComponent<Head>().ManipulateLocation();
            }
        }
    }
}
