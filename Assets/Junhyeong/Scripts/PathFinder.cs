using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스타트 타일부터 엔드타일까지 이어지는 색이 있는가 검출하는 클래스
/// </summary>

public class PathFinder
{
    public PathFinder() { }
    public PathFinder(BoardMaker _boardMaker)
    {
        board = new CTile[_boardMaker.GetWidthSize, _boardMaker.GetHeightSize];
        board = _boardMaker.GetBoardData;
        boardMaker = _boardMaker;
    }

    CTile startTile;
    CTile endTile;
    BoardMaker boardMaker;
    CTile[,] board;

    bool firstSearch = true;
    public struct ClearCondition
    {
        public bool isClear;
        public ETileType? clearColor;
    }

    public void InitPathFinder()
    {
        //         startTile = CheckStartTile();
        //         endTile = CheckEndTile();

        startTile = CheckStartTile_New();
        endTile = CheckEndTile_New();
        firstSearch = true;
    }

    public CTile CheckIllGeneratorTile()
    {
        foreach (var tile in boardMaker.GetBoardData)
        {
            if (tile.colorType == ETileType.InfectionMother) return tile;
        }
        Debug.LogWarning("스타트가 없네요???");
        return null;
    }

    public CTile CheckStartTile()
    {
        foreach (var tile in boardMaker.GetBoardData)
        {
            if (tile.colorType == ETileType.Start) return tile;
        }
        Debug.LogWarning("스타트가 없네요???");
        return null;
    }
    public CTile CheckStartTile_New()
    {
        foreach (var tile in boardMaker.GetBoardData)
        {
            if (tile.GetIsStartTile) return tile;
        }
        Debug.LogWarning("스타트가 없네요???");
        return null;
    }
    CTile CheckEndTile()
    {
        foreach (var tile in boardMaker.GetBoardData)
        {
            if (tile.colorType == ETileType.End) return tile;
        }
        Debug.LogWarning("엔드가 없네요???");
        return null;
    }
    CTile CheckEndTile_New()
    {
        foreach (var tile in boardMaker.GetBoardData)
        {
            if (tile.GetIsEndTile) return tile;
        }
        Debug.LogWarning("엔드가 없네요???");
        return null;
    }

