using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelTile : MonoBehaviour
{
    public float VoxelSize = 0.2f;
    public int TileSideVoxels = 10;

    [Range(1, 100)]
    public int Weight = 50;

    public RotationType Rotation;

    public enum RotationType
    {
        OnlyRotation,
        TwoRotations,
        FourRotations
    }
    public int ForwardSide;
    public int RightSide;
    public int BackSide;
    public int LeftSide;

    [HideInInspector] public byte[] ColorsRight;
    [HideInInspector] public byte[] ColorsForward;
    [HideInInspector] public byte[] ColorsLeft;
    [HideInInspector] public byte[] ColorsBack;

    public void CalculateSidesColors()
    {
        ColorsRight = new byte[TileSideVoxels * TileSideVoxels];
        ColorsForward = new byte[TileSideVoxels * TileSideVoxels];
        ColorsLeft = new byte[TileSideVoxels * TileSideVoxels];
        ColorsBack = new byte[TileSideVoxels * TileSideVoxels];

        for (int y = 0; y < TileSideVoxels; y++)
        {
            for (int i = 0; i < TileSideVoxels; i++)
            {
                ColorsRight[y * TileSideVoxels + i] = GetVoxelColor(y, i, Direction.Right);
                ColorsForward[y * TileSideVoxels + i] = GetVoxelColor(y, i, Direction.Forward);
                ColorsLeft[y * TileSideVoxels + i] = GetVoxelColor(y, i, Direction.Left);
                ColorsBack[y * TileSideVoxels + i] = GetVoxelColor(y, i, Direction.Back);
            }
        }
    }

    public void Rotate90()
    {
        transform.Rotate(0, 90, 0);

        byte[] colorsRightNew = new byte[TileSideVoxels * TileSideVoxels];
        byte[] colorsForwardNew = new byte[TileSideVoxels * TileSideVoxels];
        byte[] colorsLeftNew = new byte[TileSideVoxels * TileSideVoxels];
        byte[] colorsBackNew = new byte[TileSideVoxels * TileSideVoxels];

        int RightSideNew = RightSide;
        int ForwardSideNew = ForwardSide;
        int LeftSideNew = LeftSide;
        int BacktSideNew = BackSide;

        for (int layer = 0; layer < TileSideVoxels; layer++)
        {
            for (int offset = 0; offset < TileSideVoxels; offset++)
            {
                colorsRightNew[layer * TileSideVoxels + offset] = ColorsForward[layer * TileSideVoxels + TileSideVoxels - offset - 1];
                colorsForwardNew[layer * TileSideVoxels + offset] = ColorsLeft[layer * TileSideVoxels + offset];
                colorsLeftNew[layer * TileSideVoxels + offset] = ColorsBack[layer * TileSideVoxels + TileSideVoxels - offset - 1];
                colorsBackNew[layer * TileSideVoxels + offset] = ColorsRight[layer * TileSideVoxels + offset];
            }
        }

        ColorsRight = colorsRightNew;
        ColorsForward = colorsForwardNew;
        ColorsLeft = colorsLeftNew;
        ColorsBack = colorsBackNew;

        RightSide = ForwardSideNew;
        ForwardSide = LeftSideNew;
        LeftSide = BacktSideNew;
        BackSide = RightSideNew;
    }

    private byte GetVoxelColor(int verticalLayer, int horizontalOffset, Direction direction)
    {
        var meshCollider = GetComponentInChildren<MeshCollider>();

        float vox = VoxelSize;
        float half = VoxelSize / 2;

        Vector3 rayStart;
        Vector3 rayDir;
        Color color;
        int dirNumber = 0;
        if (direction == Direction.Right)
        {
            rayStart = meshCollider.bounds.min +
                       new Vector3(-half, 0, half + horizontalOffset * vox);
            rayDir = Vector3.right;
            color = Color.red;
            dirNumber = RightSide;
        }
        else if (direction == Direction.Forward)
        {
            rayStart = meshCollider.bounds.min +
                       new Vector3(half + horizontalOffset * vox, 0, -half);
            rayDir = Vector3.forward;
            color = Color.green;
            dirNumber = ForwardSide;
        }
        else if (direction == Direction.Left)
        {
            rayStart = meshCollider.bounds.max +
                       new Vector3(half, 0, -half - (TileSideVoxels - horizontalOffset - 1) * vox);
            rayDir = Vector3.left;
            color = Color.blue;
            dirNumber = LeftSide;
        }
        else if (direction == Direction.Back)
        {
            rayStart = meshCollider.bounds.max +
                       new Vector3(-half - (TileSideVoxels - horizontalOffset - 1) * vox, 0, half);
            rayDir = Vector3.back;
            color = Color.black;
            dirNumber = BackSide;
        }
        else
        {
            throw new ArgumentException("Wrong direction value, should be Direction.left/right/back/forward",
                nameof(direction));
        }

        rayStart.y = meshCollider.bounds.min.y + half + verticalLayer * vox;

        

        
        if (Physics.Raycast(new Ray(rayStart, rayDir), out RaycastHit hit, vox))
        {
            Debug.DrawRay(rayStart, rayDir * .1f, color);
            byte colorIndex = (byte)(dirNumber * 100 / (10 + dirNumber) * 8);

            if (colorIndex == 0) Debug.LogWarning("Found color 0 in mesh palette, this can cause conflicts");

            return (byte)dirNumber;
        }
        return (byte)dirNumber;

    }

}
