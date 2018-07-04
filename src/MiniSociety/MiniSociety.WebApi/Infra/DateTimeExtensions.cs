using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSociety.WebApi.Infra
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
