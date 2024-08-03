namespace Api.Helpers
{
    public static class AgeHelper
    {
        public static int GetAge(DateTime dateOfBirth)
        {
            // ASSUMPTION: dateOfBirth is stored in the Utc format (thus using DateTime.UtcNow is okay).
            DateTime currentUtcDateTime = DateTime.UtcNow;
            int age = currentUtcDateTime.Year - dateOfBirth.Year;

            if (currentUtcDateTime.Date < dateOfBirth.Date)
            {
                age--;
            }

            // ASSUMPTION: Create/Update service methods for any objects that could call this method should be responsible for
            // validating the dateOfBirth field (no date in future).
            return age;
        }
    }
}
