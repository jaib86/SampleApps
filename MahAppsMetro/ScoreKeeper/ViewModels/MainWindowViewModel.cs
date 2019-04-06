using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ScoreKeeper.Model;
using ScoreKeeper.Views;

namespace ScoreKeeper.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        private readonly IDialogService dialogService;
        private EditMatchViewModel editMatchViewModel;
        private int selectedTabIndex;
        private bool isSettingsFlyoutOpen;

        public ObservableCollection<string> AllPlayers { get; private set; }
        public RelayCommand NewMatch { get; private set; }
        public RelayCommand Exit { get; private set; }
        public RelayCommand Publish { get; private set; }
        public RelayCommand Settings { get; private set; }

        public StatsViewModel StatsViewModel { get; set; }
        public MatchesViewModel MatchesViewModel { get; set; }
        public SettingsViewModel SettingsViewModel { get; set; }

        public EditMatchViewModel EditMatchViewModel
        {
            get { return editMatchViewModel; }
            set
            {
                if (Equals(value, editMatchViewModel)) return;
                editMatchViewModel = value;
                OnPropertyChanged();
            }
        }

        public bool IsSettingsFlyoutOpen
        {
            get { return isSettingsFlyoutOpen; }
            set
            {
                if (value.Equals(isSettingsFlyoutOpen)) return;
                isSettingsFlyoutOpen = value;
                OnPropertyChanged();
            }
        }

        public int SelectedTabIndex
        {
            get { return selectedTabIndex; }
            set
            {
                if (value == selectedTabIndex) return;
                selectedTabIndex = value;
                OnPropertyChanged();
            }
        }

        public MainWindowViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
            Initializate();
            NewMatch = new RelayCommand(_ => EditMatch(null));
            Exit = new RelayCommand(_ => Application.Current.MainWindow.Close());
            Publish = new RelayCommand(_ => OnPublishCommand());
            Settings = new RelayCommand(_ => OnSettingsCommand());
        }

        private void OnSettingsCommand()
        {
            IsSettingsFlyoutOpen = true;
        }

        private async void OnPublishCommand()
        {
            var controller = await dialogService.ShowProgressAsync("Publishing", "Uploading Matches");
            controller.SetCancelable(true);
            int uploadCount = 0;
            foreach (var m in MatchesViewModel.Matches)
            {
                await UploadMatch(m);
                if (controller.IsCanceled) break;
                uploadCount++;
                controller.SetProgress((double)uploadCount / MatchesViewModel.Matches.Count);
            }
            await controller.CloseAsync();
            if (controller.IsCanceled)
                await dialogService.ShowMessageAsync("Publishing", "Publish cancelled");
            else
                await dialogService.ShowMessageAsync("Publishing", "Publish successful");
        }

        private Task UploadMatch(MatchViewModel matchViewModel)
        {
            return Task.Delay(250);
        }

        private void Initializate()
        {
            var matchesJson = File.ReadAllText("Matches.json");
            var matches = JsonConvert.DeserializeObject<List<Match>>(matchesJson, 
                new StringEnumConverter());
            //var s = JsonConvert.SerializeObject(matches, Formatting.Indented, new StringEnumConverter());
            //var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            //var p = Path.Combine(appData, "ScoreKeeper");
            //Directory.CreateDirectory(p);
            //var f = Path.Combine(p, "matches.json");        
            //File.WriteAllText(f, s);
            var matchViewModels = new ObservableCollection<MatchViewModel>(matches.Select(m => new MatchViewModel(m)));

            AllPlayers = new ObservableCollection<string>(matches.SelectMany(m => m.StartingEleven));

            MatchesViewModel = new MatchesViewModel(matchViewModels,
                new RelayCommand(_ => EditMatch(null)),
                new RelayCommand(m => EditMatch(((MatchViewModel)m).Match),
                    o => o != null),
                    dialogService);

            StatsViewModel = new StatsViewModel(matchViewModels);

            SettingsViewModel = new SettingsViewModel(() => IsSettingsFlyoutOpen = false);
        }

        private void EditMatch(Match m)
        {
            EditMatchViewModel = new EditMatchViewModel(m ?? Match.CreateNew(), AllPlayers);
            SelectedTabIndex = 2;
        }
    }
}
