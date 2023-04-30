using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTurn : StateMachineBehaviour
{
    private SpriteRenderer _renderer;
    private BossPattern _test;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_renderer == null)
        {
            _renderer = animator.GetComponent<SpriteRenderer>();
        }
        if(_test == null)
        {
            _test = animator.GetComponent<BossPattern>();
        }

        if(animator.transform.parent.position.x < 0)
        {
            _renderer.flipX = false;
        }
        else
        {
            _renderer.flipX = true;
        }
        _test.bossCollider.enabled = false;
    }
}
