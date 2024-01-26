using System.Collections.ObjectModel;

namespace MobileMouse;

public partial class Logs : ContentPage
{


    public Logs()
    {
        InitializeComponent();
        update();
        Assets.instance.logs.CollectionChanged += Logs_CollectionChanged;


    }

    private void update()
    {
        foreach (var item in Assets.instance.logs)
        {
            layout.Add(new Label() { Text = item });
        }
    }

    private void Logs_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        update();
    }
}