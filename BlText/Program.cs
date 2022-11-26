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
        CustomerAddress = null,
        CustomerEmail = null,
        CustomerName = null,
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
            catch (ChangeInCartItemsDetailsException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex) // for system exceptions
            {
                Console.WriteLine(ex);
            }

        } while (menuChoose != 0);
        Console.WriteLine("\nThank you for your visit. Have a splendid day!\n");
    }


    /// <summary>
    /// The main caretaker: presents the 2nd menu and takes care of all options for the Product entity
    /// </summary>
    private static void productCheck()
    {
        do
        {
            Console.WriteLine(@"
Please choose one of the followling options:

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
    /// Activates when option.ADD  or UPDATE is selected. takes care of adding/update a product 
    /// </summary>
    private static void productCheckAddOrUpdate(PRODUCT action)
    {

        Product newProduct = new Product();
        if (action == PRODUCT.UPDATE_ADMIN)// checker if prod exist so we won't recieve update input for nothing
        {
            Console.WriteLine("Enter product ID to update: ");
            newProduct = ibl.Product.RequestProductDetailsAdmin(yourChoiceInt());
        }

        Console.WriteLine("Please enter the product details: \nEnter the name of the product: ");
        newProduct.Name = yourChoiceString();

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

    /// <summary>
    /// The main caretaker: presents the 3nd menu and takes care of all options for the Order entity
    /// </summary>
    private static void orderCheck()
    {
        do
        {
            Console.WriteLine(@"
Please choose one of the fowling options:

 0.EXIT / BACK.
 1.Get order list.
 2.Get order
 3.Update order ship date.
 4.update order delivery date.
 5.order tracking information.
 6.Update order information (admin)
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

                case ORDER.UPDATE_ORDER_ADMIN: // BONUS
                    Console.WriteLine(@"Enter order ID, Product ID, Order item ID and The new amount you wish to order 
In case of removing item from order, set new amount to 0
In case of adding a new item to the order, set order item ID to 0");
                    ibl.Order.UpdateOrderAdmin(yourChoiceInt(), yourChoiceInt(), yourChoiceInt(), yourChoiceInt());
                    break;

            }
        } while (option != 0);
    }

    /// <summary>
    /// The main caretaker: presents the 4nd menu and takes care of all options for the Cart entity
    /// </summary>
    private static void cartCheck()
    {
        do
        {
            Console.WriteLine(@"
Please choose one of the fowling options:

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
                    cart.CustomerName = yourChoiceString();
                    Console.Write("Email: ");
                    cart.CustomerEmail = yourChoiceString();
                    Console.Write("Address: ");
                    cart.CustomerAddress = yourChoiceString();
                    Console.WriteLine(cart);
                    Console.WriteLine("For a final confirm please enter 1:");
                    if (yourChoiceInt() == 1)
                    {
                        ibl.Cart.ConfirmOrder(cart);
                        // erasing cart only if confirmation was a success
                        cart.CustomerName = null;
                        cart.CustomerEmail = null;
                        cart.CustomerAddress = null;
                        cart.ListOfItems?.Clear();
                        cart.TotalPrice = 0;
                    }
                    break;
            }
        } while (option != 0);
    }

    /// <summary>
    /// prints all objects from a given entity's IEnumerable<>
    /// </summary>
    /// <typeparam name="list"></typeparam>
    /// the IEnumerable<> type
    /// <param name="items"></param>
    /// name of the container 
    private static void printCollection<list>(IEnumerable<list> items)
    {
        if (items == null)
            return;

        foreach (var obj in items)
            Console.WriteLine(obj);
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
        while (!(double.TryParse(Console.ReadLine(), out tempChoice))) // BONUS
        {
            Console.WriteLine("Error please enter again");
        }
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
        while (!(int.TryParse(Console.ReadLine(), out tempChoice))) //BONUS
        {
            Console.WriteLine("Error. please enter a valid input");
        }
        return tempChoice;
    }

    /// <summary>
    /// general string input machine with checker
    /// </summary>
    /// <returns></returns>
    private static string yourChoiceString()
    {
        string tempChoice = Console.ReadLine();
        int notInt = 0;
        while (string.IsNullOrEmpty(tempChoice) || string.IsNullOrWhiteSpace(tempChoice) || int.TryParse(tempChoice, out notInt))
        {
            Console.WriteLine("Error. please enter a valid input");
            tempChoice = Console.ReadLine();
        }
        return tempChoice;
    }
}