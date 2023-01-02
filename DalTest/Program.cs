using Dal;
using DalApi;
using DO;
namespace Test;

/// <summary>
/// tester for the DAL
/// </summary>
public class Program
{
    /// <summary>
    /// creating an obejct from each DAL entity in order to have access to each entity's methods
    /// </summary>
    private readonly static IDal? dal = Factory.Get(); // new DalList();


    /// <summary>
    /// enum for the first menu in DalTest\Program.cs
    /// </summary>
    public enum ENTITIES { EXIT, PRODUCT, ORDER, ORDER_ITEM, CART, };

    /// <summary>
    /// enum for the second menu for each selction from menu_1 in DalTest\Program.cs
    /// </summary>
    public enum OPTIONS { EXIT, ADD, UPDATE, DELETE, SEARCH, GET_LIST, ORDER_ITEM_LIST, ORDER_ITEM_SEARCH };


    /// <summary>
    /// main program. presents the Main menu.
    /// </summary>
    /// <param name="args"></param>
    static void Main(string[] args)
    {

        int menuChoose;
        do
        {
            Console.WriteLine("Please select 1 of the options below: \n 0.EXIT. \n 1.PRODUCT.\n 2.ORDER\n 3.ORDER ITEM.");
            menuChoose = yourChoiceInt();
            switch ((ENTITIES)menuChoose)
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
        } while (menuChoose != 0);
        Console.WriteLine("\nThank you for your visit. Have a splendid day!\n");
    }

    #region Entity Functions

    #region PRODUCT

