namespace Backend.Models;

public class FoodsDB_config
{
    public string Database_Name { get; set; }
    public string Menu_Collection_Name { get; set; }
    public string Connection_String { get; set; }               //for guest
    public string Admin_Connection_String { get; set; }         //for admin
}