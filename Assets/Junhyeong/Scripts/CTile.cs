using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// ť�� �ϳ��� Ÿ���̶�� ģ��.
/// Ŭ���ϸ� ���� ���Ѵ�.
/// </summary>
/// 
public class CTile : MonoBehaviour
{
    public ETileType colorType;

    //Material tileMaterial;
    public SpriteRenderer tileSpriteRenderer;

    [SerializeField] Vector2 myPosition;
    bool isBlackTile = false;
    bool isMoveTile = false;
    bool isIllGeneratorTile = false;
    bool isIll = false;

    [SerializeField] bool isEndTile = false;
    [SerializeField] bool isStartTile = false;

    ParticleSystem touchEffect;
    [SerializeField] ParticleSystem clearEffect;
    public bool isUsedEffect { get; set; } = false;

    public CTile nodeParent = null;

    public Vector2 GetTilePosition => myPosition;
    public bool GetIsStartTile { get => isStartTile; }
    public bool GetIsEndTile { get => isEndTile; }
    public void SetTilePosition(float _x, float _y)
    {
        myPosition.x = _x;
        myPosition.y = _y;
    }

    void InitTile()
    {
        tileSpriteRenderer = GetComponent<SpriteRenderer>();

        touchEffect = transform.GetChild(1).GetComponent<ParticleSystem>();

        ChangeColor();

        if (this.colorType != ETileType.None) GetParticles();
    }

    private void Start()
    {
        InitTile();

        //if (this.colorType == ETileType.None) Destroy(this.gameObject); /// ��Ÿ���� ������ �ƿ� �������� ���ؾ���

        if (isBlackTile)
        {
            BecomeBlackTile();
        }
//         else if (isMoveTile)
//         {
//             GameManager.Instance.StartMoveTiles();
//             //StartCoroutine(MoveTileMovingLegacy());
//         }
//         else if (isIllGeneratorTile)
//         {
//             GameManager.Instance.StartInfectionTiles();
//         }
    }

    void GetParticles()
    {
        clearEffect = Instantiate(GameManager.Instance.clearParticlePrefabs[GameManager.Instance.usingParticleIndex]);
        clearEffect.transform.position = transform.position;
    }

    //     private void OnMouseDown()
    //     {
    //         //Ư��Ÿ���� ���� �� ����.
    //         if (colorType == ETileType.Start || colorType == ETileType.End || colorType == ETileType.Black || isIllGeneratorTile) return;
    // 
    //         // �����Ǿ��ִ� ģ����� ���� Ǯ���ְ� ���������� (���������� �Ϸ��� original���� ���� �����ɶ� �����ؾ���)
    //         if (isIll)
    //         {
    //             isIll = false;
    //             colorType = (ETileType)Random.Range((int)ETileType.Red - 1, (int)ETileType.Purple); // �Ʒ��� ++���ֱ⶧���� �ϳ�����
    //             GameManager.Instance.infectionCount--;
    //         }
    // 
    //         colorType++;
    // 
    //         if (7 < (int)colorType) colorType = ETileType.Red;
    // 
    //         ChangeColor();
    // 
    //         GameManager.Instance.ClearEffect();
    // 
    //         touchEffect.Play();
    //     }

    public void TouchedFunction()
    {
        if (GameManager.Instance.isClearChapter) return;

        if (colorType == ETileType.None) return;
        //Ư��Ÿ���� ���� �� ����.
        if (/*colorType == ETileType.Start || colorType == ETileType.End || */colorType == ETileType.Black || colorType == ETileType.Move || isIllGeneratorTile) return;

        // �����Ǿ��ִ� ģ����� ���� Ǯ���ְ� ���������� (���������� �Ϸ��� original���� ���� �����ɶ� �����ؾ���)
        if (isIll)
        {
            isIll = false;
            colorType = (ETileType)Random.Range((int)ETileType.Red - 1, (int)ETileType.Purple); // �Ʒ��� ++���ֱ⶧���� �ϳ�����
        }

        colorType++;

        if (7 < (int)colorType) colorType = ETileType.Red;

        ChangeColor();

        GameManager.Instance.ClearEffect();

        touchEffect.Play();

        SoundManager.Instance.PlayTapSound();
    }

