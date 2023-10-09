using System.Diagnostics;

namespace CommonLib;

public static class Advertisement
{
    static bool isEnable = false;
    public static void showAD_OneTime()
    {
        //ShowURL("https://www.2345.com/?7856");
    }
    private static void ShowURL(string url)
    {
        Process p = new Process();
        p.StartInfo.FileName = "cmd.exe";
        p.StartInfo.UseShellExecute = false;    //不使用shell启动
        p.StartInfo.RedirectStandardInput = true;//喊cmd接受标准输入
        p.StartInfo.RedirectStandardOutput = false;//不想听cmd讲话所以不要他输出
        p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
        p.StartInfo.CreateNoWindow = true;//不显示窗口
        p.Start();

        //向cmd窗口发送输入信息 后面的&exit告诉cmd运行好之后就退出
        p.StandardInput.WriteLine("start " + url + "&exit");
        //p.StandardInput.WriteLine("iexplore.exe " + url + "&exit");
        p.StandardInput.AutoFlush = true;
        //p.WaitForExit();//等待程序执行完退出进程
        p.Close();


    }
    private static ThreadStart showAD = () =>
    {
        showAD_OneTime();
        Reset();
    };
    static CountdownTask countdownTask = new(showAD, timeForAd);
    static Advertisement()
    {
        if(isEnable)
        {
            countdownTask.Cancel();
        }
        
    }
    public static void Start(int timeForAd = 60 * 60 * 000)
    {
        Reset();
        Advertisement.timeForAd = timeForAd;
    }

    public static int timeForAd = 60 * 60 * 000;
    

    public static void Reset()
    {
        if(isEnable)
        {
            countdownTask?.Cancel();
            countdownTask = new CountdownTask(showAD, timeForAd);
        }

    }

}
class CountdownTask
{
    public int CountdownTime;
    public int CheckInterval;
    private bool isCancel = false;
    public ThreadStart task;
    public void Cancel()
    {
        isCancel = true;
    }
    public CountdownTask(ThreadStart task, int CountdownTime = 60 * 60 * 1000, int CheckInterval = 100)
    {
        this.CountdownTime = CountdownTime;
        this.CheckInterval = CheckInterval;
        this.task = task;
        Countdown();
    }
    async void Countdown()
    {
        int loopTimes = CountdownTime / CheckInterval;
        for (int i = 0; i < loopTimes; i++)
        {
            await Task.Delay(CheckInterval);
            if (isCancel)
            {
                return;
            }
        }
        task.Invoke();
    }
}