using FluentValidation;
using Forever.Api.DTOs.User;

namespace Forever.Api.Validators.User
{
    public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
    {
        public LoginRequestDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email must be a valid email address.");

            RuleFor(x => x.PashwordHash)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}