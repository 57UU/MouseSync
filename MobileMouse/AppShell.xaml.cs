namespace MobileMouse
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Assets.instance.mainThread=Thread.CurrentThread;
        }
    }
}
