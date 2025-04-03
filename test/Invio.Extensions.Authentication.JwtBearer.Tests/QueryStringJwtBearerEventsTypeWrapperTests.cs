using System;
using System.Threading.Tasks;
using Invio.Xunit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Invio.Extensions.Authentication.JwtBearer {

    [UnitTest]
    public sealed class QueryStringJwtBearerEventsTypeWrapperTests : QueryStringJwtBearerEventsWrapperBaseTests {

        [Fact]
        public void Constructor_DefaultQueryStringParameterName_NullInner() {

            // Arrange

            Type innerType = null;

            // Act

            var exception = Record.Exception(
                () => new QueryStringJwtBearerEventsWrapper(innerType)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void Constructor_CustomQueryStringParameterName_NullInner() {

            // Arrange

            Type innerType = null;

            // Act

            var exception = Record.Exception(
                () => new QueryStringJwtBearerEventsWrapper(innerType, "myCustomQueryStringParameterName")
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void Constructor_CustomQueryStringParameterName_NullParameterName() {

            // Arrange

            var innerType = typeof(JwtBearerEvents);

            // Act

            var exception = Record.Exception(
                () => new QueryStringJwtBearerEventsWrapper(innerType, null)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Constructor_WithCustomQueryStringParameterName_InvalidParameterName(
            string queryStringParameterName) {

            // Arrange

            var innerType = typeof(JwtBearerEvents);

            // Act

            var exception = Record.Exception(
                () => new QueryStringJwtBearerEventsWrapper(innerType, queryStringParameterName)
            );

            // Assert

            Assert.IsType<ArgumentException>(exception);

            Assert.Equal(
                "The 'queryStringParameterName' cannot be null or whitespace." +
                Environment.NewLine + "Parameter name: queryStringParameterName",
                exception.Message
            );
        }

        [Fact]
        public async Task MessageReceived_NullServiceProvider() {

            // Arrange

            var inner = new JwtBearerEvents();
            var events = this.CreateJwtBearerEvents(inner);
            var context = new DefaultMessageReceivedContext();
            context.HttpContext.RequestServices = null;

            // Act

            var exception = await Record.ExceptionAsync(
                () => events.MessageReceived(context)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        protected override void SetupContext(BaseContext<JwtBearerOptions> context, 
            JwtBearerEvents inner) {

            context.HttpContext.RequestServices = CreateServiceProvider(inner);
        }

        protected override JwtBearerEvents CreateJwtBearerEvents(JwtBearerEvents inner) {
            return this.CreateQueryStringJwtBearerEvents(inner);
        }

        protected override QueryStringJwtBearerEventsWrapper CreateQueryStringJwtBearerEvents(
            JwtBearerEvents inner) {

            return new QueryStringJwtBearerEventsWrapper(typeof(JwtBearerEvents));
        }

        protected override QueryStringJwtBearerEventsWrapper CreateQueryStringJwtBearerEvents(
            JwtBearerEvents inner, string queryStringParameterName) {

            return new QueryStringJwtBearerEventsWrapper(typeof(JwtBearerEvents), queryStringParameterName);
        }

        private static IServiceProvider CreateServiceProvider(JwtBearerEvents inner) {

            var services = new ServiceCollection();
            services.AddSingleton(inner);
            return services.BuildServiceProvider();
        }

    }

}
