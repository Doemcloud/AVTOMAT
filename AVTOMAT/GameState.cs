using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media.Animation;

namespace AVTOMAT
{
    public class GameState
    {
        public int Rows { get; }
        public int Cols { get; }
        public GridValue [,] Grid { get; }
        public Direction Dir { get; private set; }
        public int Score{ get; private set; }


        private readonly LinkedList<Position> snakeAPositions = new LinkedList<Position>();
        private readonly Random random = new Random();

        public GameState(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            Grid = new GridValue[rows, cols];
            Dir = Direction.Right;
            
            AddSnakeA();
            AddFood();
        }

        private void AddSnakeA()
        {
            int r = Rows / 2;


            for (int c = 1; c <= 3; c++)
            {
                Grid[r, c] = GridValue.SnakeA;
                snakeAPositions.AddFirst(new Position(r, c));
            }
        }

        private IEnumerable<Position> emptyPositions()
        {
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    if (Grid[r, c] == GridValue.Empty)
                    {
                        yield return new Position(r, c);
                    }
                }
            }
        }

        private void AddFood()
        {
            List<Position> empty = new List<Position>(emptyPositions());

            if (empty.Count == 0)
            {
                return;
            }

            Position pos = empty[random.Next(empty.Count)];
            Grid[pos.Row, pos.Col] = GridValue.Food;
        }

        public Position HeadPosition()
        {
            return snakeAPositions.First.Value;
        }

        public Position TailPosition()
        {
            return snakeAPositions.Last.Value;
        }


        public IEnumerable<Position> SnakeAPositions()
        {
            return snakeAPositions;
        }

        private void AddHead(Position pos)
        {
            snakeAPositions.AddFirst(pos);
            Grid[pos.Row, pos.Col] = GridValue.SnakeA;
            
        }

        private void RemoveTail()
        {
            Position tail = snakeAPositions.Last.Value;
            Grid[tail.Row, tail.Col] = GridValue.Empty;
            snakeAPositions.RemoveLast();
            
        } 

        public void ChangeDirection(Direction dir)
        {
            Dir = dir;
        }

        private bool OutsideGrid(Position pos)
        {
            return pos.Row < 0 || pos.Row >= Rows || pos.Col < 0 || pos.Col >= Cols;
        }

        private GridValue Willhit(Position newHeadPos)
        {
            if (OutsideGrid(newHeadPos))
            {
                return GridValue.Outside;
            }

            if (newHeadPos == TailPosition())
            {
                return GridValue.Empty;
            }
            return Grid[newHeadPos.Row, newHeadPos.Col];
        }

        public void Move()
        {
            Position newHeadPos = HeadPosition().Translate(Dir);
            GridValue hit = Willhit(newHeadPos);


            if (hit == GridValue.Outside || hit == GridValue.SnakeA)
            {
                GameOver = true;
            }
            
            else if (hit == GridValue.Empty)
            {
                RemoveTail();
                AddHead(newHeadPos);
                
            }
            else if (hit == GridValue.Food)
            {
                AddHead(newHeadPos);
                Score++;
                AddFood();
            }
        }
    }
    
    
}