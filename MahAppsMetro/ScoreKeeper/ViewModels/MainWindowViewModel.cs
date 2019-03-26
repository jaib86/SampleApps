using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ScoreKeeper.Model;
using ScoreKeeper.Views;

namespace ScoreKeeper.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        private readonly Action<UserControl> navigateToView;
        private MatchesView matchesView;
        private StatsView statsView;
        public RelayCommand NewMatch { get; private set; }
        public RelayCommand ViewMatches { get; private set; }
        public RelayCommand ViewStats { get; private set; }
        public RelayCommand Exit { get; private set; }
        public RelayCommand Publish { get; private set; }
        public RelayCommand Settings { get; private set; }

        public MainWindowViewModel(Action<UserControl> navigateToView)
        {
            this.navigateToView = navigateToView;
            Initializate();
            NewMatch = new RelayCommand(_ => EditMatch(null));
            ViewMatches = new RelayCommand(_ => navigateToView(matchesView));
            ViewStats = new RelayCommand(_ => navigateToView(statsView));
            Exit = new RelayCommand(_ => Application.Current.MainWindow.Close());
            Publish = new RelayCommand(_ => OnPublishCommand());
            Settings = new RelayCommand(_ => OnSettingsCommand());

        }

        private void OnSettingsCommand()
        {
            var w = new Window();
            w.Title = "Settings";
            w.Content = new SettingsView();
            w.SizeToContent = SizeToContent.WidthAndHeight;
            var vm = new SettingsViewModel(() => w.Close());
            w.DataContext = vm;
            w.Owner = Application.Current.MainWindow;
            w.ShowDialog();
        }

        private void OnPublishCommand()
        {
            var w = new Window();
            w.Width = 400;
            w.Height = 150;
            w.Title = "Publishing";
            w.Content = new PublishView();
            var vm = new PublishViewModel(() => w.Close());
            w.DataContext = vm;
            w.Owner = Application.Current.MainWindow;
            w.ShowDialog();
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
            matchesView = new MatchesView();
            var matchViewModels = new ObservableCollection<MatchViewModel>(matches.Select(m => new MatchViewModel(m)));

            matchesView.DataContext = new MatchesViewModel(matchViewModels,
                new RelayCommand(_ => EditMatch(null)),
                new RelayCommand(m => EditMatch(((MatchViewModel)m).Match),
                    o => o != null));

            statsView = new StatsView();
            var statsViewModel = new StatsViewModel(matchViewModels);
            statsView.DataContext = statsViewModel;

            navigateToView(matchesView);
        }

        private void EditMatch(Match m)
        {
            var editMatchView = new EditMatchView();
            editMatchView.DataContext = new EditMatchViewModel(m ?? Match.CreateNew());
            navigateToView(editMatchView);
        }


    }
}
