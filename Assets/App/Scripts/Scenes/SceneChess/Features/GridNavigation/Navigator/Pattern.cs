using System.Collections.Generic;
using UnityEngine;

using App.Scripts.Scenes.SceneChess.Features.ChessField.GridMatrix;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Types;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Piece;


namespace App.Scripts.Scenes.SceneChess.Features.GridNavigation.Navigator
{
    public abstract class Pattern
    {
        public abstract List<Vector2Int> PatternPositions { get; }
        public abstract List<Vector2Int> GetPatternIndexes(Vector2Int pos);
        public abstract Direction DirectionPattern { get; set; }
        public abstract bool IsFree(Vector2Int pos);
    }


    public class UpPattern : Pattern
    {
        List<Vector2Int> massPatternPositions = new List<Vector2Int>(1);
        private Direction direction;
        private ChessGrid grid;
        public override List<Vector2Int> PatternPositions => massPatternPositions;

        public override Direction DirectionPattern { get => direction; set => direction = value; }

        public UpPattern(Vector2Int pos, Direction dir, ChessGrid _grid)
        {
            DirectionPattern = dir;
            grid = _grid;
            massPatternPositions = GetPatternIndexes(pos);
        }
        public override bool IsFree(Vector2Int pos)
        {
            ChessUnit piece = grid.Get(pos);
            if (piece == null) return true;
            else return false;
        }


        public override List<Vector2Int> GetPatternIndexes(Vector2Int pos)
        {
            int offset = 0;
            switch (DirectionPattern)
            {
                case Direction.up:
                    offset = 1;
                    break;
                case Direction.down:
                    offset = -1;
                    break;
            }

            List<Vector2Int> list = new List<Vector2Int>();

            if (pos.y + offset >= 0 && pos.y + offset < 8)
            {
                Vector2Int newPos = new Vector2Int(pos.x, pos.y + offset);
                if (IsFree(newPos) == true)
                {
                    list.Add(new Vector2Int(pos.x, pos.y + offset));
                }

            }

            return list;
        }
    }


    public class DiagonalPattern : Pattern
    {
        List<Vector2Int> massPatternPositions = new List<Vector2Int>();
        private Direction direction;
        private ChessGrid grid;

        public override List<Vector2Int> PatternPositions => massPatternPositions;
        public override Direction DirectionPattern { get => direction; set => direction = value; }

        public DiagonalPattern(Vector2Int pos, Direction dir, ChessGrid _grid)
        {
            grid = _grid;
            DirectionPattern = dir;
            massPatternPositions = GetPatternIndexes(pos);
        }

        public override bool IsFree(Vector2Int pos)
        {
            if (pos.x >= 0 && pos.x < 8 && pos.y >= 0 && pos.y < 8)
            {
                ChessUnit piece = grid.Get(pos);
                if (piece == null) return true;
            }

            return false;
        }

        private List<Vector2Int> Up(Vector2Int pos)
        {
            List<Vector2Int> list = new List<Vector2Int>();

            int i = pos.y + 1;

            int offsetX = 1;

            Vector2Int _pos = pos;
            _pos = new Vector2Int(_pos.x + offsetX, i);

            while (i < 8 && IsFree(_pos) == true)
            {
                if (_pos.x >= 0 && _pos.x < 8)
                {
                    list.Add(new Vector2Int(_pos.x, i));
                }
                i++;
                _pos = new Vector2Int(_pos.x + offsetX, i);
            }

            i = pos.y + 1;
            _pos = new Vector2Int(pos.x - offsetX, i);
            while (i < 8 && IsFree(_pos) == true)
            {
                if (pos.x >= 0 && pos.x < 8)
                {
                    list.Add(new Vector2Int(_pos.x, i));
                }
                i++;
                _pos = new Vector2Int(_pos.x - offsetX, i);
            }

            return list;
        }

