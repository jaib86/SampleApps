namespace FriendStorage.UI.ViewModel
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            this.NavigationViewModel = new NavigationViewModel();
        }

        public NavigationViewModel NavigationViewModel { get; private set; }

        internal void Load()
        {
            this.NavigationViewModel.Load();
        }
    }
}