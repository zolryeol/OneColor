using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ŸƮ Ÿ�Ϻ��� ����Ÿ�ϱ��� �̾����� ���� �ִ°� �����ϴ� Ŭ����
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
        Debug.LogWarning("��ŸƮ�� ���׿�???");
        return null;
    }

    public CTile CheckStartTile()
    {
        foreach (var tile in boardMaker.GetBoardData)
        {
            if (tile.colorType == ETileType.Start) return tile;
        }
        Debug.LogWarning("��ŸƮ�� ���׿�???");
        return null;
    }
    public CTile CheckStartTile_New()
    {
        foreach (var tile in boardMaker.GetBoardData)
        {
            if (tile.GetIsStartTile) return tile;
        }
        Debug.LogWarning("��ŸƮ�� ���׿�???");
        return null;
    }
    CTile CheckEndTile()
    {
        foreach (var tile in boardMaker.GetBoardData)
        {
            if (tile.colorType == ETileType.End) return tile;
        }
        Debug.LogWarning("���尡 ���׿�???");
        return null;
    }
    CTile CheckEndTile_New()
    {
        foreach (var tile in boardMaker.GetBoardData)
        {
            if (tile.GetIsEndTile) return tile;
        }
        Debug.LogWarning("���尡 ���׿�???");
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

        if (firstSearch) // currentile�� start�̱⶧���� ó������ start�ֺ� 4������� Ž���Ѵ�
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

        // ����
        if (0 < x && tile[x - 1, y].colorType == _currentTile.colorType ||
            (0 < x && tile[x - 1, y].colorType == endTile.colorType))
        {
            CTile left = tile[x - 1, y];

            if (left.colorType == ETileType.End)
            {
                _allPath.Add(_way);
                return null;
            }

            // �鸰���� �ƴ϶�� ���ÿ� �״´�.
            if (!_way.Contains(left))
            {
                _way.Push(left);
                GetNeighborsDFS(left, _way, _allPath);
            }
        }

        // ����
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
        // �Ʒ�
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
        // ��
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
        return null; // ���� ������ ���ߴٸ� null�� �����Ѵ�.
    }
    public void CheckPathDFS() // �����������
    {
        firstSearch = true;

        startTile = CheckStartTile();
        endTile = CheckEndTile();

        if (startTile == null || endTile == null) // Ȯ��
        {
            Debug.LogWarning("Start or End tile �����!");
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

        //����
        if (x > 0 && tile[x - 1, y].colorType == currentTile.colorType ||
            (x > 0 && tile[x - 1, y].colorType == endTile.colorType)) // ����
        {
            neighbors.Add(tile[x - 1, y]);
        }
        // ����
        if (x < widthSize - 1 && tile[x + 1, y].colorType == currentTile.colorType ||
            (x < widthSize - 1 && tile[x + 1, y].colorType == endTile.colorType)) //������
        {
            neighbors.Add(tile[x + 1, y]);
        }
        // �Ʒ�
        if (y > 0 && tile[x, y - 1].colorType == currentTile.colorType ||
            (y > 0 && tile[x, y - 1].colorType == endTile.colorType)) // �Ʒ�
        {
            neighbors.Add(tile[x, y - 1]);
        }
        // ��
        if (y < heightSize - 1 && tile[x, y + 1].colorType == currentTile.colorType ||
            (y < heightSize - 1 && tile[x, y + 1].colorType == endTile.colorType)) // ��
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

        if (startTile == null || endTile == null) // Ȯ��
        {
            Debug.LogWarning("Start or End tile �����!");
            _clearCondition.clearColor = null;
            _clearCondition.isClear = false;

            return _clearCondition;
        }

        foreach (var firstStartTile in GetStartNeighbors(startTile))
        {
            queue.Enqueue(firstStartTile);
            Debug.Log("��ŸƮ �÷� = " + firstStartTile.colorType.ToString());
            visited.Add(firstStartTile);
        }

        while (0 < queue.Count)
        {
            CTile currentTile = queue.Dequeue();

            if (currentTile.colorType == endTile.colorType) // ���忡 �����ߴ°� Ȯ���ؾ��Ѵ�.
            {
                if (GameManager.Instance.missionColor != ETileType.Any) // �̼��÷��� �����Ҷ� 
                {
                    if (currentTile.nodeParent.colorType == GameManager.Instance.missionColor && currentTile.nodeParent.colorType != ETileType.Infection)
                    {
                        Debug.Log(GameManager.Instance.missionColor.ToString() + " �̼Ǽ���");
                        // �̼� ����

                        Debug.Log("ã�Ҵ�");
                        Debug.Log("����Ÿ�� ��ǥ = " + currentTile.GetTilePosition);

                        currentTile.ReturnNodeParent(currentTile);

                        firstSearch = true;

                        _clearCondition.clearColor = currentTile.nodeParent.colorType;
                        _clearCondition.isClear = true;
                        return _clearCondition;
                    }
                }
                else if (currentTile.nodeParent.colorType != ETileType.Infection)// �̼��÷��� Ư���ȵɶ�
                {
                    Debug.Log(" �̼Ǿ��� ����");

                    Debug.Log("ã�Ҵ�");
                    Debug.Log("����Ÿ�� ��ǥ = " + currentTile.GetTilePosition);

                    currentTile.ReturnNodeParent(currentTile);

                    firstSearch = true;

                    _clearCondition.clearColor = currentTile.nodeParent.colorType;
                    _clearCondition.isClear = true;
                    return _clearCondition;
                }
            }

            foreach (var neighbor in GetNeighborsBFS(currentTile))
            {
                if (!visited.Contains(neighbor)) // ���� �湮���� ������ �Ʒ��� �湮�� ��� ����ϰ� ť�� �ִ´�.
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

        Debug.Log("��ã��");
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

        if (!startTile.GetIsStartTile || !endTile.GetIsEndTile) // Ȯ��
        {
            Debug.LogWarning("Start or End tile �����!");
            _clearCondition.clearColor = null;
            _clearCondition.isClear = false;

            return _clearCondition;
        }

        foreach (var firstStartTile in GetStartNeighbors_New(startTile))
        {
            queue.Enqueue(firstStartTile);
            Debug.Log("��ŸƮ �÷� = " + firstStartTile.colorType.ToString());
            visited.Add(firstStartTile);
        }

        while (0 < queue.Count)
        {
            CTile currentTile = queue.Dequeue();

            if (currentTile.GetIsEndTile && currentTile.colorType == startTile.colorType) // ���忡 �����ߴ°� Ȯ���ؾ��Ѵ�.
            {
                if (GameManager.Instance.missionColor != ETileType.Any) // �̼��÷��� �����Ҷ� 
                {
                    if (currentTile.nodeParent.colorType == GameManager.Instance.missionColor && currentTile.nodeParent.colorType != ETileType.Infection)
                    {
                        Debug.Log(GameManager.Instance.missionColor.ToString() + " �̼Ǽ���");
                        // �̼� ����

                        Debug.Log("ã�Ҵ�");
                        Debug.Log("����Ÿ�� ��ǥ = " + currentTile.GetTilePosition);

                        currentTile.ReturnNodeParent(currentTile);

                        firstSearch = true;

                        _clearCondition.clearColor = currentTile.nodeParent.colorType;
                        _clearCondition.isClear = true;
                        return _clearCondition;
                    }
                }
                else if (currentTile.nodeParent.colorType != ETileType.Infection)// �̼��÷��� Ư���ȵɶ�
                {
                    Debug.Log(" �̼Ǿ��� ����");
                    Debug.Log("ã�Ҵ�");
                    //Debug.Log("����Ÿ�� ��ǥ = " + currentTile.GetTilePosition);

                    currentTile.ReturnNodeParent(currentTile);

                    firstSearch = true;

                    _clearCondition.clearColor = currentTile.nodeParent.colorType;
                    _clearCondition.isClear = true;
                    return _clearCondition;
                }
            }

            foreach (var neighbor in GetNeighborsBFS(currentTile))
            {
                if (!visited.Contains(neighbor)) // ���� �湮���� ������ �Ʒ��� �湮�� ��� ����ϰ� ť�� �ִ´�.
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

        Debug.Log("��ã��");
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

        if (firstSearch) // currentile�� start�̱⶧���� ó������ start�ֺ� 4������� Ž���Ѵ�
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
