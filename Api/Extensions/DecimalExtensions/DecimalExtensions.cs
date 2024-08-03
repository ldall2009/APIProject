namespace Api.Extensions.DecimalExtensions
{
    public static class DecimalExtensions
    {
        public static decimal RoundToNearestHundreths(this decimal value)
        {
            return Math.Round(value, 2, MidpointRounding.AwayFromZero);
        }
    }
}
