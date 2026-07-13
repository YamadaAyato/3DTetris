using System.Collections.Generic;
using ThreeDTetris.Model;
using UnityEditor;
using UnityEngine;

/// <summary>
///     ピース定義アセットのInspector表示を拡張するエディタ。
/// </summary>
[CustomEditor(typeof(PieceDefinitionAsset))]
public sealed class PieceDefinitionAssetEditor : UnityEditor.Editor
{
    private const float CELL_SIZE = 24f;
    private const float CELL_MARGIN = 2f;
    private const float PREVIEW_PADDING = 8f;

    private SerializedProperty _idProperty;
    private SerializedProperty _cellsProperty;

    private void OnEnable()
    {
        _idProperty = serializedObject.FindProperty("_id");
        _cellsProperty = serializedObject.FindProperty("_cellOffsets");
    }

    /// <summary>
    ///     Inspectorの表示内容を描画する。
    /// </summary>
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(_idProperty);
        EditorGUILayout.PropertyField(_cellsProperty, true);

        EditorGUILayout.Space(12f);
        DrawPiecePreview();

        serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    ///     ピース形状のプレビューを描画する。
    /// </summary>
    private void DrawPiecePreview()
    {
        EditorGUILayout.LabelField("Piece Preview", EditorStyles.boldLabel);

        List<Vector2Int> cells = GetCells();

        if (cells.Count == 0)
        {
            EditorGUILayout.HelpBox("セルが設定されていません。", MessageType.Info);
            return;
        }

        GetCellRange(cells, out int minX, out int maxX, out int minY, out int maxY);

        int width = maxX - minX + 1;
        int height = maxY - minY + 1;

        float previewWidth = width * CELL_SIZE + PREVIEW_PADDING * 2f;
        float previewHeight = height * CELL_SIZE + PREVIEW_PADDING * 2f;

        Rect previewRect = GUILayoutUtility.GetRect(
            previewWidth,
            previewHeight,
            GUILayout.ExpandWidth(false));

        EditorGUI.DrawRect(previewRect, new Color(0.16f, 0.16f, 0.16f));

        HashSet<Vector2Int> cellSet = new HashSet<Vector2Int>(cells);

        for (int y = maxY; y >= minY; y--)
        {
            for (int x = minX; x <= maxX; x++)
            {
                bool isFilled = cellSet.Contains(new Vector2Int(x, y));
                DrawCell(previewRect, x, y, minX, maxY, isFilled);
            }
        }

        DrawOriginMarker(previewRect, minX, maxY);
    }

    /// <summary>
    ///     セル一覧をSerializedPropertyから取得する。
    /// </summary>
    /// <returns>取得したセル一覧。</returns>
    private List<Vector2Int> GetCells()
    {
        List<Vector2Int> cells = new List<Vector2Int>();

        for (int i = 0; i < _cellsProperty.arraySize; i++)
        {
            SerializedProperty cellProperty = _cellsProperty.GetArrayElementAtIndex(i);
            SerializedProperty xProperty = cellProperty.FindPropertyRelative("_xOffset");
            SerializedProperty yProperty = cellProperty.FindPropertyRelative("_yOffset");

            if (xProperty == null || yProperty == null)
            {
                continue;
            }

            cells.Add(new Vector2Int(xProperty.intValue, yProperty.intValue));
        }

        return cells;
    }

    /// <summary>
    ///     セル一覧からX方向とY方向の範囲を取得する。
    /// </summary>
    /// <param name="cells">セル一覧。</param>
    /// <param name="minX">X方向の最小値。</param>
    /// <param name="maxX">X方向の最大値。</param>
    /// <param name="minY">Y方向の最小値。</param>
    /// <param name="maxY">Y方向の最大値。</param>
    private static void GetCellRange(
        List<Vector2Int> cells,
        out int minX,
        out int maxX,
        out int minY,
        out int maxY)
    {
        minX = cells[0].x;
        maxX = cells[0].x;
        minY = cells[0].y;
        maxY = cells[0].y;

        for (int i = 1; i < cells.Count; i++)
        {
            Vector2Int cell = cells[i];

            if (cell.x < minX)
            {
                minX = cell.x;
            }

            if (cell.x > maxX)
            {
                maxX = cell.x;
            }

            if (cell.y < minY)
            {
                minY = cell.y;
            }

            if (cell.y > maxY)
            {
                maxY = cell.y;
            }
        }

        // 基準位置が必ず見えるように、原点も表示範囲に含める。
        if (0 < minX)
        {
            minX = 0;
        }

        if (0 > maxX)
        {
            maxX = 0;
        }

        if (0 < minY)
        {
            minY = 0;
        }

        if (0 > maxY)
        {
            maxY = 0;
        }
    }

    /// <summary>
    ///     1マス分のプレビューを描画する。
    /// </summary>
    /// <param name="previewRect">プレビュー全体の描画範囲。</param>
    /// <param name="x">描画するセルのX座標。</param>
    /// <param name="y">描画するセルのY座標。</param>
    /// <param name="minX">表示範囲の最小X座標。</param>
    /// <param name="maxY">表示範囲の最大Y座標。</param>
    /// <param name="isFilled">ピースのセルが存在するか。</param>
    private static void DrawCell(
        Rect previewRect,
        int x,
        int y,
        int minX,
        int maxY,
        bool isFilled)
    {
        float drawX = previewRect.x + PREVIEW_PADDING + (x - minX) * CELL_SIZE;
        float drawY = previewRect.y + PREVIEW_PADDING + (maxY - y) * CELL_SIZE;

        Rect cellRect = new Rect(
            drawX + CELL_MARGIN,
            drawY + CELL_MARGIN,
            CELL_SIZE - CELL_MARGIN * 2f,
            CELL_SIZE - CELL_MARGIN * 2f);

        Color color = isFilled
            ? new Color(0.2f, 0.65f, 1f)
            : new Color(0.28f, 0.28f, 0.28f);

        EditorGUI.DrawRect(cellRect, color);
    }

    /// <summary>
    ///     基準位置を示すマーカーを描画する。
    /// </summary>
    /// <param name="previewRect">プレビュー全体の描画範囲。</param>
    /// <param name="minX">表示範囲の最小X座標。</param>
    /// <param name="maxY">表示範囲の最大Y座標。</param>
    private static void DrawOriginMarker(Rect previewRect, int minX, int maxY)
    {
        float drawX = previewRect.x + PREVIEW_PADDING + (0 - minX) * CELL_SIZE;
        float drawY = previewRect.y + PREVIEW_PADDING + (maxY - 0) * CELL_SIZE;

        Rect originRect = new Rect(
            drawX + CELL_SIZE * 0.35f,
            drawY + CELL_SIZE * 0.35f,
            CELL_SIZE * 0.3f,
            CELL_SIZE * 0.3f);

        EditorGUI.DrawRect(originRect, Color.white);
    }
}