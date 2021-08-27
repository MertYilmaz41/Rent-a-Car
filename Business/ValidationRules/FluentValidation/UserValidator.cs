﻿using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.ValidationRules.FluentValidation
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(u => u.FirstName).NotEmpty();
            RuleFor(u => u.FirstName).NotNull();
            RuleFor(u => u.FirstName).MinimumLength(2);
            RuleFor(u => u.FirstName).MaximumLength(20);

            RuleFor(u => u.LastName).NotEmpty();
            RuleFor(u => u.LastName).NotNull();
            RuleFor(u => u.LastName).MinimumLength(2);
            RuleFor(u => u.LastName).MaximumLength(30);

            RuleFor(u => u.Password).NotEmpty();
            RuleFor(u => u.Password).NotNull();


            RuleFor(u => u.Email).NotEmpty();
            RuleFor(u => u.Email).NotNull();
            RuleFor(u => u.Email).MinimumLength(2);
            RuleFor(u => u.Email).MaximumLength(50);

        }
    }
}