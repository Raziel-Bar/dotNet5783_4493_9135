using Dal;
using DO;
namespace Test;

enum ENTITIES { EXIT = 0, PRODUCT, ORDER, ORDER_ITEM };
enum OPTIONS { EXIT = 0, ADD = 1, UPDATE, DELETE, SEARCH, GET_LIST, ORDER_ITEM_LIST, ORDER_ITEM_SEARCH };
public class Program
{
    private static DalProduct _dalProduct = new DalProduct();
    private static DalOrder _dalOrder = new DalOrder();
    private static DalOrderItem _dalOrderItem = new DalOrderItem();
    static void Main(string[] args)
    {
        int menuchoose;
        do
        {
            Console.WriteLine("Please select the entity: \n 0.EXIT. \n 1.PRODUCT.\n 2.ORDER\n 3.ORDER ITEM.");

            menuchoose = yourChoiceInt();
            switch ((ENTITIES)menuchoose)
            {
                case ENTITIES.PRODUCT:
                    productCheck();
                    break;

                case ENTITIES.ORDER:
                    orderCheck();
                    break;

                case ENTITIES.ORDER_ITEM:
                    orderItemCheck();
                    break;
            }

        } while (menuchoose != 0);
    }


    // ------------------------------------------------- PRODUCT ENTITIE FUNCTIONS ---------------------------------------------------
    private static void productCheck()
    {

        double tempPrice = -1;
        int option;
        do
        {
            printMenu("Product");
            option = yourChoiceInt();
            try
            {
                switch ((OPTIONS)option)
                {

                    case OPTIONS.ADD:
                        productCheckAdd(tempPrice);
                        break;

                    case OPTIONS.UPDATE:
                        productCheckUpdate(tempPrice);
                        break;

                    case OPTIONS.DELETE:
                        Console.WriteLine("Enter the product ID to delete: ");
                        _dalProduct.DeleteProduct(yourChoiceInt());
                        break;
                    case OPTIONS.SEARCH:
                        Console.WriteLine("Enter the product ID to search: ");
                        Console.WriteLine(_dalProduct.SearchProduct(yourChoiceInt()));
                        break;

                    case OPTIONS.GET_LIST:
                        printCollection(_dalProduct.ListOfProducts());
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        while (option != 0);
    }

    private static void productCheckAdd(double tempPrice)
    {
        Product newProduct = new Product();

        Console.WriteLine("Please enter the product details: \nEnter the name of product: ");

        newProduct.Name = Console.ReadLine();

        Console.WriteLine("Enter product ID: ");
        newProduct.ID = yourChoiceInt();

        Console.WriteLine("Please enter the product price: ");
        tryParseDouble(ref tempPrice);
        newProduct.Price = tempPrice;


        Console.WriteLine(@"Please choice the product category: 

1 for GOLAN
2 for DALTON
3 for TEPERBERG
4 for CARMEL
5 for BARKAN
Enter your choice: ");

        int catgory = 0;
        do
        {
            catgory = yourChoiceInt();
        } while (catgory > 5 || catgory < 0);

        newProduct.Category = (WINERYS)catgory;

        Console.WriteLine("Please enter the amount in stock");
        newProduct.InStock = yourChoiceInt();

        _dalProduct.AddNewProduct(newProduct);

    }

    private static void productCheckUpdate(double tempPrice)
    {
        Console.WriteLine("Please enter the product ID for searching: ");
        Product productUpdate = _dalProduct.SearchProduct(yourChoiceInt());

        int chengeChoice;
        do
        {
            Console.WriteLine(@"Please choose what do you want to change in the product: 
0. EXIT
1. change name.
2. change price.
3. change amount.
4. change category.       

Enter your choice:");
            chengeChoice = yourChoiceInt();
            while (chengeChoice > 4 || chengeChoice < 0)
            {
                Console.WriteLine("ERROR please enter again");
                chengeChoice = yourChoiceInt();
            }

            switch (chengeChoice)
            {

                case 1:
                    productUpdate.Name = Console.ReadLine();
                    break;

                case 2:
                    tryParseDouble(ref tempPrice);
                    productUpdate.Price = tempPrice;
                    break;

                case 3:
                    productUpdate.InStock = yourChoiceInt();
                    break;

                case 4:
                    Console.WriteLine(@"Please choice the product category: 
 1 for GOLAN
 2 for DALTON
 3 for TEPERBERG
 4 for CARMEL
 5 for BARKAN

Enter your choice: ");

                    int catgory = 0;
                    catgory = yourChoiceInt();
                    while (catgory > 5 || catgory < 0)
                    {
                        Console.WriteLine("ERROR please enter again");
                        catgory = yourChoiceInt();
                    }
                    break;
            };
        } while (chengeChoice != 0);

        _dalProduct.UpdateProduct(productUpdate);
    }

    // ------------------------------------------------- END OF PRODUCT ENTITIE FUNCTIONS ---------------------------------------------------

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    //--------------------------------------------- ORDER ENTITIE FUNCTIONS ----------------------------------------

    private static void orderCheck()
    {
        int option;
        do
        {
            printMenu("Order");
            option = yourChoiceInt();
            try
            {
                switch ((OPTIONS)option)
                {
                    case OPTIONS.ADD:
                        orderCheckAdd();
                        break;

                    case OPTIONS.UPDATE:
                        orderCheckUpdate();
                        break;

                    case OPTIONS.DELETE:
                        Console.WriteLine("Enter the order ID to delete: ");
                        _dalOrder.DeleteOrder(yourChoiceInt());
                        break;
                    case OPTIONS.SEARCH:
                        Console.WriteLine("Enter the order ID to search: ");
                        Console.WriteLine(_dalOrder.SearchOrder(yourChoiceInt()));
                        break;

                    case OPTIONS.GET_LIST:
                        printCollection(_dalOrder.ListOfOrders());
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        } while (option != 0);
    }

    private static void orderCheckAdd()
    {
        Order newOrder = new Order();
        Console.WriteLine("Please enter the order details: \n Enter order ID:  ");
        newOrder.ID = yourChoiceInt();

        Console.WriteLine("Enter the customer full name: ");
        newOrder.CustomerName = Console.ReadLine();

        Console.WriteLine("Enter the customer email: ");
        newOrder.CustomerEmail = Console.ReadLine();

        Console.WriteLine("Enter the customer address: ");
        newOrder.CustomerAdress = Console.ReadLine();

        DateTime dateTime;

        Console.WriteLine("Enter the order date: ");
        DateTime.TryParse(Console.ReadLine(), out dateTime);
        newOrder.OrderDate = dateTime;

        Console.WriteLine("Enter the ship date: ");
        DateTime.TryParse(Console.ReadLine(), out dateTime);
        newOrder.ShipDate = dateTime;

        Console.WriteLine("Enter the delivery date: ");
        DateTime.TryParse(Console.ReadLine(), out dateTime);
        newOrder.DelveryrDate = dateTime;

        _dalOrder.AddNewOrder(newOrder);

    }

    private static void orderCheckUpdate()
    {
        DateTime dateTime;
        int chengeChoice;

        Console.WriteLine("Please enter the order ID for searching: ");
        Order orderUpdate = _dalOrder.SearchOrder(yourChoiceInt());
        do
        {
            Console.WriteLine(@"Please choose what do you want to change in the order: 
 0. EXIT
 1. change customer name.
 2. change customer email.
 3. change customer address. 
 4. change ship date.
 5. change delivery day. 

Enter your choice:");
            chengeChoice = yourChoiceInt();
            while (chengeChoice > 5 || chengeChoice < 0)
            {
                Console.WriteLine("ERROR please enter again");
                chengeChoice = yourChoiceInt();
            }

            switch (chengeChoice)
            {
                case 1:
                    orderUpdate.CustomerName = Console.ReadLine();
                    break;

                case 2:
                    orderUpdate.CustomerEmail = Console.ReadLine();
                    break;

                case 3:
                    orderUpdate.CustomerAdress = Console.ReadLine();
                    break;

                case 4:
                    Console.WriteLine("Enter the new ship date: ");
                    DateTime.TryParse(Console.ReadLine(), out dateTime);
                    orderUpdate.ShipDate = dateTime;
                    break;
                case 5:
                    Console.WriteLine("Enter the delivery date: ");
                    DateTime.TryParse(Console.ReadLine(), out dateTime);
                    orderUpdate.DelveryrDate = dateTime;
                    break;
            };
        }
        while (chengeChoice != 0);

        _dalOrder.UpdateOrder(orderUpdate);
    }

    //--------------------------------------------- END OF ORDER ENTITIE FUNCTIONS ----------------------------------------

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    //--------------------------------------------- ORDER ITEM ENTITIE FUNCTIONS ----------------------------------------

    private static void orderItemCheck()
    {

        int option;
        do
        {
            printMenu("Order item");
            option = yourChoiceInt();
            try
            {
                switch ((OPTIONS)option)
                {
                    case OPTIONS.ADD:
                        orderItemCheckAdd();
                        break;

                    case OPTIONS.UPDATE:
                        orderCheckUpdate();
                        break;

                    case OPTIONS.DELETE:
                        Console.WriteLine("Enter the order item ID to delete: ");
                        _dalOrderItem.DeleteOrderItem(yourChoiceInt());
                        break;
                    case OPTIONS.SEARCH:
                        Console.WriteLine("Enter the order item ID to search: ");
                        Console.WriteLine(_dalOrderItem.SearchOrderItem(yourChoiceInt()));
                        break;

                    case OPTIONS.GET_LIST:
                        printCollection(_dalOrderItem.OrderItemsList());
                        break;

                    case OPTIONS.ORDER_ITEM_LIST:
                        Console.WriteLine("Enter order ID: ");
                        printCollection(_dalOrderItem.OrdersList(yourChoiceInt()));
                        break;

                    case OPTIONS.ORDER_ITEM_SEARCH:
                        Console.WriteLine("Enter the order ID and product ID for searching: ");
                        Console.WriteLine(_dalOrderItem.SearchProductItem(yourChoiceInt(), yourChoiceInt()));
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        } while (option != 0);
    }

    private static void orderItemCheckAdd()
    {
        double tempPrice = -1;
        OrderItem newOrderItem = new OrderItem();

        Console.WriteLine("Please enter the order item details: \n Enter order item ID: ");
        newOrderItem.OrderItemID = yourChoiceInt();

        Console.WriteLine("Enter product ID:");
        newOrderItem.ProductID = yourChoiceInt();

        Console.WriteLine("Enter order ID: ");
        newOrderItem.OrderID = yourChoiceInt();

        Console.WriteLine("Enter price of the product:");
        tryParseDouble(ref tempPrice);
        newOrderItem.Price = tempPrice;

        Console.WriteLine("Enter product amount:");
        newOrderItem.Amount = yourChoiceInt();

        _dalOrderItem.AddNewOrderItem(newOrderItem);
    }

    private static void orderItemCheckUpdate()
    {
        double tempPrice = -1;
        int chengeChoice;
        Console.WriteLine("Please enter the order item ID for searching: ");
        OrderItem orderItemUpdate = _dalOrderItem.SearchOrderItem(yourChoiceInt());
        do
        {
            Console.WriteLine(@"Please choose what do you want to change in the order item: 
 0. EXIT / GO BACK
 1. change product ID.
 2. change order Id.
 3. change price. 
 4. change amount.

Enter your choice:");

            chengeChoice = yourChoiceInt();
            while (chengeChoice > 4 || chengeChoice < 0)
            {
                Console.WriteLine("ERROR please enter again");
                chengeChoice = yourChoiceInt();
            }

            switch (chengeChoice)
            {
                case 1:
                    orderItemUpdate.ProductID = yourChoiceInt();
                    break;

                case 2:
                    orderItemUpdate.OrderID = yourChoiceInt();
                    break;

                case 3:
                    tryParseDouble(ref tempPrice);
                    orderItemUpdate.Price = tempPrice;
                    break;

                case 4:
                    orderItemUpdate.Amount = yourChoiceInt();
                    break;
            };
        } while (chengeChoice != 0);

        _dalOrderItem.UpdateOrderItem(orderItemUpdate);
    }



    //--------------------------------------- GENRAL FUNCTIONS ------------------------------------
    private static void printMenu(string type)
    {
        Console.WriteLine(@$"
Please choose on of the following options:

 0. EXIT.
 1. Add {type}.
 2. Update {type}. 
 3. Delete {type}
 4. Search {type}.
 5. Get List of {type}.");

        if (type == "Order item")
            Console.WriteLine(@" 6. Get list of orders from order item.  
 7. Search by order id and product id.
");

        Console.WriteLine("Enter your choice:");

    }
    private static void printCollection<Item>(Item[] items)
    {
        foreach (var obj in items)
            Console.WriteLine(obj);
    }
    private static void tryParseInt(ref int choice)
    {
        while (!(int.TryParse(Console.ReadLine(), out choice)))
        {
            Console.WriteLine("Error please enter again");
        }
    } // CHECK OUT VS REF
    private static void tryParseDouble(ref double choice)
    {
        while (!(double.TryParse(Console.ReadLine(), out choice)))
        {
            Console.WriteLine("Error please enter again");
        }
    }
    private static int yourChoiceInt()
    {
        int tempChoice = -1;
        tryParseInt(ref tempChoice);
        return tempChoice;
    }
    //--------------------------------------- GENRAL FUNCTIONS ------------------------------------
}

