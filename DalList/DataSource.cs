using DO;
namespace Dal;

/// <summary>
/// internal class: no access from other projects
/// Runs the database of our shop, including lists of the products, orders and the order items.
/// </summary>
internal static class DataSource
{
    /// <summary>
    /// Empty ctor
    /// </summary>
    static DataSource()
    {
        s_Initialize();
    }
    /// <summary>
    /// The empty ctor calls all entities inits
    /// </summary>
    private static void s_Initialize()
    {
        InitProduct();
        initOrder();
        initOrderItem();
    }

    #region countersAndRunningNumbers
    //internal static class Config // TO BE IGNORED ATM BASED ON INSTRUCTIONS GIVEN BY DAN ZILBERSTEIN
    //{
    private static int runNumberOrderID = 1;
    private static int runNumberOrderItemID = 1;
    internal static int getRunNumberOrderID => runNumberOrderID++;
    internal static int getRunNumberOrderItemID => runNumberOrderItemID++;
    //}
    #endregion

    #region variablesAndLists
    private static readonly Random _random = new();// random numbers maker
    /// <summary>
    /// defining lists
    /// </summary>
    internal static List<Product?> _products = new();
    internal static List<Order?> _orders = new();
    internal static List<OrderItem?> _orderItems = new();
    #endregion

    #region init entities
    /// <summary>
    /// The maker of the default database of products - wines of 5 different WINERIES
    /// </summary>
    private static void InitProduct()
    {
        /// <summary>
        /// list of lists! each inner list for an ENUM WINERY category accordingly.
        /// </summary>
        List<List<string>> categoriesAndNames = new()
        {
          new List<string>() { "Gamla Cabernet Sauvignon-Merlot -  2017", "Yarden Rom - 2014", "Yarden Katzrin - 2016" },

          new List<string>() { "NADAV SINGLE VINEYARD - 2020",  "GALILO - 2017", "SINGLE VINEYARD SHIRAZ ELKOSH - 2016 " },

          new List<string>() { "PLATINUM CABERNET SAUVIGNON - 2018 ", "CABERNET SAUVIGNON - 2019 "  , "REICHAN 2014" },

          new List<string>()  { "Carmel Private Collection Cabernet Sauvignon - 2019", "Carmel VineYard 2020", "MIDITERRANEAN 2016" },

          new List<string>()  {"BarKan Platinum Merlot 2018", "Altitude 720 Petite Verdot 2015", "Barkan Superior 2017", }
        };

        //int countCatgories = categoriesAndNames.Count; &*&*&*&*&*

        int counterOfInitialAmount = categoriesAndNames.Sum(x => x.Count);
        /*for (int i = 0; i < countCatgories; i++) // we count how many products are to be made in the initial list
        {
            counterOfInitialAmount += categoriesAndNames[i].Count;
        }                                                                &*&*&*&*&*&**/
        int fivePercent = (int)(counterOfInitialAmount * 0.05) == 0 ? 1 : (int)(counterOfInitialAmount * 0.05); // 5% of the products shall have 0 in stock (in case 5% is less then 1, at least 1 product will have 0 amount)
/*
        int myIdNumber;
        for (int i = 0; i < countCatgories; i++)
        {
            foreach (var name in categoriesAndNames[i])
            {
                Product newProduct = new Product();

                myIdNumber = _random.Next(100000, 1000000);

                while (_products.Exists(p => p?.ID == myIdNumber))
                    myIdNumber = _random.Next(100000, 1000000);

                newProduct.ID = myIdNumber;

                newProduct.Category = (WINERIES)i;

                newProduct.InStock = fivePercent-- > 0 ? 0 : _random.Next(1, 101); // first 5% are getting 0 in stock

                newProduct.Name = name;

                newProduct.Price = newProduct.Category switch
                {
                    WINERIES.GOLAN => _random.Next(100, 250),
                    WINERIES.DALTON => _random.Next(70, 150),
                    WINERIES.BARKAN => _random.Next(60, 100),
                    WINERIES.CARMEL => _random.Next(60, 100),
                    WINERIES.TEPERBERG => _random.Next(70, 200),
                    _ => 0,// null option. program is not supposed to ever get here.
                };

                AddProduct(newProduct); // Product's ready! adding to database
            }
        }                                           &*&*&*&*&*&*&*&*&*&*&*/
        int i = 0;
        _products = (from list in categoriesAndNames
                     let x = i++
                     from name in list
                     select (Product?)(new Product
                     {
                         ID = IDMaker(),
                         Category = (WINERIES)x,
                         InStock = fivePercent-- > 0 ? 0 : _random.Next(1, 101),
                         Name = name,
                         Price = (WINERIES)x switch
                         {
                             WINERIES.GOLAN => _random.Next(100, 250),
                             WINERIES.DALTON => _random.Next(70, 150),
                             WINERIES.BARKAN => _random.Next(60, 100),
                             WINERIES.CARMEL => _random.Next(60, 100),
                             WINERIES.TEPERBERG => _random.Next(70, 200),
                             _ => 0,// null option. program is not supposed to ever get here.
                         }
                     })).ToList();

    }

