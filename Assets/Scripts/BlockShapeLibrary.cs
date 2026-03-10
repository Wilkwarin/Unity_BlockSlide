using UnityEngine;
using System.Collections.Generic;

public static class BlockShapeLibrary
{
    // 1 БЛОК

    public static Vector2Int[] Single = new Vector2Int[]
    {
        new Vector2Int(0, 0)
    };

    // 2 БЛОКА

    public static Vector2Int[] Stick_2x1 = new Vector2Int[]
    {
        new Vector2Int(0, 0),
        new Vector2Int(1, 0)
    };

    public static Vector2Int[] Stick_1x2 = new Vector2Int[]
    {
        new Vector2Int(0, 0),
        new Vector2Int(0, 1)
    };

    // 3 БЛОКА

    public static Vector2Int[] Stick_3x1 = new Vector2Int[]
    {
        new Vector2Int(0, 0),
        new Vector2Int(1, 0),
        new Vector2Int(2, 0)
    };

    public static Vector2Int[] Stick_1x3 = new Vector2Int[]
    {
        new Vector2Int(0, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, 2)
    };

    public static Vector2Int[] L_BottomLeft = new Vector2Int[]
    {
        new Vector2Int(1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(1, 1)
    };

    public static Vector2Int[] L_BottomRight = new Vector2Int[]
    {
        new Vector2Int(0, 0),
        new Vector2Int(0, 1),
        new Vector2Int(1, 1)
    };

    public static Vector2Int[] L_TopLeft = new Vector2Int[]
    {
        new Vector2Int(0, 0),
        new Vector2Int(1, 0),
        new Vector2Int(1, 1)
    };

    public static Vector2Int[] L_TopRight = new Vector2Int[]
    {
        new Vector2Int(0, 0),
        new Vector2Int(1, 0),
        new Vector2Int(0, 1)
    };

    // 4 БЛОКА

    // Квадрат 2×2

    public static Vector2Int[] Square_2x2 = new Vector2Int[]
    {
        new Vector2Int(0, 0),
        new Vector2Int(1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(1, 1)
    };

    // Г-образные из 4 блоков (8 вариантов)

    public static Vector2Int[] L4_0deg = new Vector2Int[]
    {
        new Vector2Int(0, 0),
        new Vector2Int(1, 0),
        new Vector2Int(2, 0),
        new Vector2Int(0, 1)
    };

    public static Vector2Int[] L4_90deg = new Vector2Int[]
    {
        new Vector2Int(0, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, 2),
        new Vector2Int(1, 2)
    };

    public static Vector2Int[] L4_180deg = new Vector2Int[]
    {
        new Vector2Int(0, 0),
        new Vector2Int(1, 0),
        new Vector2Int(2, 0),
        new Vector2Int(2, 1)
    };

    public static Vector2Int[] L4_270deg = new Vector2Int[]
    {
        new Vector2Int(0, 0),
        new Vector2Int(1, 0),
        new Vector2Int(1, 1),
        new Vector2Int(1, 2)
    };

    public static Vector2Int[] L4_Mirror_0deg = new Vector2Int[]
    {
        new Vector2Int(0, 0),
        new Vector2Int(1, 0),
        new Vector2Int(2, 0),
        new Vector2Int(2, 1)
    };

    public static Vector2Int[] L4_Mirror_90deg = new Vector2Int[]
    {
        new Vector2Int(0, 0),
        new Vector2Int(1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, 2)
    };

    public static Vector2Int[] L4_Mirror_180deg = new Vector2Int[]
    {
        new Vector2Int(0, 0),
        new Vector2Int(1, 0),
        new Vector2Int(2, 0),
        new Vector2Int(0, 1)
    };

    public static Vector2Int[] L4_Mirror_270deg = new Vector2Int[]
    {
        new Vector2Int(0, 0),
        new Vector2Int(1, 0),
        new Vector2Int(1, 1),
        new Vector2Int(1, 2)
    };

    // Z-образные

    public static Vector2Int[] Z_Horizontal = new Vector2Int[]
    {
        new Vector2Int(0, 0),
        new Vector2Int(1, 0),
        new Vector2Int(1, 1),
        new Vector2Int(2, 1)
    };

    public static Vector2Int[] Z_Vertical = new Vector2Int[]
    {
        new Vector2Int(0, 0),
        new Vector2Int(0, 1),
        new Vector2Int(1, 1),
        new Vector2Int(1, 2)
    };

    public static Vector2Int[] Z_Mirror_Horizontal = new Vector2Int[]
    {
        new Vector2Int(0, 0),
        new Vector2Int(1, 0),
        new Vector2Int(1, 1),
        new Vector2Int(2, 1)
    };

    public static Vector2Int[] Z_Mirror_Vertical = new Vector2Int[]
    {
        new Vector2Int(0, 0),
        new Vector2Int(0, 1),
        new Vector2Int(1, 1),
        new Vector2Int(1, 2)
    };

    public static Vector2Int[] Z_0deg = new Vector2Int[]
    {
        new Vector2Int(0, 1),
        new Vector2Int(1, 1),
        new Vector2Int(1, 0),
        new Vector2Int(2, 0)
    };

    public static Vector2Int[] Z_90deg = new Vector2Int[]
    {
        new Vector2Int(1, 2),
        new Vector2Int(1, 1),
        new Vector2Int(0, 1),
        new Vector2Int(0, 0)
    };

    public static Vector2Int[] Z_180deg = new Vector2Int[]
    {
        new Vector2Int(1, 1),
        new Vector2Int(2, 1),
        new Vector2Int(0, 0),
        new Vector2Int(1, 0)
    };

    public static Vector2Int[] Z_270deg = new Vector2Int[]
    {
        new Vector2Int(0, 2),
        new Vector2Int(0, 1),
        new Vector2Int(1, 1),
        new Vector2Int(1, 0)
    };

    // Т-образные

    public static Vector2Int[] T_0deg = new Vector2Int[]
    {
        new Vector2Int(0, 1),
        new Vector2Int(1, 1),
        new Vector2Int(2, 1),
        new Vector2Int(1, 0)
    };

    public static Vector2Int[] T_90deg = new Vector2Int[]
    {
        new Vector2Int(1, 0),
        new Vector2Int(1, 1),
        new Vector2Int(1, 2),
        new Vector2Int(0, 1)
    };

    public static Vector2Int[] T_180deg = new Vector2Int[]
    {
        new Vector2Int(0, 0),
        new Vector2Int(1, 0),
        new Vector2Int(2, 0),
        new Vector2Int(1, 1)
    };

    public static Vector2Int[] T_270deg = new Vector2Int[]
    {
        new Vector2Int(0, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, 2),
        new Vector2Int(1, 1)
    };

    // 5 БЛОКОВ

    public static Vector2Int[] Cross = new Vector2Int[]
    {
        new Vector2Int(1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(1, 1),
        new Vector2Int(2, 1),
        new Vector2Int(1, 2)
    };

    public static Vector2Int[] C_Right = new Vector2Int[]
    {
        new Vector2Int(0, 0),
        new Vector2Int(1, 0),
        new Vector2Int(2, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, 2),
        new Vector2Int(1, 2),
        new Vector2Int(2, 2)
    };

    public static Vector2Int[] C_Left = new Vector2Int[]
    {
        new Vector2Int(0, 0),
        new Vector2Int(1, 0),
        new Vector2Int(2, 0),
        new Vector2Int(2, 1),
        new Vector2Int(0, 2),
        new Vector2Int(1, 2),
        new Vector2Int(2, 2)
    };

    public static Vector2Int[] C_Up = new Vector2Int[]
    {
        new Vector2Int(0, 0),
        new Vector2Int(1, 0),
        new Vector2Int(2, 0),
        new Vector2Int(0, 1),
        new Vector2Int(2, 1),
        new Vector2Int(0, 2),
        new Vector2Int(2, 2)
    };

    public static Vector2Int[] C_Down = new Vector2Int[]
    {
        new Vector2Int(0, 0),
        new Vector2Int(2, 0),
        new Vector2Int(0, 1),
        new Vector2Int(2, 1),
        new Vector2Int(0, 2),
        new Vector2Int(1, 2),
        new Vector2Int(2, 2)
    };

    public static List<Vector2Int[]> GetShapesBySize(int blockCount)
    {
        List<Vector2Int[]> shapes = new List<Vector2Int[]>();

        switch (blockCount)
        {
            case 1:
                shapes.Add(Single);
                break;

            case 2:
                shapes.Add(Stick_2x1);
                shapes.Add(Stick_1x2);
                break;

            case 3:
                shapes.Add(Stick_3x1);
                shapes.Add(Stick_1x3);
                shapes.Add(L_BottomLeft);
                shapes.Add(L_BottomRight);
                shapes.Add(L_TopLeft);
                shapes.Add(L_TopRight);
                break;

            case 4:
                shapes.Add(Square_2x2);
                // Г-образные
                shapes.Add(L4_0deg);
                shapes.Add(L4_90deg);
                shapes.Add(L4_180deg);
                shapes.Add(L4_270deg);
                shapes.Add(L4_Mirror_0deg);
                shapes.Add(L4_Mirror_90deg);
                shapes.Add(L4_Mirror_180deg);
                shapes.Add(L4_Mirror_270deg);
                // Z-образные
                shapes.Add(Z_0deg);
                shapes.Add(Z_90deg);
                shapes.Add(Z_180deg);
                shapes.Add(Z_270deg);
                shapes.Add(Z_Mirror_Horizontal);
                shapes.Add(Z_Mirror_Vertical);
                // Т-образные
                shapes.Add(T_0deg);
                shapes.Add(T_90deg);
                shapes.Add(T_180deg);
                shapes.Add(T_270deg);
                break;

            case 5:
                shapes.Add(Cross);
                shapes.Add(C_Right);
                shapes.Add(C_Left);
                shapes.Add(C_Up);
                shapes.Add(C_Down);
                break;
        }

        return shapes;
    }

    // Получить случайную форму определённого размера
    public static Vector2Int[] GetRandomShape(int blockCount)
    {
        List<Vector2Int[]> shapes = GetShapesBySize(blockCount);
        if (shapes.Count == 0)
            return Single;

        return shapes[Random.Range(0, shapes.Count)];
    }

    public static List<Vector2Int[]> GetAllShapes()
    {
        List<Vector2Int[]> allShapes = new List<Vector2Int[]>();

        for (int size = 1; size <= 5; size++)
        {
            allShapes.AddRange(GetShapesBySize(size));
        }

        return allShapes;
    }
}