        private List<Vector2Int> Down(Vector2Int pos)
        {
            List<Vector2Int> list = new List<Vector2Int>();
            int offsetX = 1;
            int i = pos.y - 1;


            Vector2Int _pos = pos;
            _pos = new Vector2Int(_pos.x + offsetX, i);

            while (i >= 0 && IsFree(_pos) == true)
            {
                if (_pos.x >= 0 && _pos.x < 8)
                {
                    list.Add(new Vector2Int(_pos.x, i));
                }
                i--;
                _pos = new Vector2Int(_pos.x + offsetX, i);
            }

            i = pos.y - 1;
            _pos = new Vector2Int(pos.x - offsetX, i);
            while (i >= 0 && IsFree(_pos) == true)
            {
                if (pos.x >= 0 && pos.x < 8)
                {
                    list.Add(new Vector2Int(_pos.x, i));
                }
                i--;
                _pos = new Vector2Int(_pos.x - offsetX, i);
            }
            return list;
        }

        public override List<Vector2Int> GetPatternIndexes(Vector2Int pos)
        {
            List<Vector2Int> list = new List<Vector2Int>();

            switch (DirectionPattern)
            {
                case Direction.up:
                    list = Up(pos);
                    break;
                case Direction.down:
                    list = Down(pos);
                    break;
            }
            return list;
        }
    }


    public class NextFieldPattern : Pattern
    {
        List<Vector2Int> massPatternPositions = new List<Vector2Int>();
        private Direction direction;
        private ChessGrid grid;
        public override List<Vector2Int> PatternPositions => massPatternPositions;
        public override Direction DirectionPattern { get => direction; set => direction = value; }
        public NextFieldPattern(Vector2Int pos, Direction dir, ChessGrid _grid)
        {
            grid = _grid;
            DirectionPattern = dir;
            massPatternPositions = GetPatternIndexes(pos);


        }

        public override bool IsFree(Vector2Int pos)
        {
            ChessUnit piece = grid.Get(pos);
            if (piece == null) return true;
            else return false;
        }

        private int DirectionOffset()
        {
            int offset = 0;

            switch (DirectionPattern)
            {
                case Direction.up:
                    offset = 1;
                    break;
                case Direction.down:
                    offset = -1;
                    break;
            }

            return offset;
        }

        private List<Vector2Int> Up(Vector2Int pos)
        {
            List<Vector2Int> list = new List<Vector2Int>();
            for (int i = DirectionOffset(); i < 2; i++)
            {
                for (int j = DirectionOffset(); j < 2;)
                {
                    int col = pos.x + i;
                    int row = pos.y + j;

                    Vector2Int newPos = new Vector2Int(col, row);

                    if (pos != new Vector2Int(col, row) && col >= 0 && col < 8 && row >= 0 && row < 8 && row >= pos.y && IsFree(newPos) == true)
                    {
                        list.Add(new Vector2Int(col, row));
                        j++;
                    }

                    else j++;
                }
            }
            return list;
        }

        private List<Vector2Int> Down(Vector2Int pos)
        {
            List<Vector2Int> list = new List<Vector2Int>();
            for (int i = DirectionOffset(); i < 2; i++)
            {
                for (int j = DirectionOffset(); j < 2;)
                {
                    int col = pos.x + i;
                    int row = pos.y + j;

                    Vector2Int newPos = new Vector2Int(col, row);

                    if (pos != new Vector2Int(col, row) && col >= 0 && col < 8 && row >= 0 && row < 8 && row <= pos.y && IsFree(newPos) == true)
                    {

                        list.Add(newPos);
                        j++;
                    }

                    else j++;
                }
            }

            return list;
        }


        public override List<Vector2Int> GetPatternIndexes(Vector2Int pos)
        {
            List<Vector2Int> list = new List<Vector2Int>();

            switch (DirectionPattern)
            {
                case Direction.up:
                    list = Up(pos);
                    break;
                case Direction.down:
                    list = Down(pos);
                    break;
            }

            return list;
        }
    }


