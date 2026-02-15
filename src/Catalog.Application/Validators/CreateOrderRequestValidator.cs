using FluentValidation;

namespace Catalog.Application.Validators;
//public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
//{
//    public CreateOrderRequestValidator()
//    {
//        RuleFor(x => x.CustomerId)
//            .NotEmpty().WithMessage("CustomerId is required.");

//        RuleFor(x => x.Items)
//            .NotEmpty().WithMessage("At least one item is required.");

//        RuleForEach(x => x.Items)
//            .ChildRules(item =>
//            {
//                item.RuleFor(i => i.ProductId).NotEmpty();
//                item.RuleFor(i => i.Quantity).GreaterThan(0);
//            });
//    }
//}
