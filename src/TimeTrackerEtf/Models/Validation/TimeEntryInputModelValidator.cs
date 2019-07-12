using System;
using FluentValidation;

namespace TimeTrackerEtf.Models.Validation
{
    public class TimeEntryInputModelValidator
        : AbstractValidator<TimeEntryInputModel>
    {
        public TimeEntryInputModelValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0);

            RuleFor(x => x.ProjectId)
                .GreaterThan(0);

            RuleFor(x => x.EntryDate)
                .GreaterThan(new DateTime(2019, 1, 1))
                .LessThan(new DateTime(2100, 1, 1));

            RuleFor(x => x.Hours)
                .InclusiveBetween(1, 24);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(10_000);
        }
    }
}
