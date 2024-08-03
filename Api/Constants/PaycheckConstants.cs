namespace Api.Constants
{
    public static class PaycheckConstants
    {
        // I noticed the coding question document provided values in different units (years vs months).
        // In an attempt to simplify calculations & improve code readability, I decided to use the year values for relevant constants.

        public const int PaychecksPerYear = 26;
        public const decimal BaseBenefitsCostPerYear = 12_000m;
        public const decimal DependentsBenefitsCostPerYear = 7_200m;
        public const int AgeThresholdForAdditionalCost = 50;
        public const decimal DependentsAboveAgeThresholdBenefitsCostPerYear = 2_400m;
        public const decimal SalaryThresholdForAdditionalBenefits = 80_000m;
        public const decimal HighSalaryBenefitsMultiplier = 0.02m;

        public const string BaseBenefitsCostLabel = "Base Benefits Cost";
        public const string DependentsBenefitsCostLabel = "Benefits Cost for Number of Dependents";
        public const string DependentsAboveAgeThresholdBenefitsCostLabel = "Benefits Cost for Number of Dependents Above Age Threshold";
        public const string HighSalaryBenefitsCostLabel = "High Salary Benefits Cost";
    }
}
