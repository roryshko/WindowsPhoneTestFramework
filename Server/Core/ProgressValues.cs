//  ----------------------------------------------------------------------
//  <copyright file="ProgressValues.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

namespace WindowsPhoneTestFramework.Server.Core
{
    /// <summary>
    /// Passes the min, max and current values for a progress bar between the client and the tests
    /// </summary>
    public struct ProgressValues
    {
        public ProgressValues(
            double min,
            double max,
            double current)
        {
            Min = min;
            Max = max;
            Current = current;
        }

        public double Min;
        public double Max;
        public double Current;
    }
}