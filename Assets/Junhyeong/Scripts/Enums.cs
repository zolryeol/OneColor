using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ETileType
{
    None, Red, Orange, Yellow, Green, Blue, Indigo, Purple, Black, Move, InfectionMother, Infection,
    Start, End, Any, Random,
}

public enum EBackGroundType
{
    Spring, Summer, Fall, Winter,
}

public enum EStageRange
{
    Spring = 19, Summer = 39, Fall = 49, Winter = 69,
}
public enum EClearStar
{
    Zero, One, Two, Three,
}

static class CBoardMaxSize
{
    public const int MaxHeight = 7;
    public const int MaxWidth = 5; // km per sec.
}
public enum ESoundType
{
    TabEffect,
    Bgm,
    ClearEffect,
}

