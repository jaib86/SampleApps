namespace WpfApp
{
    public class MainViewModel : ObservableObject
    {
        public int LeftTextBlockWidth { get; set; }

        public string LeftTextBlockText { get; set; } = "Left Text :)";

        private string centerTextBlockText;

        public string CenterTextBlockText
        {
            get
            {
                return this.centerTextBlockText;
            }
            set
            {
                this.centerTextBlockText = $"Center Text {this.LeftTextBlockWidth}";
            }
        }
    }
}