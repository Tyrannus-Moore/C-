using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrePokerSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Poker[] poker = new Poker[54];
            Table table = new Table();
            Dealer dealer = new Dealer();
            Player p1 = new Player(1);
            Player p2 = new Player(2);
            dealer.InitialPoker(poker);//洗牌
            dealer.Deal(poker, p1, p2);//发牌
            dealer.Amuse(table, poker, p1, p2);
        }
    }
}
