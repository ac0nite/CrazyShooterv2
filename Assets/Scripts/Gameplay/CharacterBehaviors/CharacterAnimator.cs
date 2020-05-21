using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator = null;
    public Transform LeftHandIKTarget { get; set; }
    public Transform RightHandIKTarget { get; set; }

    public event Action EventStartFlyingGrenade;
    public event Action EventEndAnimation;
    public event Action EventOneShot; //используется в пулемёте

    public void Die()
    {
        _animator.SetLayerWeight(1,0f);
        _animator.SetTrigger("DeathTrigger");
    }
    
    public void SetAnimation(string animation)
    {
        _animator.SetTrigger(animation);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if(LeftHandIKTarget != null)
        {
            _animator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHandIKTarget.position);
           // _animator.SetIKRotation(AvatarIKGoal.LeftHand, LeftHandIKTarget.rotation);
            _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
            //_animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
        }
        
        // if(RightHandIKTarget != null)
        // {
        //     _animator.SetIKPosition(AvatarIKGoal.RightHand, RightHandIKTarget.position);
        //     _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);   
        // }
    }
    
    private void StartFlyingGrenade()
    {
        EventStartFlyingGrenade?.Invoke();
    }

    private void EndAnimation()
    {
        Debug.Log("EndAnimation");
        EventEndAnimation?.Invoke();
        // var character = gameObject.GetComponent<Character>();
        // if (character != null)
        // {
        //     //Debug.Log($"Character {character.CurrentWeapon.name} CanUse = true");
        //     character.CurrentWeapon.CanUse = true;
        // }
    }

    private void OneShot()
    {
        Debug.Log("OneShot");
        EventOneShot?.Invoke();

        // var character = gameObject.GetComponent<Character>();
        // if (character != null)
        // {
        //     character.CurrentWeapon.Shoot(character.CharacterMovemevt.StateLocomotion);
        // }
    }
}
