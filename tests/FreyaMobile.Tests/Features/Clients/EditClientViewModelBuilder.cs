using FreyaMobile.Core.Services;
using FreyaMobile.Features.Clients;
using FreyaMobile.Features.Clients.Services;
using FreyaMobile.Services.Interfaces;
using Models.Core.Clients;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreyaMobile.Tests.Features.Clients
{
    internal class EditClientViewModelBuilder
    {
        private Mock<ICurrentUserService> CurrentUserService;
        public Mock<IEditClientService> EditClientService;
        private Mock<DialogService> DialogService;

        public EditClientViewModelBuilder()
        {
            DialogService = new Mock<DialogService>();
            CurrentUserService = new Mock<ICurrentUserService>();
            EditClientService = new Mock<IEditClientService>();
        }

        internal EditClientViewModel Build()
        {
            return new EditClientViewModel(
                CurrentUserService.Object,
                EditClientService.Object,
                DialogService.Object);
        }

        internal EditClientViewModelBuilder WithCurrentUser(Client user)
        {
            CurrentUserService.Setup(x => x.GetCurrentUserAsync()).ReturnsAsync(user);
            return this;
        }
    }
}