    /// <summary>
    /// The maker of the default database of orders list
    /// </summary>
    private static void initOrder()
    {
        /// <summary>
        /// list of lists! all synchronized. Meaning: the Nth cell in each list contains a piece of information about the Nth person
        /// </summary>
        List<List<string>> customerDetails = new List<List<string>>()
        {
         // full name
         new List<string>() { "Urson Haffner","Candide Oldacre","Clovis McLay","Torrie Jacobson", "Tildi Kiggel",

                            "Marisa Canniffe","Annecorinne O'Kynsillaghe","Brenden Hamments","Mae Semor","Robbert Pietrowicz",

                            "Kali Shayler","Betti Esland","Charlton Sheehan","Reena Montague","Jillian Rattery",

                             "Ethelbert Lodin","Cristen Bevans","Genia Prydie","Langston Landes","Melanie Attwoul"},

         // Email Address
         new List<string>(){ "uhaffner0@google.com.hk", "cmclay2@zdnet.com","bmesnard3@theatlantic.com","tkiggel5@1688.com",

                    "mcanniffe6@cocolog-nifty.com","aokynsillaghe7@edublogs.org","bhamments8@ocn.ne.jp", "msemor9@canalblog.com",

                               "rpietrowicza@about.me","kshaylerb@soup.io","beslandc@nps.gov","csheehand@sitemeter.com",

                         "rmontaguee@csmonitor.com","jratteryf@friendfeed.com","eloding@nymag.com","cbevansh@foxnews.com",

                              "gprydiei@goo.gl", "llandesj@meetup.com", "mattwoulk@dailymotion.com","jclareu@mozilla.com"},

         // Address 
          new List<string>()  { "2983 Gerald Alley",  "8498 Havey Avenue", "4380 Arapahoe Lane", "5 Warrior Lane ",

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

            int randomOrder = _random.Next(0, countCustomerDetails); // we roll the person who made the order. (same person can create several orders...)

            newOrder.ID = getRunNumberOrderID;

            newOrder.CustomerName = customerDetails[0][randomOrder];

            newOrder.CustomerEmail = customerDetails[1][randomOrder];

            newOrder.CustomerAddress = customerDetails[2][randomOrder];

            newOrder.OrderDate = DateTime.Now - new TimeSpan(_random.NextInt64(TimeSpan.TicksPerDay * 10, TimeSpan.TicksPerDay * 1000)); // latest order is at least 10 days ago

            if (i < countCustomerDetails * 8 / 10) // 80% of orders has a Shipping date
            {
                newOrder.ShipDate = newOrder.OrderDate + new TimeSpan(_random.NextInt64(TimeSpan.TicksPerDay * 2, TimeSpan.TicksPerDay * 5)); // shipping date is between 2-5 days after the order date

                if (i < (countCustomerDetails * 8 / 10) * 6 / 10) // 60% of orders THAT HAS BEEN SHIPPED has a delivery date
                {
                    newOrder.DeliveryDate = newOrder.ShipDate + new TimeSpan(_random.NextInt64(TimeSpan.TicksPerDay * 2)); // Delivery date is at most 2 days after shipping date. Thus, the latest delivery date is at least 3 days ago!
                }
                else
                {
                    newOrder.DeliveryDate = null;
                }
            }
            else
            {
                newOrder.ShipDate = null;
                newOrder.DeliveryDate = null;
            }
            _orders.Add(newOrder); // Order's ready! adding to database
        }
    }

    /// <summary>
    /// The maker of the default database of order items list
    /// </summary>
    private static void initOrderItem()
    {

        for (int j = 0; j < _orders.Count; ++j) // running over orders
        {

            for (int i = 0; i < _random.Next(1, 5); ++i) // up to 4 items in an order
            {

                if (_products[_random.Next(0, _products.Count)] is Product product &&
                    _orders[j] is Order order)
                {

                    OrderItem item = new OrderItem();

                    item.OrderItemID = getRunNumberOrderItemID;

                    item.OrderID = order.ID;

                    item.ProductID = product.ID;

                    item.Amount = _random.Next(1, 25);

                    item.Price = product.Price;  //price for 1 unit!!  (in case we will want the final price: (double)(product.Price * item.Amount);)

                    _orderItems.Add(item); // Order item's ready! Adding to database...
                }
            }
        }
    }
    #endregion


    static readonly List<int> col = new();
    /// <summary>
    /// makes IDs for products randomly but makes sure not to repeat an ID twice using the col List
    /// </summary>
    /// <returns>
    /// A 6 digit Positive number
    /// </returns>
    private static int IDMaker()
    {
        int myIdNumber = _random.Next(100000, 1000000);

        while (col.Exists(p => p == myIdNumber))
            myIdNumber = _random.Next(100000, 1000000);
        col.Add(myIdNumber);
        return myIdNumber;
    }
}