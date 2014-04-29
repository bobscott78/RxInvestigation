using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using NUnit.Framework;

namespace ShoppingKata
{
    public class ShoppingKataTests
    {
        [TestCase("A", 50)]
        [TestCase("B", 30)]
        [TestCase("C", 20)]
        [TestCase("A,A", 100)]
        [TestCase("A,A,A", 130)]
        [TestCase("A,A,A,A,A,A", 260)]
        [TestCase("B,B", 45)]
        public void WhenScanningABasket(string basketItems, int expectedTotal)
        {
            var basket = basketItems.Split(',').ToObservable();
            var checkout = new Checkout();
            var receipt = checkout.Scan(basket);
            Assert.That(receipt.Select(r => r.Amount).Sum().Last(), Is.EqualTo(expectedTotal));
        } 
    }

    public class Checkout
    {
        public IObservable<BillItem> Scan(IObservable<string> basket)
        {
            var prices = new Pricer().Price(basket);
            var discounts = new Discounter().Discount(basket);

            return prices.Merge(discounts);
        }
    }

    public class Pricer
    {
        public IObservable<BillItem> Price(IObservable<string> basket)
        {
            return basket.Select(Price);
        }

        private BillItem Price(string item)
        {
            return !_prices.ContainsKey(item) ? new BillItem(0) : new BillItem(_prices[item]);
        }

        private readonly IDictionary<string, int> _prices = new Dictionary<string, int>
            {
                {"A", 50},
                {"B", 30},
                {"C", 20}
            };
    }

    public class Discounter
    {
        public IObservable<BillItem> Discount(IObservable<string> basket)
        {
            return basket
                .Select(DiscountItem)
                .Where(i => i.Amount < 0);
        }

        private BillItem DiscountItem(string item)
        {
            if (!_counter.ContainsKey(item))
            {
                _counter.Add(item, 1);
            }
            else
            {
                _counter[item]++;
            }
            
            if ((item == "A") && (_counter[item] % 3 == 0))
            {
                return new BillItem(-20);
            }
            if ((item == "B") && (_counter[item]%2 == 0))
            {
                return new BillItem(-15);
            }
            return new BillItem(0);
        }

        private readonly IDictionary<string, int> _counter = new Dictionary<string, int>(); 
    }

    public class BillItem
    {
        public int Amount { get; private set; }

        public BillItem(int amount)
        {
            Amount = amount;
        }
    }
}