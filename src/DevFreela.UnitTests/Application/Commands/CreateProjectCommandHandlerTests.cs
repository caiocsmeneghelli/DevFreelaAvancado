using DevFreela.Application.Commands.CreateProject;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using DevFreela.Infrastructure.Persistence;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DevFreela.UnitTests.Application.Commands
{
    public class CreateProjectCommandHandlerTests
    {
        [Fact]
        public async Task InputDataIsOk_Executed_ReturnProjectId()
        {
            // Arrange
            var unitOfWork = new Mock<IUnitOfWork>();
            var projectRepository = new Mock<IProjectRepository>();
            var skillRepository = new Mock<ISkillRepository>();

            unitOfWork.SetupGet(uow => uow.Projects).Returns(projectRepository.Object);
            unitOfWork.SetupGet(uow => uow.Skills).Returns(skillRepository.Object);

            var createProjectCommand = new CreateProjectCommand
            {
                Title = "Titulo de Teste",
                Description = "Uma descrição Daora",
                TotalCost = 50000,
                IdClient = 1,
                IdFreelancer = 2
            };

            var createProjectCommandHandler = new CreateProjectCommandHandler(unitOfWork.Object);

            // Act
            var id = await createProjectCommandHandler.Handle(createProjectCommand, new CancellationToken());

            // Assert
            Assert.True(id >= 0);

            projectRepository.Verify(pr => pr.AddAsync(It.IsAny<Project>()), Times.Once);
        }
    }
}
