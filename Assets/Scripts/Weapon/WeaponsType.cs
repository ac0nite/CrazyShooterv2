using System;
using System.Collections;
using System.Collections.Generic;
using PolygonCrazyShooter;
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

        public static (float, float) GetSpeed(this WeaponType _this)
        {
            if(_this == WeaponType.undefined || _this == WeaponType.Grenade)
                throw new Exception("Type weapon UNDEFINED OR GRENADE!");

            if (_this == WeaponType.Knife)
                return (6f, 4f);
            
            if(_this == WeaponType.HandGun)
                return (5.5f, 4.5f);

            if(_this == WeaponType.Infantry)
                return (4f, 3f);
            
            if(_this == WeaponType.Heavy)
                return (3.5f, 2.5f);
            
            return (0f, 0f);
        }
    }
}