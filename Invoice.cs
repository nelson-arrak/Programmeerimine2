Console.WriteLine("Hello world!");

public class Invoice
{
    public int Id {get; set;}
    public string InvoiceNo {get; set;}
    public DateTime InvoiceDate {get; set;}
    public DateTime DueDate {get;set;}

    public Customer Customer {get;set;}
    public int CustomerId {get;set;}
}

public class InvoiceLine
{
    public int Id {get;set;}
    public string Item {get;set;}
    public string Description {get;set;}

    public decimal UnitPrice {get;set;}
    public string Unit {get;set;} // kg, tk jne
    public decimal Quantity {get;set;}
    public decimal Total 
    {
        get {return UnitPrice * Quantity;}
    }
    public Invoice Invoice {get;set;}
    public int InvoiceId {get;set;}
}

public class Customer
{
    public int Id {get;set;}
    public string Name {get;set;}
    public string Address {get;set;}

    public string Email {get;set;}
    public string Phone {get;set;}
}

