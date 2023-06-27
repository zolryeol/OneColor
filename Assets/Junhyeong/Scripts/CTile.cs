using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 큐브 하나를 타일이라고 친다.
/// 클릭하면 색이 변한다.
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

        //if (this.colorType == ETileType.None) Destroy(this.gameObject); /// 빈타일을 넣을지 아에 삭제할지 정해야함

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
    //         //특수타일은 누를 수 없다.
    //         if (colorType == ETileType.Start || colorType == ETileType.End || colorType == ETileType.Black || isIllGeneratorTile) return;
    // 
    //         // 감염되어있는 친구라면 감염 풀어주고 랜덤색으로 (원래색으로 하려면 original변수 만들어서 감염될때 저장해야함)
    //         if (isIll)
    //         {
    //             isIll = false;
    //             colorType = (ETileType)Random.Range((int)ETileType.Red - 1, (int)ETileType.Purple); // 아래서 ++해주기때문에 하나씩뺌
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
        //특수타일은 누를 수 없다.
        if (/*colorType == ETileType.Start || colorType == ETileType.End || */colorType == ETileType.Black || colorType == ETileType.Move || isIllGeneratorTile) return;

        // 감염되어있는 친구라면 감염 풀어주고 랜덤색으로 (원래색으로 하려면 original변수 만들어서 감염될때 저장해야함)
        if (isIll)
        {
            isIll = false;
            colorType = (ETileType)Random.Range((int)ETileType.Red - 1, (int)ETileType.Purple); // 아래서 ++해주기때문에 하나씩뺌
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

                while (GameManager.Instance.pathFinder.CheckStartTile_New().colorType == colorType) // 스타트와 안겹치게
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

    /// 일정시간 후 검은색 타일로 변한 후 다시 일정시간 후 원래 타일로 돌아온다.
    public void BecomeBlackTile()
    {
        Timer timer = new Timer();

        ETileType oriColor = (ETileType)Random.Range(1, 8);
        //ETileType oriColor = colorType;

        System.Action acBeBlack = BeBlack;
        System.Action acBackColor = BackOriColor;

        /// 시간이 지난 뒤 검은색으로

        float becomeTime = GameManager.Instance.becomeBlackTime;
        float returnTime = becomeTime + GameManager.Instance.returnOriColorTime;

        StartCoroutine(timer.SpectialTileTimer(becomeTime, acBeBlack));

        /// 시간이 지난 뒤 원래색으로
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

        Debug.Log("무브타일의 타갯타일은 = " + targetTile.GetTilePosition);

        this.isMoveTile = false;
        targetTile.isMoveTile = true;

        /// 스왑후 타겟의 무브 코루틴 실행
        StartCoroutine(targetTile.MoveTileMovingLegacy());

        /// 색변화
        targetTile.ChangeColor();
        ChangeColor();

        // 주변을 조사해서 리스트에 넣는 지역함수
        List<CTile> DirectionCheck()
        {
            List<CTile> directionList = new List<CTile>(); // 상하좌우의 타일들을 보관해서 Random으로 인덱스에 접근하기 위한 list

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

    /// 일정시간마다 자리를 바꾸는 타일 4방향 랜덤으로 돌아다닌다.
    public void MoveTileMovingLegacy2()
    {
        int _x = (int)myPosition.x;
        int _y = (int)myPosition.y;

        Timer timer = new Timer();

        System.Action ac = MoveTileFucntion;

        StartCoroutine(timer.SpectialTileTimer(GameManager.Instance.moveTileMovingTime, ac));

        void MoveTileFucntion() // 타겟과 색을 스왑한다;
        {

            var count = DirectionCheck().Count;

            if (count <= 0) return;

            int randomDirectionIndex = Random.Range(0, DirectionCheck().Count);

            CTile myTile = GameManager.Instance.boardMaker.GetBoardData[_x, _y];

            ETileType tempColor = DirectionCheck()[randomDirectionIndex].colorType;

            CTile targetTile = DirectionCheck()[randomDirectionIndex];

            targetTile.colorType = myTile.colorType;

            myTile.colorType = tempColor;

            Debug.Log("무브타일의 타갯타일은 = " + targetTile.GetTilePosition);

            this.isMoveTile = false;
            targetTile.isMoveTile = true;

            /// 스왑후 타겟의 무브 코루틴 실행
            targetTile.MoveTileMovingLegacy2();

            /// 색변화
            targetTile.ChangeColor();
            ChangeColor();

            GameManager.Instance.ClearEffect();
        }

        // 주변을 조사해서 리스트에 넣는 지역함수
        List<CTile> DirectionCheck()
        {
            List<CTile> directionList = new List<CTile>(); // 상하좌우의 타일들을 보관해서 Random으로 인덱스에 접근하기 위한 list

            CTile leftTile;
            CTile rightTile;
            CTile upTile;
            CTile downTile;

            /// Start,End 등 특수타일별 예외처리가 필요함

            ETileType[] condition = new ETileType[] { ETileType.None, ETileType.Start, ETileType.End, ETileType.Infection, ETileType.InfectionMother, ETileType.Black, ETileType.Move };

            bool CheckStartEnd(CTile ct)
            {
                if (ct.isStartTile || ct.isEndTile) return false;
                else return true; // 조건에 해당되지않음
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

            Debug.Log("디랙션 리스트 카운트" + directionList.Count);

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

        Debug.Log("무브타일의 타갯타일은 = " + targetTile.GetTilePosition);

        this.isMoveTile = false;
        targetTile.isMoveTile = true;

        /// 색변화
        targetTile.ChangeColor();
        ChangeColor();

        GameManager.Instance.ClearEffect();

        List<CTile> DirectionCheck()
        {
            List<CTile> directionList = new List<CTile>(); // 상하좌우의 타일들을 보관해서 Random으로 인덱스에 접근하기 위한 list

            CTile leftTile;
            CTile rightTile;
            CTile upTile;
            CTile downTile;

            /// Start,End 등 특수타일별 예외처리가 필요함

            ETileType[] condition = new ETileType[] { ETileType.None, ETileType.Start, ETileType.End, ETileType.Infection, ETileType.InfectionMother, ETileType.Black, ETileType.Move };

            bool CheckStartEnd(CTile ct)
            {
                if (ct.isStartTile || ct.isEndTile) return false;
                else return true; // 조건에 해당되지않음
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

            Debug.Log("디랙션 리스트 카운트" + directionList.Count);

            return directionList;
        }
    }

    public void InfectTile()
    {
        int _x = (int)myPosition.x;
        int _y = (int)myPosition.y;

        // n초후에 주변 4방향 중 특수타일이 아니면 ill로 감염시킨다.

        var infectableAroundTile = DirectionCheck();

        foreach (var eachTile in infectableAroundTile)
        {
            if (colorType == ETileType.Infection || colorType == ETileType.InfectionMother) // 내가 여전히 감염되어있을때 일어난다.
            {
                if (eachTile.colorType != ETileType.Move && eachTile.colorType != ETileType.Black) // 특수타일일때 예외처리
                {
                    eachTile.colorType = ETileType.Infection;

                    eachTile.isIll = true;

                    eachTile.ChangeColor();
                }
            }
        }

        if (infectableAroundTile.Count <= 0)
        {
            Debug.Log("주변에 감염시킬 친구 없음 + return");
            return;
        }

        // 주변을 조사해서 리스트에 넣는 지역함수
        List<CTile> DirectionCheck()
        {
            List<CTile> directionList = new List<CTile>(); // 상하좌우의 타일들을 보관해서 Random으로 인덱스에 접근하기 위한 list

            CTile leftTile;
            CTile rightTile;
            CTile upTile;
            CTile downTile;

            /// Start,End 등 특수타일별 예외처리가 필요함 // 블랙도아니고 , ill도아니고, illgenerator도아니고, Move도 아니어야한다.
            /// 
            /// ill 타일 컨디션
            ETileType[] condition = new ETileType[] { ETileType.None, ETileType.Start, ETileType.End, ETileType.Infection, ETileType.InfectionMother, ETileType.Black };

            bool CheckStartEnd(CTile ct)
            {
                if (ct.isStartTile || ct.isEndTile) return false;
                else return true; // 조건에 해당되지않음
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

    /// 일정시간 후 주변 타일을 감염시킨다.
    public void InfectTileRegacy()
    {
        int _x = (int)myPosition.x;
        int _y = (int)myPosition.y;

        Timer timer = new Timer(); // 일단 타이머 생성

        System.Action ac = InfectTiles;

        Debug.Log("감염시작");

        StartCoroutine(GameManager.Instance.infectionTimer.SpectialTileTimer(GameManager.Instance.illSpreadTime, ac));

        // n초후에 주변 4방향 중 특수타일이 아니면 ill로 감염시킨다.

        void InfectTiles()
        {
            var temp = DirectionCheck();

            if (temp.Count <= 0)
            {
                Debug.Log("주변에 감염시킬 친구 없음 + return");
                return;
            }

            foreach (var eachTile in temp)
            {
                if (colorType == ETileType.Infection || colorType == ETileType.InfectionMother) // 내가 여전히 감염되어있을때 일어난다.
                {
                    if (eachTile.isMoveTile == false || eachTile.isBlackTile == false) // 무브타일이 아닐때 예외처리
                    {
                        eachTile.colorType = ETileType.Infection;

                        eachTile.isIll = true;

                        eachTile.ChangeColor();
                    }
                }
            }
        }

        // 주변을 조사해서 리스트에 넣는 지역함수
        List<CTile> DirectionCheck()
        {
            List<CTile> directionList = new List<CTile>(); // 상하좌우의 타일들을 보관해서 Random으로 인덱스에 접근하기 위한 list

            CTile leftTile;
            CTile rightTile;
            CTile upTile;
            CTile downTile;

            /// Start,End 등 특수타일별 예외처리가 필요함 // 블랙도아니고 , ill도아니고, illgenerator도아니고, Move도 아니어야한다.
            /// 
            /// ill 타일 컨디션
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
            Debug.Log("무한루프");
            return null;
        }

        if (_tile.nodeParent == null)
        {
            //Debug.Log("마지막" + _tile.GetTilePosition);
            return null;
        }
        else
        {
            //Debug.Log("경로는 = " + _tile.GetTilePosition);
            return ReturnNodeParent(_tile.nodeParent);
        }
    }
}
