using BO;

namespace Simulator;

public static class Simulator
{
    private static readonly BlApi.IBl? _bl = BlApi.Factory.Get();

    private const int c_sleepTime = 1000;

    private static volatile bool s_flagStopSimulation;

    private static readonly Random _random = new();// random numbers maker

    private static event Action? s_stopSimulation;

    public static event Action? s_StopSimulation
    {
        add => s_stopSimulation += value;
        remove => s_stopSimulation -= value;    
    }

    private static event Action<int, object?>? s_updateSimulation;

    public static event Action<int, object?>? s_UpdateSimulation
    {
        add => s_updateSimulation += value;
        remove => s_updateSimulation -= value;
    }

    public static void BeginSimulation() => new Thread(simulationProgress).Start();

    private static void simulationProgress()
    {
        Delegate[] delegates = s_updateSimulation!.GetInvocationList();

        while (!s_flagStopSimulation)
        {


            Order? order = _bl!.Order.NextOrderInLine()!;

            if (order is null)
                Thread.Sleep(c_sleepTime);
           
            else
            {
                int timeOfTreatment = _random.Next(3, 10);
                int nextStatus = ((int)order.Status! + 1);
                BO.ORDER_STATUS? oRDER_STATUS = (ORDER_STATUS)nextStatus;

                
                Tuple<Order, BO.ORDER_STATUS?, string, string> items = 
                    Tuple.Create(order, oRDER_STATUS, DateTime.Now.ToString(), DateTime.Now.AddSeconds(timeOfTreatment).ToString());

                baginInvoke(delegates, 1, items);

                int sleepTime = timeOfTreatment * 1000;

                new Thread(() => updateTime(timeOfTreatment, delegates)).Start();// for the bar

                Thread.Sleep(sleepTime);

                order = order.Status == ORDER_STATUS.PENDING ? _bl.Order.UpdateOrderShipDateAdmin(order.ID)
                        : _bl.Order.UpdateOrderDeliveryDateAdmin(order.ID);

                items =
                     Tuple.Create(order, (order.Status == ORDER_STATUS.SHIPPED ? (ORDER_STATUS?)ORDER_STATUS.DELIVERED : null), "", "");

                baginInvoke(delegates, 1, items);
            }

            Thread.Sleep(c_sleepTime);
        }

        s_flagStopSimulation = false;
    }

    private static void updateTime(int sleepTime, Delegate[] delegates)
    {
        int jump = Convert.ToInt32(100 / sleepTime);

        for (int i = jump; i <= 100; i += jump)
        {
            if (!s_flagStopSimulation)
            {
                baginInvoke(delegates, 2, i);
                Thread.Sleep(1000);
            }
            else
                break;
            
        }
    }

    public static void StopSimulation()
    {
        s_flagStopSimulation = true; 
        baginInvoke(s_stopSimulation?.GetInvocationList());
    }

    private static void baginInvoke(Delegate[]? delegates, params object[]? objects)
    {
        foreach (var @delegate in delegates!)
            @delegate?.DynamicInvoke(objects);
    }
}