    public class VerticalPattern : Pattern
    {
        List<Vector2Int> massPatternPositions = new List<Vector2Int>();
        private Direction direction;
        private ChessGrid grid;
        private bool isCanMove = true;

        public override List<Vector2Int> PatternPositions => massPatternPositions;
        public override Direction DirectionPattern { get => direction; set => direction = value; }

        public VerticalPattern(Vector2Int pos, Direction dir, ChessGrid _grid)
        {
            grid = _grid;
            DirectionPattern = dir;
            massPatternPositions = GetPatternIndexes(pos);
        }

        public override bool IsFree(Vector2Int pos)
        {
            if (pos.x >= 0 && pos.x < 8 && pos.y >= 0 && pos.y < 8)
            {
                ChessUnit piece = grid.Get(pos);
                if (piece == null) return true;
            }

            return false;
        }

        private void IsCanMove(Vector2Int pos)
        {
            switch (DirectionPattern)
            {
                case Direction.down:
                    if (pos.y == 0) isCanMove = false;
                    break;
                case Direction.up:
                    if (pos.y == 7) isCanMove = false;
                    break;
            }
        }

        public List<Vector2Int> Up(Vector2Int pos)
        {

            List<Vector2Int> list = new List<Vector2Int>();
            // Debug.Log(isCanMove);
            if (isCanMove == true)
            {

                for (int i = pos.y + 1; i < 8; i++)
                {
                    // Debug.Log(i);
                    Vector2Int newPos = new Vector2Int(pos.x, i);
                    if (IsFree(newPos) == true)
                    {
                        if (pos != newPos)
                        {
                            list.Add(new Vector2Int(pos.x, i));
                        }
                    }
                    return list;

                }
                return list;

            }

            return null;

        }

        public List<Vector2Int> Down(Vector2Int pos)
        {
            List<Vector2Int> list = new List<Vector2Int>();
            if (isCanMove == true)
            {
                for (int i = pos.y; i >= 0; i--)
                {
                    Vector2Int newPos = new Vector2Int(pos.x, i);
                    if (IsFree(newPos) == true)
                    {
                        if (pos != newPos)
                        {

                            list.Add(new Vector2Int(pos.x, i));
                        }
                    }
                    i = 1;
                }
                return list;
            }
            return null;
        }

        public override List<Vector2Int> GetPatternIndexes(Vector2Int pos)
        {
            List<Vector2Int> list = new List<Vector2Int>();

            switch (DirectionPattern)
            {
                case Direction.up:
                    list = Up(pos);
                    break;
                case Direction.down:
                    list = Down(pos);
                    break;
            }

            return list;
        }
    }

    public class HorizontalPattern : Pattern
    {
        List<Vector2Int> massPatternPositions = new List<Vector2Int>();
        private Direction direction;
        private ChessGrid grid;
        public override List<Vector2Int> PatternPositions => massPatternPositions;
        public override Direction DirectionPattern { get => direction; set => direction = value; }
        private bool isCanMove = true;

        public HorizontalPattern(Vector2Int pos, Direction dir, ChessGrid _grid)
        {
            grid = _grid;
            DirectionPattern = dir;
            massPatternPositions = GetPatternIndexes(pos);
        }

        private void IsCanMove(Vector2Int pos)
        {
            switch (DirectionPattern)
            {
                case Direction.down:
                    if (pos.y == 0) isCanMove = false;
                    break;
                case Direction.up:
                    if (pos.y == 7) isCanMove = false;
                    break;
            }
        }

        public override bool IsFree(Vector2Int pos)
        {
            if (pos.x >= 0 && pos.x < 8 && pos.y >= 0 && pos.y < 8)
            {
                ChessUnit piece = grid.Get(pos);
                if (piece == null) return true;
            }

            return false;
        }

