using System.Text.RegularExpressions;
using FluentValidation;
using Zdimk.Abstractions.Commands;

namespace Zdimk.Application.CommandHandlers.Validators
{
    public class CreateAlbumCommandValidator : AbstractValidator<CreateAlbumCommand>
    {
        public CreateAlbumCommandValidator()
        {
            RuleFor(a => a.Name).Length(1, 15);
            RuleFor(a => a.Description).Length(0, 50);
            RuleFor(a => a.CoverUrl).Must(o =>
                Regex.IsMatch(o, @"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$"));
        }
    }
}