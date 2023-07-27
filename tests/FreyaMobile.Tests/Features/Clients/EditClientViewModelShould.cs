using FreyaMobile.Features.Clients;
using Models.Core.Clients;
using Moq;
using NUnit.Framework;

namespace FreyaMobile.Tests.Features.Clients
{
    [TestFixture]
    public class EditClientViewModelShould
    {
        private EditClientViewModelBuilder builder;

        [SetUp]
        public void Setup()
        {
            builder = new EditClientViewModelBuilder();
        }

        [Test]
        public async Task EditClient_Expect_CurrentUser()
        {
            var client = new Client()
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Email = "email@test.com",
                LastName = "lastName",
            };

            var viewModel = builder
                .WithCurrentUser(client)
                .Build();

            await viewModel.OnAppearingAsync();
            Assert.Multiple(() =>
            {
                Assert.That(viewModel.UpdateUser.Name.Value, Is.EqualTo(client.Name));
                Assert.That(viewModel.UpdateUser.LastName.Value, Is.EqualTo(client.LastName));
            });
        }
        
        [Test]
        public async Task Verify_UpdateUser_When_Click_SaveUser()
        {
            var client = new Client()
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Email = "email@test.com",
                LastName = "lastName",
            };

            var viewModel = builder
                .WithCurrentUser(client)
                .Build();

            await viewModel.OnAppearingAsync();

            await viewModel.SaveUserCommand.ExecuteAsync();

            builder.EditClientService.Verify(x => x.UpdateUser(It.IsAny<UpdateUserValidatable>()), Times.Once);
        }
    }
}
