using BlApi;
using BlImplementation;
using DalApi;
using DO;

namespace BlText;

internal class Program
{
    private enum ENTITIES { EXIT, PRODUCT, ORDER, CART };

    private enum PTODUCT { EXIT, GET_LIST, PRODUCT_DETAILS };

    private enum PTODUCT_ADMIN { EXIT, GET_LIST, PRODUCT_DETAILS, ADD, DELETE, UPDATE };

    private static int _adminpassword = 322234493;

    private static IBl ibl = new Bl();

    static void Main(string[] args)
    {
        int menuChoose;
        do
        {
            Console.WriteLine("Please select 1 of the options below: \n 0.EXIT. \n 1.PRODUCT.\n 2.ORDER\n 3.CART.");
            menuChoose = yourChoiceInt();
            switch ((ENTITIES)menuChoose)
            {
                case ENTITIES.PRODUCT:
                    productCheck();
                    break;

                case ENTITIES.ORDER:

                    break;

                case ENTITIES.CART:

                    break;
            }
        } while (menuChoose != 0);
        Console.WriteLine("\nThank you for your visit. Have a splendid day!\n");
    }

    /// <summary>
    /// The main caretaker: presents the 2nd menu and takes care of all options for the Product entity
    /// </summary>
    private static void productCheck()
    {


        //int option;
        //do
        //{
        //    productPrintMenu();
        //    option = yourChoiceInt();
        //    try
        //    {
        //        switch ((OPTIONS)option)
        //        {


        //        }
        //    catch ()
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    catch ()
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}
        //while (option != 0);
    }



    /// <summary>
    /// prints the second action menu
    /// </summary>
    /// <param name="type">
    /// The entity we chose from the main menu
    /// </param>
    private static void productPrintMenu()
    {
        Console.WriteLine("For admin zone please enter password Otherwise press 1 to continue \n");

        int password = yourChoiceInt();
        if (password != 1)
        {
            while (password != _adminpassword)
                password = yourChoiceInt();

            Console.WriteLine(@"
 0. EXIT / BACK
 1. Get products list
 2. Get product details
 3. Add product
 4. Remove product 
 5. update product
");
            Console.WriteLine("Enter your choice: ");
            return;
        }

        Console.WriteLine(@"
 0. EXIT / BACK
 1. Get products list
 2. Get product details ");

        Console.WriteLine("Enter your choice: ");
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