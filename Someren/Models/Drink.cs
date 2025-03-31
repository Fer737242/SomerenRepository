using System;
namespace Someren.Models
{
    public class Drink
    {
        public int Type { get; set; }

        public string Drink_name { get; set; }
        public string Alcoholic { get; set; }
        public decimal Price { get; set; }
        public int Voucher { get; set; }
    
    public Drink(int type, string drink_name, string alcoholic, decimal price, int voucher)
    {
        Type = type;
        Drink_name = drink_name;
        Alcoholic = alcoholic;
        Price = price;
        Voucher = voucher;
    }
    }
}

