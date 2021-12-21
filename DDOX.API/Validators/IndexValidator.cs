using DDOX.API.Dtos;
using DDOX.API.Repository.Data;
using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestrictionLibrary;

namespace DDOX.API.Validators
{
    public class IndexValidator<T, TElement> : PropertyValidator<T, TElement> where TElement : class, IEntityIndices
    {
        public override string Name => "IndexValidator";

        private readonly DataContext _dataContext;
        public IndexValidator(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public override bool IsValid(ValidationContext<T> context, TElement element)
        {
            var fetchedIndexRestrictions = _dataContext.IndexRestrictions.Where(x => x.IndexId == element.IndexId).ToList();
            foreach (var restriction in fetchedIndexRestrictions)
            {
                var isValid = Validator.CheckRestriction(new Restriction
                {
                    Condition = (RestrictionType)restriction.RestrictionId,
                    RightOperand = restriction.Value,
                    ExtraOperand = restriction.SecondValueOption
                }, element.Value);

                if (!isValid)
                    return false;
            }
            return true;
        }

        protected override string GetDefaultMessageTemplate(string errorCode)
       => "{PropertyName} Invalidate its Restrictions.";

    }
}
