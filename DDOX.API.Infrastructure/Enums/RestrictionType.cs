using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Infrastructure.Enums
{
    public enum RestrictionType
    {
        MaxStringLength = 1,
        MinStringLength = 2,
        StringContains = 3,
        StringDontContains = 4,
        StringEqual = 5,
        StringNotEqual = 6,
        StringStartWith = 7,
        StringEndWith = 8,
        IsNumberInteger = 9,
        IsNumberDouble = 10,
        NumberIsLessThan = 11,
        NumberIsLessThanOrEqual = 12,
        NumberIsBiggerThan = 13,
        NumberIsBiggerThanOrEqual = 14,
        NumberEqual = 15,
        NumberDoNotEqual = 16,
        NumberIsBetween = 17,
        NumberIsNotBetween = 18,
        DateIsAfter = 19,
        DateIsAfterOrEqual = 20,
        DateIsEqual = 21,
        DateIsNotEqual = 22,
        DateIsBefore = 23,
        DateIsBeforeOrEqual = 24,
        DateIsBetween = 25,
        DateIsNotBetween = 26,
        CheckboxAtLeastChecked = 27,
        CheckboxExactlyChecked = 28,
        CheckboxAtMostChecked = 29,
        EmailType = 30,
        URLType = 31
    }
}
