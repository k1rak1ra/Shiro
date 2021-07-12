using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GN_Shiro
{
    static class BoardRenderer
    {
        public static void Render(int[] board)
        {
            //put board[] contents into Lines[]. Replace _0_ with _____
            String[] Lines = new String[9];
            Lines[0] = "______|___1__|___2__|___3__|___4__|___5__|___6__|___7__|___8__|";
            //array for dynamic line:
            for (int j = 1; j < 9; j++)
            {
                Lines[j] = "__"+(j*10)+"__|";
                for (int i = 1; i < 9; i++)
                {
                    if (board[j*10 + i] == 0)
                    {
                        Lines[j] = Lines[j] + "______|";
                    }
                    else
                    {
                        Lines[j] = Lines[j] + "_" + board[j*10 + i] + "_|";
                    }
                }
            }


            for (int i = 0; i < 9; i++)
            {
                Console.WriteLine(Lines[i]);
                LogWriter.Log(Lines[i]);
            }

            for (int i = 0; i < Program.WhitespaceAmount; i++)
            {
                Console.WriteLine("");
            }
        }
    }
}