    /// <summary>
    /// The main caretaker: presents the 2nd menu and takes care of all options for the Product entity
    /// </summary>
    private static void productCheck()
    {
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
                        productCheckAdd();
                        break;

                    case OPTIONS.UPDATE:
                        productCheckUpdate();
                        break;

                    case OPTIONS.DELETE:
                        Console.WriteLine("Enter the product's ID to delete: ");
                        dal?.Product.Delete(yourChoiceInt());

                        break;
                    case OPTIONS.SEARCH:
                        Console.WriteLine("Enter the product's ID: ");

                        Console.WriteLine(dal?.Product.Get(yourChoiceInt()));


                        break;

                    case OPTIONS.GET_LIST: // the GetList may return null
                        printCollection(dal?.Product.GetList() ?? throw new UnexpectedException());
                        break;
                }
            }
            catch (NotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (AlreadyExistException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        while (option != 0);
    }

    /// <summary>
    /// Activates when option.ADD is selected. takes care of adding a product to the database
    /// </summary>
    private static void productCheckAdd()
    {
        Product newProduct = new Product();

        Console.WriteLine("Please enter the product details: \nEnter the name of the product: ");

        newProduct.Name = Console.ReadLine();

        Console.WriteLine("Enter the product's ID: ");
        newProduct.ID = yourChoiceInt();

        Console.WriteLine("Please enter the product's price: ");
        newProduct.Price = yourChoiceDouble();

        Console.WriteLine(@"Please choice the product's category: 

0 for GOLAN
1 for DALTON
2 for TEPERBERG
3 for CARMEL
4 for BARKAN
Enter your choice: ");

        int catgory = yourChoiceInt();
        while (catgory > 4 || catgory < 0)
        {
            Console.WriteLine("Error. Please enter a valid option: ");
            catgory = yourChoiceInt();
        }

        newProduct.Category = (WINERIES)catgory;

        Console.WriteLine("Please enter the amount in stock: ");
        newProduct.InStock = yourChoiceInt();

        dal?.Product.Add(newProduct);
    }

    /// <summary>
    /// Activates when option.UPDATE is selected. takes care of updating a product's details in the database
    /// </summary>
    private static void productCheckUpdate()
    {
        Console.WriteLine("Please enter the product's ID: ");


        //Product productUpdate = dal.Product.Get(yourChoiceInt());
        if (dal?.Product.Get(yourChoiceInt()) is Product productUpdate)
        {


            int changeChoice;
            do
            {
                Console.WriteLine(@"What details do you wish to change? 
0. EXIT / BACK
1. change product's name.
2. change product's price.
3. change product's amount.
4. change product's category.       

Enter your choice: ");
                changeChoice = yourChoiceInt();
                while (changeChoice > 4 || changeChoice < 0)
                {
                    Console.WriteLine("Error. Please enter a valid option: ");
                    changeChoice = yourChoiceInt();
                }

                switch (changeChoice)
                {

                    case 1:
                        Console.WriteLine("Enter a new name of the product: ");
                        productUpdate.Name = Console.ReadLine();
                        break;

                    case 2:
                        Console.WriteLine("Enter a new price: ");
                        productUpdate.Price = yourChoiceDouble();
                        break;

                    case 3:
                        Console.WriteLine("Enter a new amount: ");
                        productUpdate.InStock = yourChoiceInt();
                        break;

                    case 4:
                        Console.WriteLine(@"Please choose the product's category: 
 0 for GOLAN
 1 for DALTON
 2 for TEPERBERG
 3 for CARMEL
 4 for BARKAN

Enter your choice: ");

                        int catgory = yourChoiceInt();
                        while (catgory > 4 || catgory < 0)
                        {
                            Console.WriteLine("Error. Please enter a valid option: ");
                            catgory = yourChoiceInt();
                        }
                        productUpdate.Category = (WINERIES)catgory;
                        break;
                };
            } while (changeChoice != 0);

            dal.Product.Update(productUpdate);
        }
    }

    #endregion

    #region ORDER 

    /// <summary>
    /// The main caretaker: presents the 2nd menu and takes care of all options for the Order entity
    /// </summary>
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
                        Console.WriteLine("Enter the order's ID to delete: ");
                        dal?.Order.Delete(yourChoiceInt());

                        break;
                    case OPTIONS.SEARCH:
                        Console.WriteLine("Enter the order's ID: ");
                        Console.WriteLine(dal?.Order.Get(yourChoiceInt()));

                        break;

                    case OPTIONS.GET_LIST:
                        printCollection(dal?.Order.GetList() ?? throw new UnexpectedException());

                        break;
                }
            }
            catch (NotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
        } while (option != 0);
    }

    /// <summary>
    /// Activates when option.ADD is selected. takes care of adding an order to the database
    /// </summary>
    private static void orderCheckAdd()
    {
        Order newOrder = new Order();

        Console.WriteLine("Please enter the order details: \n");

        newOrder.ID = 0; // ID will be given in the AddNewOrder() method

        Console.WriteLine("Enter the customer full name: ");
        newOrder.CustomerName = Console.ReadLine();

        Console.WriteLine("Enter the customer email: ");
        newOrder.CustomerEmail = Console.ReadLine();

        Console.WriteLine("Enter the customer address: ");
        newOrder.CustomerAddress = Console.ReadLine();

        newOrder.OrderDate = DateTime.Now;
        newOrder.ShipDate = null; //DateTime.MinValue;
        newOrder.DeliveryDate = null;//DateTime.MinValue;

        /*
        //we don't recive input from user for the order date.
        
        Console.WriteLine("Enter the order date: ");
        DateTime.TryParse(Console.ReadLine(), out dateTime);
        newOrder.OrderDate = dateTime;

        //ship and delivery dates will only be later and we can't put values at the moment

        Console.WriteLine("Enter the ship date: ");             
        DateTime.TryParse(Console.ReadLine(), out dateTime);
        newOrder.ShipDate = dateTime;

        Console.WriteLine("Enter the delivery date: ");
        DateTime.TryParse(Console.ReadLine(), out dateTime);
        newOrder.DeliveryDate = dateTime;*/

        dal?.Order.Add(newOrder);



    }

    /// <summary>
    /// Activates when option.UPDATE is selected. takes care of updating an order's details in the database
    /// </summary>
    private static void orderCheckUpdate()
    {
        DateTime dateTime;
        int changeChoice;

        Console.WriteLine("Please enter the order's ID: ");

        //  Order orderUpdate = dal.Order.Get(yourChoiceInt());

        if (dal?.Order.Get(yourChoiceInt()) is Order orderUpdate)
        {
            do
            {
                Console.WriteLine(@"What details do you wish to change? 
 0. EXIT / BACK
 1. change customer's name.
 2. change customer's email.
 3. change customer's address. 
 4. change order's date
 5. change ship date.
 6. change delivery day. 

Enter your choice: ");
                changeChoice = yourChoiceInt();
                while (changeChoice > 6 || changeChoice < 0)
                {
                    Console.WriteLine("Error. Please enter a valid option: ");
                    changeChoice = yourChoiceInt();
                }

                switch (changeChoice)
                {
                    case 1:
                        Console.WriteLine("Enter the new customer name:");
                        orderUpdate.CustomerName = Console.ReadLine();
                        break;

                    case 2:
                        Console.WriteLine("Enter the new customer email ");
                        orderUpdate.CustomerEmail = Console.ReadLine();
                        break;

                    case 3:
                        Console.WriteLine("Enter the new customer address");
                        orderUpdate.CustomerAddress = Console.ReadLine();
                        break;

                    case 4:
                        Console.WriteLine("Enter the new order date in the folowing format: {DD/MM/YY HH:MM:SS}");
                        DateTime.TryParse(Console.ReadLine(), out dateTime);
                        orderUpdate.OrderDate = dateTime;
                        break;
                    case 5:
                        Console.WriteLine("Enter the new ship date in the folowing format: {DD/MM/YY HH:MM:SS}");
                        DateTime.TryParse(Console.ReadLine(), out dateTime);
                        orderUpdate.ShipDate = dateTime;
                        break;
                    case 6:
                        Console.WriteLine("Enter the delivery date in the folowing format: {DD/MM/YY HH:MM:SS}");
                        DateTime.TryParse(Console.ReadLine(), out dateTime);
                        orderUpdate.DeliveryDate = dateTime;
                        break;
                };
            }
            while (changeChoice != 0);

            dal.Order.Update(orderUpdate);
        }
    }

    #endregion

    #region ORDER_ITEM

    /// <summary>
    /// The main caretaker: presents the 2nd menu and takes care of all options for the OrderItem entity
    /// </summary>
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
                        Console.WriteLine("Enter the order item's ID to delete: ");
                        dal?.OrderItem.Delete(yourChoiceInt());

                        break;
                    case OPTIONS.SEARCH:
                        Console.WriteLine("Enter the order item's ID to search: ");

                        Console.WriteLine(dal?.OrderItem.Get(yourChoiceInt()));


                        break;

                    case OPTIONS.GET_LIST:
                        printCollection(dal?.OrderItem.GetList()?? throw new UnexpectedException());
                        break;

                    case OPTIONS.ORDER_ITEM_LIST:
                        Console.WriteLine("Enter order's ID: ");
                        int id = yourChoiceInt();
                        printCollection(dal?.OrderItem.GetList(orderItem => orderItem?.OrderID == id)!);

                        // printCollection(dal.OrderItem.GetItemsInOrder(yourChoiceInt()));
                        break;

                    case OPTIONS.ORDER_ITEM_SEARCH:
                        Console.WriteLine("Enter the order's ID first, press 'enter' and then enter the product's ID: ");
                        int orderID = yourChoiceInt();
                        int productID = yourChoiceInt();
                        Console.WriteLine(dal?.OrderItem.Get(orderItem => orderItem?.OrderID == orderID && orderItem?.ProductID == productID));

                        // Console.WriteLine(dal.OrderItem.GetByOrderAndProcuctIDs(yourChoiceInt(), yourChoiceInt()));
                        break;
                }
            }
            catch (NotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
        } while (option != 0);
    }

    /// <summary>
    /// Activates when option.ADD is selected. takes care of adding an orderItem to the database
    /// </summary>
    private static void orderItemCheckAdd()
    {
        OrderItem newOrderItem = new OrderItem();

        Console.WriteLine("Please enter the order item details: \n");

        newOrderItem.OrderItemID = 0; // ID will be given in AddNewOrderItem()

        Console.WriteLine("Enter the product's ID:");
        newOrderItem.ProductID = yourChoiceInt();



        newOrderItem.Price = (double)(dal?.Product.Get(newOrderItem.ProductID)?.Price)!; // price is given based on the product's price in the dalProduct

        Console.WriteLine("Enter the order's ID: ");
        newOrderItem.OrderID = yourChoiceInt();


        Console.WriteLine("Enter the product's amount: ");
        newOrderItem.Amount = yourChoiceInt();

        dal.OrderItem.Add(newOrderItem);
    }

    /// <summary>
    /// Activates when option.UPDATE is selected. takes care of updating an orderItem's details in the database
    /// </summary>
    private static void orderItemCheckUpdate()
    {
        int changeChoice;
        Console.WriteLine("Please enter the order item's ID: ");

        // OrderItem? orderItemUpdate = dal.OrderItem.Get(yourChoiceInt());
        if (dal?.OrderItem.Get(yourChoiceInt()) is OrderItem orderitem)
        {
            do
            {
                Console.WriteLine(@"What details do you wish to change? 
 0. EXIT / BACK
 1. change product's ID.
 2. change order's ID.
 3. change amount.

Enter your choice: ");

                changeChoice = yourChoiceInt();
                while (changeChoice > 3 || changeChoice < 0)
                {
                    Console.WriteLine("Error. Please enter a valid option: ");
                    changeChoice = yourChoiceInt();
                }

                switch (changeChoice)
                {
                    case 1:
                        orderitem.ProductID = yourChoiceInt();
                        break;

                    case 2:
                        orderitem.OrderID = yourChoiceInt();
                        break;

                    case 3:
                        orderitem.Amount = yourChoiceInt();
                        break;
                };
            } while (changeChoice != 0);

            dal.OrderItem.Update(orderitem);
        }
    }

    #endregion

    #endregion

    #region General functions

    /// <summary>
    /// prints the second action menu
    /// </summary>
    /// <param name="type">
    /// The entity we chose from the main menu
    /// </param>
    private static void printMenu(string type)
    {
        Console.WriteLine(@$"
Please choose on of the following options:

 0. EXIT / BACK
 1. Add {type}.
 2. Update {type}. 
 3. Delete {type}
 4. Search {type}.
 5. Get List of {type}.");

        if (type == "Order item")
            Console.WriteLine(@" 6. Get list of order items in a specific order.  
 7. Search an order item by order id and product id.");
        Console.WriteLine("Enter your choice: ");
    }

    /// <summary>
    /// prints all objects from a given entity's list
    /// </summary>
    /// <typeparam name="Item">
    /// The entity we chose from the main menu
    /// </typeparam>
    /// <param name="items">
    /// The entity's list from the dataSource
    /// </param>
    private static void printCollection<list>(IEnumerable<list> items)
    {
        if (items == null)
            return;

        foreach (var obj in items)
            Console.WriteLine(obj);
    }

    /// <summary>
    /// recieves input for an int variable that is given by ref
    /// </summary>
    /// <param name="choice">
    /// the variable that is given by ref
    /// </param>
    private static void tryParseInt(ref int choice)
    {
        while (!(int.TryParse(Console.ReadLine(), out choice)))
        {
            Console.WriteLine("Error. please enter a valid input");
        }
    }

    /// <summary>
    /// recieves input for a double variable that is given by ref
    /// </summary>
    /// <param name="choice">
    /// the variable that is given by ref
    /// </param>
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

    /// <summary>
    /// general double input machine with checker
    /// </summary>
    /// <returns>
    /// valid double input
    /// </returns>
    private static double yourChoiceDouble()
    {
        double tempChoice = -1;
        tryParseDouble(ref tempChoice);
        return tempChoice;
    }

    #endregion
}