    public void PlayClearEffect()
    {
        clearEffect.Play();
    }

    public void RemainingParticle()
    {
        if (isUsedEffect == false && clearEffect != null)
        {
            Destroy(clearEffect.gameObject);
        }
    }

    public void SetStartTile()
    {
        colorType = ETileType.Start;
    }

    public void SetEndTile()
    {
        colorType = ETileType.End;
    }

    public void RefreshColorForMaker()
    {
        tileSpriteRenderer = GetComponent<SpriteRenderer>();

        ChangeColorForMaker();
    }
    void ChangeColor()
    {
        switch (colorType)
        {
            case ETileType.None:
                tileSpriteRenderer.sprite = GameManager.Instance.spriteList[(int)ETileType.None];
                break;
            case ETileType.Red:
                tileSpriteRenderer.sprite = GameManager.Instance.spriteList[(int)ETileType.Red];
                break;
            case ETileType.Orange:
                tileSpriteRenderer.sprite = GameManager.Instance.spriteList[(int)ETileType.Orange];
                break;
            case ETileType.Yellow:
                tileSpriteRenderer.sprite = GameManager.Instance.spriteList[(int)ETileType.Yellow];
                break;
            case ETileType.Green:
                tileSpriteRenderer.sprite = GameManager.Instance.spriteList[(int)ETileType.Green];
                break;
            case ETileType.Blue:
                tileSpriteRenderer.sprite = GameManager.Instance.spriteList[(int)ETileType.Blue];
                break;
            case ETileType.Indigo:
                tileSpriteRenderer.sprite = GameManager.Instance.spriteList[(int)ETileType.Indigo];
                break;
            case ETileType.Purple:
                tileSpriteRenderer.sprite = GameManager.Instance.spriteList[(int)ETileType.Purple];
                break;
            case ETileType.Black:
                tileSpriteRenderer.sprite = GameManager.Instance.spriteList[(int)ETileType.Black];
                isBlackTile = true;
                break;
            case ETileType.Move:
                tileSpriteRenderer.sprite = GameManager.Instance.spriteList[(int)ETileType.Move];
                isMoveTile = true;
                break;
            case ETileType.InfectionMother:
                tileSpriteRenderer.sprite = GameManager.Instance.spriteList[(int)ETileType.InfectionMother];
                isIllGeneratorTile = true;
                break;
            case ETileType.Infection:
                tileSpriteRenderer.sprite = GameManager.Instance.spriteList[(int)ETileType.Infection];
                isIll = true;
                break;
            case ETileType.Start:
                int _indexStart = Random.Range(1, 8);
                tileSpriteRenderer.sprite = GameManager.Instance.spriteList[_indexStart];
                isStartTile = true;
                //AddStartEndEffect();
                colorType = (ETileType)_indexStart;
                break;
            case ETileType.End:
                int _indexEnd = Random.Range(1, 8);
                tileSpriteRenderer.sprite = GameManager.Instance.spriteList[_indexEnd];
                isEndTile = true;
                //AddStartEndEffect();
                colorType = (ETileType)_indexEnd;

                while (GameManager.Instance.pathFinder.CheckStartTile_New().colorType == colorType) // ��ŸƮ�� �Ȱ�ġ��
                {
                    int v = Random.Range(1, 8);
                    colorType = (ETileType)v;
                    tileSpriteRenderer.sprite = GameManager.Instance.spriteList[v];
                }
                break;

            case ETileType.Random:
                int _randomIndex = Random.Range(1, 8);
                tileSpriteRenderer.sprite = GameManager.Instance.spriteList[_randomIndex];
                colorType = (ETileType)_randomIndex;
                break;
        }
    }

