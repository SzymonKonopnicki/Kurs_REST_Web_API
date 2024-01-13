using FluentValidation;

namespace RestaurantAPI.Models.Validators
{
    public class RestaurantQueryValidator : AbstractValidator<RestaurantQuery>
    {
        public int[] allowedPageSize = new int[] { 5, 10, 15 };

        public RestaurantQueryValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).Custom((value, context) =>
            {
                if (!allowedPageSize.Contains(value))
                {
                    context.AddFailure("PageSize", $"PasgeSize must be {string.Join(",",allowedPageSize)}");
                }
            });
        }
    }
}
