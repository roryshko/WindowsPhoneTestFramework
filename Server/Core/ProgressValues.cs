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
