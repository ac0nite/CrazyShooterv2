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
        Grenade = 5,
        Medicine = 6
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
                return (9f, 9f);
            
            if(_this == WeaponType.HandGun)
                return (8.5f, 8f);

            if(_this == WeaponType.Infantry)
                return (8f, 7f);
            
            if(_this == WeaponType.Heavy)
                return (7f, 6f);
            
            return (0f, 0f);
        }
        
        public static float GetScore(this WeaponType _this, float distance = 0f)
        {
            if (_this == WeaponType.undefined || _this == WeaponType.Medicine)
                return 0f;

            if (_this == WeaponType.Knife)
                return 25 * 20;

            if (_this == WeaponType.HandGun)
                return 15 * distance;

            if(_this == WeaponType.Infantry)
                return 11 * distance;
            
            if(_this == WeaponType.Heavy)
                return 13 * distance;
            
            if (_this == WeaponType.Grenade)
                return 20 * 20;
            
            return 0f;
        }
    }
}