    void ChangeColorForMaker()
    {
        switch (colorType)
        {
            case ETileType.None:
                tileSpriteRenderer.sprite = GameManager.Instance.spriteList[(int)ETileType.None];
                break;
            case ETileType.Red:
                tileSpriteRenderer.sprite = GameManager.Instance.spriteList[(int)ETileType.Red];
                break;
            case ETileType.Orange:
                tileSpriteRenderer.sprite = GameManager.Instance.spriteList[(int)ETileType.Orange];
                break;
            case ETileType.Yellow:
                tileSpriteRenderer.sprite = GameManager.Instance.spriteList[(int)ETileType.Yellow];
                break;
            case ETileType.Green:
                tileSpriteRenderer.sprite = GameManager.Instance.spriteList[(int)ETileType.Green];
                break;
            case ETileType.Blue:
                tileSpriteRenderer.sprite = GameManager.Instance.spriteList[(int)ETileType.Blue];
                break;
            case ETileType.Indigo:
                tileSpriteRenderer.sprite = GameManager.Instance.spriteList[(int)ETileType.Indigo];
                break;
            case ETileType.Purple:
                tileSpriteRenderer.sprite = GameManager.Instance.spriteList[(int)ETileType.Purple];
                break;
            case ETileType.Black:
                tileSpriteRenderer.sprite = GameManager.Instance.spriteList[(int)ETileType.Black];
                isMoveTile = true;
                break;
            case ETileType.Move:
                tileSpriteRenderer.sprite = GameManager.Instance.spriteList[(int)ETileType.Move];
                isMoveTile = true;
                break;
            case ETileType.InfectionMother:
                tileSpriteRenderer.sprite = GameManager.Instance.spriteList[(int)ETileType.InfectionMother];
                isIllGeneratorTile = true;
                break;
            case ETileType.Infection:
                tileSpriteRenderer.sprite = GameManager.Instance.spriteList[(int)ETileType.Infection];
                isIll = true;
                break;
            case ETileType.Start:
                int _indexStart = Random.Range(1, 8);
                tileSpriteRenderer.sprite = GameManager.Instance.spriteList[_indexStart];
                isStartTile = true;
                AddStartEndEffect();
                break;
            case ETileType.End:
                int _indexEnd = Random.Range(1, 8);
                tileSpriteRenderer.sprite = GameManager.Instance.spriteList[_indexEnd];
                isEndTile = true;
                AddStartEndEffect();
                break;
            case ETileType.Random:
                int _indexEnd2 = Random.Range(1, 8);
                tileSpriteRenderer.sprite = GameManager.Instance.spriteList[_indexEnd2];
                break;

        }
    }

    /// �����ð� �� ������ Ÿ�Ϸ� ���� �� �ٽ� �����ð� �� ���� Ÿ�Ϸ� ���ƿ´�.
    public void BecomeBlackTile()
    {
        Timer timer = new Timer();

        ETileType oriColor = (ETileType)Random.Range(1, 8);
        //ETileType oriColor = colorType;

        System.Action acBeBlack = BeBlack;
        System.Action acBackColor = BackOriColor;

        /// �ð��� ���� �� ����������

        float becomeTime = GameManager.Instance.becomeBlackTime;
        float returnTime = becomeTime + GameManager.Instance.returnOriColorTime;

        StartCoroutine(timer.SpectialTileTimer(becomeTime, acBeBlack));

        /// �ð��� ���� �� ����������
        /// 

        StartCoroutine(timer.SpectialTileTimer(returnTime, acBackColor));

        void BeBlack()
        {
            colorType = ETileType.Black;

            ChangeColor();
        }

        void BackOriColor()
        {
            colorType = oriColor;

            ChangeColor();
        }
    }

    void AddStartEndEffect()
    {
        var obj = Instantiate<GameObject>(GameManager.Instance.startEndParticle, this.transform);
    }

