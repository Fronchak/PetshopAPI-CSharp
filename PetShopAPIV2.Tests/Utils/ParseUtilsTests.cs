using FluentAssertions;
using PetShopAPIV2.Exceptions;
using PetShopAPIV2.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShopAPIV2.Tests.Utils
{
    public class ParseUtilsTests
    {
        [Fact]
        public void ParsePathParamShouldReturnIntValueWhenStringIsAValidInteger()
        {
            int result = ParseUtils.ParsePathParam("12");

            result.Should().Be(12);
        }

        [Fact]
        public void ParsePathParamShouldThrowBadRequestExceptionWhenStringDoesNotRepresentAnInteger()
        {
            Action action = () => ParseUtils.ParsePathParam("A");

            action.Should().Throw<BadRequestException>()
                .WithMessage("Id must be a number");
        }
    }
}
