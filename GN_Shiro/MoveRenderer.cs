using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GN_Shiro
{
    public static class MoveRenderer
    {
        public static int[][] RenderFeindlyTree(int[] board)
        {
            int[] TmpBoard = new int[100];

            //now FOR THE ACTUAL LOGIC
            int EnemyNo = 0;
            if (Program.PlayerNo == 1) { EnemyNo = 2; } else { EnemyNo = 1; }

            int EnemyIdentifierNumber = EnemyNo * 1000;
            int IdentifierNumber = Program.PlayerNo * 1000;
            int[] FreindlyPieces = new int[17];
            int[] FreindlyPieceLocations = new int[17];

            int[] EnemyPieces = new int[17];
            int[] EnemyPieceLocations = new int[17];

            int[] PawnDoubleMove = new int[9];
            int[] EnemyPawnDoubleMove = new int[9];

            int KingLocation = 0;
            int EnemyKingLocation = 0;

            int NumPossibleMoves = 0;
            int NumPossibleEnemyMoves = 0;

            int[][] TreeBoards = new int[TallyPossibleMoves(board) + 2][];
            //treeboards cannot be all null so create a empty array that will be filled
            for (int i = 1; i < TreeBoards.Length; i++)
            {
                TreeBoards[i] = new int[100];
            }



            int[] TreeScores = new int[TallyPossibleMoves(board) + 2];


                //Figure out what pieces are available and where
                int SecondaryIndexer = 0;
                int TertiaryIndexer = 0;
                for (int i = 10; i < 90; i++)
                {
                    //if freindly piece, add to freindly array
                    if (board[i] - IdentifierNumber < 1000 && board[i] - IdentifierNumber > 10)
                    {
                        FreindlyPieces[SecondaryIndexer] = board[i] - IdentifierNumber;
                        if (board[i] - IdentifierNumber == 901)
                        {
                            KingLocation = i;
                        }
                        FreindlyPieceLocations[SecondaryIndexer] = i;
                        SecondaryIndexer++;
                    }
                    //if enemy, add to enemy array
                    if (board[i] - EnemyIdentifierNumber < 1000 && board[i] - EnemyIdentifierNumber > 10)
                    {
                        EnemyPieces[TertiaryIndexer] = board[i] - EnemyIdentifierNumber;
                        if (board[i] - EnemyIdentifierNumber == 901)
                        {
                            EnemyKingLocation = i;
                        }
                        EnemyPieceLocations[TertiaryIndexer] = i;
                        TertiaryIndexer++;
                    }
                }

                //print out pieces and their absolute values as a test
                /*
                LogWriter.Log("You:");
                for (int i = 0; i < 16; i++)
                {
                    LogWriter.Log(FreindlyPieces[i].Fix() + "---" + FreindlyPieceLocations[i]);
                }
                LogWriter.Log("Enemies:");
                for (int i = 0; i < 16; i++)
                {
                    LogWriter.Log(EnemyPieces[i].Fix() + "---" + EnemyPieceLocations[i]);
                }*/


                //determine possible moves that can be made, no castling or pawn promotion for now. Assume player is player 1
                for (int i = 0; i < 16; i++)
                {
                    //if piece is a pawn
                    if (FreindlyPieces[i] > 10 && FreindlyPieces[i] < 20)
                    {
                        LogWriter.Log("Pawn id#" + FreindlyPieces[i]);
                        //check for feasability of double moves:
                        if (FreindlyPieceLocations[i] - 70 == FreindlyPieces[i] - 10 && FreindlyPieceLocations[i].Fix() == 70 && board[FreindlyPieceLocations[i] - 20] == 0 && board[FreindlyPieceLocations[i] - 10] == 0)
                        {
                            Array.Copy(board, TmpBoard, 100);
                            TmpBoard[FreindlyPieceLocations[i]] = 0;
                            TmpBoard[FreindlyPieceLocations[i] - 20] = board[FreindlyPieceLocations[i]];

                            if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                            {
                                LogWriter.Log(" Double move:");
                                LogWriter.Log(" " + FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] - 20));
                                NumPossibleMoves++;

                                //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                TreeScores[NumPossibleMoves] = 0;

                            }
                        }
                        //check availability of single moves:
                        if (board[FreindlyPieceLocations[i] - 10] == 0)
                        {
                            Array.Copy(board, TmpBoard, 100);
                            TmpBoard[FreindlyPieceLocations[i]] = 0;
                            TmpBoard[FreindlyPieceLocations[i] - 10] = board[FreindlyPieceLocations[i]];

                            if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                            {
                                LogWriter.Log("   Single move:");
                                LogWriter.Log("   " + FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] - 10));
                                NumPossibleMoves++;

                                //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                TreeScores[NumPossibleMoves] = 0;
                            }
                        }
                        //check availability of capture moves, and list # of possible points gained:
                        if (board[FreindlyPieceLocations[i] - 11].Fix() > 0 || board[FreindlyPieceLocations[i] - 9].Fix() > 0)
                        {
                            //check column the piece is in. Col1 only allows capture to the right, col8 only allows to the left because it's the edge of the board
                            //if piece is in columns 2 through 7
                            if (FreindlyPieceLocations[i] - (FreindlyPieceLocations[i].Fix() + 1) != 0 && FreindlyPieceLocations[i] - (FreindlyPieceLocations[i].Fix() + 8) != 0)
                            {
                                //if square up and to the left has a piece on it and is not freindly
                                if (board[FreindlyPieceLocations[i] - 11].Fix() > 0 && board[FreindlyPieceLocations[i] - 11].Fix() - 2000 > 0)
                                {
                                    Array.Copy(board, TmpBoard, 100);
                                    TmpBoard[FreindlyPieceLocations[i]] = 0;
                                    TmpBoard[FreindlyPieceLocations[i] - 11] = board[FreindlyPieceLocations[i]];

                                    if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                    {

                                        LogWriter.Log(" Capture move to the left:");
                                        LogWriter.Log(" " + FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] - 11));
                                        LogWriter.Log(" Captures:" + board[FreindlyPieceLocations[i] - 11] + ", Valued at " + (board[FreindlyPieceLocations[i] - 11].Fix() - 2000));
                                        NumPossibleMoves++;

                                        //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                        Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                        TreeScores[NumPossibleMoves] = (board[FreindlyPieceLocations[i] - 11].Fix() - 2000);
                                    }
                                }
                                //if square up and to the right has a piece on it and is not freindly
                                if (board[FreindlyPieceLocations[i] - 9].Fix() > 0 && board[FreindlyPieceLocations[i] - 9].Fix() - 2000 > 0)
                                {
                                    Array.Copy(board, TmpBoard, 100);
                                    TmpBoard[FreindlyPieceLocations[i]] = 0;
                                    TmpBoard[FreindlyPieceLocations[i] - 9] = board[FreindlyPieceLocations[i]];

                                    if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                    {
                                        LogWriter.Log(" Capture move to the right:");
                                        LogWriter.Log(" " + FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] - 9));
                                        LogWriter.Log(" Captures:" + board[FreindlyPieceLocations[i] - 9] + ", Valued at " + (board[FreindlyPieceLocations[i] - 9].Fix() - 2000));
                                        NumPossibleMoves++;

                                        //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                        Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                        TreeScores[NumPossibleMoves] = (board[FreindlyPieceLocations[i] - 9].Fix() - 2000);
                                    }
                                }
                            }
                            //if piece is in column 1
                            if (FreindlyPieceLocations[i] - (FreindlyPieceLocations[i].Fix() + 1) == 0)
                            {
                                //if square up and to the right has a piece on it and is not freindly
                                if (board[FreindlyPieceLocations[i] - 9].Fix() > 0 && board[FreindlyPieceLocations[i] - 9].Fix() - 2000 > 0)
                                {
                                    Array.Copy(board, TmpBoard, 100);
                                    TmpBoard[FreindlyPieceLocations[i]] = 0;
                                    TmpBoard[FreindlyPieceLocations[i] - 9] = board[FreindlyPieceLocations[i]];

                                    if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                    {
                                        LogWriter.Log(" Capture move to the right:");
                                        LogWriter.Log(" " + FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] - 9));
                                        LogWriter.Log(" Captures:" + board[FreindlyPieceLocations[i] - 9] + ", Valued at " + (board[FreindlyPieceLocations[i] - 9].Fix() - 2000));
                                        NumPossibleMoves++;

                                        //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                        Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                        TreeScores[NumPossibleMoves] = (board[FreindlyPieceLocations[i] - 9].Fix() - 2000);
                                    }
                                }
                            }
                            //if piece is in column 8
                            if (FreindlyPieceLocations[i] - (FreindlyPieceLocations[i].Fix() + 8) == 0)
                            {
                                //if square up and to the left has a piece on it and is not freindly
                                if (board[FreindlyPieceLocations[i] - 11].Fix() > 0 && board[FreindlyPieceLocations[i] - 11].Fix() - 2000 > 0)
                                {
                                    Array.Copy(board, TmpBoard, 100);
                                    TmpBoard[FreindlyPieceLocations[i]] = 0;
                                    TmpBoard[FreindlyPieceLocations[i] - 11] = board[FreindlyPieceLocations[i]];

                                    if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                    {
                                        LogWriter.Log(" Capture move to the left:");
                                        LogWriter.Log(" " + FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] - 11));
                                        LogWriter.Log(" Captures:" + board[FreindlyPieceLocations[i] - 11] + ", Valued at " + (board[FreindlyPieceLocations[i] - 11].Fix() - 2000));
                                        NumPossibleMoves++;

                                        //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                        Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                        TreeScores[NumPossibleMoves] = (board[FreindlyPieceLocations[i] - 11].Fix() - 2000);
                                    }
                                }
                            }

                        }

                        //add line of whitespace
                        LogWriter.Log("");


                    }
                    //END all possible moves for pawns
                    //if piece is a tower
                    if (FreindlyPieces[i] > 50 && FreindlyPieces[i] < 60)
                    {
                        LogWriter.Log("Tower id#" + FreindlyPieces[i]);
                        //vertical moves up
                        for (int j = 1; j < 8; j++)
                        {
                            //if target space is empty
                            if (board[FreindlyPieceLocations[i] - (10 * j)] == 0)
                            {
                                //just break immediately if the end of the board is reached
                                if (FreindlyPieceLocations[i].Fix() - (10 * j) == 0)
                                {
                                    break;
                                }
                                else
                                {
                                    Array.Copy(board, TmpBoard, 100);
                                    TmpBoard[FreindlyPieceLocations[i]] = 0;
                                    TmpBoard[FreindlyPieceLocations[i] - (10 * j)] = board[FreindlyPieceLocations[i]];

                                    if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                    {
                                        LogWriter.Log(" Move vertically up " + j);
                                        LogWriter.Log(FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] - (10 * j)));
                                        NumPossibleMoves++;

                                        //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                        Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                        TreeScores[NumPossibleMoves] = 0;
                                    }
                                }
                            }
                            //if target space has a piece on it
                            else
                            {
                                //if piece belongs to the enemy
                                if (board[FreindlyPieceLocations[i] - (10 * j)].Fix() - 2000 > 0)
                                {
                                    Array.Copy(board, TmpBoard, 100);
                                    TmpBoard[FreindlyPieceLocations[i]] = 0;
                                    TmpBoard[FreindlyPieceLocations[i] - (10 * j)] = board[FreindlyPieceLocations[i]];

                                    if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                    {
                                        LogWriter.Log(" Capture move " + j + " spaces up");
                                        LogWriter.Log(" " + FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] - (10 * j)));
                                        LogWriter.Log(" Captures:" + board[FreindlyPieceLocations[i] - (10 * j)] + ", Valued at " + (board[FreindlyPieceLocations[i] - (10 * j)].Fix() - 2000));
                                        NumPossibleMoves++;

                                        //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                        Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                        TreeScores[NumPossibleMoves] = (board[FreindlyPieceLocations[i] - (10 * j)].Fix() - 2000);
                                    }
                                    break;
                                }
                                //break if it's freindly
                                else
                                {
                                    break;
                                }

                            }

                        }
                        //vertical moves down
                        for (int j = 1; j < 8; j++)
                        {
                            //if target space is empty
                            if (board[FreindlyPieceLocations[i] + (10 * j)] == 0)
                            {
                                //just break immediately if the end of the board is reached
                                if (FreindlyPieceLocations[i].Fix() + (10 * j) == 90)
                                {
                                    break;
                                }
                                else
                                {
                                    Array.Copy(board, TmpBoard, 100);
                                    TmpBoard[FreindlyPieceLocations[i]] = 0;
                                    TmpBoard[FreindlyPieceLocations[i] + (10 * j)] = board[FreindlyPieceLocations[i]];

                                    if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                    {
                                        LogWriter.Log("         Move vertically down " + j);
                                        LogWriter.Log(FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] + (10 * j)));
                                        NumPossibleMoves++;

                                        //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                        Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                        TreeScores[NumPossibleMoves] = 0;
                                    }
                                }
                            }
                            //if target space has a piece on it
                            else
                            {
                                //if piece belongs to the enemy
                                if (board[FreindlyPieceLocations[i] + (10 * j)].Fix() - 2000 > 0)
                                {
                                    Array.Copy(board, TmpBoard, 100);
                                    TmpBoard[FreindlyPieceLocations[i]] = 0;
                                    TmpBoard[FreindlyPieceLocations[i] + (10 * j)] = board[FreindlyPieceLocations[i]];

                                    if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                    {
                                        LogWriter.Log("      Capture move " + j + " spaces down");
                                        LogWriter.Log("      " + FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] + (10 * j)));
                                        LogWriter.Log("         Captures:" + board[FreindlyPieceLocations[i] + (10 * j)] + ", Valued at " + (board[FreindlyPieceLocations[i] + (10 * j)].Fix() - 2000));
                                        NumPossibleMoves++;

                                        //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                        Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                        TreeScores[NumPossibleMoves] = (board[FreindlyPieceLocations[i] + (10 * j)].Fix() - 2000);
                                    }
                                    break;
                                }
                                //break if it's freindly
                                else
                                {
                                    break;
                                }

                            }

                        }
                        //horizontal moves right
                        for (int j = 1; j < 8; j++)
                        {
                            //if target space is empty
                            if (board[FreindlyPieceLocations[i] + j] == 0)
                            {
                                //just break immediately if the end of the board is reached
                                if (FreindlyPieceLocations[i] + j == FreindlyPieceLocations[i].Fix() + 9)
                                {
                                    break;
                                }
                                else
                                {
                                    Array.Copy(board, TmpBoard, 100);
                                    TmpBoard[FreindlyPieceLocations[i]] = 0;
                                    TmpBoard[FreindlyPieceLocations[i] + j] = board[FreindlyPieceLocations[i]];

                                    if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                    {
                                        LogWriter.Log("     Move horizontally right " + j);
                                        LogWriter.Log("     " + FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] + j));
                                        NumPossibleMoves++;

                                        //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                        Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                        TreeScores[NumPossibleMoves] = 0;
                                    }
                                }
                            }
                            //if target space has a piece on it
                            else
                            {
                                //if piece belongs to the enemy
                                if (board[FreindlyPieceLocations[i] + j].Fix() - 2000 > 0)
                                {
                                    Array.Copy(board, TmpBoard, 100);
                                    TmpBoard[FreindlyPieceLocations[i]] = 0;
                                    TmpBoard[FreindlyPieceLocations[i] + j] = board[FreindlyPieceLocations[i]];

                                    if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                    {
                                        LogWriter.Log("  Capture move " + j + " spaces right");
                                        LogWriter.Log("     " + FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] + j));
                                        LogWriter.Log("  Captures:" + board[FreindlyPieceLocations[i] + j] + ", Valued at " + (board[FreindlyPieceLocations[i] + j].Fix() - 2000));
                                        NumPossibleMoves++;

                                        //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                        Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                        TreeScores[NumPossibleMoves] = (board[FreindlyPieceLocations[i] + j].Fix() - 2000);
                                    }
                                    break;
                                }
                                //break if it's freindly
                                else
                                {
                                    break;
                                }

                            }

                        }
                        //horizontal moves left
                        for (int j = 1; j < 8; j++)
                        {
                            //if target space is empty
                            if (board[FreindlyPieceLocations[i] - j] == 0)
                            {
                                //just break immediately if the end of the board is reached
                                if (FreindlyPieceLocations[i] - j == FreindlyPieceLocations[i].Fix())
                                {
                                    break;
                                }
                                else
                                {
                                    Array.Copy(board, TmpBoard, 100);
                                    TmpBoard[FreindlyPieceLocations[i]] = 0;
                                    TmpBoard[FreindlyPieceLocations[i] - j] = board[FreindlyPieceLocations[i]];

                                    if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                    {
                                        LogWriter.Log("   Move horizontally left " + j);
                                        LogWriter.Log("   " + FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] - j));
                                        NumPossibleMoves++;

                                        //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                        Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                        TreeScores[NumPossibleMoves] = 0;
                                    }
                                }
                            }
                            //if target space has a piece on it
                            else
                            {
                                //if piece belongs to the enemy
                                if (board[FreindlyPieceLocations[i] - j].Fix() - 2000 > 0)
                                {
                                    Array.Copy(board, TmpBoard, 100);
                                    TmpBoard[FreindlyPieceLocations[i]] = 0;
                                    TmpBoard[FreindlyPieceLocations[i] - j] = board[FreindlyPieceLocations[i]];

                                    if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                    {
                                        LogWriter.Log("Capture move " + j + " spaces left");
                                        LogWriter.Log("" + FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] - j));
                                        LogWriter.Log("Captures:" + board[FreindlyPieceLocations[i] - j] + ", Valued at " + (board[FreindlyPieceLocations[i] - j].Fix() - 2000));
                                        NumPossibleMoves++;

                                        //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                        Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                        TreeScores[NumPossibleMoves] = (board[FreindlyPieceLocations[i] - j].Fix() - 2000);
                                    }
                                    break;
                                }
                                //break if it's freindly
                                else
                                {
                                    break;
                                }

                            }

                        }






                        //add line of whitespace
                        LogWriter.Log("");


                    }
                    //END all possible moves for towers
                    //if piece is a knight
                    if (FreindlyPieces[i] > 20 && FreindlyPieces[i] < 30)
                    {
                        LogWriter.Log("Knight id#" + FreindlyPieces[i]);

                        int[] PossibleKnightMoves = new int[9];
                        //moves up and to the left
                        PossibleKnightMoves[0] = -21;
                        PossibleKnightMoves[1] = -12;
                        //moves down and to the left
                        PossibleKnightMoves[2] = 19;
                        PossibleKnightMoves[3] = 8;
                        //moves up and to the right
                        PossibleKnightMoves[4] = -19;
                        PossibleKnightMoves[5] = -8;
                        //moves down and to the right
                        PossibleKnightMoves[6] = 21;
                        PossibleKnightMoves[7] = 12;



                        for (int j = 0; j < 9; j++)
                        {

                            //if move does not exceed bounds of board
                            //up and left bounds
                            if (FreindlyPieceLocations[i] + PossibleKnightMoves[j] > (FreindlyPieceLocations[i] + PossibleKnightMoves[j]).Fix() && (FreindlyPieceLocations[i] + PossibleKnightMoves[j]).Fix() > 10
                            //below and right bounds
                            && FreindlyPieceLocations[i] + PossibleKnightMoves[j] < (FreindlyPieceLocations[i] + PossibleKnightMoves[j]).Fix() + 9 && (FreindlyPieceLocations[i] + PossibleKnightMoves[j]).Fix() < 90)
                            {
                                //if move is to an empty space
                                if (board[FreindlyPieceLocations[i] + PossibleKnightMoves[j]].Fix() == 0)
                                {
                                    Array.Copy(board, TmpBoard, 100);
                                    TmpBoard[FreindlyPieceLocations[i]] = 0;
                                    TmpBoard[FreindlyPieceLocations[i] + PossibleKnightMoves[j]] = board[FreindlyPieceLocations[i]];

                                    if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                    {
                                        LogWriter.Log("Move: " + PossibleKnightMoves[j]);
                                        LogWriter.Log(FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] + PossibleKnightMoves[j]));
                                        NumPossibleMoves++;

                                        //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                        Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                        TreeScores[NumPossibleMoves] = 0;
                                    }
                                }

                                //if move is to space with an enemy
                                if (board[FreindlyPieceLocations[i] + PossibleKnightMoves[j]].Fix() > 0 && board[FreindlyPieceLocations[i] + PossibleKnightMoves[j]].Fix() - 2000 > 0)
                                {
                                    Array.Copy(board, TmpBoard, 100);
                                    TmpBoard[FreindlyPieceLocations[i]] = 0;
                                    TmpBoard[FreindlyPieceLocations[i] + PossibleKnightMoves[j]] = board[FreindlyPieceLocations[i]];

                                    if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                    {
                                        LogWriter.Log(" Capture move: " + PossibleKnightMoves[j]);
                                        LogWriter.Log(" " + FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] + PossibleKnightMoves[j]));
                                        LogWriter.Log(" Captures:" + board[FreindlyPieceLocations[i] + PossibleKnightMoves[j]] + ", Valued at " + (board[FreindlyPieceLocations[i] + PossibleKnightMoves[j]].Fix() - 2000));
                                        NumPossibleMoves++;

                                        //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                        Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                        TreeScores[NumPossibleMoves] = (board[FreindlyPieceLocations[i] + PossibleKnightMoves[j]].Fix() - 2000);
                                    }
                                }
                            }

                        }

                        //add line of whitespace
                        LogWriter.Log("");


                    }
                    //END all possible moves for knights
                    //if piece is a bishop
                    if (FreindlyPieces[i] > 30 && FreindlyPieces[i] < 40)
                    {
                        LogWriter.Log("Bishop id#" + FreindlyPieces[i]);

                        //up and to the left
                        for (int j = 1; j < 8; j++)
                        {
                            //if target space is empty
                            if (board[FreindlyPieceLocations[i] - (11 * j)] == 0)
                            {
                                //just break immediately if the end of the board is reached
                                if (FreindlyPieceLocations[i].Fix() - (10 * j) == 0 || FreindlyPieceLocations[i] - j == FreindlyPieceLocations[i].Fix())
                                {
                                    break;
                                }
                                else
                                {
                                    Array.Copy(board, TmpBoard, 100);
                                    TmpBoard[FreindlyPieceLocations[i]] = 0;
                                    TmpBoard[FreindlyPieceLocations[i] - (11 * j)] = board[FreindlyPieceLocations[i]];

                                    if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                    {
                                        LogWriter.Log(" Move up and to the left " + j);
                                        LogWriter.Log(FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] - (11 * j)));
                                        NumPossibleMoves++;

                                        //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                        Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                        TreeScores[NumPossibleMoves] = 0;
                                    }
                                }
                            }
                            //if target space has a piece on it
                            else
                            {
                                //if piece belongs to the enemy
                                if (board[FreindlyPieceLocations[i] - (11 * j)].Fix() - 2000 > 0)
                                {
                                    Array.Copy(board, TmpBoard, 100);
                                    TmpBoard[FreindlyPieceLocations[i]] = 0;
                                    TmpBoard[FreindlyPieceLocations[i] - (11 * j)] = board[FreindlyPieceLocations[i]];

                                    if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                    {
                                        LogWriter.Log(" Capture move " + j + " spaces up and to the left");
                                        LogWriter.Log(" " + FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] - (11 * j)));
                                        LogWriter.Log(" Captures:" + board[FreindlyPieceLocations[i] - (11 * j)] + ", Valued at " + (board[FreindlyPieceLocations[i] - (11 * j)].Fix() - 2000));
                                        NumPossibleMoves++;

                                        //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                        Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                        TreeScores[NumPossibleMoves] = (board[FreindlyPieceLocations[i] - (11 * j)].Fix() - 2000);
                                    }
                                    break;
                                }
                                //break if it's freindly
                                else
                                {
                                    break;
                                }

                            }

                        }
                        //vertical moves down and to the right
                        for (int j = 1; j < 8; j++)
                        {
                            //if target space is empty
                            if (board[FreindlyPieceLocations[i] + (11 * j)] == 0)
                            {
                                //just break immediately if the end of the board is reached
                                if (FreindlyPieceLocations[i].Fix() + (10 * j) == 90 || FreindlyPieceLocations[i] + j == FreindlyPieceLocations[i].Fix() + 9)
                                {
                                    break;
                                }
                                else
                                {
                                    Array.Copy(board, TmpBoard, 100);
                                    TmpBoard[FreindlyPieceLocations[i]] = 0;
                                    TmpBoard[FreindlyPieceLocations[i] + (11 * j)] = board[FreindlyPieceLocations[i]];

                                    if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                    {
                                        LogWriter.Log("         Move down and to the right " + j);
                                        LogWriter.Log(FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] + (11 * j)));
                                        NumPossibleMoves++;

                                        //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                        Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                        TreeScores[NumPossibleMoves] = 0;
                                    }
                                }
                            }
                            //if target space has a piece on it
                            else
                            {
                                //if piece belongs to the enemy
                                if (board[FreindlyPieceLocations[i] + (11 * j)].Fix() - 2000 > 0)
                                {
                                    Array.Copy(board, TmpBoard, 100);
                                    TmpBoard[FreindlyPieceLocations[i]] = 0;
                                    TmpBoard[FreindlyPieceLocations[i] + (11 * j)] = board[FreindlyPieceLocations[i]];

                                    if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                    {
                                        LogWriter.Log("      Capture move " + j + " spaces down and to the right");
                                        LogWriter.Log("      " + FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] + (11 * j)));
                                        LogWriter.Log("         Captures:" + board[FreindlyPieceLocations[i] + (11 * j)] + ", Valued at " + (board[FreindlyPieceLocations[i] + (11 * j)].Fix() - 2000));
                                        NumPossibleMoves++;

                                        //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                        Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                        TreeScores[NumPossibleMoves] = (board[FreindlyPieceLocations[i] + (11 * j)].Fix() - 2000);
                                    }
                                    break;
                                }
                                //break if it's freindly
                                else
                                {
                                    break;
                                }

                            }

                        }
                        //up and to the right
                        for (int j = 1; j < 8; j++)
                        {
                            //if target space is empty
                            if (board[FreindlyPieceLocations[i] - (9 * j)] == 0)
                            {
                                //just break immediately if the end of the board is reached
                                if (FreindlyPieceLocations[i].Fix() - (10 * j) == 0 || FreindlyPieceLocations[i] + j == FreindlyPieceLocations[i].Fix() + 9)
                                {
                                    break;
                                }
                                else
                                {
                                    Array.Copy(board, TmpBoard, 100);
                                    TmpBoard[FreindlyPieceLocations[i]] = 0;
                                    TmpBoard[FreindlyPieceLocations[i] - (9 * j)] = board[FreindlyPieceLocations[i]];

                                    if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                    {
                                        LogWriter.Log(" Move up and to the right " + j);
                                        LogWriter.Log(FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] - (9 * j)));
                                        NumPossibleMoves++;

                                        //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                        Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                        TreeScores[NumPossibleMoves] = 0;
                                    }
                                }
                            }
                            //if target space has a piece on it
                            else
                            {
                                //if piece belongs to the enemy
                                if (board[FreindlyPieceLocations[i] - (9 * j)].Fix() - 2000 > 0)
                                {
                                    Array.Copy(board, TmpBoard, 100);
                                    TmpBoard[FreindlyPieceLocations[i]] = 0;
                                    TmpBoard[FreindlyPieceLocations[i] - (9 * j)] = board[FreindlyPieceLocations[i]];

                                    if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                    {
                                        LogWriter.Log(" Capture move " + j + " spaces up and to the right");
                                        LogWriter.Log(" " + FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] - (9 * j)));
                                        LogWriter.Log(" Captures:" + board[FreindlyPieceLocations[i] - (9 * j)] + ", Valued at " + (board[FreindlyPieceLocations[i] - (9 * j)].Fix() - 2000));
                                        NumPossibleMoves++;

                                        //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                        Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                        TreeScores[NumPossibleMoves] = (board[FreindlyPieceLocations[i] - (9 * j)].Fix() - 2000);
                                    }
                                    break;
                                }
                                //break if it's freindly
                                else
                                {
                                    break;
                                }

                            }

                        }
                        //vertical moves down and to the left
                        for (int j = 1; j < 8; j++)
                        {
                            //if target space is empty
                            if (board[FreindlyPieceLocations[i] + (9 * j)] == 0)
                            {
                                //just break immediately if the end of the board is reached
                                if (FreindlyPieceLocations[i].Fix() + (10 * j) == 90 || FreindlyPieceLocations[i] - j == FreindlyPieceLocations[i].Fix())
                                {
                                    break;
                                }
                                else
                                {
                                    Array.Copy(board, TmpBoard, 100);
                                    TmpBoard[FreindlyPieceLocations[i]] = 0;
                                    TmpBoard[FreindlyPieceLocations[i] + (9 * j)] = board[FreindlyPieceLocations[i]];

                                    if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                    {
                                        LogWriter.Log("         Move down and to the left " + j);
                                        LogWriter.Log(FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] + (9 * j)));
                                        NumPossibleMoves++;

                                        //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                        Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                        TreeScores[NumPossibleMoves] = 0;
                                    }
                                }
                            }
                            //if target space has a piece on it
                            else
                            {
                                //if piece belongs to the enemy
                                if (board[FreindlyPieceLocations[i] + (9 * j)].Fix() - 2000 > 0)
                                {
                                    Array.Copy(board, TmpBoard, 100);
                                    TmpBoard[FreindlyPieceLocations[i]] = 0;
                                    TmpBoard[FreindlyPieceLocations[i] + (9 * j)] = board[FreindlyPieceLocations[i]];

                                    if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                    {
                                        LogWriter.Log("      Capture move " + j + " spaces down and to the left");
                                        LogWriter.Log("      " + FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] + (9 * j)));
                                        LogWriter.Log("         Captures:" + board[FreindlyPieceLocations[i] + (9 * j)] + ", Valued at " + (board[FreindlyPieceLocations[i] + (9 * j)].Fix() - 2000));
                                        NumPossibleMoves++;

                                        //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                        Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                        TreeScores[NumPossibleMoves] = (board[FreindlyPieceLocations[i] + (9 * j)].Fix() - 2000);
                                    }
                                    break;
                                }
                                //break if it's freindly
                                else
                                {
                                    break;
                                }

                            }

                        }
                        //add line of whitespace
                        LogWriter.Log("");


                    }
                    //END all possible moves for bishops
                    //if piece is a queen
                    if (FreindlyPieces[i] > 70 && FreindlyPieces[i] < 80)
                    {
                        LogWriter.Log("Queen id#" + FreindlyPieces[i]);


                        //vertical moves up
                        for (int j = 1; j < 8; j++)
                        {
                            //if target space is empty
                            if (board[FreindlyPieceLocations[i] - (10 * j)] == 0)
                            {
                                //just break immediately if the end of the board is reached
                                if (FreindlyPieceLocations[i].Fix() - (10 * j) == 0)
                                {
                                    break;
                                }
                                else
                                {
                                    Array.Copy(board, TmpBoard, 100);
                                    TmpBoard[FreindlyPieceLocations[i]] = 0;
                                    TmpBoard[FreindlyPieceLocations[i] - (10 * j)] = board[FreindlyPieceLocations[i]];

                                    if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                    {
                                        LogWriter.Log(" Move vertically up " + j);
                                        LogWriter.Log(FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] - (10 * j)));
                                        NumPossibleMoves++;

                                        //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                        Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                        TreeScores[NumPossibleMoves] = 0;
                                    }
                                }
                            }
                            //if target space has a piece on it
                            else
                            {
                                //if piece belongs to the enemy
                                if (board[FreindlyPieceLocations[i] - (10 * j)].Fix() - 2000 > 0)
                                {
                                    Array.Copy(board, TmpBoard, 100);
                                    TmpBoard[FreindlyPieceLocations[i]] = 0;
                                    TmpBoard[FreindlyPieceLocations[i] - (10 * j)] = board[FreindlyPieceLocations[i]];

                                    if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                    {
                                        LogWriter.Log(" Capture move " + j + " spaces up");
                                        LogWriter.Log(" " + FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] - (10 * j)));
                                        LogWriter.Log(" Captures:" + board[FreindlyPieceLocations[i] - (10 * j)] + ", Valued at " + (board[FreindlyPieceLocations[i] - (10 * j)].Fix() - 2000));
                                        NumPossibleMoves++;

                                        //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                        Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                        TreeScores[NumPossibleMoves] = (board[FreindlyPieceLocations[i] - (10 * j)].Fix() - 2000);
                                    }
                                    break;
                                }
                                //break if it's freindly
                                else
                                {
                                    break;
                                }

                            }

                        }
                        //vertical moves down
                        for (int j = 1; j < 8; j++)
                        {
                            //if target space is empty
                            if (board[FreindlyPieceLocations[i] + (10 * j)] == 0)
                            {
                                //just break immediately if the end of the board is reached
                                if (FreindlyPieceLocations[i].Fix() + (10 * j) == 90)
                                {
                                    break;
                                }
                                else
                                {
                                    Array.Copy(board, TmpBoard, 100);
                                    TmpBoard[FreindlyPieceLocations[i]] = 0;
                                    TmpBoard[FreindlyPieceLocations[i] + (10 * j)] = board[FreindlyPieceLocations[i]];

                                    if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                    {
                                        LogWriter.Log("         Move vertically down " + j);
                                        LogWriter.Log(FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] + (10 * j)));
                                        NumPossibleMoves++;

                                        //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                        Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                        TreeScores[NumPossibleMoves] = 0;
                                    }
                                }
                            }
                            //if target space has a piece on it
                            else
                            {
                                //if piece belongs to the enemy
                                if (board[FreindlyPieceLocations[i] + (10 * j)].Fix() - 2000 > 0)
                                {
                                    Array.Copy(board, TmpBoard, 100);
                                    TmpBoard[FreindlyPieceLocations[i]] = 0;
                                    TmpBoard[FreindlyPieceLocations[i] + (10 * j)] = board[FreindlyPieceLocations[i]];

                                    if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                    {
                                        LogWriter.Log("      Capture move " + j + " spaces down");
                                        LogWriter.Log("      " + FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] + (10 * j)));
                                        LogWriter.Log("         Captures:" + board[FreindlyPieceLocations[i] + (10 * j)] + ", Valued at " + (board[FreindlyPieceLocations[i] + (10 * j)].Fix() - 2000));
                                        NumPossibleMoves++;

                                        //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                        Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                        TreeScores[NumPossibleMoves] = (board[FreindlyPieceLocations[i] + (10 * j)].Fix() - 2000);
                                    }
                                    break;
                                }
                                //break if it's freindly
                                else
                                {
                                    break;
                                }

                            }

                        }
                        //horizontal moves right
                        for (int j = 1; j < 8; j++)
                        {
                            //if target space is empty
                            if (board[FreindlyPieceLocations[i] + j] == 0)
                            {
                                //just break immediately if the end of the board is reached
                                if (FreindlyPieceLocations[i] + j == FreindlyPieceLocations[i].Fix() + 9)
                                {
                                    break;
                                }
                                else
                                {
                                    Array.Copy(board, TmpBoard, 100);
                                    TmpBoard[FreindlyPieceLocations[i]] = 0;
                                    TmpBoard[FreindlyPieceLocations[i] + j] = board[FreindlyPieceLocations[i]];

                                    if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                    {
                                        LogWriter.Log("     Move horizontally right " + j);
                                        LogWriter.Log("     " + FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] + j));
                                        NumPossibleMoves++;

                                        //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                        Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                        TreeScores[NumPossibleMoves] = 0;
                                    }
                                }
                            }
                            //if target space has a piece on it
                            else
                            {
                                //if piece belongs to the enemy
                                if (board[FreindlyPieceLocations[i] + j].Fix() - 2000 > 0)
                                {
                                    Array.Copy(board, TmpBoard, 100);
                                    TmpBoard[FreindlyPieceLocations[i]] = 0;
                                    TmpBoard[FreindlyPieceLocations[i] + j] = board[FreindlyPieceLocations[i]];

                                    if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                    {
                                        LogWriter.Log("  Capture move " + j + " spaces right");
                                        LogWriter.Log("     " + FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] + j));
                                        LogWriter.Log("  Captures:" + board[FreindlyPieceLocations[i] + j] + ", Valued at " + (board[FreindlyPieceLocations[i] + j].Fix() - 2000));
                                        NumPossibleMoves++;

                                        //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                        Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                        TreeScores[NumPossibleMoves] = (board[FreindlyPieceLocations[i] + j].Fix() - 2000);
                                    }
                                    break;
                                }
                                //break if it's freindly
                                else
                                {
                                    break;
                                }

                            }

                        }
                        //horizontal moves left
                        for (int j = 1; j < 8; j++)
                        {
                            //if target space is empty
                            if (board[FreindlyPieceLocations[i] - j] == 0)
                            {
                                //just break immediately if the end of the board is reached
                                if (FreindlyPieceLocations[i] - j == FreindlyPieceLocations[i].Fix())
                                {
                                    break;
                                }
                                else
                                {
                                    Array.Copy(board, TmpBoard, 100);
                                    TmpBoard[FreindlyPieceLocations[i]] = 0;
                                    TmpBoard[FreindlyPieceLocations[i] - j] = board[FreindlyPieceLocations[i]];

                                    if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                    {
                                        LogWriter.Log("   Move horizontally left " + j);
                                        LogWriter.Log("   " + FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] - j));
                                        NumPossibleMoves++;

                                        //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                        Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                        TreeScores[NumPossibleMoves] = 0;
                                    }
                                }
                            }
                            //if target space has a piece on it
                            else
                            {
                                //if piece belongs to the enemy
                                if (board[FreindlyPieceLocations[i] - j].Fix() - 2000 > 0)
                                {
                                    Array.Copy(board, TmpBoard, 100);
                                    TmpBoard[FreindlyPieceLocations[i]] = 0;
                                    TmpBoard[FreindlyPieceLocations[i] - j] = board[FreindlyPieceLocations[i]];

                                    if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                    {
                                        LogWriter.Log("Capture move " + j + " spaces left");
                                        LogWriter.Log("" + FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] - j));
                                        LogWriter.Log("Captures:" + board[FreindlyPieceLocations[i] - j] + ", Valued at " + (board[FreindlyPieceLocations[i] - j].Fix() - 2000));
                                        NumPossibleMoves++;

                                        //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                        Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                        TreeScores[NumPossibleMoves] = (board[FreindlyPieceLocations[i] - j].Fix() - 2000);
                                    }
                                    break;
                                }
                                //break if it's freindly
                                else
                                {
                                    break;
                                }

                            }

                        }
                        //up and to the left
                        for (int j = 1; j < 8; j++)
                        {
                            //if target space is empty
                            if (board[FreindlyPieceLocations[i] - (11 * j)] == 0)
                            {
                                //just break immediately if the end of the board is reached
                                if (FreindlyPieceLocations[i].Fix() - (10 * j) == 0 || FreindlyPieceLocations[i] - j == FreindlyPieceLocations[i].Fix())
                                {
                                    break;
                                }
                                else
                                {
                                    Array.Copy(board, TmpBoard, 100);
                                    TmpBoard[FreindlyPieceLocations[i]] = 0;
                                    TmpBoard[FreindlyPieceLocations[i] - (11 * j)] = board[FreindlyPieceLocations[i]];

                                    if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                    {
                                        LogWriter.Log(" Move up and to the left " + j);
                                        LogWriter.Log(FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] - (11 * j)));
                                        NumPossibleMoves++;

                                        //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                        Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                        TreeScores[NumPossibleMoves] = 0;
                                    }
                                }
                            }
                            //if target space has a piece on it
                            else
                            {
                                //if piece belongs to the enemy
                                if (board[FreindlyPieceLocations[i] - (11 * j)].Fix() - 2000 > 0)
                                {
                                    Array.Copy(board, TmpBoard, 100);
                                    TmpBoard[FreindlyPieceLocations[i]] = 0;
                                    TmpBoard[FreindlyPieceLocations[i] - (11 * j)] = board[FreindlyPieceLocations[i]];

                                    if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                    {
                                        LogWriter.Log(" Capture move " + j + " spaces up and to the left");
                                        LogWriter.Log(" " + FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] - (11 * j)));
                                        LogWriter.Log(" Captures:" + board[FreindlyPieceLocations[i] - (11 * j)] + ", Valued at " + (board[FreindlyPieceLocations[i] - (11 * j)].Fix() - 2000));
                                        NumPossibleMoves++;

                                        //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                        Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                        TreeScores[NumPossibleMoves] = (board[FreindlyPieceLocations[i] - (11 * j)].Fix() - 2000);
                                    }
                                    break;
                                }
                                //break if it's freindly
                                else
                                {
                                    break;
                                }

                            }

                        }
                        //vertical moves down and to the right
                        for (int j = 1; j < 8; j++)
                        {
                            //if target space is empty
                            if (board[FreindlyPieceLocations[i] + (11 * j)] == 0)
                            {
                                //just break immediately if the end of the board is reached
                                if (FreindlyPieceLocations[i].Fix() + (10 * j) == 90 || FreindlyPieceLocations[i] + j == FreindlyPieceLocations[i].Fix() + 9)
                                {
                                    break;
                                }
                                else
                                {
                                    Array.Copy(board, TmpBoard, 100);
                                    TmpBoard[FreindlyPieceLocations[i]] = 0;
                                    TmpBoard[FreindlyPieceLocations[i] + (11 * j)] = board[FreindlyPieceLocations[i]];

                                    if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                    {
                                        LogWriter.Log("         Move down and to the right " + j);
                                        LogWriter.Log(FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] + (11 * j)));
                                        NumPossibleMoves++;

                                        //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                        Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                        TreeScores[NumPossibleMoves] = 0;
                                    }
                                }
                            }
                            //if target space has a piece on it
                            else
                            {
                                //if piece belongs to the enemy
                                if (board[FreindlyPieceLocations[i] + (11 * j)].Fix() - 2000 > 0)
                                {
                                    Array.Copy(board, TmpBoard, 100);
                                    TmpBoard[FreindlyPieceLocations[i]] = 0;
                                    TmpBoard[FreindlyPieceLocations[i] + (11 * j)] = board[FreindlyPieceLocations[i]];

                                    if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                    {
                                        LogWriter.Log("      Capture move " + j + " spaces down and to the right");
                                        LogWriter.Log("      " + FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] + (11 * j)));
                                        LogWriter.Log("         Captures:" + board[FreindlyPieceLocations[i] + (11 * j)] + ", Valued at " + (board[FreindlyPieceLocations[i] + (11 * j)].Fix() - 2000));
                                        NumPossibleMoves++;

                                        //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                        Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                        TreeScores[NumPossibleMoves] = (board[FreindlyPieceLocations[i] + (11 * j)].Fix() - 2000);
                                    }
                                    break;
                                }
                                //break if it's freindly
                                else
                                {
                                    break;
                                }

                            }

                        }
                        //up and to the right
                        for (int j = 1; j < 8; j++)
                        {
                            //if target space is empty
                            if (board[FreindlyPieceLocations[i] - (9 * j)] == 0)
                            {
                                //just break immediately if the end of the board is reached
                                if (FreindlyPieceLocations[i].Fix() - (10 * j) == 0 || FreindlyPieceLocations[i] + j == FreindlyPieceLocations[i].Fix() + 9)
                                {
                                    break;
                                }
                                else
                                {
                                    Array.Copy(board, TmpBoard, 100);
                                    TmpBoard[FreindlyPieceLocations[i]] = 0;
                                    TmpBoard[FreindlyPieceLocations[i] - (9 * j)] = board[FreindlyPieceLocations[i]];

                                    if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                    {
                                        LogWriter.Log(" Move up and to the right " + j);
                                        LogWriter.Log(FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] - (9 * j)));
                                        NumPossibleMoves++;

                                        //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                        Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                        TreeScores[NumPossibleMoves] = 0;
                                    }
                                }
                            }
                            //if target space has a piece on it
                            else
                            {
                                //if piece belongs to the enemy
                                if (board[FreindlyPieceLocations[i] - (9 * j)].Fix() - 2000 > 0)
                                {
                                    Array.Copy(board, TmpBoard, 100);
                                    TmpBoard[FreindlyPieceLocations[i]] = 0;
                                    TmpBoard[FreindlyPieceLocations[i] - (9 * j)] = board[FreindlyPieceLocations[i]];

                                    if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                    {
                                        LogWriter.Log(" Capture move " + j + " spaces up and to the right");
                                        LogWriter.Log(" " + FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] - (9 * j)));
                                        LogWriter.Log(" Captures:" + board[FreindlyPieceLocations[i] - (9 * j)] + ", Valued at " + (board[FreindlyPieceLocations[i] - (9 * j)].Fix() - 2000));
                                        NumPossibleMoves++;

                                        //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                        Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                        TreeScores[NumPossibleMoves] = (board[FreindlyPieceLocations[i] - (9 * j)].Fix() - 2000);
                                    }
                                    break;
                                }
                                //break if it's freindly
                                else
                                {
                                    break;
                                }

                            }

                        }
                        //vertical moves down and to the left
                        for (int j = 1; j < 8; j++)
                        {
                            //if target space is empty
                            if (board[FreindlyPieceLocations[i] + (9 * j)] == 0)
                            {
                                //just break immediately if the end of the board is reached
                                if (FreindlyPieceLocations[i].Fix() + (10 * j) == 90 || FreindlyPieceLocations[i] - j == FreindlyPieceLocations[i].Fix())
                                {
                                    break;
                                }
                                else
                                {
                                    Array.Copy(board, TmpBoard, 100);
                                    TmpBoard[FreindlyPieceLocations[i]] = 0;
                                    TmpBoard[FreindlyPieceLocations[i] + (9 * j)] = board[FreindlyPieceLocations[i]];

                                    if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                    {
                                        LogWriter.Log("         Move down and to the left " + j);
                                        LogWriter.Log(FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] + (9 * j)));
                                        NumPossibleMoves++;

                                        //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                        Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                        TreeScores[NumPossibleMoves] = 0;
                                    }
                                }
                            }
                            //if target space has a piece on it
                            else
                            {
                                //if piece belongs to the enemy
                                if (board[FreindlyPieceLocations[i] + (9 * j)].Fix() - 2000 > 0)
                                {
                                    Array.Copy(board, TmpBoard, 100);
                                    TmpBoard[FreindlyPieceLocations[i]] = 0;
                                    TmpBoard[FreindlyPieceLocations[i] + (9 * j)] = board[FreindlyPieceLocations[i]];

                                    if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                    {
                                        LogWriter.Log("      Capture move " + j + " spaces down and to the left");
                                        LogWriter.Log("      " + FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] + (9 * j)));
                                        LogWriter.Log("         Captures:" + board[FreindlyPieceLocations[i] + (9 * j)] + ", Valued at " + (board[FreindlyPieceLocations[i] + (9 * j)].Fix() - 2000));
                                        NumPossibleMoves++;

                                        //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                        Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                        TreeScores[NumPossibleMoves] = (board[FreindlyPieceLocations[i] + (9 * j)].Fix() - 2000);
                                    }
                                    break;
                                }
                                //break if it's freindly
                                else
                                {
                                    break;
                                }

                            }

                        }
                        //add line of whitespace
                        LogWriter.Log("");


                    }
                    //END all possible moves for queens
                    //if piece is a king
                    if (FreindlyPieces[i] > 900 && FreindlyPieces[i] < 950)
                    {
                        LogWriter.Log("King id#" + FreindlyPieces[i]);




                        //vertical moves up

                        //if target space is empty
                        if (board[FreindlyPieceLocations[i] - 10] == 0)
                        {
                             Array.Copy(board, TmpBoard, 100);
                             TmpBoard[FreindlyPieceLocations[i]] = 0;
                             TmpBoard[FreindlyPieceLocations[i] - 10] = board[FreindlyPieceLocations[i]];
                             //just break immediately if the end of the board is reached
                            if (FreindlyPieceLocations[i].Fix() - 10 == 0)
                            {
                            }
                            //The board passed to the possibility projector here still has the current king location on it. 
                            //Move the king on TmpBoard and then pass TmpBoard here insted of board
                            else if (PossibilityProjector.RenderPosition(TmpBoard, FreindlyPieceLocations[i] - 10))
                            {

                                LogWriter.Log(" Move vertically up");
                                LogWriter.Log(FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] - 10));
                                NumPossibleMoves++;


                                //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                //Array.Copy(board, TmpBoard, 100);
                                //TmpBoard[FreindlyPieceLocations[i]] = 0;
                                //TmpBoard[FreindlyPieceLocations[i] - 10] = board[FreindlyPieceLocations[i]];
                                Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);

                                TreeScores[NumPossibleMoves] = 0; //should be TreeScores[NumPossibleMoves] = 0;
                                //this is the bugging code, it results in 0-2000. Since this code only executes when nothing is in the space, it should just set to 0


                            }
                        }
                        //if target space has a piece on it
                        else
                        {
                        //if piece belongs to the enemy
                             Array.Copy(board, TmpBoard, 100);
                             TmpBoard[FreindlyPieceLocations[i]] = 0;
                            TmpBoard[FreindlyPieceLocations[i] - 10] = board[FreindlyPieceLocations[i]];
                            if (board[FreindlyPieceLocations[i] - 10].Fix() - 2000 > 0 && PossibilityProjector.RenderPosition(TmpBoard, FreindlyPieceLocations[i] - 10))
                            {
                                LogWriter.Log(" Capture move up");
                                LogWriter.Log(" " + FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] - 10));
                                LogWriter.Log(" Captures:" + board[FreindlyPieceLocations[i] - 10] + ", Valued at " + (board[FreindlyPieceLocations[i] - 10].Fix() - 2000));
                                NumPossibleMoves++;

                                //Copy TmpBoard into TreeBoards array and score into TreeScores array
                               
                                Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                TreeScores[NumPossibleMoves] = (board[FreindlyPieceLocations[i] - 10].Fix() - 2000);
                            }

                        }


                        //vertical moves down

                        //if target space is empty
                        if (board[FreindlyPieceLocations[i] + 10] == 0)
                        {
                            Array.Copy(board, TmpBoard, 100);
                            TmpBoard[FreindlyPieceLocations[i]] = 0;
                            TmpBoard[FreindlyPieceLocations[i] + 10] = board[FreindlyPieceLocations[i]];
                            //just break immediately if the end of the board is reached
                            if (FreindlyPieceLocations[i].Fix() + 10 == 90)
                            {
                            }
                            else if (PossibilityProjector.RenderPosition(TmpBoard, FreindlyPieceLocations[i] + 10))
                            {
                                LogWriter.Log("         Move vertically down");
                                LogWriter.Log(FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] + 10));
                                NumPossibleMoves++;

                                //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                TreeScores[NumPossibleMoves] = 0;
                            }
                        }
                        //if target space has a piece on it
                        else
                        {
                            //if piece belongs to the enemy
                            Array.Copy(board, TmpBoard, 100);
                            TmpBoard[FreindlyPieceLocations[i]] = 0;
                            TmpBoard[FreindlyPieceLocations[i] + 10] = board[FreindlyPieceLocations[i]];
                            if (board[FreindlyPieceLocations[i] + 10].Fix() - 2000 > 0 && PossibilityProjector.RenderPosition(TmpBoard, FreindlyPieceLocations[i] + 10))
                            {
                                LogWriter.Log("      Capture move down");
                                LogWriter.Log("      " + FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] + 10));
                                LogWriter.Log("         Captures:" + board[FreindlyPieceLocations[i] + 10] + ", Valued at " + (board[FreindlyPieceLocations[i] + 10].Fix() - 2000));
                                NumPossibleMoves++;

                                //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                TreeScores[NumPossibleMoves] = (board[FreindlyPieceLocations[i] + 10].Fix() - 2000);
                            }

                        }


                        //horizontal moves right

                        //if target space is empty
                        if (board[FreindlyPieceLocations[i] + 1] == 0)
                        {
                            Array.Copy(board, TmpBoard, 100);
                            TmpBoard[FreindlyPieceLocations[i]] = 0;
                            TmpBoard[FreindlyPieceLocations[i] + 1] = board[FreindlyPieceLocations[i]];
                            //just break immediately if the end of the board is reached
                            if (FreindlyPieceLocations[i] + 1 == FreindlyPieceLocations[i].Fix() + 9)
                            {
                            }
                            else if (PossibilityProjector.RenderPosition(TmpBoard, FreindlyPieceLocations[i] + 1))
                            {
                                LogWriter.Log("     Move horizontally right");
                                LogWriter.Log("     " + FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] + 1));
                                NumPossibleMoves++;

                                //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                TreeScores[NumPossibleMoves] = 0;
                            }
                        }
                        //if target space has a piece on it
                        else
                        {
                        //if piece belongs to the enemy
                            Array.Copy(board, TmpBoard, 100);
                            TmpBoard[FreindlyPieceLocations[i]] = 0;
                            TmpBoard[FreindlyPieceLocations[i] + 1] = board[FreindlyPieceLocations[i]];
                            if (board[FreindlyPieceLocations[i] + 1].Fix() - 2000 > 0 && PossibilityProjector.RenderPosition(TmpBoard, FreindlyPieceLocations[i] + 1))
                            {
                                LogWriter.Log("  Capture move right");
                                LogWriter.Log("     " + FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] + 1));
                                LogWriter.Log("  Captures:" + board[FreindlyPieceLocations[i] + 1] + ", Valued at " + (board[FreindlyPieceLocations[i] + 1].Fix() - 2000));
                                NumPossibleMoves++;

                                //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                TreeScores[NumPossibleMoves] = (board[FreindlyPieceLocations[i] + 1].Fix() - 2000);
                            }

                        }


                        //horizontal moves left

                        //if target space is empty
                        if (board[FreindlyPieceLocations[i] - 1] == 0)
                        {
                        //just break immediately if the end of the board is reached
                            Array.Copy(board, TmpBoard, 100);
                            TmpBoard[FreindlyPieceLocations[i]] = 0;
                            TmpBoard[FreindlyPieceLocations[i] - 1] = board[FreindlyPieceLocations[i]];
                            if (FreindlyPieceLocations[i] - 1 == FreindlyPieceLocations[i].Fix())
                            {
                            }
                            else if (PossibilityProjector.RenderPosition(TmpBoard, FreindlyPieceLocations[i] - 1))
                            {
                                LogWriter.Log("   Move horizontally left");
                                LogWriter.Log("   " + FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] - 1));
                                NumPossibleMoves++;

                                //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                TreeScores[NumPossibleMoves] = 0;
                            }
                        }
                        //if target space has a piece on it
                        else
                        {
                            //if piece belongs to the enemy
                            Array.Copy(board, TmpBoard, 100);
                            TmpBoard[FreindlyPieceLocations[i]] = 0;
                            TmpBoard[FreindlyPieceLocations[i] - 1] = board[FreindlyPieceLocations[i]];
                            if (board[FreindlyPieceLocations[i] - 1].Fix() - 2000 > 0 && PossibilityProjector.RenderPosition(TmpBoard, FreindlyPieceLocations[i] - 1))
                            {
                                LogWriter.Log("Capture move left");
                                LogWriter.Log("" + FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] - 1));
                                LogWriter.Log("Captures:" + board[FreindlyPieceLocations[i] - 1] + ", Valued at " + (board[FreindlyPieceLocations[i] - 1].Fix() - 2000));
                                NumPossibleMoves++;

                                //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                TreeScores[NumPossibleMoves] = (board[FreindlyPieceLocations[i] - 1].Fix() - 2000);
                            }

                        }


                        //up and to the left

                        //if target space is empty
                        if (board[FreindlyPieceLocations[i] - 11] == 0)
                        {
                            Array.Copy(board, TmpBoard, 100);
                            TmpBoard[FreindlyPieceLocations[i]] = 0;
                            TmpBoard[FreindlyPieceLocations[i] - 11] = board[FreindlyPieceLocations[i]];
                            //just break immediately if the end of the board is reached
                            if (FreindlyPieceLocations[i].Fix() - 10 == 0 || FreindlyPieceLocations[i] - 1 == FreindlyPieceLocations[i].Fix())
                            {
                            }
                            else if (PossibilityProjector.RenderPosition(TmpBoard, FreindlyPieceLocations[i] - 11))
                            {
                                LogWriter.Log(" Move up and to the left");
                                LogWriter.Log(FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] - 11));
                                NumPossibleMoves++;

                                //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                TreeScores[NumPossibleMoves] = 0;
                            }
                        }
                        //if target space has a piece on it
                        else
                        {
                            //if piece belongs to the enemy
                            Array.Copy(board, TmpBoard, 100);
                            TmpBoard[FreindlyPieceLocations[i]] = 0;
                            TmpBoard[FreindlyPieceLocations[i] - 11] = board[FreindlyPieceLocations[i]];
                            if (board[FreindlyPieceLocations[i] - 11].Fix() - 2000 > 0 && PossibilityProjector.RenderPosition(TmpBoard, FreindlyPieceLocations[i] - 11))
                            {
                                LogWriter.Log(" Capture move up and to the left");
                                LogWriter.Log(" " + FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] - 11));
                                LogWriter.Log(" Captures:" + board[FreindlyPieceLocations[i] - 11] + ", Valued at " + (board[FreindlyPieceLocations[i] - 11].Fix() - 2000));
                                NumPossibleMoves++;

                                //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                TreeScores[NumPossibleMoves] = (board[FreindlyPieceLocations[i] - 11].Fix() - 2000);
                            }


                        }


                        //vertical moves down and to the right

                        //if target space is empty
                        if (board[FreindlyPieceLocations[i] + 11] == 0)
                        {
                            //just break immediately if the end of the board is reached
                            Array.Copy(board, TmpBoard, 100);
                            TmpBoard[FreindlyPieceLocations[i]] = 0;
                            TmpBoard[FreindlyPieceLocations[i] + 11] = board[FreindlyPieceLocations[i]];
                            if (FreindlyPieceLocations[i].Fix() + 10 == 90 || FreindlyPieceLocations[i] + 1 == FreindlyPieceLocations[i].Fix() + 9)
                            {
                            }
                            else if (PossibilityProjector.RenderPosition(TmpBoard, FreindlyPieceLocations[i] + 11))
                            {
                                LogWriter.Log("         Move down and to the right");
                                LogWriter.Log(FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] + 11));
                                NumPossibleMoves++;

                                //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                TreeScores[NumPossibleMoves] = 0;
                            }
                        }
                        //if target space has a piece on it
                        else
                        {
                            //if piece belongs to the enemy
                            Array.Copy(board, TmpBoard, 100);
                            TmpBoard[FreindlyPieceLocations[i]] = 0;
                            TmpBoard[FreindlyPieceLocations[i] + 11] = board[FreindlyPieceLocations[i]];
                            if (board[FreindlyPieceLocations[i] + 11].Fix() - 2000 > 0 && PossibilityProjector.RenderPosition(TmpBoard, FreindlyPieceLocations[i] + 11))
                            {
                                LogWriter.Log("      Capture move down and to the right");
                                LogWriter.Log("      " + FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] + 11));
                                LogWriter.Log("         Captures:" + board[FreindlyPieceLocations[i] + 11] + ", Valued at " + (board[FreindlyPieceLocations[i] + 11].Fix() - 2000));
                                NumPossibleMoves++;

                                //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                TreeScores[NumPossibleMoves] = (board[FreindlyPieceLocations[i] + 11].Fix() - 2000);
                            }

                        }


                        //up and to the right

                        //if target space is empty
                        if (board[FreindlyPieceLocations[i] - 9] == 0)
                        {
                            //just break immediately if the end of the board is reached
                            Array.Copy(board, TmpBoard, 100);
                            TmpBoard[FreindlyPieceLocations[i]] = 0;
                            TmpBoard[FreindlyPieceLocations[i] - 9] = board[FreindlyPieceLocations[i]];
                            if (FreindlyPieceLocations[i].Fix() - 10 == 0 || FreindlyPieceLocations[i] + 1 == FreindlyPieceLocations[i].Fix() + 9)
                            {
                            }
                            else if (PossibilityProjector.RenderPosition(TmpBoard, FreindlyPieceLocations[i] - 9))
                            {
                                LogWriter.Log(" Move up and to the right");
                                LogWriter.Log(FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] - 9));
                                NumPossibleMoves++;

                                //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                TreeScores[NumPossibleMoves] = 0;
                            }
                        }
                        //if target space has a piece on it
                        else
                        {
                            //if piece belongs to the enemy
                            Array.Copy(board, TmpBoard, 100);
                            TmpBoard[FreindlyPieceLocations[i]] = 0;
                            TmpBoard[FreindlyPieceLocations[i] - 9] = board[FreindlyPieceLocations[i]];
                            if (board[FreindlyPieceLocations[i] - 9].Fix() - 2000 > 0 && PossibilityProjector.RenderPosition(TmpBoard, FreindlyPieceLocations[i] - 9))
                            {
                                LogWriter.Log(" Capture move up and to the right");
                                LogWriter.Log(" " + FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] - 9));
                                LogWriter.Log(" Captures:" + board[FreindlyPieceLocations[i] - 9] + ", Valued at " + (board[FreindlyPieceLocations[i] - 9].Fix() - 2000));
                                NumPossibleMoves++;

                                //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                TreeScores[NumPossibleMoves] = (board[FreindlyPieceLocations[i] - 9].Fix() - 2000);
                            }

                        }


                        //vertical moves down and to the left

                        //if target space is empty
                        if (board[FreindlyPieceLocations[i] + 9] == 0)
                        {
                            //just break immediately if the end of the board is reached
                            Array.Copy(board, TmpBoard, 100);
                            TmpBoard[FreindlyPieceLocations[i]] = 0;
                            TmpBoard[FreindlyPieceLocations[i] + 9] = board[FreindlyPieceLocations[i]];
                            if (FreindlyPieceLocations[i].Fix() + 10 == 90 || FreindlyPieceLocations[i] - 1 == FreindlyPieceLocations[i].Fix())
                            {

                            }
                            else if (PossibilityProjector.RenderPosition(TmpBoard, FreindlyPieceLocations[i] + 9))
                            {
                                LogWriter.Log("         Move down and to the left");
                                LogWriter.Log(FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] + 9));
                                NumPossibleMoves++;

                                //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                TreeScores[NumPossibleMoves] = 0;
                            }
                        }
                        //if target space has a piece on it
                        else
                        {
                            //if piece belongs to the enemy
                            Array.Copy(board, TmpBoard, 100);
                            TmpBoard[FreindlyPieceLocations[i]] = 0;
                            TmpBoard[FreindlyPieceLocations[i] + 9] = board[FreindlyPieceLocations[i]];
                            if (board[FreindlyPieceLocations[i] + 9].Fix() - 2000 > 0 && PossibilityProjector.RenderPosition(TmpBoard, FreindlyPieceLocations[i] + 9))
                            {
                                LogWriter.Log("      Capture move down and to the left");
                                LogWriter.Log("      " + FreindlyPieceLocations[i] + "-->" + (FreindlyPieceLocations[i] + 9));
                                LogWriter.Log("         Captures:" + board[FreindlyPieceLocations[i] + 9] + ", Valued at " + (board[FreindlyPieceLocations[i] + 9].Fix() - 2000));
                                NumPossibleMoves++;

                                //Copy TmpBoard into TreeBoards array and score into TreeScores array
                                Array.Copy(TmpBoard, TreeBoards[NumPossibleMoves], 100);
                                TreeScores[NumPossibleMoves] = (board[FreindlyPieceLocations[i] + 9].Fix() - 2000);
                            }

                        }





                        //add line of whitespace
                        LogWriter.Log("");
                    }
                    //END all possible moves for king

                }

            LogWriter.Log("Number of possible moves: "+NumPossibleMoves);

            Program.PossibilitiesProjected = Program.PossibilitiesProjected + NumPossibleMoves;

                //transfer move # and move score in array of board for the given tree, value 0 is the move number, 10 is the score
                for (int i = 1; i < TreeBoards.Length; i++)
                {
                    TreeBoards[i][0] = i;
                    TreeBoards[i][10] = TreeScores[i];
                }


            





            return TreeBoards;
        }





        //function to tally amount of possible moves
        public static int TallyPossibleMoves(int[] board)
        {
            int[] TmpBoard = new int[100];

            int EnemyNo = 0;
            if (Program.PlayerNo == 1) { EnemyNo = 2; } else { EnemyNo = 1; }

            int EnemyIdentifierNumber = EnemyNo * 1000;
            int IdentifierNumber = Program.PlayerNo * 1000;
            int[] FreindlyPieces = new int[17];
            int[] FreindlyPieceLocations = new int[17];

            int[] EnemyPieces = new int[17];
            int[] EnemyPieceLocations = new int[17];

            int[] PawnDoubleMove = new int[9];
            int[] EnemyPawnDoubleMove = new int[9];

            int KingLocation = 0;
            int EnemyKingLocation = 0;

            int NumPossibleMoves = 0;
                //Figure out what pieces are available and where
                int SecondaryIndexer = 0;
                int TertiaryIndexer = 0;
                for (int i = 10; i < 90; i++)
                {
                    //if freindly piece, add to freindly array
                    if (board[i] - IdentifierNumber < 1000 && board[i] - IdentifierNumber > 10)
                    {
                        FreindlyPieces[SecondaryIndexer] = board[i] - IdentifierNumber;
                        if (board[i] - IdentifierNumber == 901)
                        {
                            KingLocation = i;
                        }
                        FreindlyPieceLocations[SecondaryIndexer] = i;
                        SecondaryIndexer++;
                    }
                    //if enemy, add to enemy array
                    if (board[i] - EnemyIdentifierNumber < 1000 && board[i] - EnemyIdentifierNumber > 10)
                    {
                        EnemyPieces[TertiaryIndexer] = board[i] - EnemyIdentifierNumber;
                        if (board[i] - EnemyIdentifierNumber == 901)
                        {
                            EnemyKingLocation = i;
                        }
                        EnemyPieceLocations[TertiaryIndexer] = i;
                        TertiaryIndexer++;
                    }
                }

            for (int i = 0; i < 16; i++)
            {
                //if piece is a pawn
                if (FreindlyPieces[i] > 10 && FreindlyPieces[i] < 20)
                {
                    //check for feasability of double moves:
                    if (FreindlyPieceLocations[i] - 70 == FreindlyPieces[i] - 10 && FreindlyPieceLocations[i].Fix() == 70 && board[FreindlyPieceLocations[i] - 20] == 0 && board[FreindlyPieceLocations[i] - 10] == 0)
                    {
                        Array.Copy(board, TmpBoard, 100);
                        TmpBoard[FreindlyPieceLocations[i]] = 0;
                        TmpBoard[FreindlyPieceLocations[i] - 20] = board[FreindlyPieceLocations[i]];

                        if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                        {
                            NumPossibleMoves++;
                            
                        }
                    }
                    //check availability of single moves:
                    if (board[FreindlyPieceLocations[i] - 10] == 0)
                    {
                        Array.Copy(board, TmpBoard, 100);
                        TmpBoard[FreindlyPieceLocations[i]] = 0;
                        TmpBoard[FreindlyPieceLocations[i] - 10] = board[FreindlyPieceLocations[i]];

                        if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                        {
                            NumPossibleMoves++;
                        }
                    }
                    //check availability of capture moves, and list # of possible points gained:
                    if (board[FreindlyPieceLocations[i] - 11].Fix() > 0 || board[FreindlyPieceLocations[i] - 9].Fix() > 0)
                    {
                        //check column the piece is in. Col1 only allows capture to the right, col8 only allows to the left because it's the edge of the board
                        //if piece is in columns 2 through 7
                        if (FreindlyPieceLocations[i] - (FreindlyPieceLocations[i].Fix() + 1) != 0 && FreindlyPieceLocations[i] - (FreindlyPieceLocations[i].Fix() + 8) != 0)
                        {
                            //if square up and to the left has a piece on it and is not freindly
                            if (board[FreindlyPieceLocations[i] - 11].Fix() > 0 && board[FreindlyPieceLocations[i] - 11].Fix() - 2000 > 0)
                            {
                                Array.Copy(board, TmpBoard, 100);
                                TmpBoard[FreindlyPieceLocations[i]] = 0;
                                TmpBoard[FreindlyPieceLocations[i] - 11] = board[FreindlyPieceLocations[i]];

                                if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                {

                                    NumPossibleMoves++;
                                }
                            }
                            //if square up and to the right has a piece on it and is not freindly
                            if (board[FreindlyPieceLocations[i] - 9].Fix() > 0 && board[FreindlyPieceLocations[i] - 9].Fix() - 2000 > 0)
                            {
                                Array.Copy(board, TmpBoard, 100);
                                TmpBoard[FreindlyPieceLocations[i]] = 0;
                                TmpBoard[FreindlyPieceLocations[i] - 9] = board[FreindlyPieceLocations[i]];

                                if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                {
                                    NumPossibleMoves++;
                                }
                            }
                        }
                        //if piece is in column 1
                        if (FreindlyPieceLocations[i] - (FreindlyPieceLocations[i].Fix() + 1) == 0)
                        {
                            //if square up and to the right has a piece on it and is not freindly
                            if (board[FreindlyPieceLocations[i] - 9].Fix() > 0 && board[FreindlyPieceLocations[i] - 9].Fix() - 2000 > 0)
                            {
                                Array.Copy(board, TmpBoard, 100);
                                TmpBoard[FreindlyPieceLocations[i]] = 0;
                                TmpBoard[FreindlyPieceLocations[i] - 9] = board[FreindlyPieceLocations[i]];

                                if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                {
                                    NumPossibleMoves++;
                                }
                            }
                        }
                        //if piece is in column 8
                        if (FreindlyPieceLocations[i] - (FreindlyPieceLocations[i].Fix() + 8) == 0)
                        {
                            //if square up and to the left has a piece on it and is not freindly
                            if (board[FreindlyPieceLocations[i] - 11].Fix() > 0 && board[FreindlyPieceLocations[i] - 11].Fix() - 2000 > 0)
                            {
                                Array.Copy(board, TmpBoard, 100);
                                TmpBoard[FreindlyPieceLocations[i]] = 0;
                                TmpBoard[FreindlyPieceLocations[i] - 11] = board[FreindlyPieceLocations[i]];

                                if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                {
                                    NumPossibleMoves++;
                                }
                            }
                        }

                    }

                   

                }
                //END all possible moves for pawns
                //if piece is a tower
                if (FreindlyPieces[i] > 50 && FreindlyPieces[i] < 60)
                {
                    //vertical moves up
                    for (int j = 1; j < 8; j++)
                    {
                        //if target space is empty
                        if (board[FreindlyPieceLocations[i] - (10 * j)] == 0)
                        {
                            //just break immediately if the end of the board is reached
                            if (FreindlyPieceLocations[i].Fix() - (10 * j) == 0)
                            {
                                break;
                            }
                            else
                            {
                                Array.Copy(board, TmpBoard, 100);
                                TmpBoard[FreindlyPieceLocations[i]] = 0;
                                TmpBoard[FreindlyPieceLocations[i] - (10 * j)] = board[FreindlyPieceLocations[i]];

                                if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                {
                                    NumPossibleMoves++;
                                }
                            }
                        }
                        //if target space has a piece on it
                        else
                        {
                            //if piece belongs to the enemy
                            if (board[FreindlyPieceLocations[i] - (10 * j)].Fix() - 2000 > 0)
                            {
                                Array.Copy(board, TmpBoard, 100);
                                TmpBoard[FreindlyPieceLocations[i]] = 0;
                                TmpBoard[FreindlyPieceLocations[i] - (10 * j)] = board[FreindlyPieceLocations[i]];

                                if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                {
                                    NumPossibleMoves++;
                                }
                                break;
                            }
                            //break if it's freindly
                            else
                            {
                                break;
                            }

                        }

                    }
                    //vertical moves down
                    for (int j = 1; j < 8; j++)
                    {
                        //if target space is empty
                        if (board[FreindlyPieceLocations[i] + (10 * j)] == 0)
                        {
                            //just break immediately if the end of the board is reached
                            if (FreindlyPieceLocations[i].Fix() + (10 * j) == 90)
                            {
                                break;
                            }
                            else
                            {
                                Array.Copy(board, TmpBoard, 100);
                                TmpBoard[FreindlyPieceLocations[i]] = 0;
                                TmpBoard[FreindlyPieceLocations[i] + (10 * j)] = board[FreindlyPieceLocations[i]];

                                if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                {
                                    NumPossibleMoves++;
                                }
                            }
                        }
                        //if target space has a piece on it
                        else
                        {
                            //if piece belongs to the enemy
                            if (board[FreindlyPieceLocations[i] + (10 * j)].Fix() - 2000 > 0)
                            {
                                Array.Copy(board, TmpBoard, 100);
                                TmpBoard[FreindlyPieceLocations[i]] = 0;
                                TmpBoard[FreindlyPieceLocations[i] + (10 * j)] = board[FreindlyPieceLocations[i]];

                                if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                {
                                    NumPossibleMoves++;
                                }
                                break;
                            }
                            //break if it's freindly
                            else
                            {
                                break;
                            }

                        }

                    }
                    //horizontal moves right
                    for (int j = 1; j < 8; j++)
                    {
                        //if target space is empty
                        if (board[FreindlyPieceLocations[i] + j] == 0)
                        {
                            //just break immediately if the end of the board is reached
                            if (FreindlyPieceLocations[i] + j == FreindlyPieceLocations[i].Fix() + 9)
                            {
                                break;
                            }
                            else
                            {
                                Array.Copy(board, TmpBoard, 100);
                                TmpBoard[FreindlyPieceLocations[i]] = 0;
                                TmpBoard[FreindlyPieceLocations[i] + j] = board[FreindlyPieceLocations[i]];

                                if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                {
                                   NumPossibleMoves++;
                                }
                            }
                        }
                        //if target space has a piece on it
                        else
                        {
                            //if piece belongs to the enemy
                            if (board[FreindlyPieceLocations[i] + j].Fix() - 2000 > 0)
                            {
                                Array.Copy(board, TmpBoard, 100);
                                TmpBoard[FreindlyPieceLocations[i]] = 0;
                                TmpBoard[FreindlyPieceLocations[i] + j] = board[FreindlyPieceLocations[i]];

                                if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                {
                                    NumPossibleMoves++;
                                }
                                break;
                            }
                            //break if it's freindly
                            else
                            {
                                break;
                            }

                        }

                    }
                    //horizontal moves left
                    for (int j = 1; j < 8; j++)
                    {
                        //if target space is empty
                        if (board[FreindlyPieceLocations[i] - j] == 0)
                        {
                            //just break immediately if the end of the board is reached
                            if (FreindlyPieceLocations[i] - j == FreindlyPieceLocations[i].Fix())
                            {
                                break;
                            }
                            else
                            {
                                Array.Copy(board, TmpBoard, 100);
                                TmpBoard[FreindlyPieceLocations[i]] = 0;
                                TmpBoard[FreindlyPieceLocations[i] - j] = board[FreindlyPieceLocations[i]];

                                if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                {
                                   NumPossibleMoves++;
                                }
                            }
                        }
                        //if target space has a piece on it
                        else
                        {
                            //if piece belongs to the enemy
                            if (board[FreindlyPieceLocations[i] - j].Fix() - 2000 > 0)
                            {
                                Array.Copy(board, TmpBoard, 100);
                                TmpBoard[FreindlyPieceLocations[i]] = 0;
                                TmpBoard[FreindlyPieceLocations[i] - j] = board[FreindlyPieceLocations[i]];

                                if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                {
                                    NumPossibleMoves++;
                                }
                                break;
                            }
                            //break if it's freindly
                            else
                            {
                                break;
                            }

                        }

                    }






                   

                }
                //END all possible moves for towers
                //if piece is a knight
                if (FreindlyPieces[i] > 20 && FreindlyPieces[i] < 30)
                {
                   
                    int[] PossibleKnightMoves = new int[9];
                    //moves up and to the left
                    PossibleKnightMoves[0] = -21;
                    PossibleKnightMoves[1] = -12;
                    //moves down and to the left
                    PossibleKnightMoves[2] = 19;
                    PossibleKnightMoves[3] = 8;
                    //moves up and to the right
                    PossibleKnightMoves[4] = -19;
                    PossibleKnightMoves[5] = -8;
                    //moves down and to the right
                    PossibleKnightMoves[6] = 21;
                    PossibleKnightMoves[7] = 12;



                    for (int j = 0; j < 9; j++)
                    {

                        //if move does not exceed bounds of board
                        //up and left bounds
                        if (FreindlyPieceLocations[i] + PossibleKnightMoves[j] > (FreindlyPieceLocations[i] + PossibleKnightMoves[j]).Fix() && (FreindlyPieceLocations[i] + PossibleKnightMoves[j]).Fix() > 10
                        //below and right bounds
                        && FreindlyPieceLocations[i] + PossibleKnightMoves[j] < (FreindlyPieceLocations[i] + PossibleKnightMoves[j]).Fix() + 9 && (FreindlyPieceLocations[i] + PossibleKnightMoves[j]).Fix() < 90)
                        {
                            //if move is to an empty space
                            if (board[FreindlyPieceLocations[i] + PossibleKnightMoves[j]].Fix() == 0)
                            {
                                Array.Copy(board, TmpBoard, 100);
                                TmpBoard[FreindlyPieceLocations[i]] = 0;
                                TmpBoard[FreindlyPieceLocations[i] + PossibleKnightMoves[j]] = board[FreindlyPieceLocations[i]];

                                if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                {
                                    NumPossibleMoves++;
                                }
                            }

                            //if move is to space with an enemy
                            if (board[FreindlyPieceLocations[i] + PossibleKnightMoves[j]].Fix() > 0 && board[FreindlyPieceLocations[i] + PossibleKnightMoves[j]].Fix() - 2000 > 0)
                            {
                                Array.Copy(board, TmpBoard, 100);
                                TmpBoard[FreindlyPieceLocations[i]] = 0;
                                TmpBoard[FreindlyPieceLocations[i] + PossibleKnightMoves[j]] = board[FreindlyPieceLocations[i]];

                                if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                {
                                   NumPossibleMoves++;
                                }
                            }
                        }

                    }

                    //add line of whitespace
                    LogWriter.Log("");


                }
                //END all possible moves for knights
                //if piece is a bishop
                if (FreindlyPieces[i] > 30 && FreindlyPieces[i] < 40)
                {
                    
                    //up and to the left
                    for (int j = 1; j < 8; j++)
                    {
                        //if target space is empty
                        if (board[FreindlyPieceLocations[i] - (11 * j)] == 0)
                        {
                            //just break immediately if the end of the board is reached
                            if (FreindlyPieceLocations[i].Fix() - (10 * j) == 0 || FreindlyPieceLocations[i] - j == FreindlyPieceLocations[i].Fix())
                            {
                                break;
                            }
                            else
                            {
                                Array.Copy(board, TmpBoard, 100);
                                TmpBoard[FreindlyPieceLocations[i]] = 0;
                                TmpBoard[FreindlyPieceLocations[i] - (11 * j)] = board[FreindlyPieceLocations[i]];

                                if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                {
                                   NumPossibleMoves++;
                                }
                            }
                        }
                        //if target space has a piece on it
                        else
                        {
                            //if piece belongs to the enemy
                            if (board[FreindlyPieceLocations[i] - (11 * j)].Fix() - 2000 > 0)
                            {
                                Array.Copy(board, TmpBoard, 100);
                                TmpBoard[FreindlyPieceLocations[i]] = 0;
                                TmpBoard[FreindlyPieceLocations[i] - (11 * j)] = board[FreindlyPieceLocations[i]];

                                if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                {
                                    NumPossibleMoves++;
                                }
                                break;
                            }
                            //break if it's freindly
                            else
                            {
                                break;
                            }

                        }

                    }
                    //vertical moves down and to the right
                    for (int j = 1; j < 8; j++)
                    {
                        //if target space is empty
                        if (board[FreindlyPieceLocations[i] + (11 * j)] == 0)
                        {
                            //just break immediately if the end of the board is reached
                            if (FreindlyPieceLocations[i].Fix() + (10 * j) == 90 || FreindlyPieceLocations[i] + j == FreindlyPieceLocations[i].Fix() + 9)
                            {
                                break;
                            }
                            else
                            {
                                Array.Copy(board, TmpBoard, 100);
                                TmpBoard[FreindlyPieceLocations[i]] = 0;
                                TmpBoard[FreindlyPieceLocations[i] + (11 * j)] = board[FreindlyPieceLocations[i]];

                                if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                {
                                    NumPossibleMoves++;
                                }
                            }
                        }
                        //if target space has a piece on it
                        else
                        {
                            //if piece belongs to the enemy
                            if (board[FreindlyPieceLocations[i] + (11 * j)].Fix() - 2000 > 0)
                            {
                                Array.Copy(board, TmpBoard, 100);
                                TmpBoard[FreindlyPieceLocations[i]] = 0;
                                TmpBoard[FreindlyPieceLocations[i] + (11 * j)] = board[FreindlyPieceLocations[i]];

                                if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                {
                                    NumPossibleMoves++;
                                }
                                break;
                            }
                            //break if it's freindly
                            else
                            {
                                break;
                            }

                        }

                    }
                    //up and to the right
                    for (int j = 1; j < 8; j++)
                    {
                        //if target space is empty
                        if (board[FreindlyPieceLocations[i] - (9 * j)] == 0)
                        {
                            //just break immediately if the end of the board is reached
                            if (FreindlyPieceLocations[i].Fix() - (10 * j) == 0 || FreindlyPieceLocations[i] + j == FreindlyPieceLocations[i].Fix() + 9)
                            {
                                break;
                            }
                            else
                            {
                                Array.Copy(board, TmpBoard, 100);
                                TmpBoard[FreindlyPieceLocations[i]] = 0;
                                TmpBoard[FreindlyPieceLocations[i] - (9 * j)] = board[FreindlyPieceLocations[i]];

                                if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                {
                                    NumPossibleMoves++;
                                }
                            }
                        }
                        //if target space has a piece on it
                        else
                        {
                            //if piece belongs to the enemy
                            if (board[FreindlyPieceLocations[i] - (9 * j)].Fix() - 2000 > 0)
                            {
                                Array.Copy(board, TmpBoard, 100);
                                TmpBoard[FreindlyPieceLocations[i]] = 0;
                                TmpBoard[FreindlyPieceLocations[i] - (9 * j)] = board[FreindlyPieceLocations[i]];

                                if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                {
                                    NumPossibleMoves++;
                                }
                                break;
                            }
                            //break if it's freindly
                            else
                            {
                                break;
                            }

                        }

                    }
                    //vertical moves down and to the left
                    for (int j = 1; j < 8; j++)
                    {
                        //if target space is empty
                        if (board[FreindlyPieceLocations[i] + (9 * j)] == 0)
                        {
                            //just break immediately if the end of the board is reached
                            if (FreindlyPieceLocations[i].Fix() + (10 * j) == 90 || FreindlyPieceLocations[i] - j == FreindlyPieceLocations[i].Fix())
                            {
                                break;
                            }
                            else
                            {
                                Array.Copy(board, TmpBoard, 100);
                                TmpBoard[FreindlyPieceLocations[i]] = 0;
                                TmpBoard[FreindlyPieceLocations[i] + (9 * j)] = board[FreindlyPieceLocations[i]];

                                if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                {
                                   NumPossibleMoves++;
                                }
                            }
                        }
                        //if target space has a piece on it
                        else
                        {
                            //if piece belongs to the enemy
                            if (board[FreindlyPieceLocations[i] + (9 * j)].Fix() - 2000 > 0)
                            {
                                Array.Copy(board, TmpBoard, 100);
                                TmpBoard[FreindlyPieceLocations[i]] = 0;
                                TmpBoard[FreindlyPieceLocations[i] + (9 * j)] = board[FreindlyPieceLocations[i]];

                                if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                {
                                   NumPossibleMoves++;
                                }
                                break;
                            }
                            //break if it's freindly
                            else
                            {
                                break;
                            }

                        }

                    }
                   

                }
                //END all possible moves for bishops
                //if piece is a queen
                if (FreindlyPieces[i] > 70 && FreindlyPieces[i] < 80)
                {
                    

                    //vertical moves up
                    for (int j = 1; j < 8; j++)
                    {
                        //if target space is empty
                        if (board[FreindlyPieceLocations[i] - (10 * j)] == 0)
                        {
                            //just break immediately if the end of the board is reached
                            if (FreindlyPieceLocations[i].Fix() - (10 * j) == 0)
                            {
                                break;
                            }
                            else
                            {
                                Array.Copy(board, TmpBoard, 100);
                                TmpBoard[FreindlyPieceLocations[i]] = 0;
                                TmpBoard[FreindlyPieceLocations[i] - (10 * j)] = board[FreindlyPieceLocations[i]];

                                if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                {
                                   NumPossibleMoves++;
                                }
                            }
                        }
                        //if target space has a piece on it
                        else
                        {
                            //if piece belongs to the enemy
                            if (board[FreindlyPieceLocations[i] - (10 * j)].Fix() - 2000 > 0)
                            {
                                Array.Copy(board, TmpBoard, 100);
                                TmpBoard[FreindlyPieceLocations[i]] = 0;
                                TmpBoard[FreindlyPieceLocations[i] - (10 * j)] = board[FreindlyPieceLocations[i]];

                                if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                {
                                   NumPossibleMoves++;
                                }
                                break;
                            }
                            //break if it's freindly
                            else
                            {
                                break;
                            }

                        }

                    }
                    //vertical moves down
                    for (int j = 1; j < 8; j++)
                    {
                        //if target space is empty
                        if (board[FreindlyPieceLocations[i] + (10 * j)] == 0)
                        {
                            //just break immediately if the end of the board is reached
                            if (FreindlyPieceLocations[i].Fix() + (10 * j) == 90)
                            {
                                break;
                            }
                            else
                            {
                                Array.Copy(board, TmpBoard, 100);
                                TmpBoard[FreindlyPieceLocations[i]] = 0;
                                TmpBoard[FreindlyPieceLocations[i] + (10 * j)] = board[FreindlyPieceLocations[i]];

                                if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                {
                                    NumPossibleMoves++;
                                }
                            }
                        }
                        //if target space has a piece on it
                        else
                        {
                            //if piece belongs to the enemy
                            if (board[FreindlyPieceLocations[i] + (10 * j)].Fix() - 2000 > 0)
                            {
                                Array.Copy(board, TmpBoard, 100);
                                TmpBoard[FreindlyPieceLocations[i]] = 0;
                                TmpBoard[FreindlyPieceLocations[i] + (10 * j)] = board[FreindlyPieceLocations[i]];

                                if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                {
                                   NumPossibleMoves++;
                                }
                                break;
                            }
                            //break if it's freindly
                            else
                            {
                                break;
                            }

                        }

                    }
                    //horizontal moves right
                    for (int j = 1; j < 8; j++)
                    {
                        //if target space is empty
                        if (board[FreindlyPieceLocations[i] + j] == 0)
                        {
                            //just break immediately if the end of the board is reached
                            if (FreindlyPieceLocations[i] + j == FreindlyPieceLocations[i].Fix() + 9)
                            {
                                break;
                            }
                            else
                            {
                                Array.Copy(board, TmpBoard, 100);
                                TmpBoard[FreindlyPieceLocations[i]] = 0;
                                TmpBoard[FreindlyPieceLocations[i] + j] = board[FreindlyPieceLocations[i]];

                                if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                {
                                   NumPossibleMoves++;
                                }
                            }
                        }
                        //if target space has a piece on it
                        else
                        {
                            //if piece belongs to the enemy
                            if (board[FreindlyPieceLocations[i] + j].Fix() - 2000 > 0)
                            {
                                Array.Copy(board, TmpBoard, 100);
                                TmpBoard[FreindlyPieceLocations[i]] = 0;
                                TmpBoard[FreindlyPieceLocations[i] + j] = board[FreindlyPieceLocations[i]];

                                if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                {
                                    NumPossibleMoves++;
                                }
                                break;
                            }
                            //break if it's freindly
                            else
                            {
                                break;
                            }

                        }

                    }
                    //horizontal moves left
                    for (int j = 1; j < 8; j++)
                    {
                        //if target space is empty
                        if (board[FreindlyPieceLocations[i] - j] == 0)
                        {
                            //just break immediately if the end of the board is reached
                            if (FreindlyPieceLocations[i] - j == FreindlyPieceLocations[i].Fix())
                            {
                                break;
                            }
                            else
                            {
                                Array.Copy(board, TmpBoard, 100);
                                TmpBoard[FreindlyPieceLocations[i]] = 0;
                                TmpBoard[FreindlyPieceLocations[i] - j] = board[FreindlyPieceLocations[i]];

                                if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                {
                                    NumPossibleMoves++;
                                }
                            }
                        }
                        //if target space has a piece on it
                        else
                        {
                            //if piece belongs to the enemy
                            if (board[FreindlyPieceLocations[i] - j].Fix() - 2000 > 0)
                            {
                                Array.Copy(board, TmpBoard, 100);
                                TmpBoard[FreindlyPieceLocations[i]] = 0;
                                TmpBoard[FreindlyPieceLocations[i] - j] = board[FreindlyPieceLocations[i]];

                                if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                {
                                    NumPossibleMoves++;
                                }
                                break;
                            }
                            //break if it's freindly
                            else
                            {
                                break;
                            }

                        }

                    }
                    //up and to the left
                    for (int j = 1; j < 8; j++)
                    {
                        //if target space is empty
                        if (board[FreindlyPieceLocations[i] - (11 * j)] == 0)
                        {
                            //just break immediately if the end of the board is reached
                            if (FreindlyPieceLocations[i].Fix() - (10 * j) == 0 || FreindlyPieceLocations[i] - j == FreindlyPieceLocations[i].Fix())
                            {
                                break;
                            }
                            else
                            {
                                Array.Copy(board, TmpBoard, 100);
                                TmpBoard[FreindlyPieceLocations[i]] = 0;
                                TmpBoard[FreindlyPieceLocations[i] - (11 * j)] = board[FreindlyPieceLocations[i]];

                                if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                {
                                    NumPossibleMoves++;
                                }
                            }
                        }
                        //if target space has a piece on it
                        else
                        {
                            //if piece belongs to the enemy
                            if (board[FreindlyPieceLocations[i] - (11 * j)].Fix() - 2000 > 0)
                            {
                                Array.Copy(board, TmpBoard, 100);
                                TmpBoard[FreindlyPieceLocations[i]] = 0;
                                TmpBoard[FreindlyPieceLocations[i] - (11 * j)] = board[FreindlyPieceLocations[i]];

                                if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                {
                                   NumPossibleMoves++;
                                }
                                break;
                            }
                            //break if it's freindly
                            else
                            {
                                break;
                            }

                        }

                    }
                    //vertical moves down and to the right
                    for (int j = 1; j < 8; j++)
                    {
                        //if target space is empty
                        if (board[FreindlyPieceLocations[i] + (11 * j)] == 0)
                        {
                            //just break immediately if the end of the board is reached
                            if (FreindlyPieceLocations[i].Fix() + (10 * j) == 90 || FreindlyPieceLocations[i] + j == FreindlyPieceLocations[i].Fix() + 9)
                            {
                                break;
                            }
                            else
                            {
                                Array.Copy(board, TmpBoard, 100);
                                TmpBoard[FreindlyPieceLocations[i]] = 0;
                                TmpBoard[FreindlyPieceLocations[i] + (11 * j)] = board[FreindlyPieceLocations[i]];

                                if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                {
                                    NumPossibleMoves++;
                                }
                            }
                        }
                        //if target space has a piece on it
                        else
                        {
                            //if piece belongs to the enemy
                            if (board[FreindlyPieceLocations[i] + (11 * j)].Fix() - 2000 > 0)
                            {
                                Array.Copy(board, TmpBoard, 100);
                                TmpBoard[FreindlyPieceLocations[i]] = 0;
                                TmpBoard[FreindlyPieceLocations[i] + (11 * j)] = board[FreindlyPieceLocations[i]];

                                if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                {
                                   NumPossibleMoves++;
                                }
                                break;
                            }
                            //break if it's freindly
                            else
                            {
                                break;
                            }

                        }

                    }
                    //up and to the right
                    for (int j = 1; j < 8; j++)
                    {
                        //if target space is empty
                        if (board[FreindlyPieceLocations[i] - (9 * j)] == 0)
                        {
                            //just break immediately if the end of the board is reached
                            if (FreindlyPieceLocations[i].Fix() - (10 * j) == 0 || FreindlyPieceLocations[i] + j == FreindlyPieceLocations[i].Fix() + 9)
                            {
                                break;
                            }
                            else
                            {
                                Array.Copy(board, TmpBoard, 100);
                                TmpBoard[FreindlyPieceLocations[i]] = 0;
                                TmpBoard[FreindlyPieceLocations[i] - (9 * j)] = board[FreindlyPieceLocations[i]];

                                if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                {
                                   NumPossibleMoves++;
                                }
                            }
                        }
                        //if target space has a piece on it
                        else
                        {
                            //if piece belongs to the enemy
                            if (board[FreindlyPieceLocations[i] - (9 * j)].Fix() - 2000 > 0)
                            {
                                Array.Copy(board, TmpBoard, 100);
                                TmpBoard[FreindlyPieceLocations[i]] = 0;
                                TmpBoard[FreindlyPieceLocations[i] - (9 * j)] = board[FreindlyPieceLocations[i]];

                                if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                {
                                    NumPossibleMoves++;
                                }
                                break;
                            }
                            //break if it's freindly
                            else
                            {
                                break;
                            }

                        }

                    }
                    //vertical moves down and to the left
                    for (int j = 1; j < 8; j++)
                    {
                        //if target space is empty
                        if (board[FreindlyPieceLocations[i] + (9 * j)] == 0)
                        {
                            //just break immediately if the end of the board is reached
                            if (FreindlyPieceLocations[i].Fix() + (10 * j) == 90 || FreindlyPieceLocations[i] - j == FreindlyPieceLocations[i].Fix())
                            {
                                break;
                            }
                            else
                            {
                                Array.Copy(board, TmpBoard, 100);
                                TmpBoard[FreindlyPieceLocations[i]] = 0;
                                TmpBoard[FreindlyPieceLocations[i] + (9 * j)] = board[FreindlyPieceLocations[i]];

                                if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                {
                                    NumPossibleMoves++;
                                }
                            }
                        }
                        //if target space has a piece on it
                        else
                        {
                            //if piece belongs to the enemy
                            if (board[FreindlyPieceLocations[i] + (9 * j)].Fix() - 2000 > 0)
                            {
                                Array.Copy(board, TmpBoard, 100);
                                TmpBoard[FreindlyPieceLocations[i]] = 0;
                                TmpBoard[FreindlyPieceLocations[i] + (9 * j)] = board[FreindlyPieceLocations[i]];

                                if (PossibilityProjector.RenderPosition(TmpBoard, KingLocation))
                                {
                                    NumPossibleMoves++;
                                }
                                break;
                            }
                            //break if it's freindly
                            else
                            {
                                break;
                            }

                        }

                    }
                   

                }
                //END all possible moves for queens
                //if piece is a king
                if (FreindlyPieces[i] > 900 && FreindlyPieces[i] < 950)
                {
                    



                    //vertical moves up

                    //if target space is empty
                    if (board[FreindlyPieceLocations[i] - 10] == 0)
                    {
                        //just break immediately if the end of the board is reached
                        Array.Copy(board, TmpBoard, 100);
                        TmpBoard[FreindlyPieceLocations[i]] = 0;
                        TmpBoard[FreindlyPieceLocations[i] - 10] = board[FreindlyPieceLocations[i]];
                        if (FreindlyPieceLocations[i].Fix() - 10 == 0)
                        {
                        }
                        else if (PossibilityProjector.RenderPosition(TmpBoard, FreindlyPieceLocations[i] - 10))
                        {

                            NumPossibleMoves++;
                        }
                    }
                    //if target space has a piece on it
                    else
                    {
                        //if piece belongs to the enemy
                        Array.Copy(board, TmpBoard, 100);
                        TmpBoard[FreindlyPieceLocations[i]] = 0;
                        TmpBoard[FreindlyPieceLocations[i] - 10] = board[FreindlyPieceLocations[i]];
                        if (board[FreindlyPieceLocations[i] - 10].Fix() - 2000 > 0 && PossibilityProjector.RenderPosition(TmpBoard, FreindlyPieceLocations[i] - 10))
                        {
                            NumPossibleMoves++;
                        }

                    }


                    //vertical moves down

                    //if target space is empty
                    if (board[FreindlyPieceLocations[i] + 10] == 0)
                    {
                        //just break immediately if the end of the board is reached
                        Array.Copy(board, TmpBoard, 100);
                        TmpBoard[FreindlyPieceLocations[i]] = 0;
                        TmpBoard[FreindlyPieceLocations[i] + 10] = board[FreindlyPieceLocations[i]];
                        if (FreindlyPieceLocations[i].Fix() + 10 == 90)
                        {
                        }
                        else if (PossibilityProjector.RenderPosition(TmpBoard, FreindlyPieceLocations[i] + 10))
                        {
                            NumPossibleMoves++;
                        }
                    }
                    //if target space has a piece on it
                    else
                    {
                        //if piece belongs to the enemy
                        Array.Copy(board, TmpBoard, 100);
                        TmpBoard[FreindlyPieceLocations[i]] = 0;
                        TmpBoard[FreindlyPieceLocations[i] + 10] = board[FreindlyPieceLocations[i]];
                        if (board[FreindlyPieceLocations[i] + 10].Fix() - 2000 > 0 && PossibilityProjector.RenderPosition(TmpBoard, FreindlyPieceLocations[i] + 10))
                        {
                            NumPossibleMoves++;
                        }

                    }


                    //horizontal moves right

                    //if target space is empty
                    if (board[FreindlyPieceLocations[i] + 1] == 0)
                    {
                        //just break immediately if the end of the board is reached
                        Array.Copy(board, TmpBoard, 100);
                        TmpBoard[FreindlyPieceLocations[i]] = 0;
                        TmpBoard[FreindlyPieceLocations[i] + 1] = board[FreindlyPieceLocations[i]];
                        if (FreindlyPieceLocations[i] + 1 == FreindlyPieceLocations[i].Fix() + 9)
                        {
                        }
                        else if (PossibilityProjector.RenderPosition(TmpBoard, FreindlyPieceLocations[i] + 1))
                        {
                            NumPossibleMoves++;
                        }
                    }
                    //if target space has a piece on it
                    else
                    {
                        //if piece belongs to the enemy
                        Array.Copy(board, TmpBoard, 100);
                        TmpBoard[FreindlyPieceLocations[i]] = 0;
                        TmpBoard[FreindlyPieceLocations[i] + 1] = board[FreindlyPieceLocations[i]];
                        if (board[FreindlyPieceLocations[i] + 1].Fix() - 2000 > 0 && PossibilityProjector.RenderPosition(TmpBoard, FreindlyPieceLocations[i] + 1))
                        {
                            NumPossibleMoves++;
                        }

                    }


                    //horizontal moves left

                    //if target space is empty
                    if (board[FreindlyPieceLocations[i] - 1] == 0)
                    {
                        //just break immediately if the end of the board is reached
                        Array.Copy(board, TmpBoard, 100);
                        TmpBoard[FreindlyPieceLocations[i]] = 0;
                        TmpBoard[FreindlyPieceLocations[i] - 1] = board[FreindlyPieceLocations[i]];
                        if (FreindlyPieceLocations[i] - 1 == FreindlyPieceLocations[i].Fix())
                        {
                        }
                        else if (PossibilityProjector.RenderPosition(TmpBoard, FreindlyPieceLocations[i] - 1))
                        {
                            NumPossibleMoves++;
                        }
                    }
                    //if target space has a piece on it
                    else
                    {
                        //if piece belongs to the enemy
                        Array.Copy(board, TmpBoard, 100);
                        TmpBoard[FreindlyPieceLocations[i]] = 0;
                        TmpBoard[FreindlyPieceLocations[i] - 1] = board[FreindlyPieceLocations[i]];
                        if (board[FreindlyPieceLocations[i] - 1].Fix() - 2000 > 0 && PossibilityProjector.RenderPosition(TmpBoard, FreindlyPieceLocations[i] - 1))
                        {
                            NumPossibleMoves++;
                        }

                    }


                    //up and to the left

                    //if target space is empty
                    if (board[FreindlyPieceLocations[i] - 11] == 0)
                    {
                        //just break immediately if the end of the board is reached
                        Array.Copy(board, TmpBoard, 100);
                        TmpBoard[FreindlyPieceLocations[i]] = 0;
                        TmpBoard[FreindlyPieceLocations[i] - 11] = board[FreindlyPieceLocations[i]];
                        if (FreindlyPieceLocations[i].Fix() - 10 == 0 || FreindlyPieceLocations[i] - 1 == FreindlyPieceLocations[i].Fix())
                        {
                        }
                        else if (PossibilityProjector.RenderPosition(TmpBoard, FreindlyPieceLocations[i] - 11))
                        {
                            NumPossibleMoves++;
                        }
                    }
                    //if target space has a piece on it
                    else
                    {
                        //if piece belongs to the enemy
                        Array.Copy(board, TmpBoard, 100);
                        TmpBoard[FreindlyPieceLocations[i]] = 0;
                        TmpBoard[FreindlyPieceLocations[i] - 11] = board[FreindlyPieceLocations[i]];
                        if (board[FreindlyPieceLocations[i] - 11].Fix() - 2000 > 0 && PossibilityProjector.RenderPosition(TmpBoard, FreindlyPieceLocations[i] - 11))
                        {
                            NumPossibleMoves++;
                        }


                    }


                    //vertical moves down and to the right

                    //if target space is empty
                    if (board[FreindlyPieceLocations[i] + 11] == 0)
                    {
                        //just break immediately if the end of the board is reached
                        Array.Copy(board, TmpBoard, 100);
                        TmpBoard[FreindlyPieceLocations[i]] = 0;
                        TmpBoard[FreindlyPieceLocations[i] + 11] = board[FreindlyPieceLocations[i]];
                        if (FreindlyPieceLocations[i].Fix() + 10 == 90 || FreindlyPieceLocations[i] + 1 == FreindlyPieceLocations[i].Fix() + 9)
                        {
                        }
                        else if (PossibilityProjector.RenderPosition(TmpBoard, FreindlyPieceLocations[i] + 11))
                        {
                            NumPossibleMoves++;
                        }
                    }
                    //if target space has a piece on it
                    else
                    {
                        //if piece belongs to the enemy
                        Array.Copy(board, TmpBoard, 100);
                        TmpBoard[FreindlyPieceLocations[i]] = 0;
                        TmpBoard[FreindlyPieceLocations[i] + 11] = board[FreindlyPieceLocations[i]];
                        if (board[FreindlyPieceLocations[i] + 11].Fix() - 2000 > 0 && PossibilityProjector.RenderPosition(TmpBoard, FreindlyPieceLocations[i] + 11))
                        {
                            NumPossibleMoves++;
                        }

                    }


                    //up and to the right

                    //if target space is empty
                    if (board[FreindlyPieceLocations[i] - 9] == 0)
                    {
                        //just break immediately if the end of the board is reached
                        Array.Copy(board, TmpBoard, 100);
                        TmpBoard[FreindlyPieceLocations[i]] = 0;
                        TmpBoard[FreindlyPieceLocations[i] - 9] = board[FreindlyPieceLocations[i]];
                        if (FreindlyPieceLocations[i].Fix() - 10 == 0 || FreindlyPieceLocations[i] + 1 == FreindlyPieceLocations[i].Fix() + 9)
                        {
                        }
                        else if (PossibilityProjector.RenderPosition(TmpBoard, FreindlyPieceLocations[i] - 9))
                        {
                            NumPossibleMoves++;
                        }
                    }
                    //if target space has a piece on it
                    else
                    {
                        //if piece belongs to the enemy
                        Array.Copy(board, TmpBoard, 100);
                        TmpBoard[FreindlyPieceLocations[i]] = 0;
                        TmpBoard[FreindlyPieceLocations[i] - 9] = board[FreindlyPieceLocations[i]];
                        if (board[FreindlyPieceLocations[i] - 9].Fix() - 2000 > 0 && PossibilityProjector.RenderPosition(TmpBoard, FreindlyPieceLocations[i] - 9))
                        {
                             NumPossibleMoves++;
                        }

                    }


                    //vertical moves down and to the left

                    //if target space is empty
                    if (board[FreindlyPieceLocations[i] + 9] == 0)
                    {
                        //just break immediately if the end of the board is reached
                        Array.Copy(board, TmpBoard, 100);
                        TmpBoard[FreindlyPieceLocations[i]] = 0;
                        TmpBoard[FreindlyPieceLocations[i] + 9] = board[FreindlyPieceLocations[i]];
                        if (FreindlyPieceLocations[i].Fix() + 10 == 90 || FreindlyPieceLocations[i] - 1 == FreindlyPieceLocations[i].Fix())
                        {

                        }
                        else if (PossibilityProjector.RenderPosition(TmpBoard, FreindlyPieceLocations[i] + 9))
                        {
                            
                            NumPossibleMoves++;
                        }
                    }
                    //if target space has a piece on it
                    else
                    {
                        //if piece belongs to the enemy
                        Array.Copy(board, TmpBoard, 100);
                        TmpBoard[FreindlyPieceLocations[i]] = 0;
                        TmpBoard[FreindlyPieceLocations[i] + 9] = board[FreindlyPieceLocations[i]];
                        if (board[FreindlyPieceLocations[i] + 9].Fix() - 2000 > 0 && PossibilityProjector.RenderPosition(TmpBoard, FreindlyPieceLocations[i] + 9))
                        {
                          
                            NumPossibleMoves++;
                        }

                    }





                 
                }
                //END all possible moves for king

            }



            return NumPossibleMoves;
        }

    }
}
