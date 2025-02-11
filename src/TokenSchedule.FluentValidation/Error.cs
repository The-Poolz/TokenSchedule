﻿using Net.Utils.ErrorHandler.Attributes;

namespace TokenSchedule.FluentValidation
{
    public enum Error
    {
        [Error("Schedule must contain 1 or more elements.")]
        SCHEDULE_IS_EMPTY,
        [Error("The sum of the ratios must be 1.")]
        SUM_OF_RATIOS_MUST_BE_ONE,
        [Error("The first element must be the TGE (Token Generation Event).")]
        FIRST_ELEMENT_MUST_BE_TGE,
        [Error("Ratio must be greater than or equal to 1e-18.")]
        MINIMUM_RATIO_1E_MINUS_18,
        [Error("End time must be greater than start time.")]
        END_TIME_MUST_BE_GREATER_THAN_START_TIME,
    }
}
