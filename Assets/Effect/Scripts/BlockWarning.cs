using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockWarning : Warning
{
    protected override void AfterWarning()
    {
        gameObject.SetActive(false);
        transform.parent.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        transform.parent.gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }
}
