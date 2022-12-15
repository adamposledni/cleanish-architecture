using Mapster;
using Moq;
using Cleanish.App.Data.Cache;
using Cleanish.App.Data.Database.Entities;
using Cleanish.App.Data.Database.Entities.Fields;
using Cleanish.App.Data.Database.Repositories;
using Cleanish.App.Logic.Users.UseCases;
using System.Reflection;
using Cleanish.App.Logic.Common.Security;
using Cleanish.App.Logic.Users.Exceptions;

namespace Cleanish.App.Logic.Test.Users;

public class GetCurrentUserTest
{
    public GetCurrentUserTest()
    {
        SetupMapper();
    }

    [Fact]
    public async Task Handle_UserFound()
    {
        // Arrange
        Guid requestedUserId = Guid.NewGuid();
        var returnedUser = GetRandomUser(requestedUserId);
        var mockUserRepository = SetupCachedUserRepositoryWithGetById(requestedUserId, returnedUser);
        var mockSecurityContextProvider = SetupSecurityContextProviderWithGetSubjectId(requestedUserId);
        GetCurrentUserRequestHandler handler = new(mockUserRepository, mockSecurityContextProvider);

        // Act
        GetCurrentUserRequest request = new();
        var user = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(user != null);
        Assert.True(user.Id == requestedUserId);
    }

    [Fact]
    public async Task Handle_UserNotFound()
    {
        // Arrange
        Guid requestedUserId = Guid.NewGuid();
        var mockUserRepository = SetupCachedUserRepositoryWithGetById(requestedUserId, null);
        var mockSecurityContextProvider = SetupSecurityContextProviderWithGetSubjectId(requestedUserId);
        GetCurrentUserRequestHandler handler = new(mockUserRepository, mockSecurityContextProvider);

        // Act
        async Task act() 
        {
            GetCurrentUserRequest request = new();
            _ = await handler.Handle(request, CancellationToken.None);
        }

        // Assert
        await Assert.ThrowsAsync<UserNotFoundException>(act);
    }

    private User GetRandomUser(Guid id)
    {
        return new User()
        {
            Id = id,
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

    private Cached<IUserRepository> SetupCachedUserRepositoryWithGetById(Guid requestedId, User returnValue)
    {
        var mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository
            .Setup(m => m.GetByIdAsync(It.Is<Guid>(p => p == requestedId)))
                .ReturnsAsync(() => returnValue);
        return new Cached<IUserRepository>(mockUserRepository.Object);
    }

    private ISecurityContextProvider SetupSecurityContextProviderWithGetSubjectId(Guid returnValue)
    {
        var mockSecurityContextProvider = new Mock<ISecurityContextProvider>();
        mockSecurityContextProvider
            .Setup(m => m.GetSubjectId())
                .Returns(returnValue);

        return mockSecurityContextProvider.Object;
    }
}