    private List<CTile> GetStartNeighbors(CTile startTile)
    {
        List<CTile> startNeighbors = new List<CTile>();

        int x = (int)startTile.GetTilePosition.x;
        int y = (int)startTile.GetTilePosition.y;

        int widthSize = boardMaker.GetWidthSize;
        int heightSize = boardMaker.GetHeightSize;

        var tile = boardMaker.GetBoardData;

        if (firstSearch) // currentile이 start이기때문에 처음에는 start주변 4방향먼저 탐색한다
        {
            if (x > 0)
            {
                startNeighbors.Add(tile[x - 1, y]);
            }
            if (x < widthSize - 1)
            {
                startNeighbors.Add(tile[x + 1, y]);
            }
            if (y > 0)
            {
                startNeighbors.Add(tile[x, y - 1]);
            }
            if (y < heightSize - 1)
            {
                startNeighbors.Add(tile[x, y + 1]);
            }

            firstSearch = false;
            return startNeighbors;
        }

        return startNeighbors;
    }
    private Stack<CTile> GetStartNeighborsDFS(CTile startTile)
    {
        Stack<CTile> _startNeighbors = new Stack<CTile>();

        int x = (int)startTile.GetTilePosition.x;
        int y = (int)startTile.GetTilePosition.y;

        int widthSize = boardMaker.GetWidthSize;
        int heightSize = boardMaker.GetHeightSize;

        var tile = boardMaker.GetBoardData;

        if (x > 0)
        {
            _startNeighbors.Push(tile[x - 1, y]);
        }
        if (x < widthSize - 1)
        {
            _startNeighbors.Push(tile[x + 1, y]);
        }
        if (y > 0)
        {
            _startNeighbors.Push(tile[x, y - 1]);
        }
        if (y < heightSize - 1)
        {
            _startNeighbors.Push(tile[x, y + 1]);
        }

        return _startNeighbors;
    }
    private Stack<CTile> GetNeighborsDFS(CTile _currentTile, Stack<CTile> _way, List<Stack<CTile>> _allPath)
    {
        int x = (int)_currentTile.GetTilePosition.x;
        int y = (int)_currentTile.GetTilePosition.y;

        int widthSize = boardMaker.GetWidthSize;
        int heightSize = boardMaker.GetHeightSize;

        CTile[,] tile = boardMaker.GetBoardData;

        // 좌측
        if (0 < x && tile[x - 1, y].colorType == _currentTile.colorType ||
            (0 < x && tile[x - 1, y].colorType == endTile.colorType))
        {
            CTile left = tile[x - 1, y];

            if (left.colorType == ETileType.End)
            {
                _allPath.Add(_way);
                return null;
            }

            // 들린곳이 아니라면 스택에 쌓는다.
            if (!_way.Contains(left))
            {
                _way.Push(left);
                GetNeighborsDFS(left, _way, _allPath);
            }
        }

        // 우측
        if (x < widthSize - 1 && tile[x + 1, y].colorType == _currentTile.colorType ||
            (x < widthSize - 1 && tile[x + 1, y].colorType == endTile.colorType))
        {
            CTile right = tile[x + 1, y];

            if (right.colorType == ETileType.End)
            {
                _allPath.Add(_way);
                return null;
            }

            if (!_way.Contains(right))
            {
                _way.Push(right);
                GetNeighborsDFS(right, _way, _allPath);
            }
        }
        // 아래
        if (0 < y && tile[x, y - 1].colorType == _currentTile.colorType ||
            (0 < y && tile[x, y - 1].colorType == endTile.colorType))
        {
            CTile down = tile[x, y - 1];

            if (down.colorType == ETileType.End)
            {
                _allPath.Add(_way);
                return null;
            }

            if (!_way.Contains(down))
            {
                _way.Push(down);
                GetNeighborsDFS(down, _way, _allPath);
            }
        }
        // 위
        if (y < heightSize - 1 && tile[x, y + 1].colorType == _currentTile.colorType ||
            (y < heightSize - 1 && tile[x, y + 1].colorType == endTile.colorType))
        {
            CTile up = tile[x, y + 1];

            if (up.colorType == ETileType.End)
            {
                _allPath.Add(_way);
                return null;
            }

            if (!_way.Contains(up))
            {
                _way.Push(up);
                GetNeighborsDFS(up, _way, _allPath);
            }
        }

        _way.Pop();
        return null; // 끝을 만나지 못했다면 null을 리턴한다.
    }
    public void CheckPathDFS() // 사용하지않음
    {
        firstSearch = true;

        startTile = CheckStartTile();
        endTile = CheckEndTile();

        if (startTile == null || endTile == null) // 확인
        {
            Debug.LogWarning("Start or End tile 없어요!");
        }

        List<Stack<CTile>> allPath = new List<Stack<CTile>>();

        Stack<CTile> path = GetStartNeighborsDFS(startTile);

        while (0 < path.Count)
        {
            GetNeighborsDFS(path.Pop(), new Stack<CTile>(), allPath);
        }
    }
    private List<CTile> GetNeighborsBFS(CTile currentTile)
    {
        List<CTile> neighbors = new List<CTile>();

        int x = (int)currentTile.GetTilePosition.x;
        int y = (int)currentTile.GetTilePosition.y;

        int widthSize = boardMaker.GetWidthSize;
        int heightSize = boardMaker.GetHeightSize;

        var tile = boardMaker.GetBoardData;

        //좌측
        if (x > 0 && tile[x - 1, y].colorType == currentTile.colorType ||
            (x > 0 && tile[x - 1, y].colorType == endTile.colorType)) // 왼쪽
        {
            neighbors.Add(tile[x - 1, y]);
        }
        // 우측
        if (x < widthSize - 1 && tile[x + 1, y].colorType == currentTile.colorType ||
            (x < widthSize - 1 && tile[x + 1, y].colorType == endTile.colorType)) //오른쪽
        {
            neighbors.Add(tile[x + 1, y]);
        }
        // 아래
        if (y > 0 && tile[x, y - 1].colorType == currentTile.colorType ||
            (y > 0 && tile[x, y - 1].colorType == endTile.colorType)) // 아래
        {
            neighbors.Add(tile[x, y - 1]);
        }
        // 위
        if (y < heightSize - 1 && tile[x, y + 1].colorType == currentTile.colorType ||
            (y < heightSize - 1 && tile[x, y + 1].colorType == endTile.colorType)) // 위
        {
            neighbors.Add(tile[x, y + 1]);
        }

        return neighbors;
    }
    public ClearCondition CheckPathBFS()
    {
        ClearCondition _clearCondition = new ClearCondition();

        firstSearch = true;

        Queue<CTile> queue = new Queue<CTile>();
        HashSet<CTile> visited = new HashSet<CTile>();

        startTile = CheckStartTile();
        endTile = CheckEndTile();

        if (startTile == null || endTile == null) // 확인
        {
            Debug.LogWarning("Start or End tile 없어요!");
            _clearCondition.clearColor = null;
            _clearCondition.isClear = false;

            return _clearCondition;
        }

        foreach (var firstStartTile in GetStartNeighbors(startTile))
        {
            queue.Enqueue(firstStartTile);
            Debug.Log("스타트 컬러 = " + firstStartTile.colorType.ToString());
            visited.Add(firstStartTile);
        }

        while (0 < queue.Count)
        {
            CTile currentTile = queue.Dequeue();

            if (currentTile.colorType == endTile.colorType) // 엔드에 도착했는가 확인해야한다.
            {
                if (GameManager.Instance.missionColor != ETileType.Any) // 미션컬러가 존재할때 
                {
                    if (currentTile.nodeParent.colorType == GameManager.Instance.missionColor && currentTile.nodeParent.colorType != ETileType.Infection)
                    {
                        Debug.Log(GameManager.Instance.missionColor.ToString() + " 미션성공");
                        // 미션 성공

                        Debug.Log("찾았다");
                        Debug.Log("현재타일 좌표 = " + currentTile.GetTilePosition);

                        currentTile.ReturnNodeParent(currentTile);

                        firstSearch = true;

                        _clearCondition.clearColor = currentTile.nodeParent.colorType;
                        _clearCondition.isClear = true;
                        return _clearCondition;
                    }
                }
                else if (currentTile.nodeParent.colorType != ETileType.Infection)// 미션컬러가 특정안될때
                {
                    Debug.Log(" 미션없음 성공");

                    Debug.Log("찾았다");
                    Debug.Log("현재타일 좌표 = " + currentTile.GetTilePosition);

                    currentTile.ReturnNodeParent(currentTile);

                    firstSearch = true;

                    _clearCondition.clearColor = currentTile.nodeParent.colorType;
                    _clearCondition.isClear = true;
                    return _clearCondition;
                }
            }

            foreach (var neighbor in GetNeighborsBFS(currentTile))
            {
                if (!visited.Contains(neighbor)) // 전에 방문한적 없으면 아래서 방문자 명부 등록하고 큐에 넣는다.
                {
                    if (neighbor.colorType == ETileType.End)
                    {
                        queue.Enqueue(neighbor);
                        neighbor.nodeParent = currentTile;
                        break;
                    }

                    visited.Add(neighbor);
                    neighbor.nodeParent = currentTile;
                    queue.Enqueue(neighbor);
                }
            }
        }

        Debug.Log("못찾음");
        _clearCondition.isClear = false;
        _clearCondition.clearColor = null;
        return _clearCondition;
    }

