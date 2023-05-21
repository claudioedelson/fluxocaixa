﻿using FluentValidation;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Categories;
using FluxoDeCaixa.Shared.Extensions;

namespace FluxoCaixa.Tests.UnitTests.Shared.Extensions
{
    [UnitTest]
    public class FluentValidationExtensionsTests
    {
        [Theory]
        [InlineData("ma@hostname.com")]
        [InlineData("ma@hostname.comcom")]
        [InlineData("MA@hostname.coMCom")]
        [InlineData("MA@HOSTNAME.COM")]
        [InlineData("m.a@hostname.co")]
        [InlineData("m_a1a@hostname.com")]
        [InlineData("ma-a@hostname.com")]
        [InlineData("ma-a@hostname.com.edu")]
        [InlineData("ma-a.aa@hostname.com.edu")]
        [InlineData("ma.h.saraf.onemore@hostname.com.edu")]
        [InlineData("ma12@hostname.com")]
        [InlineData("12@hostname.com")]
        public void Should_ReturnsSuccess_WhenEmailIsValid(string emailAddress)
        {
            // Arrange
            var validator = CreateValidator();

            // Act
            var result = validator.TestValidate(emailAddress);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("")] // Empty
        [InlineData(" ")] // Whitespaces
        [InlineData("Abc.example.com")] // No `@`
        [InlineData("A@b@c@example.com")] // multiple `@`
        [InlineData("ma...ma@jjf.co")] // continuous multiple dots in name
        [InlineData("ma@jjf.c")] // only 1 char in extension
        [InlineData("ma@jjf..com")] // continuous multiple dots in domain
        [InlineData("ma@@jjf.com")] // continuous multiple `@`
        [InlineData("@majjf.com")] // nothing before `@`
        [InlineData("ma.@jjf.com")] // nothing after `.`
        [InlineData("ma_@jjf.com")] // nothing after `_`
        [InlineData("ma_@jjf")] // no domain extension
        [InlineData("ma_@jjf.")] // nothing after `_` and .
        [InlineData("ma@jjf.")] // nothing after `.`
        public void Should_ReturnsValidationError_WhenEmailIsInvalid(string emailAddress)
        {
            // Arrange
            var validator = CreateValidator();

            // Act
            var result = validator.TestValidate(emailAddress);

            // Assert
            result.ShouldHaveAnyValidationError();
        }

        private static IValidator<string> CreateValidator()
        {
            var validator = new InlineValidator<string>();
            validator.RuleFor(address => address).IsValidEmailAddress();
            return validator;
        }
    }
}
