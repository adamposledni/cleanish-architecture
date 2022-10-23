using Mapster;
using Moq;
using Onion.App.Data.Cache;
using Onion.App.Data.Database.Entities;
using Onion.App.Data.Database.Entities.Fields;
using Onion.App.Data.Database.Repositories;
using Onion.App.Logic.Users.Exceptions;
using Onion.App.Logic.Users.UseCases;
using System.Reflection;

namespace Onion.App.Logic.Test.Users;

public class GetUserTest
{
    public GetUserTest()
    {
        SetupMapper();
    }

    [Fact]
    public async Task GetUser_Found()
    {
        // Arrange
        var mockUserRepository = SetupCachedUserRepositoryWithGetById(GetRandomUser());
        GetUserRequestHandler handler = new GetUserRequestHandler(mockUserRepository);

        // Act
        GetUserRequest request = new GetUserRequest(Guid.NewGuid());
        var user = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(user != null);
    }

    [Fact]
    public async Task GetUser_NotFound()
    {
        // Arrange
        var mockUserRepository = SetupCachedUserRepositoryWithGetById(null);
        GetUserRequestHandler handler = new GetUserRequestHandler(mockUserRepository);

        // Act
        GetUserRequest request = new GetUserRequest(Guid.NewGuid());
        var userTask = handler.Handle(request, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<UserNotFoundException>(async () => await userTask);
    }

    private User GetRandomUser()
    {
        return new User()
        {
            Id = Guid.NewGuid(),
            Created = DateTime.UtcNow,
            Email = "admin@abc.com",
            GoogleSubjectId = Guid.NewGuid().ToString(),
            Role = UserRole.Admin,
            Version = Guid.NewGuid().ToByteArray(),
            PasswordHash = Guid.NewGuid().ToByteArray(),
            PasswordSalt = Guid.NewGuid().ToByteArray(),
            Updated = DateTime.UtcNow
        };
    }

    private void SetupMapper()
    {
        var mapperConfig = TypeAdapterConfig.GlobalSettings;
        mapperConfig.Scan(Assembly.GetAssembly(typeof(DependencyInjection)));
    }

    private Cached<IUserRepository> SetupCachedUserRepositoryWithGetById(User returnValue)
    {
        var mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository.Setup(m => m.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(() => returnValue);
        return new Cached<IUserRepository>(mockUserRepository.Object);
    }
}