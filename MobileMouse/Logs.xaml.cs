using System.Collections.ObjectModel;

namespace MobileMouse;

public partial class Logs : ContentPage
{


    public Logs()
    {
        InitializeComponent();
        update();
        Assets.instance.logs.CollectionChanged += Logs_CollectionChanged;
        this.Loaded += Logs_Loaded;

    }

    private void Logs_Loaded(object? sender, EventArgs e)
    {
        update();
    }

    private void update()
    {
        if(Assets.instance.mainThread!=Thread.CurrentThread)
        {
            return;
        }
        layout.Children.Clear();
        foreach (var item in Assets.instance.logs)
        {
            layout.Add(new Label() { Text = item });
        }
    }

    private void Logs_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        update();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        update();
    }
}