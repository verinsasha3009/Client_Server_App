namespace ServerMauiApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

           // MainPage = new AppShell();
            MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.FromArgb("#2980B9"),
                BarTextColor = Colors.White
            };
        }
    }
}