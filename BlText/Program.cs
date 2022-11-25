using BlApi;
using BlImplementation;
using BO;

namespace BlText;

internal class Program
{
    private enum ENTITIES { EXIT, PRODUCT, ORDER, CART };

    private enum PTODUCT { EXIT, GET_LIST, PRODUCT_DETAILS, ADMIN_PRODUCT_DETAILS, ADD, DELETE, UPDATE };

    private enum CART { EXIT, ADD_PRODUCT, UPDATE_PRODUCT, CONFIRM }

    private enum ORDER { EXIT, GET_LIST, GET_ORDER, UPDATE_SHIP, UPDATE_DELIVERY, ORDER_TRACKING, UPDATE_ORDER_ADMIN }

    private static IBl ibl = new Bl();

    private static Cart cart = new Cart();

    private static int option;//
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
                Console.WriteLine(ex);
            }
            catch (AlreadyExistInDalException ex)
            {
                Console.WriteLine(ex);
            }
            catch (BO.InvalidDataException ex)
            {
                Console.WriteLine(ex);
            }
            catch (RemoveProductThatIsInOrdersException ex)
            {
                Console.WriteLine(ex);
            }
            catch (ProductNotFoundInCartException ex)
            {
                Console.WriteLine(ex);
            }
            catch (StockNotEnoughtOrEmptyException ex)
            {
                Console.WriteLine(ex);
            }
            catch(UnexpectedException ex)
            {
                Console.WriteLine(ex);
            }
            catch (DateException ex)
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
        //int option;//////
        do
        {
            
            Console.WriteLine(@"
Please chose one of the fowling options:

 0. EXIT / BACK.
 1. Get product list.
 2. Product details. 
 3. Product admin details.
 4. Add product.
 5. Delete product.
 6. Update product.
");
            

            option = yourChoiceInt();

            switch ((PTODUCT)option)
            {
                case PTODUCT.GET_LIST:
                    printCollection(ibl.Product.RequestProducts());
                    break;

                case PTODUCT.PRODUCT_DETAILS:
                    Console.WriteLine("Enter product ID:");
                    Console.WriteLine(ibl.Product.RequestProductDetailsCart(yourChoiceInt(), cart));
                    break;

                case PTODUCT.ADMIN_PRODUCT_DETAILS:
                    Console.WriteLine("Enter product ID:");
                    Console.WriteLine(ibl.Product.RequestProductDetailsAdmin(yourChoiceInt()));
                    break;

                case PTODUCT.ADD:
                    productCheckAdd();
                    break;

                case PTODUCT.DELETE:
                    Console.WriteLine("Enter product ID to remove: ");
                    ibl.Product.RemoveProductAdmin(yourChoiceInt());
                    break;

                case PTODUCT.UPDATE:
                    Console.WriteLine("Enter product ID to update: ");
                    ibl.Product.UpdateProductAdmin(ibl.Product.RequestProductDetailsAdmin(yourChoiceInt()));
                    break;
            }
        }
        while (option != 0);

    }

    /// <summary>
    /// Activates when option.ADD is selected. takes care of adding a product 
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

        newProduct.Category = (WINERYS)catgory;

        Console.WriteLine("Please enter the amount in stock: ");
        newProduct.InStock = yourChoiceInt();

        ibl.Product.AddProductAdmin(newProduct);
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
 3.Finel confirm  
");
            option = yourChoiceInt();

            switch ((CART)option)
            {
                case CART.ADD_PRODUCT:
                    Console.WriteLine("Please chose the product you want to add:");
                    printCollection(ibl.Product.RequestProducts());
                    Console.WriteLine("Enter product ID");
                    cart = ibl.Cart.AddProductToCart(yourChoiceInt(), cart);
                    break;

                case CART.UPDATE_PRODUCT:
                    Console.WriteLine("Please enter the product ID and the new amount");
                    cart = ibl.Cart.UpdateProductInCart(yourChoiceInt(), cart, yourChoiceInt());
                    break;

                case CART.CONFIRM:
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
                    printCollection(ibl.Ordeer.RequestOrdersListAdmin());
                    break;

                case ORDER.GET_ORDER:
                    Console.WriteLine("Please enter the order ID:");
                    Console.WriteLine(ibl.Ordeer.RequestOrderDetails(yourChoiceInt()));
                    break;

                case ORDER.UPDATE_SHIP:
                    Console.WriteLine("Please enter the order ID:");
                    Console.WriteLine(ibl.Ordeer.UpdateOrderShipDateAdmin(yourChoiceInt()));
                    break;

                case ORDER.UPDATE_DELIVERY:
                    Console.WriteLine("Please enter the order ID:");
                    Console.WriteLine(ibl.Ordeer.UpdateOrderDeliveryDateAdmin(yourChoiceInt()));
                    break;

                case ORDER.ORDER_TRACKING:
                    Console.WriteLine("Please enter the order ID:");
                    Console.WriteLine(ibl.Ordeer.OrderTrackingAdmin(yourChoiceInt()));

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