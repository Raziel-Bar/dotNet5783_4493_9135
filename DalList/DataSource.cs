using DO;
using System.Linq;
using System.Collections;

namespace Dal;
internal static class DataSource // the internal in class is automatic
{

    // an empty ctr
    static DataSource()
    {
        s_Initialize();
    }

    //internal static class Config
    //{
    internal static int _productCounter = 0;

    internal static int _orderCounter = 0;

    internal static int _orderItemCounter = 0;

    private static int runNumberOrderID = 1;

    private static int runNumberOrderItemID = 1;

    internal static int getRunNumberOrderID => runNumberOrderID++;
    internal static int getRunNumberOrderItemID => runNumberOrderItemID++;
    //}


    #region variablesAndArrays

    private static readonly Random _random = new Random();// in c# we have to do a new for a class 


    internal static Product[] _products = new Product[50];
    internal static Order[] _orders = new Order[100];
    internal static OrderItem[] _orderItems = new OrderItem[200];



    // the functions that fill the arrays
    private static void addProduct(Product product) => _products[_productCounter++] = product;

    private static void addOrder(Order order) => _orders[_orderCounter++] = order;

    private static void addOrderItem(OrderItem orderItem) => _orderItems[_orderItemCounter++] = orderItem;

    #endregion


    private static void s_Initialize()
    {
        initProduct();
        initOrder();
        initOrderItem();
    }

    #region init entities///////////////////////////////////

    private static void initProduct()
    {
        List<List<string>> catgoriesAndNames = new List<List<string>>()
        {
          new List<string>() { "Gamla Cabernet Sauvignon-Merlot -  2017", "Yarden Rom - 2014", "Yarden Katzrin - 2016" },

          new List<string>() { "NADAV SINGLE VINEYARD - 2020",  "GALILO - 2017", "SINGLE VINEYARD SHIRAZ ELKOSH - 2016 " },

          new List<string>() { "PLATINUM CABERNET SAUVIGNON - 2018 ", "CABERNET SAUVIGNON - 2019 "  , "REICHAN 2014" },

          new List<string>()  { "Carmel Private Collection Cabernet Sauvignon - 2019", "Carmel VineYard 2020", "MIDITERRANEAN 2016" },

          new List<string>()  {"BarKan Platinum Merlot 2018", "Altitude 720 Petite Verdot 2015", "Barkan Superior 2017", }
        };

        int countCatgories = catgoriesAndNames.Count();

        int myIdNumber = 100000;

        for (int i = 1; i <= countCatgories; i++)
        {
            int fivePercent = (int)(catgoriesAndNames[i].Count() * 0.05);

            foreach (var name in catgoriesAndNames[i])
            {
                Product newProduct = new Product();

                newProduct.ID = myIdNumber++;

                newProduct.Category = (WINERYS)i;

                newProduct.InStock = fivePercent-- > 0 ? 0 : _random.Next(1, 101);

                newProduct.Name = name;

                newProduct.Price = newProduct.Category switch
                {
                    WINERYS.GOLAN => _random.Next(80, 250),
                    WINERYS.DALTON => _random.Next(70, 150),
                    WINERYS.BARKAN => _random.Next(60, 100),
                    WINERYS.CARMEL => _random.Next(60, 100),
                    WINERYS.TEPERBERG => _random.Next(50, 100),
                    _ => 0,// null option. program is not supposed to ever get here.
                };

                addProduct(newProduct);
            }
        }
    }

