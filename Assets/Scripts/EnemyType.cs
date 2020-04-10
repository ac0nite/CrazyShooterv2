using System;

namespace PolygonCrazyShooter
{
    [Flags]
    public enum EnemyType
    {
        Undefined   = 0,
        Weak        = 1 << 0,
        Fast        = 1 << 1,
        Strong      = 1 << 2
    }
}
