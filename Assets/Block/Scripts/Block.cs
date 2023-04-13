using Enum;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class Block : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")
            || collision.gameObject.layer == LayerMask.NameToLayer("Invincible"))
        {
            GameObject player = collision.gameObject;
            bool isOnBlock = true;
            int repeatFrame = 0;
            while(isOnBlock)
            {
                Collider2D box = Physics2D.OverlapCircle(player.transform.position, player.GetComponent<CircleCollider2D>().radius, LayerMask.GetMask("Block"));
                if(box != null)
                {
                    player.GetComponent<PlayerMovement>().CollideWithBlock();
                    ++repeatFrame;
                }
                else
                {
                    isOnBlock = false;
                }
            }
            for(int i = 0;  i < player.transform.childCount; ++i)
            {
                player.transform.GetChild(i).GetComponent<Head>().ManipulateLocation(repeatFrame);
            }
        }
    }
}
