using FluentValidation;
using Zdimk.Abstractions.Commands;

namespace Zdimk.Application.CommandHandlers.Validators
{
    public class CreatePictureValidator : AbstractValidator<CreatePictureCommand>
    {
        public CreatePictureValidator()
        {
            RuleFor(p => p.Name).Length(1, 15);
            RuleFor(p => p.Description).Length(0, 50);
            RuleFor(p => p.PictureFile).Must(f => f.ContentType.Contains("image") && f.Length <= 10 << 20);
        }
    }
}