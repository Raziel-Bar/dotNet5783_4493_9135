namespace DO;
public struct Order
{
    public int ID { get; set; } 
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }   
    public int CustomerAdress { get; set; } 
    public DateTime OrderDate { get; set; }   
    public DateTime ShipDate { get; set; }  
    public DateTime DelveryrDate { get; set; }
}
