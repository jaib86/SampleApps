using Caliburn.Micro;
using WPFUI.Interfaces;
using WPFUI.Models;

namespace WPFUI.ViewModels
{
    public class ShellViewModel : PropertyChangedBase, IShell
    {
        private string firstName = "Jaiprakash";

        private string lastName;

        private PersonModel selectedPerson;

        public ShellViewModel()
        {
            People.AddRange(new[] {
                new PersonModel { FirstName = "Jaiprakah", LastName = "Barnwal" },
                new PersonModel { FirstName = "Deepak", LastName = "Singh" },
                new PersonModel { FirstName = "Ruchika", LastName = "Mittal" },
                new PersonModel { FirstName = "Ashish", LastName = "Agarwal" }
            });
        }

        public string FirstName
        {
            get => firstName;
            set
            {
                firstName = value;
                NotifyOfPropertyChange(() => FirstName);
                NotifyOfPropertyChange(() => FullName);
            }
        }

        public string LastName
        {
            get => lastName;
            set
            {
                lastName = value;
                NotifyOfPropertyChange(() => LastName);
                NotifyOfPropertyChange(() => FullName);
            }
        }

        public string FullName { get => $"{FirstName} {LastName}"; }

        private string infoMessageCaption = "Show Message";

        public string InfoMessageCaption
        {
            get { return infoMessageCaption; }
            set
            {
                infoMessageCaption = value;
                NotifyOfPropertyChange(() => InfoMessageCaption);
            }
        }

        public BindableCollection<PersonModel> People { get; set; } = new BindableCollection<PersonModel>();

        public PersonModel SelectedPerson
        {
            get => selectedPerson;
            set
            {
                selectedPerson = value;
                NotifyOfPropertyChange(() => SelectedPerson);
            }
        }

        private int gridWidth;

        public int GridWidth
        {
            get => gridWidth;
            set
            {
                gridWidth = value;
                NotifyOfPropertyChange(() => GridWidth);
            }
        }

        public bool CanClearText(string firstName, string lastName) => !string.IsNullOrWhiteSpace(firstName) || !string.IsNullOrWhiteSpace(lastName);

        public void ClearText(string firstName, string lastName)
        {
            this.FirstName = string.Empty;
            this.LastName = string.Empty;
        }

        public void LoadPageOne()
        {
            //ActivateItem(new FirstChildViewModel());
        }

        public void LoadPageTwo()
        {
            //ActivateItem(new SecondChildViewModel());
        }

        public void InfoMessage()
        {
            this.InfoMessageCaption = this.InfoMessageCaption == "Show Message" ? "Show Message - Clicked" : "Show Message";
        }

        public void HitMe()
        {
            this.InfoMessageCaption = this.InfoMessageCaption == "Show Message" ? "Show Message - Clicked" : "Show Message";
        }
    }
}