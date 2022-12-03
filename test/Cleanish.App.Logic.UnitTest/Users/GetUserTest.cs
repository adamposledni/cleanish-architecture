using Mapster;
using Moq;
using Cleanish.App.Data.Cache;
using Cleanish.App.Data.Database.Entities;
using Cleanish.App.Data.Database.Entities.Fields;
using Cleanish.App.Data.Database.Repositories;
using Cleanish.App.Logic.Users.Exceptions;
using Cleanish.App.Logic.Users.UseCases;
using System.Reflection;

namespace Cleanish.App.Logic.Test.Users;

public class GetUserTest
{
    public GetUserTest()
    {
        SetupMapper();
    }

    [Fact]
    public async Task Handle_UserFound()
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
    public async Task Handle_UserNotFound()
    {
        // Arrange
        var mockUserRepository = SetupCachedUserRepositoryWithGetById(null);
        GetUserRequestHandler handler = new GetUserRequestHandler(mockUserRepository);

        // Act
        async Task act()
        {
            GetUserRequest request = new GetUserRequest(Guid.NewGuid());
            var user = await handler.Handle(request, CancellationToken.None);
        }

        // Assert
        await Assert.ThrowsAsync<UserNotFoundException>(act);
    }

    private User GetRandomUser()
    {
        return new User()
        {
            Id = Guid.NewGuid(),
            Created = DateTime.UtcNow,
            Email = "admin@abc.com",
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