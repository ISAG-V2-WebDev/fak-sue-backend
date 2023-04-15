namespace Backend.Models;

public class FoodsDB_config
{
    public string Menu_Database_Name { get; set; }
    public string User_Database_Name { get; set; }
    public string Menu_Collection_Name { get; set; }
    public string User_Collection_Name { get; set; }
    public string Order_Collection_Name { get; set; }
    public string Announcement_Collection_Name { get; set; }
    public string Connection_String { get; set; }               //for guest
    public string Admin_Connection_String { get; set; }         //for admin
}