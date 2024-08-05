using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Color = UnityEngine.Color;
using Random = UnityEngine.Random;

public class TilePlacerWFC : MonoBehaviour
{
    public List<VoxelTile> TilePrefabs;
    public Vector2Int MapSize = new Vector2Int(10, 10);

    [SerializeField]
    public List<GameObject> BonusPrefabs;

    [Range(0f, 100f)]
    public float BonusChanсe = 30;

    private VoxelTile[,] spawnedTiles;

    [SerializeField]
    public List<GameObject> ObtaclePrefabs;
    [Range(0f, 100f)]
    public float ObtacleChanсe = 30;

    [SerializeField]
    public List<GameObject> EnemyPrefabs;
    [Range(0f, 100f)]
    public float EnemyChanсe = 30;


    public GameObject Player;
    private Queue<Vector2Int> recalcPossibleTilesQueue = new Queue<Vector2Int>();
    private List<VoxelTile>[,] possibleTiles;

    private void Start()
    {
        spawnedTiles = new VoxelTile[MapSize.x, MapSize.y];

        foreach (VoxelTile tilePrefab in TilePrefabs)
        {
            tilePrefab.CalculateSidesColors();
        }
        //
        int countBeforeAdding = TilePrefabs.Count;
        for (int i = 0; i < countBeforeAdding; i++)
        {
            VoxelTile clone;
            
            switch (TilePrefabs[i].Rotation)
            {
                case VoxelTile.RotationType.OnlyRotation:
                    break;

                case VoxelTile.RotationType.TwoRotations:
                    TilePrefabs[i].Weight /= 2;
                    if (TilePrefabs[i].Weight <= 0) TilePrefabs[i].Weight = 1;

                    clone = Instantiate(TilePrefabs[i], TilePrefabs[i].transform.position + Vector3.forward * 3,
                        Quaternion.identity);
                    clone.Rotate90();
                    TilePrefabs.Add(clone);
                    break;

                case VoxelTile.RotationType.FourRotations:
                    TilePrefabs[i].Weight /= 4;
                    if (TilePrefabs[i].Weight <= 0) TilePrefabs[i].Weight = 1;

                    clone = Instantiate(TilePrefabs[i], TilePrefabs[i].transform.position + Vector3.forward * 3,
                        Quaternion.identity);
                    clone.Rotate90();
                    
                    TilePrefabs.Add(clone);

                    clone = Instantiate(TilePrefabs[i], TilePrefabs[i].transform.position + Vector3.forward * 6,
                        Quaternion.identity);
                    clone.Rotate90();
                    clone.Rotate90();
                    TilePrefabs.Add(clone);

                    clone = Instantiate(TilePrefabs[i], TilePrefabs[i].transform.position + Vector3.forward * 9,
                        Quaternion.identity);
                    clone.Rotate90();
                    clone.Rotate90();
                    clone.Rotate90();
                    TilePrefabs.Add(clone);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }

        Generate();
        PlaceAllBonuses();
        PlaceAllObtacles();
        PlaceAllEnemy();
        SpawnPlayer();
    }

    private void Update()
    {
        foreach (VoxelTile tilePrefab in TilePrefabs)
        {
            tilePrefab.CalculateSidesColors();
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            foreach (VoxelTile spawnedTile in spawnedTiles)
            {
                if (spawnedTile != null) Destroy(spawnedTile.gameObject);
            }

            Generate();
        }
    }

    private void Generate()
    {
        possibleTiles = new List<VoxelTile>[MapSize.x, MapSize.y];

        int maxAttempts = 10;
        int attempts = 0;
        while (attempts++ < maxAttempts)
        {
            for (int x = 0; x < MapSize.x; x++)
                for (int y = 0; y < MapSize.y; y++)
                {
                    possibleTiles[x, y] = new List<VoxelTile>(TilePrefabs);
                }

            //VoxelTile tileInCenter = TilePrefabs[0];
            VoxelTile tileInCenter = GetRandomTile(TilePrefabs);
            possibleTiles[MapSize.x / 2, MapSize.y / 2] = new List<VoxelTile> { tileInCenter };

            recalcPossibleTilesQueue.Clear();
            EnqueueNeighboursToRecalc(new Vector2Int(MapSize.x / 2, MapSize.y / 2));

            bool success = GenerateAllPossibleTiles();

            if (success) break;
        }

        PlaceAllTiles();
    }

    private bool GenerateAllPossibleTiles()
    {
        int maxIterations = 500;
        int iterations = 0;
        int backtracks = 0;

        while (iterations++ < maxIterations)
        {
            int maxInnerIterations = 500;
            int innerIterations = 0;

            while (recalcPossibleTilesQueue.Count > 0 && innerIterations++ < maxInnerIterations)
            {
                Vector2Int position = recalcPossibleTilesQueue.Dequeue();
                if (position.x == 0 || position.y == 0 ||
                    position.x == MapSize.x - 1 || position.y == MapSize.y - 1)
                {
                    continue;
                }

                List<VoxelTile> possibleTilesHere = possibleTiles[position.x, position.y];

                int countRemoved = possibleTilesHere.RemoveAll(t => !IsTilePossible(t, position));

                if (countRemoved > 0) EnqueueNeighboursToRecalc(position);

                if (possibleTilesHere.Count == 0)
                {
                    // Зашли в тупик, в этих координатах невозможен ни один тайл. Попробуем ещё раз, разрешим все тайлы
                    // в этих и соседних координатах, и посмотрим устаканится ли всё
                    possibleTilesHere.AddRange(TilePrefabs);
                    possibleTiles[position.x + 1, position.y] = new List<VoxelTile>(TilePrefabs);
                    possibleTiles[position.x - 1, position.y] = new List<VoxelTile>(TilePrefabs);
                    possibleTiles[position.x, position.y + 1] = new List<VoxelTile>(TilePrefabs);
                    possibleTiles[position.x, position.y - 1] = new List<VoxelTile>(TilePrefabs);

                    EnqueueNeighboursToRecalc(position);

                    backtracks++;
                }
            }
            if (innerIterations == maxInnerIterations) break;

            List<VoxelTile> maxCountTile = possibleTiles[1, 1];
            Vector2Int maxCountTilePosition = new Vector2Int(1, 1);

            for (int x = 1; x < MapSize.x - 1; x++)
                for (int y = 1; y < MapSize.y - 1; y++)
                {
                    if (possibleTiles[x, y].Count > maxCountTile.Count)
                    {
                        maxCountTile = possibleTiles[x, y];
                        maxCountTilePosition = new Vector2Int(x, y);
                    }
                }

            if (maxCountTile.Count == 1)
            {
                Debug.Log($"Generated for {iterations} iterations, with {backtracks} backtracks");
                return true;
            }

            VoxelTile tileToCollapse = GetRandomTile(maxCountTile);
            possibleTiles[maxCountTilePosition.x, maxCountTilePosition.y] = new List<VoxelTile> { tileToCollapse };
            EnqueueNeighboursToRecalc(maxCountTilePosition);
        }

        Debug.Log($"Failed, run out of iterations with {backtracks} backtracks");
        return false;
    }

    private bool IsTilePossible(VoxelTile tile, Vector2Int position)
    {
        bool isAllRightImpossible = possibleTiles[position.x - 1, position.y]
            .All(rightTile => !CanAppendTile(tile, rightTile, Direction.Right));
        if (isAllRightImpossible) return false;

        bool isAllLeftImpossible = possibleTiles[position.x + 1, position.y]
            .All(leftTile => !CanAppendTile(tile, leftTile, Direction.Left));
        if (isAllLeftImpossible) return false;

        bool isAllForwardImpossible = possibleTiles[position.x, position.y - 1]
            .All(fwdTile => !CanAppendTile(tile, fwdTile, Direction.Forward));
        if (isAllForwardImpossible) return false;

        bool isAllBackImpossible = possibleTiles[position.x, position.y + 1]
            .All(backTile => !CanAppendTile(tile, backTile, Direction.Back));
        if (isAllBackImpossible) return false;

        return true;
    }

    private void PlaceAllTiles()
    {
        for (int x = 1; x < MapSize.x - 1; x++)
            for (int y = 1; y < MapSize.y - 1; y++)
            {
                PlaceTile(x, y);
            }
    }

    private void EnqueueNeighboursToRecalc(Vector2Int position)
    {
        recalcPossibleTilesQueue.Enqueue(new Vector2Int(position.x + 1, position.y));
        recalcPossibleTilesQueue.Enqueue(new Vector2Int(position.x - 1, position.y));
        recalcPossibleTilesQueue.Enqueue(new Vector2Int(position.x, position.y + 1));
        recalcPossibleTilesQueue.Enqueue(new Vector2Int(position.x, position.y - 1));
    }

    private void PlaceTile(int x, int y)
    {
        if (possibleTiles[x, y].Count == 0) return;

        VoxelTile selectedTile = GetRandomTile(possibleTiles[x, y]);
        Vector3 position = selectedTile.VoxelSize * selectedTile.TileSideVoxels * new Vector3(x, 0, y);
        selectedTile.gameObject.isStatic = true; //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        spawnedTiles[x, y] = Instantiate(selectedTile, position, selectedTile.transform.rotation);
    }

    private VoxelTile GetRandomTile(List<VoxelTile> availableTiles)
    {
        List<float> chances = new List<float>();
        for (int i = 0; i < availableTiles.Count; i++)
        {
            chances.Add(availableTiles[i].Weight);
        }

        float value = Random.Range(0, chances.Sum());
        float sum = 0;

        for (int i = 0; i < chances.Count; i++)
        {
            sum += chances[i];
            if (value < sum)
            {
                return availableTiles[i];
            }
        }

        return availableTiles[availableTiles.Count - 1];
    }

    private bool CanAppendTile(VoxelTile existingTile, VoxelTile tileToAppend, Direction direction)
    {
        if (existingTile == null) return true;

        if (direction == Direction.Right)
        {
            return Enumerable.SequenceEqual(existingTile.ColorsRight, tileToAppend.ColorsLeft);
        }
        else if (direction == Direction.Left)
        {
            return Enumerable.SequenceEqual(existingTile.ColorsLeft, tileToAppend.ColorsRight);
        }
        else if (direction == Direction.Forward)
        {
            return Enumerable.SequenceEqual(existingTile.ColorsForward, tileToAppend.ColorsBack);
        }
        else if (direction == Direction.Back)
        {
            return Enumerable.SequenceEqual(existingTile.ColorsBack, tileToAppend.ColorsForward);
        }
        else
        {
            throw new ArgumentException("Wrong direction value, should be Vector3.left/right/back/forward",
                nameof(direction));
        }
    }
    private void PlaceAllBonuses()
    {
        List<GameObject> bonusTiles = new List<GameObject>();

        for(int i = 0; i < GameObject.FindGameObjectsWithTag("BonusPoint").Count(); i++)
        {
            if (BonusChanсe > Random.Range(0f,100f))
                bonusTiles.Add(GameObject.FindGameObjectsWithTag("BonusPoint")[i]);
        }
       

        for(int i = 0; i < bonusTiles.Count; i++)
        {
            int randomIndex = Random.Range(0, BonusPrefabs.Count);
            BonusPrefabs[randomIndex].gameObject.isStatic = true;
            GameObject Bonus = Instantiate(BonusPrefabs[randomIndex], bonusTiles[i].transform.position, bonusTiles[i].transform.rotation);
            Bonus.transform.parent = bonusTiles[i].transform;
            //Debug.Log(bonusTiles[i].transform.parent.position);
            Debug.DrawRay(bonusTiles[i].transform.parent.position, Vector3.up * 0.8f, Color.cyan, 1000);
        }
    }
    private void SpawnPlayer()
    {
        Player.transform.position = GameObject.FindGameObjectsWithTag("PlayerCanSpawn")[1].transform.position;
       
    }
    private void PlaceAllObtacles()
    {
        
        // за место List нужен словарь для спавна нескольких видов бонусов
        List<GameObject> obtacleTiles = new List<GameObject>();

        for (int i = 0; i < GameObject.FindGameObjectsWithTag("BonusPoint").Count(); i++)
        {
            if (ObtacleChanсe > Random.Range(0f, 100f))
                obtacleTiles.Add(GameObject.FindGameObjectsWithTag("BonusPoint")[i]);
        }


        for (int i = 0; i < obtacleTiles.Count; i++)
        {
            Vector3 randomPos = new Vector3(obtacleTiles[i].transform.position.x + Random.Range(-1.2f, 1.2f), obtacleTiles[i].transform.position.y, obtacleTiles[i].transform.position.z + Random.Range(-1.2f, 1.2f));
            int randomIndex = Random.Range(0, ObtaclePrefabs.Count);
            Debug.Log("randomIndex  = " + randomIndex);
            ObtaclePrefabs[randomIndex].gameObject.isStatic = true;
            GameObject Obtacle = Instantiate(ObtaclePrefabs[randomIndex], randomPos, obtacleTiles[i].transform.rotation);
            Obtacle.transform.parent = obtacleTiles[i].transform;
            //Debug.Log(obtacleTiles[i].transform.parent.position);
            Debug.DrawRay(obtacleTiles[i].transform.parent.position, Vector3.up * 0.8f, Color.red, 1000);
        }
    }
    private void PlaceAllEnemy()
    {

        // за место List нужен словарь для спавна нескольких видов бонусов
        List<GameObject> enemyTiles = new List<GameObject>();

        for (int i = 0; i < GameObject.FindGameObjectsWithTag("EnemyPoint").Count(); i++)
        {
            if (EnemyChanсe > Random.Range(0f, 100f))
                enemyTiles.Add(GameObject.FindGameObjectsWithTag("EnemyPoint")[i]);
        }


        for (int i = 0; i < enemyTiles.Count; i++)
        {
            Vector3 randomPos = new Vector3(enemyTiles[i].transform.position.x, enemyTiles[i].transform.position.y, enemyTiles[i].transform.position.z);
            int randomIndex = Random.Range(0, EnemyPrefabs.Count);
            Debug.Log("randomIndex  = " + randomIndex);
            EnemyPrefabs[randomIndex].gameObject.isStatic = true;
            GameObject Enemy = Instantiate(EnemyPrefabs[randomIndex], randomPos, enemyTiles[i].transform.rotation);
            Enemy.transform.parent = enemyTiles[i].transform;
            //Debug.Log(obtacleTiles[i].transform.parent.position);
            Debug.DrawRay(enemyTiles[i].transform.parent.position, Vector3.up * 0.8f, Color.red, 1000);
        }
    }
}
