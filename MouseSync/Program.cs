namespace MouseSync;
public enum Status { server,client}
internal static class Program
{
    
    public static Status Status { get; private set; }
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        if(args.Length > 0 && args[0] == "server") {
            Status = Status.server;
            Application.Run(new Server.Server());
        }
        else
        {
            Status = Status.client;
            Application.Run(new Form1());
        }
        

    }
}