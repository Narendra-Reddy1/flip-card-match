using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AYellowpaper.SerializedCollections;
using System;
namespace CardGame
{
    public class CardsManger : MonoBehaviour
    {
        [SerializeField] private BaseCard _cardPrefab;
        [SerializeField] private GridLayoutGroup gridLayoutGroup;
        [SerializeField] private RectTransform _cardsParent;
        [SerializeField] private LevelData data;

        public Vector2 minCellSize = Vector2.one;
        public Vector2 maxCellSize = Vector2.one;
        private List<BaseCard> _totalCards = new List<BaseCard>();
        void Start()
        {
            Init(data);
        }

        //this will take the level data
        ///spawns the number of cards
        /// initialize each with an icon id, unique id, and sprite and backface sprite
        public void Init(LevelData levelData)
        {
            //first get the images...for the grid.
            int iconId = 0;
            for (int i = 0; i < levelData.GridSize.x * levelData.GridSize.y; i++)
            {
                BaseCard card = Instantiate(_cardPrefab, _cardsParent);
                card.Init(i, iconId, null, null);
                _totalCards.Add(card);
            }

            Vector2 cellSize = CalculateFit(this.gridLayoutGroup, this._cardsParent, levelData.GridSize);
            gridLayoutGroup.cellSize = cellSize;
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

        private (int rows, int cols) GetTotalPossibleDimenstiones(int childCount,
            RectTransform container
        )
        {
            float containerAspect = container.rect.width / container.rect.height;
            int colsGuess = Mathf.CeilToInt(Mathf.Sqrt(childCount * containerAspect));
            return (Mathf.CeilToInt(childCount / (float)colsGuess), colsGuess);
        }


    }



    public enum CardType
    {
        NormalCard,
    }
    public class SpriteDatabase
    {
        [SerializeField] private SerializedDictionary<CardType, Sprite> _cardTypeToCardBackFaceIcon;
        public Sprite GetCardBackFace(CardType cardType)
        {
            return _cardTypeToCardBackFaceIcon[cardType];
        }
    }
}