    public ClearCondition CheckPathBFS_New()
    {
        ClearCondition _clearCondition = new ClearCondition();

        firstSearch = true;

        Queue<CTile> queue = new Queue<CTile>();
        HashSet<CTile> visited = new HashSet<CTile>();

        startTile = CheckStartTile_New();
        endTile = CheckEndTile_New();

        if (!startTile.GetIsStartTile || !endTile.GetIsEndTile) // 확인
        {
            Debug.LogWarning("Start or End tile 없어요!");
            _clearCondition.clearColor = null;
            _clearCondition.isClear = false;

            return _clearCondition;
        }

        foreach (var firstStartTile in GetStartNeighbors_New(startTile))
        {
            queue.Enqueue(firstStartTile);
            Debug.Log("스타트 컬러 = " + firstStartTile.colorType.ToString());
            visited.Add(firstStartTile);
        }

        while (0 < queue.Count)
        {
            CTile currentTile = queue.Dequeue();

            if (currentTile.GetIsEndTile && currentTile.colorType == startTile.colorType) // 엔드에 도착했는가 확인해야한다.
            {
                if (GameManager.Instance.missionColor != ETileType.Any) // 미션컬러가 존재할때 
                {
                    if (currentTile.nodeParent.colorType == GameManager.Instance.missionColor && currentTile.nodeParent.colorType != ETileType.Infection)
                    {
                        Debug.Log(GameManager.Instance.missionColor.ToString() + " 미션성공");
                        // 미션 성공

                        Debug.Log("찾았다");
                        Debug.Log("현재타일 좌표 = " + currentTile.GetTilePosition);

                        currentTile.ReturnNodeParent(currentTile);

                        firstSearch = true;

                        _clearCondition.clearColor = currentTile.nodeParent.colorType;
                        _clearCondition.isClear = true;
                        return _clearCondition;
                    }
                }
                else if (currentTile.nodeParent.colorType != ETileType.Infection)// 미션컬러가 특정안될때
                {
                    Debug.Log(" 미션없음 성공");
                    Debug.Log("찾았다");
                    //Debug.Log("현재타일 좌표 = " + currentTile.GetTilePosition);

                    currentTile.ReturnNodeParent(currentTile);

                    firstSearch = true;

                    _clearCondition.clearColor = currentTile.nodeParent.colorType;
                    _clearCondition.isClear = true;
                    return _clearCondition;
                }
            }

            foreach (var neighbor in GetNeighborsBFS(currentTile))
            {
                if (!visited.Contains(neighbor)) // 전에 방문한적 없으면 아래서 방문자 명부 등록하고 큐에 넣는다.
                {
                    if (neighbor.GetIsEndTile)
                    {
                        queue.Enqueue(neighbor);
                        neighbor.nodeParent = currentTile;
                        break;
                    }

                    visited.Add(neighbor);
                    neighbor.nodeParent = currentTile;
                    queue.Enqueue(neighbor);
                }
            }
        }

        Debug.Log("못찾음");
        _clearCondition.isClear = false;
        _clearCondition.clearColor = null;
        return _clearCondition;
    }

