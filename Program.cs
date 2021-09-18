using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ConsoleTables;


namespace games
{
    class HMAC
    {
        public string keyMove;
        public int computersMove;

        public byte[] Key ()
        {
            byte[] bytes = new byte[16];

            var rng = new RNGCryptoServiceProvider();

            rng.GetBytes(bytes);

            keyMove = BitConverter.ToString(bytes).Replace("-", "");

            return bytes;
        }
        public void KeyGeneration(string[] args)
        {
            byte[] bytes = Key ();

            computersMove = new Random().Next(args.Length);

            byte[] textInBytes = Encoding.UTF8.GetBytes(args[computersMove]);

            var hmac = new HMACSHA256(bytes);

            Console.WriteLine("HMAC:"+ "\n"+ BitConverter.ToString(hmac.ComputeHash(textInBytes)).Replace("-", ""));

        }
        public int GetComputersMove()
        {
            return computersMove;
        }

        public void GetkeyMove()
        {
            Console.WriteLine("HMAC key: " + keyMove);
        }

    }

    class Menu
    {
        public int playersmove;
        public void MenuGeneration(string[] args)
        {
            Table table = new Table();

            while (true)
            {
                Console.WriteLine("Available moves:");

                for (int i = 0; i < args.Length; i++)
                {
                    Console.WriteLine(i + 1 + " - " + args[i]);
                }

                Console.WriteLine(0 + " - exit \n? - Help");

                Console.Write("Enter your move: ");

                string str = Console.ReadLine();

                bool a = int.TryParse(str, out playersmove);

                if (str == "?")
                {
                    table.tableGeneration(args);
                }
                else if (a == false)
                {

                }
                else if (args.Length >= playersmove && playersmove > 0)
                {
                    Console.WriteLine("Your move: " + args[playersmove - 1]);
                    break;
                }
                else if (playersmove == 0)
                {
                    Process.GetCurrentProcess().Kill();
                }
            }
        }
        public int GetPlayersMove()
        {
            return (playersmove - 1);
        }

    }
    public class Rules
    {
        private int cmove;
        private int pmove;
        
        public void SetComputersMove(int computersmove)
        {
            cmove = computersmove;
        }

        public void SetPlayersMove(int playersmove)
        {
            pmove = playersmove;
        }

        public void WinnerDetermination(string[] args)
        {
            Console.WriteLine("Computer move: " + args[cmove]);

            int half = (args.Length - 1) / 2;
            
            if (cmove == pmove)
            {
                Console.WriteLine("Draw");
                
            }
            else if ( (pmove < cmove && Math.Abs(pmove - cmove) <= half)||(pmove > cmove && Math.Abs(pmove - cmove) > half))
            {
                Console.WriteLine("You Win");
            }
            else 
            {
                Console.WriteLine("You Lose");
            }
        }
    }
    public class Table
    {
        public  string[] header;
        
        public string[] row;
        public void tableGeneration(string[] args)
        {
            header = new string[args.Length + 1];

            string[] row = new string[args.Length + 1];

            Array.Copy(args, 0, header, 1, args.Length);

            header[0] = " Pc \\ User";

            TableFilling(args);
            
        }
        public void TableFilling(string [] args)
        {  
            var table = new ConsoleTable(header);

            for (int i = 0; i < args.Length; i++)
            {
                RulesFilling(i, args);

                table.AddRow(row);
            }
            table.Write();
        }

        public string[] RulesFilling (int i, string[] args)
        {
            row = new string[args.Length + 1];

            int half = (args.Length - 1) / 2;

            row[0] = args[i];
            for (int j = 0; j < args.Length; j++)
            {
                if (i == j)
                {
                    row[j + 1] = "Draw";

                }
                else if ((j < i && Math.Abs(j - i) <= half) || (j > i && Math.Abs(j - i) > half))
                {
                    row[j + 1] = "Win";
                }
                else
                {
                    row[j + 1] = "Luse";
                }
            }
            return row;  
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            var duplicates = args.GroupBy(n => n).Where(g => g.Count() > 1);

            if (duplicates.Count() > 0 || args.Length < 2 || args.Length % 2 == 0)
            {
                Console.WriteLine("Error, the data was entered incorrectly. Example - rock paper scissors");

                Process.GetCurrentProcess().Kill();
            }
            
            HMAC key = new HMAC ();
            Menu menu = new Menu();
            Rules rules = new Rules();
            Table table = new Table();



            key.KeyGeneration(args);

            menu.MenuGeneration(args);

            rules.SetComputersMove(key.GetComputersMove());
            
            rules.SetPlayersMove(menu.GetPlayersMove());

            rules.WinnerDetermination(args);

            key.GetkeyMove();

          //  table.tableGeneration(args);

        }
    }
}
