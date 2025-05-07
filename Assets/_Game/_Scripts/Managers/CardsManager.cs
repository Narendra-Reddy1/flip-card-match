using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AYellowpaper.SerializedCollections;
using System.Linq;
using System.Collections.ObjectModel;
using DG.Tweening;

namespace CardGame
{
    public class CardsManager : MonoBehaviour
    {
        [SerializeField] private BaseCard _cardPrefab;
        [SerializeField] private GridLayoutGroup gridLayoutGroup;
        [SerializeField] private RectTransform _cardsParent;
        [SerializeField] private LevelDataSO data;
        [SerializeField] private SpriteDatabase _spriteDatabase;

        public Vector2 minCellSize = Vector2.one;
        public Vector2 maxCellSize = Vector2.one;
        private List<BaseCard> _totalCards = new List<BaseCard>();

        public IReadOnlyCollection<BaseCard> TotalCards => _totalCards.AsReadOnly();

        private void OnEnable()
        {
            GlobalEventHandler.AddListener(EventID.OnCardMatchSuccess, Callback_On_Card_Match_Success);
            GlobalEventHandler.AddListener(EventID.OnNewLevelRequested, Callback_On_New_Level_Requested);
            GlobalEventHandler.AddListener(EventID.OnLevelResumeRequested, Callback_On_Level_Resume_Requested);
        }
        private void OnDisable()
        {
            GlobalEventHandler.RemoveListener(EventID.OnCardMatchSuccess, Callback_On_Card_Match_Success);
            GlobalEventHandler.RemoveListener(EventID.OnNewLevelRequested, Callback_On_New_Level_Requested);
            GlobalEventHandler.RemoveListener(EventID.OnLevelResumeRequested, Callback_On_Level_Resume_Requested);
        }

        ///this will take the level data
        ///spawns the number of cards
        /// initialize each with an icon id, unique id, and sprite and backface sprite
        public void Init(LevelDataSO levelData)
        {
            //first get the images...for the grid.
            int totalGridCells = levelData.GridSize.x * levelData.GridSize.y;
            var sprites = _spriteDatabase.GetCardSprites();
            List<Sprite> spritepairs = GetSpritePairs(sprites, totalGridCells, levelData.UniqueSets);
            _totalCards.Clear();
            Debug.Log($"SPrite paris...{spritepairs.Count}");
            GlobalVariables.canTakeInput = false;
            for (int i = 0; i < totalGridCells; i++)
            {
                BaseCard card = Instantiate(_cardPrefab, _cardsParent);
                card.Init(i, sprites.IndexOf(spritepairs[i]), spritepairs[i], null);
                _totalCards.Add(card);
                card.RevealTheCard();
                //card.ShowFrontFace();
            }
            Vector2 cellSize = CalculateFit(this.gridLayoutGroup, this._cardsParent, levelData.GridSize);
            gridLayoutGroup.cellSize = cellSize;
            DOVirtual.DelayedCall(Konstants.ICON_REVEAL_TIME_IN_SECONDS, () =>
            {
                foreach (var card in _totalCards)
                    card.HideTheCard();
                GlobalVariables.canTakeInput = true;
            });
        }

        //resuming the previous level.....
        public void Init(LevelDataSO levelData, LevelDataModel savedLevelData)
        {
            int totalGridCells = levelData.GridSize.x * levelData.GridSize.y;
            var sprites = _spriteDatabase.GetCardSprites();
            _totalCards.Clear();
            for (int i = 0; i < totalGridCells; i++)
            {
                BaseCard card = Instantiate(_cardPrefab, _cardsParent);
                // CardData cardModel = savedLevelData.cardsData.Find(x => x.uniqueId == i);
                CardData cardModel = savedLevelData.cardsData[i];

                card.Init(i, cardModel.iconId, sprites[cardModel.iconId]);
                if (cardModel.cardState is CardState.Matched)
                    card.OnMatchSuccess();
                _totalCards.Add(card);
                //card.ShowFrontFace();
            }

            Vector2 cellSize = CalculateFit(this.gridLayoutGroup, this._cardsParent, levelData.GridSize);
            gridLayoutGroup.cellSize = cellSize;
        }


        public List<Sprite> GetSpritePairs(ReadOnlyCollection<Sprite> _cardSprites, int totalGridCells, int uniqueSet)
        {
            if (totalGridCells % Konstants.MIN_CARDS_TO_MATCH != 0)
                throw new System.ArgumentException("Grid must have even number of cells");

            int totalPairs = totalGridCells / Konstants.MIN_CARDS_TO_MATCH;

            if (uniqueSet > totalPairs)
                throw new System.ArgumentException("uniqueSet cannot be more than total pairs");


            var shuffled = _cardSprites.OrderBy(x => Random.value).ToList();


            List<Sprite> selected = shuffled.Take(uniqueSet).ToList();


            List<Sprite> result = new List<Sprite>();
            foreach (var sprite in selected)
            {
                result.AddRange(Enumerable.Repeat(sprite, Konstants.MIN_CARDS_TO_MATCH));
            }
            int pairsToAdd = totalPairs - uniqueSet;
            for (int i = 0; i < pairsToAdd; i++)
            {
                var duplicate = selected[Random.Range(0, selected.Count)];
                result.AddRange(Enumerable.Repeat(duplicate, Konstants.MIN_CARDS_TO_MATCH));
            }
            result = result.OrderBy(x => Random.value).ToList();

            return result;
        }

