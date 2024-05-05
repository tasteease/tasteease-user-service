using Fiap.TasteEase.Domain.DTOs;
using FluentAssertions;

namespace Fiap.TasteEase.Domain.Tests
{
    public class DTOsTests
    {

        [Fact]
        [Trait("DTOs", "AwsSettings")]
        public void AwsSettings_Should_Have_Correct_Properties()
        {
            // Arrange
            var region = "us-east-1";
            var userPoolId = "user-pool-id";
            var userPoolClientId = "user-pool-client-id";
            var userPoolClientSecret = "user-pool-client-secret";

            // Act
            var awsSettings = new AwsSettings
            {
                Region = region,
                UserPoolId = userPoolId,
                UserPoolClientId = userPoolClientId,
                UserPoolClientSecret = userPoolClientSecret
            };

            // Assert
            awsSettings.Region.Should().Be(region);
            awsSettings.UserPoolId.Should().Be(userPoolId);
            awsSettings.UserPoolClientId.Should().Be(userPoolClientId);
            awsSettings.UserPoolClientSecret.Should().Be(userPoolClientSecret);
        }
    }
}