    private List<CTile> GetStartNeighbors_New(CTile startTile)
    {
        List<CTile> startNeighbors = new List<CTile>();

        int x = (int)startTile.GetTilePosition.x;
        int y = (int)startTile.GetTilePosition.y;

        int widthSize = boardMaker.GetWidthSize;
        int heightSize = boardMaker.GetHeightSize;

        var tile = boardMaker.GetBoardData;

        if (firstSearch) // currentile이 start이기때문에 처음에는 start주변 4방향먼저 탐색한다
        {
            if (x > 0)
            {
                if (startTile.colorType == tile[x - 1, y].colorType)
                    startNeighbors.Add(tile[x - 1, y]);
            }
            if (x < widthSize - 1)
            {
                if (startTile.colorType == tile[x + 1, y].colorType)
                    startNeighbors.Add(tile[x + 1, y]);
            }
            if (y > 0)
            {
                if (startTile.colorType == tile[x, y - 1].colorType)
                    startNeighbors.Add(tile[x, y - 1]);
            }
            if (y < heightSize - 1)
            {
                if (startTile.colorType == tile[x, y + 1].colorType)
                    startNeighbors.Add(tile[x, y + 1]);
            }

            firstSearch = false;
            return startNeighbors;
        }

        return startNeighbors;
    }
}
