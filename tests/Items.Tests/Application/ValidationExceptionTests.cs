using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentValidation.Results;
using Items.Application.Exceptions;
using Xunit;

namespace Items.Tests.Application;

public class ValidationExceptionTests
{
    [Fact]
    public void DefaultConstructor_ShouldCreatesAnEmptyErrorDictionary()
    {
        var actual = new ValidationException().Errors;

        actual.Keys.Should().BeEquivalentTo(Array.Empty<string>());
    }

    [Fact]
    public void SingleValidationFailure_ShouldCreatesASingleElementErrorDictionary()
    {
        var failures = new List<ValidationFailure>
            {
                new("Age", "must be over 18"),
            };

        var actual = new ValidationException(failures).Errors;

        actual.Keys.Should().BeEquivalentTo(new string[] { "Age" });
        actual["Age"].Should().BeEquivalentTo(new string[] { "must be over 18" });
    }

    [Fact]
    public void MultipleValidationFailure_ForMultipleProperties_ShouldCreatesAMultipleElementErrorDictionary_EachWithMultipleValues()
    {
        var failures = new List<ValidationFailure>
            {
                new ("Age", "must be 18 or older"),
                new ("Age", "must be 25 or younger"),
                new ("Password", "must contain at least 8 characters"),
                new ("Password", "must contain a digit"),
                new ("Password", "must contain upper case letter"),
                new ("Password", "must contain lower case letter"),
            };

        var actual = new ValidationException(failures).Errors;

        actual.Keys.Should().BeEquivalentTo(new string[] { "Password", "Age" });

        actual["Age"].Should().BeEquivalentTo(new string[]
        {
                "must be 25 or younger",
                "must be 18 or older",
        });

        actual["Password"].Should().BeEquivalentTo(new string[]
        {
                "must contain lower case letter",
                "must contain upper case letter",
                "must contain at least 8 characters",
                "must contain a digit",
        });
    }
}