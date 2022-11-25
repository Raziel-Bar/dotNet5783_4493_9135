using BlApi;
using BlImplementation;
using BO;

namespace BlText;

internal class Program
{
    private enum ENTITIES { EXIT, PRODUCT, ORDER, CART };
    private enum PRODUCT { EXIT, GET_LIST, PRODUCT_DETAILS_CART, ADMIN_PRODUCT_DETAILS, ADD_ADMIN, DELETE_ADMIN, UPDATE_ADMIN };
    private enum CART { EXIT, ADD_PRODUCT, UPDATE_PRODUCT, DISPLAY, CONFIRM }
    private enum ORDER { EXIT, GET_LIST, GET_ORDER, UPDATE_SHIP, UPDATE_DELIVERY, ORDER_TRACKING, UPDATE_ORDER_ADMIN }

    private static IBl ibl = new Bl();

    private static Cart cart = new Cart
    {
        CustomerAddress = "<empty>",
        CustomerEmail = "<empty>",
        CustomerName = "<empty>",
        TotalPrice = 0,
        ListOfItems = new List<OrderItem>()
    };

    private static int option;
    static void Main(string[] args)
    {
        int menuChoose;
        do
        {
            Console.WriteLine("Please select 1 of the options below: \n 0.EXIT. \n 1.PRODUCT.\n 2.ORDER\n 3.CART.");
            menuChoose = yourChoiceInt();
            try
            {
                switch ((ENTITIES)menuChoose)
                {
                    case ENTITIES.PRODUCT:
                        productCheck();
                        break;

                    case ENTITIES.ORDER:
                        orderCheck();
                        break;

                    case ENTITIES.CART:
                        cartCheck();
                        break;
                }
            }

            catch (NotFoundInDalException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
            }
            catch (AlreadyExistInDalException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
            }
            catch (BO.InvalidDataException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (RemoveProductThatIsInOrdersException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (ProductNotFoundInCartException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (StockNotEnoughtOrEmptyException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (UnexpectedException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (DateException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

        } while (menuChoose != 0);
        Console.WriteLine("\nThank you for your visit. Have a splendid day!\n");
    }


    /// <summary>
    /// The main caretaker: presents the 2nd menu and takes care of all options for the Product entity
    /// </summary>
    private static void productCheck()
    {
        //int option;//////
        do
        {

            Console.WriteLine(@"
Please chose one of the fowling options:

 0. EXIT / BACK
 1. Get product list
 2. Product details (user)
 3. Product details (admin)
 4. Add product (admin)
 5. Delete product (admin)
 6. Update product (admin)
");


            option = yourChoiceInt();

            switch ((PRODUCT)option)
            {
                case PRODUCT.GET_LIST:
                    printCollection(ibl.Product.RequestProducts());
                    break;

                case PRODUCT.PRODUCT_DETAILS_CART:
                    Console.WriteLine("Enter product ID:");
                    Console.WriteLine(ibl.Product.RequestProductDetailsUser(yourChoiceInt(), cart));
                    break;

                case PRODUCT.ADMIN_PRODUCT_DETAILS:
                    Console.WriteLine("Enter product ID:");
                    Console.WriteLine(ibl.Product.RequestProductDetailsAdmin(yourChoiceInt()));
                    break;

                case PRODUCT.ADD_ADMIN:
                    productCheckAddOrUpdate(PRODUCT.ADD_ADMIN);
                    break;

                case PRODUCT.DELETE_ADMIN:
                    Console.WriteLine("Enter product ID to remove: ");
                    ibl.Product.RemoveProductAdmin(yourChoiceInt());
                    break;

                case PRODUCT.UPDATE_ADMIN:
                    productCheckAddOrUpdate(PRODUCT.UPDATE_ADMIN);
                    break;
            }
        }
        while (option != 0);

    }

    /// <summary>
    /// Activates when option.ADD is selected. takes care of adding a product 
    /// </summary>
    private static void productCheckAddOrUpdate(PRODUCT action)
    {

        Product newProduct = new Product();
        if (action == PRODUCT.UPDATE_ADMIN)// checker if prod exist so we won't recieve update input for nothing
        {
            Console.WriteLine("Enter product ID to update: ");
            newProduct.ID = yourChoiceInt();
            ibl.Product.UpdateProductAdmin(ibl.Product.RequestProductDetailsAdmin(newProduct.ID)); 
        }

        Console.WriteLine("Please enter the product details: \nEnter the name of the product: ");
        newProduct.Name = Console.ReadLine();

        if (action == PRODUCT.ADD_ADMIN)
        {
            Console.WriteLine("Enter the product's ID: ");
            newProduct.ID = yourChoiceInt();
        }

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

        newProduct.Category = (WINERYS)catgory;

        Console.WriteLine("Please enter the amount in stock: ");
        newProduct.InStock = yourChoiceInt();
        if (action == PRODUCT.ADD_ADMIN) ibl.Product.AddProductAdmin(newProduct);
        else ibl.Product.UpdateProductAdmin(newProduct);


    }

    private static void cartCheck()
    {
        do
        {
            Console.WriteLine(@"
Please chose one of the fowling options:

 0.EXIT / BACK.
 1.Add product.
 2.Update product.
 3.Display cart
 4.Final confirm  
");
            option = yourChoiceInt();

            switch ((CART)option)
            {
                case CART.ADD_PRODUCT:
                    Console.WriteLine("Catalog:");
                    printCollection(ibl.Product.RequestProducts());
                    Console.WriteLine("Enter product ID");
                    cart = ibl.Cart.AddProductToCart(yourChoiceInt(), cart);
                    break;

                case CART.UPDATE_PRODUCT:
                    Console.WriteLine("Please enter the product ID and the new amount");
                    cart = ibl.Cart.UpdateProductInCart(yourChoiceInt(), cart, yourChoiceInt());
                    break;
                case CART.DISPLAY:
                    Console.WriteLine(cart);
                    break;
                case CART.CONFIRM:
                    Console.Write("Please enter your details:\nName: ");
                    cart.CustomerName = Console.ReadLine();
                    Console.Write("Email: ");
                    cart.CustomerEmail = Console.ReadLine();
                    Console.Write("Address: ");
                    cart.CustomerAddress = Console.ReadLine();
                    Console.WriteLine(cart);
                    Console.WriteLine("For a final confirm please enter 1:");
                    if (yourChoiceInt() == 1)
                        ibl.Cart.ConfirmOrder(cart);
                    break;
            }
        } while (option != 0);
    }

    private static void orderCheck()
    {
        do
        {
            Console.WriteLine(@"
Please chose one of the fowling options:

 0.EXIT / BACK.
 1.Get order list.
 2.Get order
 3.Update order ship date.
 4.update order delivery date.
 5.order tracking information.
");
            option = yourChoiceInt();
            switch ((ORDER)option)
            {
                case ORDER.GET_LIST:
                    printCollection(ibl.Order.RequestOrdersListAdmin());
                    break;

                case ORDER.GET_ORDER:
                    Console.WriteLine("Please enter the order ID:");
                    Console.WriteLine(ibl.Order.RequestOrderDetails(yourChoiceInt()));
                    break;

                case ORDER.UPDATE_SHIP:
                    Console.WriteLine("Please enter the order ID:");
                    Console.WriteLine(ibl.Order.UpdateOrderShipDateAdmin(yourChoiceInt()));
                    break;

                case ORDER.UPDATE_DELIVERY:
                    Console.WriteLine("Please enter the order ID:");
                    Console.WriteLine(ibl.Order.UpdateOrderDeliveryDateAdmin(yourChoiceInt()));
                    break;

                case ORDER.ORDER_TRACKING:
                    Console.WriteLine("Please enter the order ID:");
                    Console.WriteLine(ibl.Order.OrderTrackingAdmin(yourChoiceInt()));

                    break;

            }

        } while (option != 0);

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

    /// <summary>
    /// general int input machine with checker
    /// </summary>
    /// <returns>
    /// valid int input
    /// </returns>
    private static int yourChoiceInt()
    {
        int tempChoice = -1;
        tryParseInt(ref tempChoice);
        return tempChoice;
    }

    /// <summary>
    /// receives input for an int variable that is given by ref
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
}