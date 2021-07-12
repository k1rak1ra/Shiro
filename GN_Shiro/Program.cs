using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using System.Threading;
using System.Diagnostics;

//OPERATION NOTES:
//Players will be labelled 1 and 2.
//Each piece will have a number id, preceded by player ID
//If a pawn if piece ID 1, a player 1 pawn will be 11 in DB and player 2 will be 21
//Once player has made a move, column "move" where yAxis is 99 will be set to other player
//Other player makes move, changes it back, etc.
//If a piece is captured, it is outright removed from the table
//I should probably make a reset program/SQL script to restore
//PIECE IDs:
//1 = pawn
//5 = rook, the tower thing
//2 = knight, the horsey
//3 = bishop
//7 = queen
//9 = king

//TRY NEW PIECE WEIGHTS:
//1 = pawn
//2 = knight, the horsey
//3 = bishop
//4 = tower
//5 = queen
//9 = king


//for now, I'll just brute-force every possible move
//I also want to get alg to work out as many turns ahead as possible
//try to capture the highest value piece using the lowest value piece




//BOARD IMAGINED BY THE VARS IN PROGRAM:
//_____|__1__|__2__|__3__|__4__|__5__|__6__|__7__|__8__|
//__1__|_251_|_221_|_231_|_271_|_291_|_232_|_222_|_252_|
//__2__|_211_|_212_|_213_|_214_|_215_|_216_|_217_|_218_|
//__3__|_____|_____|_____|_____|_____|_____|_____|_____|
//__4__|_____|_____|_____|_____|_____|_____|_____|_____|
//__5__|_____|_____|_____|_____|_____|_____|_____|_____|
//__6__|_____|_____|_____|_____|_____|_____|_____|_____|
//__7__|_111_|_112_|_113_|_114_|_115_|_116_|_117_|_118_|
//__8__|_151_|_121_|_131_|_171_|_191_|_132_|_122_|_152_|


//assign point values to pieces depending on how much they're worth and then choose moves based on how many points they're worth





namespace GN_Shiro
{


    //ROUNDING DOWN TOOL TO GET ABS VALUE OF PIECES
    public static class ExtensionMethods
    {
        public static int Fix(this int i)
        {
            return (int)(Math.Floor(i / 10.0d) * 10);
        }
    }

    class Program
    {
        //CONFIG
        public static int PlayerNo = 1;
        public static int VerboseMode = 1;
        public static int LoggingEnabled = 1;
        public static int WhitespaceAmount = 1; //was 20, reduced to 1


        //VARS
        public static int[] board = new int[100];
        public static int[] PendingBoard = new int[100];
        public static int PlayerTurn = 1;
        public static int PossibilitiesProjected = 0;
        public static String reply = "";
        public static String p1 = "";
        public static String p2 = "";


