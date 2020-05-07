using System;
using FluentValidation;
using Zdimk.Abstractions.Commands;

namespace Zdimk.Application.CommandHandlers.Validators
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(c => c.Email).EmailAddress();
            RuleFor(c => c.FullName).Length(2, 45);
            RuleFor(c => c.UserName).Length(4, 15);
            RuleFor(c => c.Password).Length(8, 25);
            RuleFor(c => c.BirthDate).InclusiveBetween(DateTime.Now - TimeSpan.FromDays(365 * 110),
                DateTime.Now - TimeSpan.FromDays(365 * 3));
        }
    }
}