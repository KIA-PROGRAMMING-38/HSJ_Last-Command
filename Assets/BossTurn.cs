using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTurn : StateMachineBehaviour
{
    private SpriteRenderer _renderer;
    private Test _test;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_renderer == null)
        {
            _renderer = animator.GetComponent<SpriteRenderer>();
        }
        if(_test == null)
        {
            _test = animator.GetComponent<Test>();
        }
        _renderer.flipX = _renderer.flipX ^ true;
        _test._collider.enabled = false;
    }
}
