﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DelegatesApp
{
    // Интерфейс подписчика на изменения цен в акциях
    public interface ISubscriber
    {
        void Notified(object sender, MarketEventArgs args);
    }

    public class MarketEventArgs
    {
        /*Здесь можно добавить что угодно, цену, название компании и т.п.*/
        public MarketEventArgs(string name, int newPrice, int oldPrice)
        {
            Name = name;
            NewPrice = newPrice;
            OldPrice = oldPrice;
        }

        public string Name { get; set; }
        public int NewPrice { get; set; }
        public int OldPrice { get; set; }
    }

    // Биржа, торгующая акциями
    public class Market
    {
        public delegate void MarketDelegate(object sender, MarketEventArgs args);

        public event MarketDelegate Notify;

        public Dictionary<string, int> Shares { get; }
        private Random random;

        // Устанавливаем цены акций, хз как правильно они называются
        // В параметр передаем список имен акций
        public Market(string[] shares)
        {
            Shares = new Dictionary<string, int>();
            random = new Random();
            foreach (var share in shares)
            {
                Shares.Add(share, random.Next(100, 1000));
            }
        }

        // Добавляет подписчика
        public void AddSubscriber(ISubscriber sub)
        {
            Notify += sub.Notified;
        }

        // Запускает ход торгов (устанавливает цену акциям) и уведомляет подписчиков
        public void Trade()
        {
            // При реализации можно взять рандомную акцию, выставить у неё цену (тоже рандомно в некоторых пределах)
            // и проинформировать подписчиков
            var enumerator = Shares.GetEnumerator();
            var num = random.Next(Shares.Count);
            for (var i = 0; i < num; i++)
            {
                enumerator.MoveNext();
            }

            var oldPrice = enumerator.Current.Value;
            var newPrice = random.Next(100, 1000);
            Shares[enumerator.Current.Key] = newPrice;
            Notify?.Invoke(this, new MarketEventArgs(enumerator.Current.Key,
                newPrice, oldPrice));
            enumerator.Dispose();
        }

        /// <summary>
        /// Покупка акций
        /// </summary>
        /// <param name="shareName">Имя акции</param>
        /// <param name="amount">Количество покупаемых акций</param>
        /// <param name="count">Ссылка на акции брокера</param>
        /// <param name="account">Ссылка на счет брокера</param>
        public void Buy(string shareName, int amount,
            ref int count, ref int account)
        {
            int price;
            try
            {
                price = Shares[shareName];
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("It's empty, stupid!");
                return;
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine($"There is no share named \"{shareName}\"");
                return;
            }

            var sum = price * amount;
            if (sum > account)
            {
                Console.WriteLine("You don't have enough money!");
                return;
            }

            account -= sum;
            count += amount;
        }


        public void Sell(string shareName, int amount,
            ref int count, ref int account)
        {
            int price;
            try
            {
                price = Shares[shareName];
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("It's empty, stupid!");
                return;
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine($"There is no share named \"{shareName}\"");
                return;
            }

            if (count < amount)
            {
                Console.WriteLine("You don't have enough shares!");
                return;
            }

            count -= amount;
            account += amount * price;
        }
    }
}
