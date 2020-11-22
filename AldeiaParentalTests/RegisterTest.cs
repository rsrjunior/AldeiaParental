using System;
using Xunit;
using AldeiaParental.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AldeiaParentalTests
{
    public class RegisterTest
    {
        [Fact]
        public void FullNameRequired()
        {
            Assert.True(false);
        }
        [Fact]
        public void EmailRequired()
        {
            Assert.True(false);
        }
        [Fact]
        public void UserProfileRequired()
        {
            Assert.True(false);
        }
        [Fact]
        public void PasswordRequired()
        {
            Assert.True(false);
        }
        [Theory]
        [InlineData("FirtName", "LastName", "mail@mail.com", "password")]
        public void RegisterShouldPass(string firstName, string lastName, string email, string password)
        {
            //Arrange
            CheckPropertyValidation chk = new CheckPropertyValidation();
            AldeiaParentalUser nUser = new AldeiaParentalUser
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PasswordHash = password
            };
            //Act
            IList<ValidationResult> errors = chk.Validate(nUser);
            //Assert
            Assert.Equal(0, errors.Count);
        }
        [Theory]
        [InlineData("FirstName", "LastName", "mail@mail.com", null)]
        [InlineData("FirstName", "LastName", null, "password")]
        [InlineData("FirstName", "LastName", null, null)]
        [InlineData("FirstName", null, null, null)]
        [InlineData(null, "LastName", "mail@mail.com", "password")]
        [InlineData(null, "LastName", null, "password")]
        [InlineData(null, null, null, "password")]
        [InlineData(null, "LastName", null, null)]
        [InlineData(null, null, "mail@mail.com", "password")]
        [InlineData(null, null, "mail@mail.com", null)]
        [InlineData(null, null, null, "password")]
        [InlineData(null, null, null, null)]
        public void RegisterShouldFail(string firstName, string lastName, string email, string password)
        {
            //Arrange
            CheckPropertyValidation chk = new CheckPropertyValidation();
            AldeiaParentalUser nUser = new AldeiaParentalUser
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PasswordHash = password
            };
            //Act
            IList<ValidationResult> errors = chk.Validate(nUser);
            //Assert
            Assert.NotEqual(0, errors.Count);
        }

    }

}
