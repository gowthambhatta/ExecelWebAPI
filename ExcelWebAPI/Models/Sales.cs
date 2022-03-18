namespace ExcelWebAPI.Models
{
    public class Sales
    {
        public string Region { get; set; }
        public string Country { get; set; }
        public string Item_Type { get; set; }
        public string Sales_Channel { get; set; }
        public string Order_Priority  { get; set; }
        public string Order_Date { get; set; }
        public float Order_ID { get; set; }
        public string Ship_Date{ get; set; }
        public float Units_Sold { get; set; }	
        public float Unit_Price { get; set; }
        public float Unit_Cost { get; set; }
        public float Total_Revenue { get; set; }
        public float Total_Cost { get; set; }
        public float Total_Profit { get; set; }
    }
}
