using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyesWPF.Utils
{
    class MoreData
    {
        public static int ProdCount()
        {
            int count = 0;
            DateTime dateToday = DateTime.Now;

            foreach (var item in Transition.Context.ProductSale.ToList())
            {
                if ((item.SaleDate.Year == dateToday.Year - 1 && item.SaleDate.DayOfYear >= dateToday.DayOfYear)
                    || (item.SaleDate.Year == dateToday.Year && item.SaleDate.DayOfYear <= dateToday.DayOfYear))
                    count += item.ProductCount;
            }

            return count;
        }

        public static int Discount(int id)
        {
            decimal sum = 0;
            int discount;

            foreach (var item in Transition.Context.ProductSale.ToList().Where(p => p.AgentID == id))
            {
                var bb = Transition.Context.Product.ToList().First(p => p.ID == item.ProductID);
                sum += bb.MinCostForAgent * item.ProductCount;
            }

            if (sum < 10000)
                discount = 0;
            else if (sum >= 10000 && sum < 50000)
                discount = 5;
            else if (sum >= 50000 && sum < 150000)
                discount = 10;
            else if (sum >= 150000 && sum < 500000)
                discount = 20;
            else
                discount = 25;

            return discount;
        }
    }
}
