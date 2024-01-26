using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsHID;

namespace MobileMouse;

public class Assets
{
    static Assets()
    {

    }
    public static Assets instance =new Assets();
    private Assets() { }
    public ObservableCollection<string> logs = new();
    public MouseInputData mouseInputData = new MouseInputData();
}
