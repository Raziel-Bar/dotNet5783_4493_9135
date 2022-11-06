using DO;
namespace Dal;

public class DalProduct
{
    /// <summary>
    /// adds a new product to the products list
    /// </summary>
    /// <param name="newProduct">
    /// The new product's details
    /// </param>
    /// <returns>
    /// The new product's ID
    /// </returns>
    /// <exception cref="Exception">
    /// In case the product already exists in the list
    /// </exception>
    public int AddNewProduct(Product newProduct)
    {
        if (Array.Exists(DataSource._products, p => p.ID == newProduct.ID))
            throw new Exception("The product you wish to add already exists");

        DataSource._products[DataSource._productCounter++] = newProduct;

        return newProduct.ID;
    }

    /// <summary>
    /// searches for a sepecific product accoding to its ID
    /// </summary>
    /// <param name="productId">
    /// The ID of the product we are looking for
    /// </param>
    /// <returns>
    /// The product's details
    /// </returns>
    /// <exception cref="Exception">
    /// In case the product does not exist in the list
    /// </exception>
    public Product SearchProduct(int productId)
    {
        int index = Array.FindIndex(DataSource._products, p => p.ID == productId);

        if (index == -1)
            throw new Exception("The product you search for does not exist");

        return DataSource._products[index];
    }

    /// <summary>
    /// copies all products from the list into a NEW Array
    /// </summary>
    /// <returns>
    /// The new array
    /// </returns>
    public Product[] ListOfProducts()
    {
        Product[] newProductlist = new Product[DataSource._productCounter];
        for (int i = 0; i < newProductlist.Length; ++i)
            newProductlist[i] = DataSource._products[i];

        return newProductlist;
    }

    /// <summary>
    /// deletes a product from the list
    /// </summary>
    /// <param name="productId">
    /// The ID of the product we wish to delete
    /// </param>
    /// <exception cref="Exception">
    /// In case the product does not exist in the list
    /// </exception>
    public void DeleteProduct(int productId)
    {
        int index = Array.FindIndex(DataSource._products, p => p.ID == productId);

        if (index == -1)
            throw new Exception("The product you wish to delete does not exist");

        int last = (--DataSource._productCounter);

        DataSource._products[index] = DataSource._products[last]; // moving last product's details into the deleted order's cell, running over it

        Array.Clear(DataSource._products, last, last); // last cell is no longer needed. cleaning...
    }

    /// <summary>
    /// updates a product's details
    /// </summary>
    /// <param name="uppdateProduct">
    /// The product's new details
    /// </param>
    /// <exception cref="Exception">
    /// In case the product does not exist in the list
    /// </exception>
    public void UpdateProduct(Product uppdateProduct)
    {
        int index = Array.FindIndex(DataSource._products, p => p.ID == uppdateProduct.ID);

        if (index == -1)
            throw new Exception("The product you wish to update does not exist");

        DataSource._products[index] = uppdateProduct;
    }
}
// DRAFT FOR LIST BASED METHODS - IGNORE!
//public List<Product> listOfProducts()
//{
//    List<Product> list = new List<Product>();

//    foreach (Product product in DataSource._products)
//        list.Add(product);

//    return list;
//}