    IEnumerator MoveTileMovingLegacy()
    {
        yield return new WaitForSeconds(GameManager.Instance.moveTileMovingTime);

        int _x = (int)myPosition.x;
        int _y = (int)myPosition.y;

        int randomDirectionIndex = Random.Range(0, DirectionCheck().Count);

        CTile myTile = GameManager.Instance.boardMaker.GetBoardData[_x, _y];

        ETileType tempColor = DirectionCheck()[randomDirectionIndex].colorType;

        CTile targetTile = DirectionCheck()[randomDirectionIndex];

        targetTile.colorType = myTile.colorType;

        myTile.colorType = tempColor;

        Debug.Log("����Ÿ���� Ÿ��Ÿ���� = " + targetTile.GetTilePosition);

        this.isMoveTile = false;
        targetTile.isMoveTile = true;

        /// ������ Ÿ���� ���� �ڷ�ƾ ����
        StartCoroutine(targetTile.MoveTileMovingLegacy());

        /// ����ȭ
        targetTile.ChangeColor();
        ChangeColor();

        // �ֺ��� �����ؼ� ����Ʈ�� �ִ� �����Լ�
        List<CTile> DirectionCheck()
        {
            List<CTile> directionList = new List<CTile>(); // �����¿��� Ÿ�ϵ��� �����ؼ� Random���� �ε����� �����ϱ� ���� list

            CTile leftTile;
            CTile rightTile;
            CTile upTile;
            CTile downTile;

            if (0 < _x)
            {
                leftTile = GameManager.Instance.boardMaker.GetBoardData[_x - 1, _y];

                if (leftTile.colorType != ETileType.None && leftTile.colorType != ETileType.Start && leftTile.colorType != ETileType.End)
                    directionList.Add(leftTile);
            }
            if (_x < GameManager.Instance.boardMaker.GetWidthSize - 1)
            {
                rightTile = GameManager.Instance.boardMaker.GetBoardData[_x + 1, _y];

                if (rightTile.colorType != ETileType.None && rightTile.colorType != ETileType.Start && rightTile.colorType != ETileType.End)
                    directionList.Add(rightTile);
            }
            if (0 < _y)
            {
                downTile = GameManager.Instance.boardMaker.GetBoardData[_x, _y - 1];

                if (downTile.colorType != ETileType.None && downTile.colorType != ETileType.Start && downTile.colorType != ETileType.End)
                    directionList.Add(downTile);
            }
            if (_y < GameManager.Instance.boardMaker.GetHeightSize - 1)
            {
                upTile = GameManager.Instance.boardMaker.GetBoardData[_x, _y + 1];

                if (upTile.colorType != ETileType.None && upTile.colorType != ETileType.Start && upTile.colorType != ETileType.End)
                    directionList.Add(upTile);
            }

            return directionList;
        }
    }

