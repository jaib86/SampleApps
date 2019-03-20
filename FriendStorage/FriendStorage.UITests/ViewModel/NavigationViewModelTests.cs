using FriendStorage.UI.ViewModel;
using Xunit;

namespace FriendStorage.UITests.ViewModel
{
    public class NavigationViewModelTests
    {
        [Fact]
        public void ShouldLoadFriends()
        {
            // Arrange
            var viewModel = new NavigationViewModel();

            // Act
            viewModel.Load();

            // Assert
            Assert.Equal(2, viewModel.Friends.Count);
        }
    }
}