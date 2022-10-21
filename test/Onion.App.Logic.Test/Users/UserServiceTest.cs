using Moq;
using Onion.App.Data.Database.Entities;
using Onion.App.Logic.Users.Models;
using Onion.Shared.Mapper;

namespace Onion.App.Logic.Test.Users;

// TODO: Unit tests
public class UserServiceTest
{
    public UserServiceTest()
    {

    }

    [Fact]
    public async Task GetById_Found()
    {
        //// Arrange

        //var mockMapper = new Mock<IObjectMapper>();
        //mockMapper.Setup(m => m.Map<UserRes>(It.IsAny<User>())).Returns<User>(user =>
        //{
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