        static void Main(string[] args)
        {
            
            //POPULATE BOARD ARRAY (0 for no piece, refer to board comment for piece IDs
            //ROW 1
            board[11] = 2051;
            board[12] = 2021;
            board[13] = 2031;
            board[14] = 2071;
            board[15] = 2901;
            board[16] = 2032;
            board[17] = 2022;
            board[18] = 2052;
            //ROW 2
            board[21] = 2011;
            board[22] = 2012;
            board[23] = 2013;
            board[24] = 2014;
            board[25] = 2015;
            board[26] = 2016;
            board[27] = 2017;
            board[28] = 2018;
            //ROW 3
            board[31] = 0;
            board[32] = 0;
            board[33] = 0;
            board[34] = 0;
            board[35] = 0;
            board[36] = 0;
            board[37] = 0;
            board[38] = 0;
            //ROW 4
            board[41] = 0;
            board[42] = 0;
            board[43] = 0;
            board[44] = 0;
            board[45] = 0;
            board[46] = 0;
            board[47] = 0;
            board[48] = 0;
            //ROW 5
            board[51] = 0;
            board[52] = 0;
            board[53] = 0;
            board[54] = 0;
            board[55] = 0;
            board[56] = 0;
            board[57] = 0;
            board[58] = 0;
            //ROW 6
            board[61] = 0;
            board[62] = 0;
            board[63] = 0;
            board[64] = 0;
            board[65] = 0;
            board[66] = 0;
            board[67] = 0;
            board[68] = 0;
            //ROW 7
            board[71] = 1011;
            board[72] = 1012;
            board[73] = 1013;
            board[74] = 1014;
            board[75] = 1015;
            board[76] = 1016;
            board[77] = 1017;
            board[78] = 1018;
            //ROW 8
            board[81] = 1051;
            board[82] = 1021;
            board[83] = 1031;
            board[84] = 1071;
            board[85] = 1901;
            board[86] = 1032;
            board[87] = 1022;
            board[88] = 1052;
 



            //at beginning of game
            Console.Clear();
            Console.WriteLine("Shiro Version -actuallyfullyworkingIRL:1 -indev:4.43243248723");
            Console.Write("Are you Ollie? (y/n) ");
            reply = Console.ReadLine();

            if (reply == "y")
            {
                Console.WriteLine("ONIIIICHANNNNNNNN!!!! I will win for you ^-^");
                LogWriter.Log("EXECUTION HAS BEGIN!");
                Thread.Sleep(500);
                Console.Write("Are you moving first (y/n) ");
                reply = Console.ReadLine();
                if (reply == "y")
                {
                    board[74] = 0;
                    board[54] = 1014;

                    LogWriter.Log("Pre-programmed opening move performed!");

                }
                //loop in which everything runs
                while (true)
                {
                    //human turn
                    Console.Clear();
                    BoardRenderer.Render(board);
                    Console.Write("Enter position being moved: ");
                    p1 = Console.ReadLine();
                    Console.Write("Enter destination position: ");
                    p2 = Console.ReadLine();
                    board[int.Parse(p2)] = board[int.Parse(p1)];
                    board[int.Parse(p1)] = 0;
                    Console.WriteLine("Move completed!");
                    Thread.Sleep(500);

                    //Shiro's turn
                    Console.Clear();
                    board = BasicallyMagic.FindBestMove(board);
                    Console.WriteLine("");
                    Console.WriteLine("Move completed!");
                    Console.Write("Continue? ");
                    reply = Console.ReadLine();

                    //chance to enter commands to execute
                    Console.Clear();
                    Console.Write("Enter command: (manedit/next) ");
                    reply = Console.ReadLine();

                    //board editor mode
                    while (reply == "manedit")
                    {
                        Console.Clear();
                        Console.WriteLine("MANUAL BOARD EDITING MODE ENTERED");
                        BoardRenderer.Render(board);
                        Console.Write("Position to edit: ");
                        p1 = Console.ReadLine();
                        Console.Write("Set position to: ");
                        p2 = Console.ReadLine();
                        board[int.Parse(p1)] = int.Parse(p2);

                        Console.Write("Enter command: (manedit/next) ");
                        reply = Console.ReadLine();

                    }




                }






            }
            else if (reply == "n")
            {
                Console.WriteLine("Prepare to lose m8");

                //loop in which everything runs
                while (true)
                {
                    //human turn
                    Console.Clear();
                    BoardRenderer.Render(board);
                    Console.Write("Enter position being moved: ");
                    p1 = Console.ReadLine();
                    Console.Write("Enter destination position: ");
                    p2 = Console.ReadLine();
                    board[int.Parse(p2)] = board[int.Parse(p1)];
                    board[int.Parse(p1)] = 0;
                    Console.WriteLine("Move completed!");
                    Thread.Sleep(500);

                    //Shiro's turn
                    Console.Clear();
                    board = BasicallyMagic.FindBestMove(board);
                    Console.WriteLine("");
                    Console.WriteLine("Move completed!");
                    Console.Write("Continue? ");
                    reply = Console.ReadLine();

                    //chance to enter commands to execute
                    Console.Clear();
                    Console.Write("Enter command: (manedit/next) ");
                    reply = Console.ReadLine();

                    //board editor mode
                    while (reply == "manedit")
                    {
                        Console.Clear();
                        Console.WriteLine("MANUAL BOARD EDITING MODE ENTERED");
                        BoardRenderer.Render(board);
                        Console.Write("Position to edit: ");
                        p1 = Console.ReadLine();
                        Console.Write("Set position to: ");
                        p2 = Console.ReadLine();
                        board[int.Parse(p1)] = int.Parse(p2);

                        Console.Write("Enter command: (manedit/next) ");
                        reply = Console.ReadLine();

                    }


                }
            }
        }
    }
}
