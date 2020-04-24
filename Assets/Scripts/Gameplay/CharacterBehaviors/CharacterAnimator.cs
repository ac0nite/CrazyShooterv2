using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator = null;

    public void Die()
    {
        _animator.SetTrigger("DeathTrigger");
    }
}
