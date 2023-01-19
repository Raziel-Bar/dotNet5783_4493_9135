using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using BO;
using Simulator;
using static System.Net.Mime.MediaTypeNames;

namespace PL;

public class SimulatorWindowData : DependencyObject
{
    public Order? CurrentOrderInLine
    {
        get { return (Order?)GetValue(CurrentOrderInLineProperty); }
        set { SetValue(CurrentOrderInLineProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CurrentOrderInLine.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CurrentOrderInLineProperty =
        DependencyProperty.Register("CurrentOrderInLine", typeof(Order), typeof(SimulatorWindowData));

    public ORDER_STATUS? NextStatus 
    {
        get { return (ORDER_STATUS)GetValue(NextStatusProperty); }
        set { SetValue(NextStatusProperty, value); }
    }

    // Using a DependencyProperty as the backing store for NextStatus.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty NextStatusProperty =
        DependencyProperty.Register("NextStatus", typeof(ORDER_STATUS?), typeof(SimulatorWindowData));


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

    public string Timer
    {
        get { return (string)GetValue(TimerProperty); }
        set { SetValue(TimerProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Timer.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TimerProperty =
        DependencyProperty.Register("Timer", typeof(string), typeof(SimulatorWindow));

    public int TimeProgress
    {
        get { return (int)GetValue(TimeProgressProperty); }
        set { SetValue(TimeProgressProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Timer.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TimeProgressProperty =
        DependencyProperty.Register("TimeProgress", typeof(int), typeof(SimulatorWindow));

    public SimulatorWindowData Data
    {
        get { return (SimulatorWindowData)GetValue(DataProperty); }
        set { SetValue(DataProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Data.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DataProperty =
        DependencyProperty.Register("Data", typeof(SimulatorWindowData), typeof(SimulatorWindow));

    private BackgroundWorker _backgroundWorker;
    public SimulatorWindow()
    {
        InitializeComponent();
        this.WindowStyle = WindowStyle.None;
        //    if(Data.CurrentOrderInLine != null) 
        //        Data.NextStatus = Data.CurrentOrderInLine.Status == ORDER_STATUS.PENDING ? ORDER_STATUS.SHIPPED : ORDER_STATUS.DELIVERED;

        Watch = new Stopwatch();

        Watch.Restart();

        _backgroundWorker = new BackgroundWorker
        {
            WorkerReportsProgress = true,
            WorkerSupportsCancellation = true
        };

        _backgroundWorker.DoWork += _backgroundWorker_DoWork;
        _backgroundWorker.ProgressChanged += _backgroundWorker_ProgressChanged;
        _backgroundWorker.RunWorkerCompleted += _backgroundWorker_RunWorkerCompleted;
        _backgroundWorker.RunWorkerAsync();
    }

    private void _backgroundWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
    {
   
        Watch!.Stop();
        Simulator.Simulator.s_StopSimulation -= cancelAsync;
        Simulator.Simulator.s_UpdateSimulation -= reportProgress;
        _backgroundWorker.Dispose();
        this.Close();
    }

    private void _backgroundWorker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
    {
        int action = e.ProgressPercentage;

        if (action == 0)
        {
            Timer = (e.UserState as string)!;
        }

        if (action == 1)
        {
            if (Data is null)
                Data = new();

                (Data.CurrentOrderInLine, Data.NextStatus, Data.StartTime, Data.HandleTime)
               = (e.UserState as Tuple<BO.Order, BO.ORDER_STATUS?, string, string>)!;
        }

        if (action == 2)
            TimeProgress = (int)e.UserState!;
    }

    private void _backgroundWorker_DoWork(object? sender, DoWorkEventArgs e)
    {
        Simulator.Simulator.s_StopSimulation += cancelAsync;

        Simulator.Simulator.s_UpdateSimulation += reportProgress;

        Simulator.Simulator.BeginSimulation();

        while (!_backgroundWorker.CancellationPending)
        {
            string timer = Watch!.Elapsed.ToString().Substring(0, 8);
            reportProgress(0, timer);

            Thread.Sleep(1000);
        }
    }

    private void cancelAsync()
    {
        _backgroundWorker.CancelAsync();
    }

    private void reportProgress(int progressPercentage, object? userState)
    {
        if (_backgroundWorker.IsBusy)
        {
            _backgroundWorker.ReportProgress(progressPercentage, userState);
        }
    }
          

    private void EndSimulationClick(object sender, RoutedEventArgs e)
    {
        Simulator.Simulator.StopSimulation();
    }

    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
            this.DragMove();
    }
}
//private Thread? TimerThread { get; set; }
//IsRunTimer = true;
//TimerThread = new Thread(RunTimer);
//TimerThread.Start();
//private string Random(int v1, int v2)
//{
//    throw new NotImplementedException();
//}

//public void SetTextInvoke(string text)
//{
//    if (!(CheckAccess()))
//    {
//        Action<string> d = SetTextInvoke;
//        Dispatcher.BeginInvoke(d, new object[] { text });
//    }
//    else
//    {
//        Data!.Timer = text;
//    }
//}

//private void RunTimer()
//{
//    while (IsRunTimer)
//    {
//        string timer = Watch!.Elapsed.ToString().Substring(0, 8);
//        SetTextInvoke(timer);
//        Thread.Sleep(1000);
//    }
//}
//private void Window_MouseDown(object sender, MouseButtonEventArgs e)
//{
//    if (e.ChangedButton == MouseButton.Left)
//        this.DragMove();
//}

//private void EndSimulation(object sender, RoutedEventArgs e)
//{
//    Watch!.Stop();
//    IsRunTimer = false;
//    this.Close();
//}