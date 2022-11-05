using Dal;
using DO;
using System;

namespace Test
{
    enum ENTITIES { EXIT = 0, PRODUCT, ORDER, ORDER_ITEM };
    enum OPTIONS { EXIT = 0, ADD = 1, UPDATE, DELETE, SEARCH, GET_LIST, ORDER_ITEM_LIST, ORDER_ITEM_SEARCH };
    public class Program
    {
        private static DalProduct _dalProduct = new DalProduct();
        private static DalOrder _dalOrder = new DalOrder();
        private static DalOrderItem _dalOrderItem = new DalOrderItem();
        static public void productCheck()
        {
            int yourChoice;
            int tempInt;
            double tempPrice;
            do
            {
                Console.WriteLine(@"
               Please chose on of the following options:

               0. EXIT.
               1. Add product.
               2. Update product. 
               3. Get product.
               4. Get List of products.
               5. Delete product

               enter your choice: ");

                int.TryParse(Console.ReadLine(), out yourChoice);

                switch ((OPTIONS)yourChoice)
                {
                    case OPTIONS.ADD:
                        #region ADD
                        Console.WriteLine(@"Please enter the product details:

                                              Enter the name of product: ");

                        Product newProduct = new Product();

                        newProduct.Name = Console.ReadLine();

                        Console.WriteLine("Enter product ID: ");
                        int.TryParse(Console.ReadLine(), out tempInt);
                        newProduct.ID = tempInt;

                        Console.WriteLine("Please enter the product price: ");
                        double.TryParse(Console.ReadLine(), out tempPrice);
                        newProduct.Price = tempPrice;


                        Console.WriteLine(@"Please choice the product category: 
                                            1 for GOLAN
                                            2 for DALTON
                                            3 for TEPERBERG
                                            4 for CARMEL
                                            5 for BARKAN
                                            Enter your choice: ");

                        int.TryParse(Console.ReadLine(), out tempInt);
                        newProduct.Category = (WINERYS)tempInt;

                        //switch ((WINERYS)tempInt)
                        //{
                        //    case WINERYS.GOLAN:
                        //        _newProduct.Category = WINERYS.GOLAN;
                        //        break;
                        //    case WINERYS.DALTON:
                        //        _newProduct.Category = WINERYS.DALTON;
                        //        break;
                        //    case WINERYS.TEPERBERG:
                        //        _newProduct.Category = WINERYS.TEPERBERG;
                        //        break;

                        //    case WINERYS.CARMEL:

                        //        _newProduct.Category = WINERYS.CARMEL;
                        //        break;

                        //    case WINERYS.BARKAN:
                        //        _newProduct.Category = WINERYS.BARKAN;
                        //        break;
                        //}

                        Console.WriteLine("Please enter the amount in stock");
                        int.TryParse(Console.ReadLine(), out tempInt);
                        newProduct.InStock = tempInt;

                        try
                        {
                            _dalProduct.addNewProduct(newProduct);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    #endregion

                    case OPTIONS.UPDATE:
                        #region UPDATE
                        DateTime tempDate; 
                        Console.WriteLine(@"Please enter the order details:

                                              Enter the order ID: ");

                        Order newOrder = new Order();

                        int.TryParse(Console.ReadLine(), out tempInt);
                        newOrder.ID = tempInt;

                        Console.WriteLine("Enter the customer name: ");
                        newOrder.CustomerName = Console.ReadLine(); 

                        Console.WriteLine("Please enter the customer email: ");
                        newOrder.CustomerEmail = Console.ReadLine();

                        Console.WriteLine("Please enter the customer address: ");
                        newOrder.CustomerAdress = Console.ReadLine();

                        Console.WriteLine("  ");
                       
                        break;
                        #endregion



                        break;



                }
            } while (yourChoice != 0);

        }


        static void Main(string[] args)
        {



        }

    }
}

