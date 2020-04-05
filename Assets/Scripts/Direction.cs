using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PolygonCrazyShooter
    {

    //public enum Direction
    //{
    //    Undefined = 0,
    //    North = 1,
    //    NorthEast = 2,
    //    East = 3,
    //    SouthEast = 4,
    //    South = 5,
    //    SouthWest = 6,
    //    West = 7,
    //    NortWest = 8
    //}



    public enum Direction
    {
        Undefined = 0,
        Up = 1,
        UpRight = 2,
        Right = 3,
        RightDown = 4,
        Down = 5,
        DownLeft = 6,
        Left = 7,
        LeftUp = 8
    }
    public static class DirectionExtensions
    {
        public static Direction GetOpposite(this Direction _this)
        {
            //Array arr = Enum.GetValues(typeof(Direction));
            //Array arr2 = Array.CreateInstance(typeof(Direction), arr.Length-1);
            //Array.Copy(arr, 1, arr2, 0, arr.Length - 1);
            //int i = Array.IndexOf(arr2, _this);
            //Direction d = (Direction)arr2.GetValue((i + 4) % 8);
            //return d;
            
            var arr = Enum.GetValues(typeof(Direction));
            var underValue = (int)_this;
            Direction d = (Direction)(((--underValue + 4) % 8) + 1);
            return d;
        }
    }
}
