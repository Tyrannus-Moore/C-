using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltiPokerSimulator
{
    class Poker
    {
        public int ID { set; get; }
        public Poker(int id)
        {
            ID = id;
        }
    }
    class Table
    {
        LinkedList<Poker> possession = new LinkedList<Poker>();
        public void GainPoker(Poker p)
        {
            possession.AddLast(p);
        }
        public Poker LosePoker()
        {
            if (possession.Count != 0)
            {
                Poker temp = null;//为什么弄个temp因为没办法读出删除一步走
                temp = possession.Last();
                possession.RemoveLast();
                return temp;
            }
            else
            {
                return null;
            }
        }
        public int CountPoker()
        {
            return possession.Count();
        }
        public Poker ActivatePoker()
        {
            return possession.Last();
        }
        public Poker SearchPoker(Player p)
        {
            Poker temp = p.ActivatePoker();//temp是电脑或玩家的ActivatePoker
            foreach (Poker item in possession)//泛型已出，千万别用for，迭代器记死
            {
                if (item.ID % 13 == temp.ID % 13)
                {
                    return item;
                }
            }
            return null;
        }
        public void CartPoker(Table table, Player p)
        {
            Poker trump = p.ActivatePoker();
            Poker cart = table.SearchPoker(p);
            table.GainPoker(p.LosePoker());
            int cartLock = 0;
            LinkedList<Poker> tempo = new LinkedList<Poker>();
            foreach (Poker item in possession)//获得牌
            {
                if (item == cart)//开锁
                {
                    cartLock = 1;
                }
                if (cartLock == 1)
                {
                    p.GainPoker(item);
                }
                else
                {
                    tempo.AddLast(item);
                }
            }
            table.possession.Clear();
            foreach (Poker item in tempo)
            {
                possession.AddLast(item);
            }
            tempo.Clear();
        }
        public void Surrender()
        {
            possession.Clear();
        }
    }
    class Player : Table
    {
        int ID { set; get; }
        public Player(int id)
        {
            ID = id;
        }
    }
    class Dealer
    {
        public void Shuffle(Poker[] poker)
        {
            Random r = new Random();
            for (int i = 53; i >= 0; --i)
            {
                Poker temp = null;
                int j = r.Next(0, i);
                temp = poker[i];
                poker[i] = poker[j];
                poker[j] = temp;
            }
        }
        public void InitialPoker(Poker[] poker)
        {
            for (int i = 0; i < 54; i++)
            {
                poker[i] = new Poker(i);
            }
            Shuffle(poker);
        }
        public void Deal(Poker[] poker, Player p1, Player p2)
        {
            for (int i = 0; i < 27; i++)//给电脑发牌
            {
                p1.GainPoker(poker[i]);
            }
            for (int i = 27; i < 54; i++)//给玩家发牌
            {
                p2.GainPoker(poker[i]);
            }
        }
        public void Amuse(Table table, Poker[] poker, Player p1, Player p2)
        {
            table.GainPoker(p1.LosePoker());//初放牌
            table.GainPoker(p2.LosePoker());
            while (p1.CountPoker() != 0 && p2.CountPoker() != 0)
            {
                if (table.SearchPoker(p1) != null)
                {
                    table.CartPoker(table, p1);
                }
                else table.GainPoker(p1.LosePoker());


                if (table.SearchPoker(p2) != null)
                {
                    table.CartPoker(table, p2);
                }
                else table.GainPoker(p2.LosePoker());
            }
        }
    }
}
