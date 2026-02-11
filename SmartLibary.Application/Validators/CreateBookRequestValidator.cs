using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation; // Това идва от пакета, който инсталира
using SmartLibrary.Application.DTOs; // Тук е DTO-то, което валидираме

namespace SmartLibrary.Application.Validators
{
    // Валидатор за създаване на книга
    public class CreateBookRequestValidator : AbstractValidator<CreateBookRequest>
    {
        public CreateBookRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Заглавието е задължително.");

            RuleFor(x => x.Author)
                .NotEmpty().WithMessage("Авторът е задължителен.")
                .Length(2, 50).WithMessage("Името на автора трябва да е между 2 и 50 символа.");
        }
    }
}
