using FluentValidation;
using RestaurantAPI.Entities;
using RestaurantAPI.Models.Dtos;

namespace RestaurantAPI.Models.Validators
{
    public class UserCreateDtoValidator : AbstractValidator<UserCreateDto>
    {
        public UserCreateDtoValidator(RestaurantDbContext dbContext)
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .NotEmpty();

            RuleFor(x => x.Password)
                .MinimumLength(6)
                .NotEmpty();

            RuleFor(x => x.PasswordConfirm)
                .Equal(x => x.Password);

            RuleFor(x => x.Email)
                .Custom((value, context) =>
                {
                    if (dbContext.Users.Any(x => x.Email == value))
                        context.AddFailure("Email", "This Email is in use.");
                    
                });

        }
    }
}
