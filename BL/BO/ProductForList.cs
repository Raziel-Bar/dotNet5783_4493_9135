﻿using DalApi;

namespace BO;

/// <summary>
/// Presents a Product's details for product's LIST 
/// </summary>
public class ProductForList
{
    /// <summary>
    /// The product's ID
    /// </summary>
    public int ID { get; set; }
    /// <summary>
    /// The product's name
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// The product's price
    /// </summary>
    public double Price { get; set; }
    /// <summary>
    /// The product's category
    /// </summary>
    public WINERIES? Category { get; set; }
    public override string ToString() => this.ToStringProperty();
    //public override string ToString() => $@"
    //    Product ID: {ID}: {Name}
    //    category: {Category}
    //	Price: {Price}";
}