        public override List<Vector2Int> GetPatternIndexes(Vector2Int pos)
        {
            List<Vector2Int> list = new List<Vector2Int>();
            int i = pos.x - 1;
            Vector2Int _pos = new Vector2Int(i, pos.y);

            while (i >= 0 && IsFree(_pos) == true)
            {
                _pos = new Vector2Int(i, pos.y);
                if (IsFree(_pos) != false)
                {
                    list.Add(_pos);
                }
                i--;
            }

            i = pos.x + 1;
            _pos = new Vector2Int(i, pos.y);

            while (i < 8 && IsFree(_pos) == true)
            {
                _pos = new Vector2Int(i, pos.y);
                if (IsFree(_pos) != false)
                {
                    list.Add(_pos);
                }

                i++;
            }

            return list;
        }
    }


    public class TheLetterGPattern : Pattern
    {
        List<Vector2Int> massPatternPositions = new List<Vector2Int>();
        private Direction direction;
        private ChessGrid gridPattern;
        public override List<Vector2Int> PatternPositions => massPatternPositions;
        public override Direction DirectionPattern { get => direction; set => direction = value; }

        public TheLetterGPattern(Vector2Int pos, Direction dir, ChessGrid _grid)
        {
            DirectionPattern = dir;
            gridPattern = _grid;
            massPatternPositions = GetPatternIndexes(pos);

        }

        public override bool IsFree(Vector2Int pos)
        {
            if (pos.x >= 0 && pos.x < 8 && pos.y >= 0 && pos.y < 8)
            {
                ChessUnit piece = gridPattern.Get(pos);
                if (piece == null) return true;
            }

            return false;
        }

        public List<Vector2Int> Up(Vector2Int pos)
        {
            List<Vector2Int> list = new List<Vector2Int>();
            if (pos.x - 1 >= 0 && pos.y + 2 < 8 && IsFree(new Vector2Int(pos.x - 1, pos.y + 2)) == true) list.Add(new Vector2Int(pos.x - 1, pos.y + 2));
            if (pos.x + 1 < 8 && pos.y + 2 < 8 && IsFree(new Vector2Int(pos.x + 1, pos.y + 2)) == true) list.Add(new Vector2Int(pos.x + 1, pos.y + 2));
            if (pos.x - 2 >= 0 && pos.y + 1 < 8 && IsFree(new Vector2Int(pos.x - 2, pos.y + 1)) == true) list.Add(new Vector2Int(pos.x - 2, pos.y + 1));
            if (pos.x + 2 < 8 && pos.y + 1 >= 0 && pos.y + 1 < 8 && IsFree(new Vector2Int(pos.x + 2, pos.y + 1)) == true) list.Add(new Vector2Int(pos.x + 2, pos.y + 1));

            return list;
        }

        public List<Vector2Int> Down(Vector2Int pos)
        {
            List<Vector2Int> list = new List<Vector2Int>();
            if (pos.x - 1 >= 0 && pos.y - 2 >= 0) list.Add(new Vector2Int(pos.x - 1, pos.y - 2));
            if (pos.x + 1 < 8 && pos.y - 2 >= 0) list.Add(new Vector2Int(pos.x + 1, pos.y - 2));
            if (pos.x - 2 >= 0 && pos.y - 1 >= 0) list.Add(new Vector2Int(pos.x - 2, pos.y - 1));
            if (pos.x + 2 < 8 && pos.y - 1 >= 0) list.Add(new Vector2Int(pos.x + 2, pos.y - 1));

            return list;
        }


        public override List<Vector2Int> GetPatternIndexes(Vector2Int pos)
        {
            List<Vector2Int> list = new List<Vector2Int>();

            switch (DirectionPattern)
            {
                case Direction.up:
                    list = Up(pos);
                    break;
                case Direction.down:
                    list = Down(pos);
                    break;
            }

            return list;
        }

    }
}