        public Vector2 CalculateFit(
        GridLayoutGroup grid,
        RectTransform container, Vector2Int requiredGridSize)
        {
            int childCount = grid.transform.childCount;
            if (childCount == 0) return grid.cellSize;

            Vector2 spacing = grid.spacing;
            RectOffset padding = grid.padding;

            (int rows, int cols) = GetGridDimensions(grid, childCount, container, requiredGridSize);

            if (gridLayoutGroup.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
                gridLayoutGroup.constraintCount = cols;
            else if (gridLayoutGroup.constraint == GridLayoutGroup.Constraint.FixedRowCount)
                gridLayoutGroup.constraintCount = rows;


            float availableWidth = container.rect.width -
                                 (padding.left + padding.right) -
                                 ((cols - 1) * spacing.x);

            float availableHeight = container.rect.height -
                                  (padding.top + padding.bottom) -
                                  ((rows - 1) * spacing.y);


            Vector2 constrainedSize = new Vector2(
                availableWidth / cols,
                availableHeight / rows
            );
            float targetAspect = grid.cellSize.y > 0 ?
                               grid.cellSize.x / grid.cellSize.y :
                               1f;

            Vector2 aspectSize = constrainedSize;
            if (constrainedSize.x / constrainedSize.y > targetAspect)
            {
                aspectSize.x = constrainedSize.y * targetAspect;
            }
            else
            {
                aspectSize.y = constrainedSize.x / targetAspect;
            }

            Vector2 finalSize = new Vector2(
                Mathf.Clamp(aspectSize.x, minCellSize.x, maxCellSize.x),
                Mathf.Clamp(aspectSize.y, minCellSize.y, maxCellSize.y)
            );

            // float requiredWidth = cols * finalSize.x +
            //                     (cols - 1) * spacing.x +
            //                     padding.left + padding.right;

            // float requiredHeight = rows * finalSize.y +
            //                      (rows - 1) * spacing.y +
            //                      padding.top + padding.bottom;

            // bool fits = requiredWidth <= container.rect.width &&
            //           requiredHeight <= container.rect.height;

            return finalSize;
        }

        private (int rows, int cols) GetGridDimensions(
            GridLayoutGroup grid,
            int childCount,
            RectTransform container,
            Vector2Int requiredGridSize
        )
        {
            switch (grid.constraint)
            {
                case GridLayoutGroup.Constraint.FixedRowCount:
                    int rows = requiredGridSize.x;
                    return (rows, Mathf.CeilToInt(childCount / (float)rows));

                case GridLayoutGroup.Constraint.FixedColumnCount:
                    int columns = requiredGridSize.y;
                    return (Mathf.CeilToInt(childCount / (float)columns), columns);

                default: // Flexible ....row priority calculation
                    (int maxrows, int maxcolumns) = GetTotalPossibleDimenstiones(childCount, container);
                    maxrows = Mathf.Min(maxrows, requiredGridSize.x);
                    return (maxrows, Mathf.CeilToInt(childCount / (float)maxrows));
            }
        }
        public void ResetState()
        {
            foreach (var card in _totalCards)
            {
                DOTween.KillAll(card);
                Destroy(card.gameObject);
            }
            _totalCards.Clear();
        }
        private (int rows, int cols) GetTotalPossibleDimenstiones(int childCount,
            RectTransform container
        )
        {
            float containerAspect = container.rect.width / container.rect.height;
            int colsGuess = Mathf.CeilToInt(Mathf.Sqrt(childCount * containerAspect));
            return (Mathf.CeilToInt(childCount / (float)colsGuess), colsGuess);
        }

        private void Callback_On_Card_Match_Success(object args)
        {
            if (_totalCards.Any(x => x.CurrentState != CardState.Matched)) return;
            //Level complete
            Debug.Log("LEVE COMPLETE>>>>");
            GlobalVariables.isLevelComplete = true;
            PlayerDataManager.instance.ClearLevelData();

            DOVirtual.DelayedCall(1f, () =>//To complete animations 
            {
                ResetState();
                GlobalEventHandler.TriggerEvent(EventID.OnLevelComplete);
            });
        }

        private void Callback_On_New_Level_Requested(object args)
        {
            Init(args as LevelDataSO);
        }
        private void Callback_On_Level_Resume_Requested(object args)
        {

            (LevelDataSO leveldata, LevelDataModel savedLevelData) = ((LevelDataSO, LevelDataModel))args;
            Init(leveldata, savedLevelData);

        }
    }


}
