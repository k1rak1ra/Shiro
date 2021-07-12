using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace GN_Shiro
{
    class BasicallyMagic
    {

        public static int[] FindBestMove(int[] board)
        {

            //every run set possibilities projected to 0
            Program.PossibilitiesProjected = 0;

            //render game board at start
            Console.WriteLine("Rendering initial board...");
            BoardRenderer.Render(board);

            //now we choose the best move for the situation and render the board that the move would produce
            int BestMove = 0;
            int BestMoveScore = 0;
            int[] BestMoveBoard = new int[100];

            //start stopwatch for exec time readout at end
            Stopwatch sw = Stopwatch.StartNew();



            int[][] TreeLayer1 = new int[MoveRenderer.TallyPossibleMoves(board) + 2][];
            int[][][] TreeLayer2 = new int[MoveRenderer.TallyPossibleMoves(board) + 2][][];
            int[][][][] TreeLayer3 = new int[MoveRenderer.TallyPossibleMoves(board) + 2][][][];
            int[][][][][] TreeLayer4 = new int[MoveRenderer.TallyPossibleMoves(board) + 2][][][][];
            int[][][][][][] TreeLayer5 = new int[MoveRenderer.TallyPossibleMoves(board) + 2][][][][][];
            /*int[][][][][][][] TreeLayer6 = new int[MoveRenderer.TallyPossibleMoves(board) + 2][][][][][][];
            int[][][][][][][][] TreeLayer7 = new int[MoveRenderer.TallyPossibleMoves(board) + 2][][][][][][][];
            int[][][][][][][][][] TreeLayer8 = new int[MoveRenderer.TallyPossibleMoves(board) + 2][][][][][][][][];
            int[][][][][][][][][][] TreeLayer9 = new int[MoveRenderer.TallyPossibleMoves(board) + 2][][][][][][][][][];
            int[][][][][][][][][][][] TreeLayer10 = new int[MoveRenderer.TallyPossibleMoves(board) + 2][][][][][][][][][][];*/
 
            //  int[] Layer1Scores = new int[MoveRenderer.TallyPossibleMoves(board) + 2];
            int[] Layer1NumCountermoves = new int[MoveRenderer.TallyPossibleMoves(board) + 2];
            // int[][] Layer2Scores = new int[MoveRenderer.TallyPossibleMoves(board) + 2][];
            int[][] Layer2NumCountermoves = new int[MoveRenderer.TallyPossibleMoves(board) + 2][];
            // int[][][] Layer3Scores = new int[MoveRenderer.TallyPossibleMoves(board) + 2][][];
            int[][][] Layer3NumCountermoves = new int[MoveRenderer.TallyPossibleMoves(board) + 2][][];
            // int[][][][] Layer4Scores = new int[MoveRenderer.TallyPossibleMoves(board) + 2][][][];
            int[][][][] Layer4NumCountermoves = new int[MoveRenderer.TallyPossibleMoves(board) + 2][][][];
            //int[][][][][] Layer5Scores = new int[MoveRenderer.TallyPossibleMoves(board) + 2][][][][];
            int[][][][][] Layer5NumCountermoves = new int[MoveRenderer.TallyPossibleMoves(board) + 2][][][][];

            //Console.WriteLine("Layer 1:");
            Array.Copy(MoveRenderer.RenderFeindlyTree(board), TreeLayer1, MoveRenderer.TallyPossibleMoves(board) + 2);

            //choose a move that gives the enemy the least possible countermoves with the highest value (since enemy captures give negative numbers) and that results in the best possible reaction to the countermove
            Console.WriteLine("Rendering tree down to layer 5. Unless being run on Starsha, this will take a while...");
            LogWriter.Log("Rendering tree down to layer 5. Unless being run on Starsha, this will take a while...");
            Parallel.For(1, (MoveRenderer.TallyPossibleMoves(board) + 2), i =>
            {
                // Layer1Scores[i] = TreeLayer1[i][10];
                Layer1NumCountermoves[i] = EnemyMoveRenderer.TallyPossibleMoves(TreeLayer1[i]);
                // Layer2Scores[i] = new int[EnemyMoveRenderer.TallyPossibleMoves(TreeLayer1[i]) + 2];
                Layer2NumCountermoves[i] = new int[EnemyMoveRenderer.TallyPossibleMoves(TreeLayer1[i]) + 2];
                // Layer3Scores[i] = new int[EnemyMoveRenderer.TallyPossibleMoves(TreeLayer1[i]) + 2][];
                Layer3NumCountermoves[i] = new int[EnemyMoveRenderer.TallyPossibleMoves(TreeLayer1[i]) + 2][];
                // Layer4Scores[i] = new int[EnemyMoveRenderer.TallyPossibleMoves(TreeLayer1[i]) + 2][][];
                Layer4NumCountermoves[i] = new int[EnemyMoveRenderer.TallyPossibleMoves(TreeLayer1[i]) + 2][][];
                //Layer5Scores[i] = new int[EnemyMoveRenderer.TallyPossibleMoves(TreeLayer1[i]) + 2][][][];
                Layer5NumCountermoves[i] = new int[EnemyMoveRenderer.TallyPossibleMoves(TreeLayer1[i]) + 2][][][];

                //BoardRenderer.Render(TreeLayer1[i]);
                //layer 2 will be simulating all possible countermoves
                TreeLayer2[i] = new int[EnemyMoveRenderer.TallyPossibleMoves(TreeLayer1[i]) + 2][];
                TreeLayer3[i] = new int[EnemyMoveRenderer.TallyPossibleMoves(TreeLayer1[i]) + 2][][];
                TreeLayer4[i] = new int[EnemyMoveRenderer.TallyPossibleMoves(TreeLayer1[i]) + 2][][][];
                TreeLayer5[i] = new int[EnemyMoveRenderer.TallyPossibleMoves(TreeLayer1[i]) + 2][][][][];
                /* TreeLayer6[i] = new int[EnemyMoveRenderer.TallyPossibleMoves(TreeLayer1[i]) + 2][][][][][];
                 TreeLayer7[i] = new int[EnemyMoveRenderer.TallyPossibleMoves(TreeLayer1[i]) + 2][][][][][][];
                 TreeLayer8[i] = new int[EnemyMoveRenderer.TallyPossibleMoves(TreeLayer1[i]) + 2][][][][][][][];
                 TreeLayer9[i] = new int[EnemyMoveRenderer.TallyPossibleMoves(TreeLayer1[i]) + 2][][][][][][][][];
                 TreeLayer10[i] = new int[EnemyMoveRenderer.TallyPossibleMoves(TreeLayer1[i]) + 2][][][][][][][][][];*/
                //Console.WriteLine("Layer 2:");
                Parallel.For(1, (EnemyMoveRenderer.TallyPossibleMoves(TreeLayer1[i]) + 2), j =>
                {
                    TreeLayer2[i][j] = new int[100];
                });
                Array.Copy(EnemyMoveRenderer.RenderFeindlyTree(TreeLayer1[i]), TreeLayer2[i], EnemyMoveRenderer.TallyPossibleMoves(TreeLayer1[i]) + 2);
                LogWriter.Log("Layer 3:");
                Parallel.For(1, (EnemyMoveRenderer.TallyPossibleMoves(TreeLayer1[i]) + 2), j =>
                {
                    // Layer2Scores[i][j] = TreeLayer2[i][j][10];
                    Layer2NumCountermoves[i][j] = MoveRenderer.TallyPossibleMoves(TreeLayer2[i][j]);
                    // Layer3Scores[i][j] = new int[MoveRenderer.TallyPossibleMoves(TreeLayer2[i][j]) + 2];
                    Layer3NumCountermoves[i][j] = new int[MoveRenderer.TallyPossibleMoves(TreeLayer2[i][j]) + 2];
                    // Layer4Scores[i][j] = new int[MoveRenderer.TallyPossibleMoves(TreeLayer2[i][j]) + 2][];
                    Layer4NumCountermoves[i][j] = new int[MoveRenderer.TallyPossibleMoves(TreeLayer2[i][j]) + 2][];
                    //Layer5Scores[i][j] = new int[MoveRenderer.TallyPossibleMoves(TreeLayer2[i][j]) + 2][][];
                    //Layer5NumCountermoves[i][j] = new int[MoveRenderer.TallyPossibleMoves(TreeLayer2[i][j]) + 2][][];

                    TreeLayer3[i][j] = new int[MoveRenderer.TallyPossibleMoves(TreeLayer2[i][j]) + 2][];
                    TreeLayer4[i][j] = new int[MoveRenderer.TallyPossibleMoves(TreeLayer2[i][j]) + 2][][];
                    TreeLayer5[i][j] = new int[MoveRenderer.TallyPossibleMoves(TreeLayer2[i][j]) + 2][][][];
                    /*TreeLayer6[i][j] = new int[MoveRenderer.TallyPossibleMoves(TreeLayer2[i][j]) + 2][][][][];
                    TreeLayer7[i][j] = new int[MoveRenderer.TallyPossibleMoves(TreeLayer2[i][j]) + 2][][][][][];
                    TreeLayer8[i][j] = new int[MoveRenderer.TallyPossibleMoves(TreeLayer2[i][j]) + 2][][][][][][];
                    TreeLayer9[i][j] = new int[MoveRenderer.TallyPossibleMoves(TreeLayer2[i][j]) + 2][][][][][][][];
                    TreeLayer10[i][j] = new int[MoveRenderer.TallyPossibleMoves(TreeLayer2[i][j]) + 2][][][][][][][][];*/

                    Parallel.For(1, MoveRenderer.TallyPossibleMoves(TreeLayer2[i][j]) + 2, a =>
                    {
                        TreeLayer3[i][j][a] = new int[100];
                    });

                    //then layer 3 in here will simulate all possible freindly moves after enemy countermove j
                    Array.Copy(MoveRenderer.RenderFeindlyTree(TreeLayer2[i][j]), TreeLayer3[i][j], MoveRenderer.TallyPossibleMoves(TreeLayer2[i][j]) + 2);

                    //layer 4 will be another for loop inside here, more nested arrays, simulating all enemy countermoves to every move in array 3
                    Parallel.For(1, MoveRenderer.TallyPossibleMoves(TreeLayer2[i][j]) + 2, a =>
                    {
                        // Layer3Scores[i][j][a] = TreeLayer3[i][j][a][10];
                        Layer3NumCountermoves[i][j][a] = EnemyMoveRenderer.TallyPossibleMoves(TreeLayer3[i][j][a]);
                        //Layer4Scores[i][j][a] = new int[EnemyMoveRenderer.TallyPossibleMoves(TreeLayer3[i][j][a]) + 2];
                        Layer4NumCountermoves[i][j][a] = new int[EnemyMoveRenderer.TallyPossibleMoves(TreeLayer3[i][j][a]) + 2];
                        // Layer5Scores[i][j][a] = new int[EnemyMoveRenderer.TallyPossibleMoves(TreeLayer3[i][j][a]) + 2][];
                        //Layer5NumCountermoves[i][j][a] = new int[EnemyMoveRenderer.TallyPossibleMoves(TreeLayer3[i][j][a]) + 2][];


                        TreeLayer4[i][j][a] = new int[EnemyMoveRenderer.TallyPossibleMoves(TreeLayer3[i][j][a]) + 2][];
                        TreeLayer5[i][j][a] = new int[EnemyMoveRenderer.TallyPossibleMoves(TreeLayer3[i][j][a]) + 2][][];
                        /* TreeLayer6[i][j][a] = new int[EnemyMoveRenderer.TallyPossibleMoves(TreeLayer3[i][j][a]) + 2][][][];
                         TreeLayer7[i][j][a] = new int[EnemyMoveRenderer.TallyPossibleMoves(TreeLayer3[i][j][a]) + 2][][][][];
                         TreeLayer8[i][j][a] = new int[EnemyMoveRenderer.TallyPossibleMoves(TreeLayer3[i][j][a]) + 2][][][][][];
                         TreeLayer9[i][j][a] = new int[EnemyMoveRenderer.TallyPossibleMoves(TreeLayer3[i][j][a]) + 2][][][][][][];
                         TreeLayer10[i][j][a] = new int[EnemyMoveRenderer.TallyPossibleMoves(TreeLayer3[i][j][a]) + 2][][][][][][][];*/

                        Parallel.For(1, EnemyMoveRenderer.TallyPossibleMoves(TreeLayer3[i][j][a]) + 2, q =>
                        {
                            TreeLayer4[i][j][a][q] = new int[100];
                        });

                        Array.Copy(EnemyMoveRenderer.RenderFeindlyTree(TreeLayer3[i][j][a]), TreeLayer4[i][j][a], EnemyMoveRenderer.TallyPossibleMoves(TreeLayer3[i][j][a]) + 2);

                        //layer 5 will be another for loop inside here, simulating all possible freindly move to counter every possible enemy countermove calculated in layer 4
                        /* Parallel.For(1, EnemyMoveRenderer.TallyPossibleMoves(TreeLayer3[i][j][a]) + 2, q =>
                         {
                             //Layer4Scores[i][j][a][q] = TreeLayer4[i][j][a][q][10];
                             Layer4NumCountermoves[i][j][a][q] = MoveRenderer.TallyPossibleMoves(TreeLayer4[i][j][a][q]);
                            // Layer5Scores[i][j][a][q] = new int[MoveRenderer.TallyPossibleMoves(TreeLayer4[i][j][a][q]) + 2];
                             //Layer5NumCountermoves[i][j][a][q] = new int[MoveRenderer.TallyPossibleMoves(TreeLayer4[i][j][a][q]) + 2];

                             TreeLayer5[i][j][a][q] = new int[MoveRenderer.TallyPossibleMoves(TreeLayer4[i][j][a][q]) + 2][];
                             /*TreeLayer6[i][j][a][q] = new int[MoveRenderer.TallyPossibleMoves(TreeLayer4[i][j][a][q]) + 2][][];
                             TreeLayer7[i][j][a][q] = new int[MoveRenderer.TallyPossibleMoves(TreeLayer4[i][j][a][q]) + 2][][][];
                             TreeLayer8[i][j][a][q] = new int[MoveRenderer.TallyPossibleMoves(TreeLayer4[i][j][a][q]) + 2][][][][];
                             TreeLayer9[i][j][a][q] = new int[MoveRenderer.TallyPossibleMoves(TreeLayer4[i][j][a][q]) + 2][][][][][];
                             TreeLayer10[i][j][a][q] = new int[MoveRenderer.TallyPossibleMoves(TreeLayer4[i][j][a][q]) + 2][][][][][][];*/

                        //  Parallel.For(1, MoveRenderer.TallyPossibleMoves(TreeLayer4[i][j][a][q]) + 2, w =>
                        //  {
                        //       TreeLayer5[i][j][a][q][w] = new int[100];
                        ///   });

                        //   Array.Copy(MoveRenderer.RenderFeindlyTree(TreeLayer4[i][j][a][q]), TreeLayer5[i][j][a][q], MoveRenderer.TallyPossibleMoves(TreeLayer4[i][j][a][q]) + 2);

                        //layer 6 will be another for loop inside here, simulating all possible hostile countermoves to every possible freindly move in layer 5
                        //layer 6 uses up wayyyyy too much memory and has therefore been abandoned
                        /* Parallel.For(1, MoveRenderer.TallyPossibleMoves(TreeLayer4[i][j][a][q]) + 2, w =>
                         {
                             Layer5Scores[i][j][a][q][w] = TreeLayer5[i][j][a][q][w][10];
                             //Layer5NumCountermoves[i][j][a][q][w] = EnemyMoveRenderer.TallyPossibleMoves(TreeLayer5[i][j][a][q][w]);
                         });*/



                        // });*/



                    });



                });


            });




            Console.WriteLine("Operation took " + sw.Elapsed);
            LogWriter.Log("Operation took " + sw.Elapsed);
            Console.WriteLine("Number of possibilities projected this tree: " + Program.PossibilitiesProjected);
            LogWriter.Log("Number of possibilities projected this tree: " + Program.PossibilitiesProjected);
            if (sw.Elapsed.Seconds != 0)
            {
                Console.WriteLine("Computation rate: " + (Program.PossibilitiesProjected / sw.Elapsed.Seconds) + " possible boards/second");
                LogWriter.Log("Computation rate: " + (Program.PossibilitiesProjected / sw.Elapsed.Seconds) + " possible boards/second");
            }
            Console.WriteLine("");
            Console.WriteLine("Now evaluating moves that lead to the best possible outcome...");
            LogWriter.Log("Now evaluating moves that lead to the best possible outcome...");



            //Non-tired Ollie came back and put it all into nice arrays
            int[] L1BMS = new int[MoveRenderer.TallyPossibleMoves(board) + 1];
            int[][] L1BMB = new int[MoveRenderer.TallyPossibleMoves(board) + 1][];
            int[] L1BM = new int[MoveRenderer.TallyPossibleMoves(board) + 1];



            //now we try and deceide what the best move is... (I guess that this is what's called an algorithm)
            for (int i = 1; i < MoveRenderer.TallyPossibleMoves(board) + 1; i++)
            {
                L1BMB[i] = new int[100];

                L1BMS[i] = TreeLayer1[i][10];
                L1BM[i] = i;
                Array.Copy(TreeLayer1[i], L1BMB[i], 100);


            }


            int[] L1BMCS = new int[MoveRenderer.TallyPossibleMoves(board) + 1];
            L1BMCS[0] = -9999;
            int[] L1BMdiff = new int[MoveRenderer.TallyPossibleMoves(board) + 1];
            L1BMdiff[0] = -9999;    

            //start stopwatch for exec time readout at end
            Stopwatch sw2 = Stopwatch.StartNew();


            //switch this back to a normal for loop if something breaks or weird timing/addition issues occur
            Parallel.For(1, MoveRenderer.TallyPossibleMoves(board) + 1, i =>
            {
                int[] L2BMCS = new int[EnemyMoveRenderer.TallyPossibleMoves(TreeLayer1[L1BM[i]]) + 1];
                L2BMCS[0] = -9999;
                for (int j = 1; j < EnemyMoveRenderer.TallyPossibleMoves(TreeLayer1[L1BM[i]]) + 1; j++)
                {


                    int[] L3BMCS = new int[MoveRenderer.TallyPossibleMoves(TreeLayer2[L1BM[i]][j]) + 1];
                    L3BMCS[0] = -9999;
                    for (int a = 1; a < MoveRenderer.TallyPossibleMoves(TreeLayer2[L1BM[i]][j]) + 1; a++)
                    {


                        for (int q = 1; q < EnemyMoveRenderer.TallyPossibleMoves(TreeLayer3[L1BM[i]][j][a]) + 1; q++)
                        {


                                if (TreeLayer4[L1BM[i]][j][a][q][10] < L3BMCS[a])
                                {
                                    L3BMCS[a] = TreeLayer4[L1BM[i]][j][a][q][10];
                                }
                            

                        }


                            if (TreeLayer3[L1BM[i]][j][a][10] + L3BMCS[a] > L2BMCS[j])
                            {
                                L2BMCS[j] = TreeLayer3[L1BM[i]][j][a][10] + L3BMCS[a];
                            }
                        


                    }


                        if (TreeLayer2[L1BM[i]][j][10] + L2BMCS[j] < L1BMCS[i])
                        {
                            L1BMCS[i] = TreeLayer2[L1BM[i]][j][10] + L2BMCS[j];
                        }
                    


                }


            });

            Console.WriteLine("Evaluation complete!");
            LogWriter.Log("Evaluation Complete!");
            Console.WriteLine("Operation took " + sw2.Elapsed);
            LogWriter.Log("Operation took " + sw2.Elapsed);

            for (int i = 1; i < MoveRenderer.TallyPossibleMoves(board) + 1; i++)
            {
                L1BMdiff[i] = L1BMCS[i] + L1BMS[i];
            }

            int maxValue = L1BMdiff.Max();
            int maxIndex = L1BMdiff.ToList().IndexOf(maxValue);
            if (maxIndex == 0)
            {
                maxIndex = 1;
            }

            //we have the value and index of our move
            BestMoveScore = L1BMS[maxIndex];
            BestMove = L1BM[maxIndex];
            BestMoveBoard = L1BMB[maxIndex];








            Console.WriteLine("Best move is #" + BestMove + " with a score of " + BestMoveScore);
            LogWriter.Log("Best move is #" + BestMove + " with a score of " + BestMoveScore);
            BoardRenderer.Render(BestMoveBoard);

            return BestMoveBoard;

        }


    }
}