    /// �����ð����� �ڸ��� �ٲٴ� Ÿ�� 4���� �������� ���ƴٴѴ�.
    public void MoveTileMovingLegacy2()
    {
        int _x = (int)myPosition.x;
        int _y = (int)myPosition.y;

        Timer timer = new Timer();

        System.Action ac = MoveTileFucntion;

        StartCoroutine(timer.SpectialTileTimer(GameManager.Instance.moveTileMovingTime, ac));

        void MoveTileFucntion() // Ÿ�ٰ� ���� �����Ѵ�;
        {

            var count = DirectionCheck().Count;

            if (count <= 0) return;

            int randomDirectionIndex = Random.Range(0, DirectionCheck().Count);

            CTile myTile = GameManager.Instance.boardMaker.GetBoardData[_x, _y];

            ETileType tempColor = DirectionCheck()[randomDirectionIndex].colorType;

            CTile targetTile = DirectionCheck()[randomDirectionIndex];

            targetTile.colorType = myTile.colorType;

            myTile.colorType = tempColor;

            Debug.Log("����Ÿ���� Ÿ��Ÿ���� = " + targetTile.GetTilePosition);

            this.isMoveTile = false;
            targetTile.isMoveTile = true;

            /// ������ Ÿ���� ���� �ڷ�ƾ ����
            targetTile.MoveTileMovingLegacy2();

            /// ����ȭ
            targetTile.ChangeColor();
            ChangeColor();

            GameManager.Instance.ClearEffect();
        }

        // �ֺ��� �����ؼ� ����Ʈ�� �ִ� �����Լ�
        List<CTile> DirectionCheck()
        {
            List<CTile> directionList = new List<CTile>(); // �����¿��� Ÿ�ϵ��� �����ؼ� Random���� �ε����� �����ϱ� ���� list

            CTile leftTile;
            CTile rightTile;
            CTile upTile;
            CTile downTile;

            /// Start,End �� Ư��Ÿ�Ϻ� ����ó���� �ʿ���

            ETileType[] condition = new ETileType[] { ETileType.None, ETileType.Start, ETileType.End, ETileType.Infection, ETileType.InfectionMother, ETileType.Black, ETileType.Move };

            bool CheckStartEnd(CTile ct)
            {
                if (ct.isStartTile || ct.isEndTile) return false;
                else return true; // ���ǿ� �ش��������
            }

            if (0 < _x)
            {
                leftTile = GameManager.Instance.boardMaker.GetBoardData[_x - 1, _y];

                if (!condition.Contains(leftTile.colorType) && CheckStartEnd(leftTile))
                    directionList.Add(leftTile);
            }
            if (_x < GameManager.Instance.boardMaker.GetWidthSize - 1)
            {
                rightTile = GameManager.Instance.boardMaker.GetBoardData[_x + 1, _y];

                if (!condition.Contains(rightTile.colorType) && CheckStartEnd(rightTile))
                    directionList.Add(rightTile);
            }
            if (0 < _y)
            {
                downTile = GameManager.Instance.boardMaker.GetBoardData[_x, _y - 1];

                if (!condition.Contains(downTile.colorType) && CheckStartEnd(downTile))
                    directionList.Add(downTile);
            }
            if (_y < GameManager.Instance.boardMaker.GetHeightSize - 1)
            {
                upTile = GameManager.Instance.boardMaker.GetBoardData[_x, _y + 1];

                if (!condition.Contains(upTile.colorType) && CheckStartEnd(upTile))
                    directionList.Add(upTile);
            }

            Debug.Log("�𷢼� ����Ʈ ī��Ʈ" + directionList.Count);

            return directionList;
        }
    }

    public void MovingTile()
    {
        int _x = (int)myPosition.x;
        int _y = (int)myPosition.y;

        var count = DirectionCheck().Count;

        if (count <= 0) return;

        int randomDirectionIndex = Random.Range(0, DirectionCheck().Count);

        CTile myTile = GameManager.Instance.boardMaker.GetBoardData[_x, _y];

        ETileType tempColor = DirectionCheck()[randomDirectionIndex].colorType;

        CTile targetTile = DirectionCheck()[randomDirectionIndex];

        targetTile.colorType = myTile.colorType;

        myTile.colorType = tempColor;

        Debug.Log("����Ÿ���� Ÿ��Ÿ���� = " + targetTile.GetTilePosition);

        this.isMoveTile = false;
        targetTile.isMoveTile = true;

        /// ����ȭ
        targetTile.ChangeColor();
        ChangeColor();

        GameManager.Instance.ClearEffect();

        List<CTile> DirectionCheck()
        {
            List<CTile> directionList = new List<CTile>(); // �����¿��� Ÿ�ϵ��� �����ؼ� Random���� �ε����� �����ϱ� ���� list

            CTile leftTile;
            CTile rightTile;
            CTile upTile;
            CTile downTile;

            /// Start,End �� Ư��Ÿ�Ϻ� ����ó���� �ʿ���

            ETileType[] condition = new ETileType[] { ETileType.None, ETileType.Start, ETileType.End, ETileType.Infection, ETileType.InfectionMother, ETileType.Black, ETileType.Move };

            bool CheckStartEnd(CTile ct)
            {
                if (ct.isStartTile || ct.isEndTile) return false;
                else return true; // ���ǿ� �ش��������
            }

            if (0 < _x)
            {
                leftTile = GameManager.Instance.boardMaker.GetBoardData[_x - 1, _y];

                if (!condition.Contains(leftTile.colorType) && CheckStartEnd(leftTile))
                    directionList.Add(leftTile);
            }
            if (_x < GameManager.Instance.boardMaker.GetWidthSize - 1)
            {
                rightTile = GameManager.Instance.boardMaker.GetBoardData[_x + 1, _y];

                if (!condition.Contains(rightTile.colorType) && CheckStartEnd(rightTile))
                    directionList.Add(rightTile);
            }
            if (0 < _y)
            {
                downTile = GameManager.Instance.boardMaker.GetBoardData[_x, _y - 1];

                if (!condition.Contains(downTile.colorType) && CheckStartEnd(downTile))
                    directionList.Add(downTile);
            }
            if (_y < GameManager.Instance.boardMaker.GetHeightSize - 1)
            {
                upTile = GameManager.Instance.boardMaker.GetBoardData[_x, _y + 1];

                if (!condition.Contains(upTile.colorType) && CheckStartEnd(upTile))
                    directionList.Add(upTile);
            }

            Debug.Log("�𷢼� ����Ʈ ī��Ʈ" + directionList.Count);

            return directionList;
        }
    }

