﻿using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.ValidationRules.FluentValidation
{
    public class CarImageValidator : AbstractValidator<CarImage>
    {
        public CarImageValidator()
        {
            RuleFor(cı => cı.CarImageId).NotEmpty();
            RuleFor(cı => cı.CarId).NotEmpty();
        }
    }
}
