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
        new Vector2Int(2, 1)
    };

    public static Vector2Int[] L4_90deg = new Vector2Int[]
    {
        new Vector2Int(0, 2),
        new Vector2Int(1, 0),
        new Vector2Int(1, 1),
        new Vector2Int(1, 2)
    };

    public static Vector2Int[] L4_180deg = new Vector2Int[]
    {
        new Vector2Int(0, 0),
        new Vector2Int(0, 1),
        new Vector2Int(1, 1),
        new Vector2Int(2, 1)
    };

    public static Vector2Int[] L4_270deg = new Vector2Int[]
    {
        new Vector2Int(0, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, 2),
        new Vector2Int(1, 0)
    };

    public static Vector2Int[] L4_Mirror_0deg = new Vector2Int[]
    {
        new Vector2Int(0, 0),
        new Vector2Int(0, 1),
        new Vector2Int(1, 0),
        new Vector2Int(2, 0)
    };

    public static Vector2Int[] L4_Mirror_90deg = new Vector2Int[]
    {
        new Vector2Int(0, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, 2),
        new Vector2Int(1, 2)
    };

    public static Vector2Int[] L4_Mirror_180deg = new Vector2Int[]
    {
        new Vector2Int(0, 1),
        new Vector2Int(1, 1),
        new Vector2Int(2, 0),
        new Vector2Int(2, 1)
    };

    public static Vector2Int[] L4_Mirror_270deg = new Vector2Int[]
    {
        new Vector2Int(0, 0),
        new Vector2Int(0, 1),
        new Vector2Int(1, 1),
        new Vector2Int(1, 2)
    };

    // Z-образные

    public static Vector2Int[] Z_Horizontal = new Vector2Int[]
{
        new Vector2Int(0, 0),
        new Vector2Int(0, 1),
        new Vector2Int(1, 1),
        new Vector2Int(1, 2)
};

    public static Vector2Int[] Z_Vertical = new Vector2Int[]
        {
        new Vector2Int(0, 1),
        new Vector2Int(1, 0),
        new Vector2Int(1, 1),
        new Vector2Int(2, 0)
        };

    public static Vector2Int[] Z_Mirror_Horizontal = new Vector2Int[]
    {
        new Vector2Int(0, 1),
        new Vector2Int(0, 2),
        new Vector2Int(1, 0),
        new Vector2Int(1, 1)
    };

    public static Vector2Int[] Z_Mirror_Vertical = new Vector2Int[]
    {
        new Vector2Int(0, 0),
        new Vector2Int(1, 0),
        new Vector2Int(1, 1),
        new Vector2Int(2, 1)
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
        new Vector2Int(0, 1),
        new Vector2Int(1, 0),
        new Vector2Int(2, 0),
        new Vector2Int(2, 1)
    };

    public static Vector2Int[] C_Left = new Vector2Int[]
    {
        new Vector2Int(0, 0),
        new Vector2Int(0, 1),
        new Vector2Int(1, 1),
        new Vector2Int(2, 0),
        new Vector2Int(2, 1)
    };

    public static Vector2Int[] C_Up = new Vector2Int[]
    {
        new Vector2Int(0, 0),
        new Vector2Int(0, 2),
        new Vector2Int(1, 0),
        new Vector2Int(1, 1),
        new Vector2Int(1, 2)
    };

    public static Vector2Int[] C_Down = new Vector2Int[]
    {
        new Vector2Int(0, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, 2),
        new Vector2Int(1, 0),
        new Vector2Int(1, 2)
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
                shapes.Add(Z_Horizontal);
                shapes.Add(Z_Vertical);
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

    public static Vector2Int[] GetShapeByEnum(BlockShape shape)
    {
        switch (shape)
        {
            case BlockShape.Single: return Single;
            case BlockShape.Stick_2x1: return Stick_2x1;
            case BlockShape.Stick_1x2: return Stick_1x2;
            case BlockShape.Stick_3x1: return Stick_3x1;
            case BlockShape.Stick_1x3: return Stick_1x3;
            case BlockShape.L_BottomLeft: return L_BottomLeft;
            case BlockShape.L_BottomRight: return L_BottomRight;
            case BlockShape.L_TopLeft: return L_TopLeft;
            case BlockShape.L_TopRight: return L_TopRight;
            case BlockShape.Square_2x2: return Square_2x2;
            case BlockShape.L4_0deg: return L4_0deg;
            case BlockShape.L4_90deg: return L4_90deg;
            case BlockShape.L4_180deg: return L4_180deg;
            case BlockShape.L4_270deg: return L4_270deg;
            case BlockShape.L4_Mirror_0deg: return L4_Mirror_0deg;
            case BlockShape.L4_Mirror_90deg: return L4_Mirror_90deg;
            case BlockShape.L4_Mirror_180deg: return L4_Mirror_180deg;
            case BlockShape.L4_Mirror_270deg: return L4_Mirror_270deg;
            case BlockShape.Z_Horizontal: return Z_Horizontal;
            case BlockShape.Z_Vertical: return Z_Vertical;
            case BlockShape.Z_Mirror_Horizontal: return Z_Mirror_Horizontal;
            case BlockShape.Z_Mirror_Vertical: return Z_Mirror_Vertical;
            case BlockShape.T_0deg: return T_0deg;
            case BlockShape.T_90deg: return T_90deg;
            case BlockShape.T_180deg: return T_180deg;
            case BlockShape.T_270deg: return T_270deg;
            case BlockShape.Cross: return Cross;
            default: return Single;
        }
    }

    public static List<BlockShape> GetShapeEnumsBySize(int blockCount)
    {
        List<BlockShape> shapes = new List<BlockShape>();

        switch (blockCount)
        {
            case 1:
                shapes.Add(BlockShape.Single);
                break;
            case 2:
                shapes.Add(BlockShape.Stick_2x1);
                shapes.Add(BlockShape.Stick_1x2);
                break;
            case 3:
                shapes.Add(BlockShape.Stick_3x1);
                shapes.Add(BlockShape.Stick_1x3);
                shapes.Add(BlockShape.L_BottomLeft);
                shapes.Add(BlockShape.L_BottomRight);
                shapes.Add(BlockShape.L_TopLeft);
                shapes.Add(BlockShape.L_TopRight);
                break;
            case 4:
                shapes.Add(BlockShape.Square_2x2);
                shapes.Add(BlockShape.L4_0deg);
                shapes.Add(BlockShape.L4_90deg);
                shapes.Add(BlockShape.L4_180deg);
                shapes.Add(BlockShape.L4_270deg);
                shapes.Add(BlockShape.L4_Mirror_0deg);
                shapes.Add(BlockShape.L4_Mirror_90deg);
                shapes.Add(BlockShape.L4_Mirror_180deg);
                shapes.Add(BlockShape.L4_Mirror_270deg);
                shapes.Add(BlockShape.Z_Horizontal);
                shapes.Add(BlockShape.Z_Vertical);
                shapes.Add(BlockShape.Z_Mirror_Horizontal);
                shapes.Add(BlockShape.Z_Mirror_Vertical);
                shapes.Add(BlockShape.T_0deg);
                shapes.Add(BlockShape.T_90deg);
                shapes.Add(BlockShape.T_180deg);
                shapes.Add(BlockShape.T_270deg);
                break;
            case 5:
                shapes.Add(BlockShape.Cross);
                break;
        }

        return shapes;
    }
}