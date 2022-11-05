using Dal;
using DO;
using System;
using System.Globalization;
#nullable disable

namespace Test
{
    enum ENTITIES { EXIT = 0, PRODUCT, ORDER, ORDER_ITEM };
    enum OPTIONS { EXIT = 0, ADD = 1, UPDATE, DELETE, SEARCH, GET_LIST, ORDER_ITEM_LIST, ORDER_ITEM_SEARCH };
    public class Program
    {
        private static DalProduct _dalProduct = new DalProduct();
        private static DalOrder _dalOrder = new DalOrder();
        private static DalOrderItem _dalOrderItem = new DalOrderItem();
        static void Main(string[] args)
        {
            int menuChose;
            do
            {
                Console.WriteLine(@"Please select the entity:
                                 
                              0.EXIT.    
                              1.PRODUCT.
                              2.ORDER.
                              3.ORDER ITEM.");

                menuChose = yourChoiceInt();
                switch ((ENTITIES)menuChose)
                {
                    case ENTITIES.PRODUCT:
                        productCheck();
                        break;

                    case ENTITIES.ORDER:
                        orderCheck();
                        break;

                    case ENTITIES.ORDER_ITEM:
                        break;
                }

            } while (menuChose != 0);
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
                        _dalProduct.deleteProduct(yourChoiceInt());
                        break;
                    case OPTIONS.SEARCH:
                        Console.WriteLine("Enter the product ID to search: ");
                        Console.WriteLine(_dalProduct.searchProduct(yourChoiceInt()));
                        break;

                    case OPTIONS.GET_LIST:
                        printColetion(_dalProduct.listOfProducts());
                        break;
                }
            } while (option != 0);
        }

        private static void productCheckAdd(double tempPrice)
        {
            Product newProduct = new Product();

            Console.WriteLine(@"Please enter the product details:

                                              Enter the name of product: ");

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

            try
            {
                _dalProduct.addNewProduct(newProduct);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private static void productCheckUpdate(double tempPrice)
        {
            Console.WriteLine("Please enter the product ID for searching: ");
            Product productUpdate = _dalProduct.searchProduct(yourChoiceInt());

            int chengeChoice;
            do
            {
                Console.WriteLine(@"Please chose what do you want to change in the product: 
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

            try
            {
                _dalProduct.updateProduct(productUpdate);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // ------------------------------------------------- END OF PRODUCT ENTITIE FUNCTIONS ---------------------------------------------------

        //==================================================================================================

        //--------------------------------------------- ORDER ENTITIE FUNCTIONS ----------------------------------------

        private static void orderCheck()
        {
            int option;
            do
            {
                printMenu("Order");
                option = yourChoiceInt();
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
                        _dalOrder.deleteOrder(yourChoiceInt());
                        break;
                    case OPTIONS.SEARCH:
                        Console.WriteLine("Enter the order ID to search: ");
                        Console.WriteLine(_dalOrder.searchOrder(yourChoiceInt()));
                        break;

                    case OPTIONS.GET_LIST:
                        printColetion(_dalOrder.listOfOrders());
                        break;
                }


            } while (option != 0);
        }

        private static void orderCheckAdd()
        {
            Order newOrder = new Order();
            Console.WriteLine(@"Please enter the order details: 

                                             Enter order ID:  ");
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

            try
            {
                _dalOrder.addNewOrder(newOrder);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void orderCheckUpdate()
        {
            DateTime dateTime;
            int chengeChoice;

            Console.WriteLine("Please enter the order ID for searching: ");
            Order orderUpdate = _dalOrder.searchOrder(yourChoiceInt());
            do
            {
                Console.WriteLine(@"Please chose what do you want to change in the order: 
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
            } while (chengeChoice != 0);

            try
            {
                _dalOrder.updateOrder(orderUpdate);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //--------------------------------------------- END OF ORDER ENTITIE FUNCTIONS ----------------------------------------

        //==================================================================================================


        //--------------------------------------- GENRAL FUNCTIONS ------------------------------------
        private static void printColetion<item>(item[] items)
        {
            foreach (var obj in items)
                Console.WriteLine(obj);
        }
        private static void printMenu(string type)
        {
            if (type == "orderItem")
            {
                Console.WriteLine(@$"
               Please chose on of the following options:

               0. EXIT.
               1. Add {type}.
               2. Update {type}. 
               3. Delete {type}
               4. Search {type}.
               5. Get List of {type}.
               6. Get list of orders from order item.  
               7. Search by order id and product id.

               enter your choice: ");
            }
            else
            {
                Console.WriteLine(@$"
               Please chose on of the following options:

               0. EXIT.
               1. Add {type}.
               2. Update {type}. 
               3. Delete {type}
               4. Search {type}.
               5. Get List of {type}.

               enter your choice: ");
            }
        }
        private static void tryParseInt(ref int choice)
        {
            while (!(int.TryParse(Console.ReadLine(), out choice)))
            {
                Console.WriteLine("Error please enter again");
            }
        }
        private static void tryParseDouble(ref double choice)
        {
            while (!(double.TryParse(Console.ReadLine(), out choice)))
            {
                Console.WriteLine("Error please enter again");
            }
        }
        private static int yourChoiceInt()
        {
            int tempCoice = -1;
            tryParseInt(ref tempCoice);
            return tempCoice;
        }
        //--------------------------------------- GENRAL FUNCTIONS ------------------------------------



    }

}

