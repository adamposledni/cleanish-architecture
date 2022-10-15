using Moq;
using Onion.App.Data.Cache;
using Onion.App.Data.Database;
using Onion.App.Data.Database.Entities;
using Onion.App.Data.Database.Repositories;
using Onion.App.Data.Security;
using Onion.App.Logic.Security;
using Onion.App.Logic.Users;
using Onion.App.Logic.Users.Models;
using Onion.Shared.Mapper;

namespace Onion.App.Logic.Test.Users;

// TODO: Unit tests
// TODO: Integ tests
public class UserServiceTest
{
    public UserServiceTest()
    {
        
    }

    [Fact]
    public async Task Test1()
    {
        //// Arrange

        //var mockMapper = new Mock<IObjectMapper>();
        //mockMapper.Setup(m => m.Map<UserRes>(It.IsAny<User>())).Returns<User>(user => {
        //    if (user == null) return null;
        //    return new UserRes();
        //});

        //var mockPasswordProvider = new Mock<IPasswordProvider>();
        //var mockSecurityProvider = new Mock<ISecurityContextProvider>();
        //var mockGoogleAuthProvider = new Mock<IGoogleAuthProvider>();

        //var mockUserRepository = new Mock<IUserRepository>();
        //mockUserRepository
        //    .Setup(m => m.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(() => null);

        //var mockDatabaseRepositoryManager = new Mock<IDatabaseRepositoryManager>();
        //mockDatabaseRepositoryManager
        //    .Setup(m => m.GetRepository<IUserRepository, User>(It.IsAny<CacheStrategy>())).Returns(mockUserRepository.Object);

        //IUserService userService = new UserService(
        //    mockMapper.Object, 
        //    mockPasswordProvider.Object, 
        //    mockSecurityProvider.Object, 
        //    mockGoogleAuthProvider.Object,
        //    mockDatabaseRepositoryManager.Object
        // );

        //// Act
        //var user = await userService.GetAsync(Guid.NewGuid());

        //// Assert
        //Assert.True(user != null);
    }
}
