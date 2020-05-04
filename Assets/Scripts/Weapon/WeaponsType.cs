using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolygonCrazyShooter
{
    public enum WeaponType
    {
        undefined = 0,
        HandGun = 1,
        Heavy = 2,
        Infantry = 3,
        Knife = 4,
        Grenade = 5
    }

    public static class WeaponTypeExtensions
    {
        public static string GetIdAnimationTriggerName(this WeaponType _this)
        {
            if(_this == WeaponType.undefined)
                throw new Exception("Type weapon UNDEFINED!");
            
            return $"{_this.ToString()}_Trigger";
        }


        
        // public static string GetAttackingAnimationTriggerName(this WeaponType _this)
        // {
        //     if(_this == WeaponType.undefined)
        //         throw new Exception("Type weapon UNDEFINED!");
        //     
        //     return $"{_this.ToString()}AttackingTrigger";
        // }
    }
}