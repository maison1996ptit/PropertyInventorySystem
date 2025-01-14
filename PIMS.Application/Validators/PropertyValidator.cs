using FluentValidation;
using PIMS.Domain.Entities;

namespace PIMS.Application.Validators
{
    public  class PropertyValidator : AbstractValidator<Property>
    {
        public PropertyValidator() {
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");  

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required."); 

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to 0."); 

        }
    }
}
