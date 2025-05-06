using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TESTSCRIPT : MonoBehaviour
{


    public GridLayoutGroup gridLayout;
    public RectTransform rectTransform;
    public Vector2 minCellSize = Vector2.one;
    public Vector2 maxCellSize = Vector2.one;

    [ContextMenu("AdjustGrid")]
    void pp()
    {
        if (gridLayout && rectTransform)
        {
            var (fits, cellSize) = CalculateFit(gridLayout, rectTransform);
            gridLayout.cellSize = cellSize;
        }
    }
    public (bool fits, Vector2 cellSize) CalculateFit(
            GridLayoutGroup grid,
            RectTransform container)
    {
        int childCount = grid.transform.childCount;
        if (childCount == 0) return (true, grid.cellSize);

        // Get grid configuration
        Vector2 spacing = grid.spacing;
        RectOffset padding = grid.padding;

        // Calculate grid dimensions
        var (rows, cols) = GetGridDimensions(grid, childCount, container);

        // Calculate available space after padding and spacing
        float availableWidth = container.rect.width -
                             (padding.left + padding.right) -
                             ((cols - 1) * spacing.x);

        float availableHeight = container.rect.height -
                              (padding.top + padding.bottom) -
                              ((rows - 1) * spacing.y);

        // Calculate ideal cell size to perfectly fit available space
        Vector2 constrainedSize = new Vector2(
            availableWidth / cols,
            availableHeight / rows
        );

        // Apply aspect ratio from current cell size
        float targetAspect = grid.cellSize.y > 0 ?
                           grid.cellSize.x / grid.cellSize.y :
                           1f;

        // Scale to maintain aspect ratio within min/max bounds
        Vector2 aspectSize = constrainedSize;
        if (constrainedSize.x / constrainedSize.y > targetAspect)
        {
            aspectSize.x = constrainedSize.y * targetAspect;
        }
        else
        {
            aspectSize.y = constrainedSize.x / targetAspect;
        }

        // Clamp to min/max with smart scaling
        Vector2 finalSize = new Vector2(
            Mathf.Clamp(aspectSize.x, minCellSize.x, maxCellSize.x),
            Mathf.Clamp(aspectSize.y, minCellSize.y, maxCellSize.y)
        );

        // Check if final size causes overflow
        float requiredWidth = cols * finalSize.x +
                            (cols - 1) * spacing.x +
                            padding.left + padding.right;

        float requiredHeight = rows * finalSize.y +
                             (rows - 1) * spacing.y +
                             padding.top + padding.bottom;

        bool fits = requiredWidth <= container.rect.width &&
                  requiredHeight <= container.rect.height;

        return (fits, finalSize);
    }

    private static (int rows, int cols) GetGridDimensions(
        GridLayoutGroup grid,
        int childCount,
        RectTransform container
    )
    {
        switch (grid.constraint)
        {
            case GridLayoutGroup.Constraint.FixedRowCount:
                int rows = grid.constraintCount;
                return (rows, Mathf.CeilToInt(childCount / (float)rows));

            case GridLayoutGroup.Constraint.FixedColumnCount:
                int cols = grid.constraintCount;
                return (Mathf.CeilToInt(childCount / (float)cols), cols);

            default: // Flexible
                float containerAspect = container.rect.width / container.rect.height;
                int colsGuess = Mathf.CeilToInt(Mathf.Sqrt(childCount * containerAspect));
                return (Mathf.CeilToInt(childCount / (float)colsGuess), colsGuess);
        }
    }

}
