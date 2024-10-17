
using BreakInfinity;
using System;
using UnityEngine;

public static class CalculationHelper
{
    public static BigDouble CalculateCost(BigDouble bought, BigDouble baseCost, BigDouble costExponent)
    {
        BigDouble cost = BigDouble.Pow(costExponent, bought) * baseCost;
        return cost;
    }
}