    private static void initOrder()
    {
        List<List<string>> customerDetails = new List<List<string>>()
        {

         // full name
         new List<string>() { "Urson Haffner","Candide Oldacre","Clovis McLay","Torrie Jacobson", "Tildi Kiggel",

                            "Marisa Canniffe","Annecorinne O'Kynsillaghe","Brenden Hamments","Mae Semor","Robbert Pietrowicz",

                            "Kali Shayler","Betti Esland","Charlton Sheehan","Reena Montague","Jillian Rattery",

                             "Ethelbert Lodin","Cristen Bevans","Genia Prydie","Langston Landes","Melanie Attwoul"},

         // Email Address
         new List<string>(){ "uhaffner0@google.com.hk", "cmclay2@zdnet.com","bmesnard3@theatlantic.com","tkiggel5@1688.com",  // email

                    "mcanniffe6@cocolog-nifty.com","aokynsillaghe7@edublogs.org","bhamments8@ocn.ne.jp", "msemor9@canalblog.com",

                               "rpietrowicza@about.me","kshaylerb@soup.io","beslandc@nps.gov","csheehand@sitemeter.com",

                         "rmontaguee@csmonitor.com","jratteryf@friendfeed.com","eloding@nymag.com","cbevansh@foxnews.com",

                              "gprydiei@goo.gl", "llandesj@meetup.com", "mattwoulk@dailymotion.com","jclareu@mozilla.com"},

         // Address 
          new List<string>()  { "2983 Gerald Alley",  "8498 Havey Avenue", "4380 Arapahoe Lane", "5 Warrior Lane ",   // address

                            "5882 Northridge Lane", "9 Northridge Park","59 Mandrake Pass","0449 Norway Maple Place",

                                  "6 Commercial Circle","5 Nelson Lane","592 Gale Pass","91141 Mesta Point",

                               "7263 Blackbird Place","96 Bobwhite Circle","49 Harbort Center","34276 Main Point",

                                  "7 Park Meadow Way","3 Waxwing Point","18 Springview Avenue","2439 Memorial Trail" }
        };

        DateTime now = DateTime.Now;

        int countCustomerDetails = customerDetails[0].Count();

        for (int i = 0; i < countCustomerDetails; i++)
        {
            Order newOrder = new Order();

            int randomOrder = _random.Next(0, countCustomerDetails);

            newOrder.ID = getRunNumberOrderID;

            newOrder.CustomerName = customerDetails[0][randomOrder];

            newOrder.CustomerEmail = customerDetails[1][randomOrder];

            newOrder.CustomerAdress = customerDetails[2][randomOrder];

            //========================================================
            //newOrder.OrderDate = new DateTime(_random.Next(now.Year - 2, now.Year + 1), _random.Next(1, 13), _random.Next(3, 5));
            newOrder.OrderDate = new DateTime(_random.Next(now.Year - 2, now.Year + 1));
            if (newOrder.OrderDate.Year == now.Year)
            {
                newOrder.OrderDate.AddMonths(_random.Next(1, now.Month + 1));
                if (newOrder.OrderDate.Month == now.Month)
                {
                    newOrder.OrderDate.AddDays(_random.Next(1, now.Day));
                }
            }
            else
                newOrder.OrderDate.AddMonths(_random.Next(1, 13));



            DateTime shipDate = newOrder.OrderDate.AddHours(_random.Next(0, 5)).AddSeconds(5).AddDays(_random.Next(1, 14));

            newOrder.ShipDate = shipDate;

            newOrder.DelveryrDate = shipDate.AddHours(_random.Next(0, 5));
            //===========================================================

            addOrder(newOrder);

        }
    }////////////////////////////

    private static void initOrderItem()
    {

        for (int j = 0; j < _orderCounter; ++j)
        {

            for (int i = 0; i < _random.Next(1, 5); ++i)
            {

                Product product = _products[_random.Next(0, _products.Length)];

                while (product.InStock == 0)
                {
                    product = _products[_random.Next(0, _products.Length)];
                }

                OrderItem item = new OrderItem();

                item.OrderItemID = getRunNumberOrderItemID;

                item.OrderID = _orders[j].ID;

                item.ProductID = product.ID;

                item.Amount = _random.Next(1, (int)((product.InStock) / 5) + 1);

                item.Price = product.Price;  //(double)(product.Price * item.Amount);

                addOrderItem(item);
            }
        }


    }
    #endregion///////////////////////////////////
}