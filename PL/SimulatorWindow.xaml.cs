using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BO;
namespace PL;

public class SimulatorWindowData : DependencyObject
{
    public string Timer
    {
        get { return (string)GetValue(TimerProperty); }
        set { SetValue(TimerProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Timer.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TimerProperty =
        DependencyProperty.Register("Timer", typeof(string), typeof(SimulatorWindowData));

    public Order? CurrentOrderInLine
    {
        get { return (Order?)GetValue(CurrentOrderInLineProperty); }
        set { SetValue(CurrentOrderInLineProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CurrentOrderInLine.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CurrentOrderInLineProperty =
        DependencyProperty.Register("CurrentOrderInLine", typeof(Order), typeof(SimulatorWindowData));

    public ORDER_STATUS NextStatus
    {
        get { return (ORDER_STATUS)GetValue(NextStatusProperty); }
        set { SetValue(NextStatusProperty, value); }
    }

    // Using a DependencyProperty as the backing store for NextStatus.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty NextStatusProperty =
        DependencyProperty.Register("NextStatus", typeof(ORDER_STATUS), typeof(SimulatorWindowData));



    public string? StartTime
    {
        get { return (string)GetValue(startTimeProperty); }
        set { SetValue(startTimeProperty, value); }
    }

    // Using a DependencyProperty as the backing store for startTime.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty startTimeProperty =
        DependencyProperty.Register("StartTime", typeof(string), typeof(SimulatorWindowData));



    public string? HandleTime
    {
        get { return (string)GetValue(handleTimeProperty); }
        set { SetValue(handleTimeProperty, value); }
    }

    // Using a DependencyProperty as the backing store for handleTime.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty handleTimeProperty =
        DependencyProperty.Register("HandleTime", typeof(string), typeof(SimulatorWindowData));




}

/// <summary>
/// Interaction logic for SimulatorWindow.xaml
/// </summary>
public partial class SimulatorWindow : Window
{
    // Reference to the business logic layer
    private readonly BlApi.IBl? bl = BlApi.Factory.Get();
    private bool IsRunTimer { get; set; }
    private Stopwatch? Watch { get; set; }
    private Thread? TimerThread { get; set; }

    public SimulatorWindowData Data
    {
        get { return (SimulatorWindowData)GetValue(DataProperty); }
        set { SetValue(DataProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Data.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DataProperty =
        DependencyProperty.Register("Data", typeof(SimulatorWindowData), typeof(SimulatorWindow));


    public SimulatorWindow()
    {
        InitializeComponent();
        this.WindowStyle = WindowStyle.None;
        Data = new()
        {
            Timer = "00:00:00",
            CurrentOrderInLine = bl.Order.NextOrderInLine(),
            StartTime = DateTime.Now.ToString("HH:mm:ss"),
            HandleTime = $"{new Random().Next(3, 11)} seconds"
    };
        if(Data.CurrentOrderInLine != null) 
            Data.NextStatus = Data.CurrentOrderInLine.Status == ORDER_STATUS.PENDING ? ORDER_STATUS.SHIPPED : ORDER_STATUS.DELIVERED;
        IsRunTimer = true;
        Watch = new Stopwatch();
        TimerThread = new Thread(RunTimer);

        Watch.Restart();
        TimerThread.Start();
    }

    private string Random(int v1, int v2)
    {
        throw new NotImplementedException();
    }

    public void SetTextInvoke(string text)
    {
        if (!(CheckAccess()))
        {
            Action<string> d = SetTextInvoke;
            Dispatcher.BeginInvoke(d, new object[] { text });
        }
        else
        {
            Data!.Timer = text;
        }
    }

    private void RunTimer()
    {
        while (IsRunTimer)
        {
            string timer = Watch!.Elapsed.ToString().Substring(0, 8);
            SetTextInvoke(timer);
            Thread.Sleep(1000);
        }
    }
    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
            this.DragMove();
    }

    private void EndSimulation(object sender, RoutedEventArgs e)
    {
        Watch!.Stop();
        IsRunTimer = false;
        this.Close();
    }
}
