using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GN_Shiro
{
    public static class PossibilityProjector
    {
        //function to check P1 positions under risk from P2 (therefore this code is from the perspective of P2). 
        public static bool RenderPosition(int[] board, int position)
        {

            //take the board array and turn it into freindly pieces + pice locations array and enemy pieces + locations array
            int EnemyNo = 0;
            if (Program.PlayerNo == 1) { EnemyNo = 2; } else { EnemyNo = 1; }
            int EnemyIdentifierNumber = Program.PlayerNo * 1000;
            int IdentifierNumber = EnemyNo * 1000;
            int[] FreindlyPieces = new int[17];
            int[] FreindlyPieceLocations = new int[17];

            int[] EnemyPieces = new int[17];
            int[] EnemyPieceLocations = new int[17];

            int NumPossibleMoves = 0;
            int NumPossibleEnemyMoves = 0;



            //Figure out what pieces are available and where
            int SecondaryIndexer = 0;
            int TertiaryIndexer = 0;
            for (int i = 10; i < 90; i++)
            {
                //if freindly piece, add to freindly array
                if (board[i] - IdentifierNumber < 1000 && board[i] - IdentifierNumber > 10)
                {
                    FreindlyPieces[SecondaryIndexer] = board[i] - IdentifierNumber;
                    FreindlyPieceLocations[SecondaryIndexer] = i;
                    SecondaryIndexer++;
                }
                //if enemy, add to enemy array
                if (board[i] - EnemyIdentifierNumber < 1000 && board[i] - EnemyIdentifierNumber > 10)
                {
                    EnemyPieces[TertiaryIndexer] = board[i] - EnemyIdentifierNumber;
                    EnemyPieceLocations[TertiaryIndexer] = i;
                    TertiaryIndexer++;
                }
            }


            //look at each piece
            for (int i = 0; i < 16; i++)
            {
                //if piece is a pawn
                if (FreindlyPieces[i] > 10 && FreindlyPieces[i] < 20)
                {
                    //Console.WriteLine("Pawn id#" + FreindlyPieces[i]);
                        //check column the piece is in. Col1 only allows capture to the right, col8 only allows to the left because it's the edge of the board
                        //if piece is in columns 2 through 7

                        if (FreindlyPieceLocations[i] - (FreindlyPieceLocations[i].Fix() + 1) != 0 && FreindlyPieceLocations[i] - (FreindlyPieceLocations[i].Fix() + 8) != 0)
                        {
                            //if square down and to the left is target position
                            if (FreindlyPieceLocations[i] +9 == position)
                            {
                                return false;
                            }
                            //if square down and to the right is target position
                            if (FreindlyPieceLocations[i] +11 == position) 
                            {
                                return false;
                            }
                        }
                        //if piece is in column 1
                        if (FreindlyPieceLocations[i] - (FreindlyPieceLocations[i].Fix() + 1) == 0)
                        {
                            if (FreindlyPieceLocations[i] +11 == position)
                            {
                            return false;
                            }
                        }
                        //if piece is in column 8
                        if (FreindlyPieceLocations[i] - (FreindlyPieceLocations[i].Fix() + 8) == 0)
                        {

                             if (FreindlyPieceLocations[i] +9 == position)
                             {
                            return false;
                             }

                        }

                    

                    //add line of whitespace
                    //Console.WriteLine("");


                }
                //END all possible moves for pawns
                //if piece is a tower
                if (FreindlyPieces[i] > 50 && FreindlyPieces[i] < 60)
                {
                    //Console.WriteLine("Tower id#" + FreindlyPieces[i]);
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
                                if (FreindlyPieceLocations[i] - (10 * j) == position) { return false; }
                            }
                        }
                        //if target space has a piece on it
                        else
                        {

                                if (FreindlyPieceLocations[i] - (10 * j) == position) { return false; }
                                break;


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
                                if (FreindlyPieceLocations[i] + (10 * j) == position) { return false; };
                            }
                        }
                        //if target space has a piece on it
                        else
                        {

                                if (FreindlyPieceLocations[i] + (10 * j) == position) { return false; }
                                break;


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
                                if (FreindlyPieceLocations[i] + j == position) { return false; }
                            }
                        }
                        //if target space has a piece on it
                        else
                        {

                                if (FreindlyPieceLocations[i] + j == position) { return false; }
                                break;


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
                                if (FreindlyPieceLocations[i] - j == position) { return false; }
                            }
                        }
                        //if target space has a piece on it
                        else
                        {

                                if (FreindlyPieceLocations[i] - j == position) { return false; }
                                break;


                        }

                    }






                    //add line of whitespace
                    //Console.WriteLine("");


                }
                //END all possible moves for towers
                //if piece is a knight
                if (FreindlyPieces[i] > 20 && FreindlyPieces[i] < 30)
                {
                    //Console.WriteLine("Knight id#" + FreindlyPieces[i]);

                    int[] PossibleKnightMoves = new int[8];
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



                    for (int j = 0; j < 8; j++)
                    {

                        //if move does not exceed bounds of board
                        //up and left bounds
                        if (FreindlyPieceLocations[i] + PossibleKnightMoves[j] > (FreindlyPieceLocations[i] + PossibleKnightMoves[j]).Fix() && (FreindlyPieceLocations[i] + PossibleKnightMoves[j]).Fix() > 10
                        //below and right bounds
                        && FreindlyPieceLocations[i] + PossibleKnightMoves[j] < (FreindlyPieceLocations[i] + PossibleKnightMoves[j]).Fix() + 9 && (FreindlyPieceLocations[i] + PossibleKnightMoves[j]).Fix() < 90)
                        {
                            //if move is to an empty space
                            if (FreindlyPieceLocations[i] + PossibleKnightMoves[j] == position)
                            {
                                return false;
                            }
                        }

                    }

                    //add line of whitespace
                    //Console.WriteLine("");


                }
                //END all possible moves for knights
                //if piece is a bishop
                if (FreindlyPieces[i] > 30 && FreindlyPieces[i] < 40)
                {
                    //Console.WriteLine("Bishop id#" + FreindlyPieces[i]);

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
                                if (FreindlyPieceLocations[i] - (11 * j) == position) { return false; };
                            }
                        }
                        //if target space has a piece on it
                        else
                        {
                            if (FreindlyPieceLocations[i] - (11 * j) == position) { return false; };
                            break;
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
                                if (FreindlyPieceLocations[i] + (11 * j) == position) { return false; };
                            }
                        }
                        //if target space has a piece on it
                        else
                        {
                            if (FreindlyPieceLocations[i] + (11 * j) == position) { return false; };
                            break;
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
                                if (FreindlyPieceLocations[i] - (9 * j) == position) { return false; };
                            }
                        }
                        //if target space has a piece on it
                        else
                        {
                            if (FreindlyPieceLocations[i] - (9 * j) == position) { return false; };
                            break;
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
                                if (FreindlyPieceLocations[i] + (9 * j) == position) { return false; };
                            }
                        }
                        //if target space has a piece on it
                        else
                        {
                            if (FreindlyPieceLocations[i] + (9 * j) == position) { return false; };
                            break;
                        }

                    }
                    //add line of whitespace
                    //Console.WriteLine("");


                }
                //END all possible moves for bishops
                //if piece is a queen
                if (FreindlyPieces[i] > 70 && FreindlyPieces[i] < 80)
                {
                    //Console.WriteLine("Queen id#" + FreindlyPieces[i]);

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
                                if (FreindlyPieceLocations[i] - (10 * j) == position) { return false; }
                            }
                        }
                        //if target space has a piece on it
                        else
                        {

                            if (FreindlyPieceLocations[i] - (10 * j) == position) { return false; }
                            break;


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
                                if (FreindlyPieceLocations[i] + (10 * j) == position) { return false; };
                            }
                        }
                        //if target space has a piece on it
                        else
                        {

                            if (FreindlyPieceLocations[i] + (10 * j) == position) { return false; }
                            break;


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
                                if (FreindlyPieceLocations[i] + j == position) { return false; }
                            }
                        }
                        //if target space has a piece on it
                        else
                        {

                            if (FreindlyPieceLocations[i] + j == position) { return false; }
                            break;


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
                                if (FreindlyPieceLocations[i] - j == position) { return false; }
                            }
                        }
                        //if target space has a piece on it
                        else
                        {

                            if (FreindlyPieceLocations[i] - j == position) { return false; }
                            break;


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
                                if (FreindlyPieceLocations[i] - (11 * j) == position) { return false; };
                            }
                        }
                        //if target space has a piece on it
                        else
                        {
                            if (FreindlyPieceLocations[i] - (11 * j) == position) { return false; };
                            break;
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
                                if (FreindlyPieceLocations[i] + (11 * j) == position) { return false; };
                            }
                        }
                        //if target space has a piece on it
                        else
                        {
                            if (FreindlyPieceLocations[i] + (11 * j) == position) { return false; };
                            break;
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
                                if (FreindlyPieceLocations[i] - (9 * j) == position) { return false; };
                            }
                        }
                        //if target space has a piece on it
                        else
                        {
                            if (FreindlyPieceLocations[i] - (9 * j) == position) { return false; };
                            break;
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
                                if (FreindlyPieceLocations[i] + (9 * j) == position) { return false; };
                            }
                        }
                        //if target space has a piece on it
                        else
                        {
                            if (FreindlyPieceLocations[i] + (9 * j) == position) { return false; };
                            break;
                        }
                    }



                        //add line of whitespace
                        //Console.WriteLine("");


                }
                //END all possible moves for queens

            }


            LogWriter.Log("Projection pass on "+position);

            return true; 
        }



    }
}
