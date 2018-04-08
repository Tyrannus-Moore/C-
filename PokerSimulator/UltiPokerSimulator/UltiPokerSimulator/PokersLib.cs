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
        public int[] DisplayPoker()
        {
            int[] presentPossession=new int[possession.Count()];
            int i=0;//用于记录数组长度
            foreach (Poker item in possession)
            {
                presentPossession[i++] = item.ID;
            }
            return presentPossession;
        }
        public Poker ActivatePoker()
        {
            return possession.Last();
        }
        //现在不是自动游戏时代了
        //public Poker SearchPoker(Poker clickedPoker)
        //{//传进来的不能是玩家而应该是扑克了
        //    foreach (Poker item in possession)//泛型已出，千万别用for，迭代器记死
        //    {
        //        if (item.ID % 13 == clickedPoker.ID % 13)
        //        {
        //            return item;
        //        }
        //    }
        //    return null;
        //}
        public int CartPoker(Table table, Player p,Poker clickedPoker)
        {//如上改动
            Poker lastPoker = p.ActivatePoker();
            table.GainPoker(p.LosePoker());
            if (lastPoker.ID%13 == clickedPoker.ID%13)
            {
                int cartLock = 0;
                LinkedList<Poker> tempo = new LinkedList<Poker>();
                foreach (Poker item in possession)//获得牌
                {
                    if (item.ID == clickedPoker.ID)//开锁
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
                return 1;//成功加牌
            }
            else
            {
                return 0;//用户瞎点的，不予加牌
            }
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
        public void Amuse(Table table, Player p, Poker clickedPoker)
        {
            if (clickedPoker == null || clickedPoker.ID==-1)//若啥也没点，就直接把牌往上加
            {
                table.GainPoker(p.LosePoker());
            }
            else
            {
                table.CartPoker(table, p, clickedPoker);
            }
        }
        public Table UpdateObsoleteTable(string strSRecMsg)
        {
            Table neoTable = new Table();
            if (strSRecMsg != "-1" &&strSRecMsg!=null)
            {
                string[] strArray = strSRecMsg.Split(',');
                for (int i = 0; i < strArray.Length - 1; i++)
                {
                    Poker p = new Poker(int.Parse(strArray[i]));
                    neoTable.GainPoker(p);
                }
                return neoTable;
            }
            else
            {
                return neoTable;//千万不要直接Return null不然满城血雨！！
            }
        }
    }
}
