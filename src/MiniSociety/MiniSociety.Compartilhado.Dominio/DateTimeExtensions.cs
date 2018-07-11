
namespace System
{
    public static class DateTimeExtensions
    {
        public static int GetAge(this DateTime date)
        {
            var diferenca = DateTime.Today - date;
            return (new DateTime() + diferenca).AddYears(-1).AddDays(-1).Year;
        }
    }
}