    public void InfectTile()
    {
        int _x = (int)myPosition.x;
        int _y = (int)myPosition.y;

        // n���Ŀ� �ֺ� 4���� �� Ư��Ÿ���� �ƴϸ� ill�� ������Ų��.

        var infectableAroundTile = DirectionCheck();

        foreach (var eachTile in infectableAroundTile)
        {
            if (colorType == ETileType.Infection || colorType == ETileType.InfectionMother) // ���� ������ �����Ǿ������� �Ͼ��.
            {
                if (eachTile.colorType != ETileType.Move && eachTile.colorType != ETileType.Black) // Ư��Ÿ���϶� ����ó��
                {
                    eachTile.colorType = ETileType.Infection;

                    eachTile.isIll = true;

                    eachTile.ChangeColor();
                }
            }
        }

        if (infectableAroundTile.Count <= 0)
        {
            Debug.Log("�ֺ��� ������ų ģ�� ���� + return");
            return;
        }

        // �ֺ��� �����ؼ� ����Ʈ�� �ִ� �����Լ�
        List<CTile> DirectionCheck()
        {
            List<CTile> directionList = new List<CTile>(); // �����¿��� Ÿ�ϵ��� �����ؼ� Random���� �ε����� �����ϱ� ���� list

            CTile leftTile;
            CTile rightTile;
            CTile upTile;
            CTile downTile;

            /// Start,End �� Ư��Ÿ�Ϻ� ����ó���� �ʿ��� // �����ƴϰ� , ill���ƴϰ�, illgenerator���ƴϰ�, Move�� �ƴϾ���Ѵ�.
            /// 
            /// ill Ÿ�� �����
            ETileType[] condition = new ETileType[] { ETileType.None, ETileType.Start, ETileType.End, ETileType.Infection, ETileType.InfectionMother, ETileType.Black };

            bool CheckStartEnd(CTile ct)
            {
                if (ct.isStartTile || ct.isEndTile) return false;
                else return true; // ���ǿ� �ش��������
            }

            if (0 < _x)
            {
                leftTile = GameManager.Instance.boardMaker.GetBoardData[_x - 1, _y];

                if (!condition.Contains(leftTile.colorType) && CheckStartEnd(leftTile))
                    directionList.Add(leftTile);
            }
            if (_x < GameManager.Instance.boardMaker.GetWidthSize - 1)
            {
                rightTile = GameManager.Instance.boardMaker.GetBoardData[_x + 1, _y];

                if (!condition.Contains(rightTile.colorType) && CheckStartEnd(rightTile))
                    directionList.Add(rightTile);
            }
            if (0 < _y)
            {
                downTile = GameManager.Instance.boardMaker.GetBoardData[_x, _y - 1];

                if (!condition.Contains(downTile.colorType) && CheckStartEnd(downTile))
                    directionList.Add(downTile);
            }
            if (_y < GameManager.Instance.boardMaker.GetHeightSize - 1)
            {
                upTile = GameManager.Instance.boardMaker.GetBoardData[_x, _y + 1];

                if (!condition.Contains(upTile.colorType) && CheckStartEnd(upTile))
                    directionList.Add(upTile);
            }

            return directionList;
        }
    }

