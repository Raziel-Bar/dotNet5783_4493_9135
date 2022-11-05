using DO;
namespace Dal;

public class DalProduct
{
    public int addNewProduct(Product newProduct)
    {
        if (Array.Exists(DataSource._products, p => p.ID == newProduct.ID))
            throw new Exception("The product you want to add is already exist");

        DataSource._products[DataSource.Config._productCounter++] = newProduct;

        return newProduct.ID;
    }

    public Product searchProduct(int productId)
    {
        int index = Array.FindIndex(DataSource._products, p => p.ID == productId);

        if (index == -1)
            throw new Exception("The product you want is not exist");

        return DataSource._products[index];
    }

    public List<Product> listOfProducts()//////////////////////////////////////
    {
        List<Product> list = new List<Product>();

        foreach (Product product in DataSource._products)
            list.Add(product);

        return list;
    }

    public void deleteProduct(int productId)
    {
        int index = Array.FindIndex(DataSource._products, p => p.ID == productId);

        if (index == -1)
            throw new Exception("The product you want to delete is not exist");

        int last = (--DataSource.Config._productCounter);

        DataSource._products[index] = DataSource._products[last];

        Array.Clear(DataSource._products, last, last);
    }

    public void updateProduct(Product uppdateProduct)
    {
        int index = Array.FindIndex(DataSource._products, p => p.ID == uppdateProduct.ID);

        if (index == -1)
            throw new Exception("The product you want to update is not exist");

        DataSource._products[index] = uppdateProduct;
    }
}
