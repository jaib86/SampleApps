using System;

namespace Library.API.Helpers
{
    public static class DateTimeOffsetExensions
    {
        public static int GetCurrentAge(this DateTimeOffset dateTimeOffset, DateTimeOffset? dateOfDeath)
        {
            var dateToCalculateTo = dateOfDeath != null ? dateOfDeath.Value.UtcDateTime : DateTime.UtcNow;

            int age = dateToCalculateTo.Year - dateTimeOffset.Year;

            if (dateToCalculateTo < dateTimeOffset.AddYears(age))
            {
                age--;
            }

            return age;
        }
    }
}