    /// �����ð� �� �ֺ� Ÿ���� ������Ų��.
    public void InfectTileRegacy()
    {
        int _x = (int)myPosition.x;
        int _y = (int)myPosition.y;

        Timer timer = new Timer(); // �ϴ� Ÿ�̸� ����

        System.Action ac = InfectTiles;

        Debug.Log("��������");

        StartCoroutine(GameManager.Instance.infectionTimer.SpectialTileTimer(GameManager.Instance.illSpreadTime, ac));

        // n���Ŀ� �ֺ� 4���� �� Ư��Ÿ���� �ƴϸ� ill�� ������Ų��.

        void InfectTiles()
        {
            var temp = DirectionCheck();

            if (temp.Count <= 0)
            {
                Debug.Log("�ֺ��� ������ų ģ�� ���� + return");
                return;
            }

            foreach (var eachTile in temp)
            {
                if (colorType == ETileType.Infection || colorType == ETileType.InfectionMother) // ���� ������ �����Ǿ������� �Ͼ��.
                {
                    if (eachTile.isMoveTile == false || eachTile.isBlackTile == false) // ����Ÿ���� �ƴҶ� ����ó��
                    {
                        eachTile.colorType = ETileType.Infection;

                        eachTile.isIll = true;

                        eachTile.ChangeColor();
                    }
                }
            }
        }

        // �ֺ��� �����ؼ� ����Ʈ�� �ִ� �����Լ�
        List<CTile> DirectionCheck()
        {
            List<CTile> directionList = new List<CTile>(); // �����¿��� Ÿ�ϵ��� �����ؼ� Random���� �ε����� �����ϱ� ���� list

            CTile leftTile;
            CTile rightTile;
            CTile upTile;
            CTile downTile;

            /// Start,End �� Ư��Ÿ�Ϻ� ����ó���� �ʿ��� // �����ƴϰ� , ill���ƴϰ�, illgenerator���ƴϰ�, Move�� �ƴϾ���Ѵ�.
            /// 
            /// ill Ÿ�� �����
            ETileType[] condition = new ETileType[] { ETileType.None, ETileType.Start, ETileType.End, ETileType.Infection, ETileType.InfectionMother, ETileType.Black };

            if (0 < _x)
            {
                leftTile = GameManager.Instance.boardMaker.GetBoardData[_x - 1, _y];

                if (!condition.Contains(leftTile.colorType))
                    directionList.Add(leftTile);
            }
            if (_x < GameManager.Instance.boardMaker.GetWidthSize - 1)
            {
                rightTile = GameManager.Instance.boardMaker.GetBoardData[_x + 1, _y];

                if (!condition.Contains(rightTile.colorType))
                    directionList.Add(rightTile);
            }
            if (0 < _y)
            {
                downTile = GameManager.Instance.boardMaker.GetBoardData[_x, _y - 1];

                if (!condition.Contains(downTile.colorType))
                    directionList.Add(downTile);
            }
            if (_y < GameManager.Instance.boardMaker.GetHeightSize - 1)
            {
                upTile = GameManager.Instance.boardMaker.GetBoardData[_x, _y + 1];

                if (!condition.Contains(upTile.colorType))
                    directionList.Add(upTile);
            }

            return directionList;
        }
    }

    static int preventLoop = 0;
    public CTile ReturnNodeParent(CTile _tile)
    {
        preventLoop++;

        if (preventLoop > 200)
        {
            Debug.Log("���ѷ���");
            return null;
        }

        if (_tile.nodeParent == null)
        {
            //Debug.Log("������" + _tile.GetTilePosition);
            return null;
        }
        else
        {
            //Debug.Log("��δ� = " + _tile.GetTilePosition);
            return ReturnNodeParent(_tile.nodeParent);
        }
    }
}
