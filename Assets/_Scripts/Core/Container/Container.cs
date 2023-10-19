using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Container : Singleton<Container>
{
    #region EventPublisher
    public static event Action OnTileMatching; 
    #endregion

    #region Fields
    [SerializeField] LevelDataSO levelDataSO;
    [Range(3, 8)]
    [SerializeField] int startingSlot;
    [SerializeField] bool canUnlockNewSlot;
    [SerializeField] List<Slot> allSlots;
    [SerializeField] List<Slot> usableSlots = new List<Slot>();
    [SerializeField] List<Tile> assignedTiles = new List<Tile>();
    #endregion

    #region Properties
    public List<Slot> UsableSlots
    {
        get => usableSlots;
        private set => usableSlots = value;
    }
    public List<Tile> AssignedTiles
    {
        get => assignedTiles;
        set
        {
            assignedTiles = value;
        }
    }
    public LevelDataSO LevelDataSO
    {
        get => levelDataSO;
        set
        {
            GetDataFromLevelData(value);
            InitStartingSlots();
            InitLockedSlots();
            levelDataSO = value;
        }
    }

    public List<Slot> AllSlots { get => allSlots; set => allSlots = value; }
    #endregion

    #region Initialize
    private void GetDataFromLevelData(LevelDataSO levelDataSO)
    {
        startingSlot = levelDataSO.startingSlot;
        canUnlockNewSlot = levelDataSO.canUnlockNewSlot;
    }
    private void InitStartingSlots()
    {
        if (AllSlots.Count == 0)
        {
            Debug.LogWarning("Did you forget to assign slots into the container?");
            return;
        }

        /* Sort the AllSlots list by position.x in ascending order */
        AllSlots = AllSlots.OrderBy(slot => slot.transform.position.x).ToList();

        TerminateSlot();
        for (int i = 0; i < startingSlot; i++)
        {
            AllSlots[i].gameObject.SetActive(true);
            AllSlots[i].UnlockSlot();
        }
    }
    private void InitLockedSlots()
    {
        if (!canUnlockNewSlot) return;
        foreach (Slot slot in AllSlots)
        {
            if (!slot.gameObject.activeSelf)
            {
                slot.gameObject.SetActive(true);

                /*Make sure this slot is not in the usableSlots*/
                if (UsableSlots.Contains(slot))
                {
                    UsableSlots.Remove(slot);
                }
            }
        }
    }
    #endregion

    #region Finalize
    private void TerminateSlot()
    {
        foreach (var slot in AllSlots)
        {
            slot.LockSlot();
            slot.gameObject.SetActive(false);
        }
    } 
    #endregion

    #region SortingAndMatching
    public void SortingAssignedTilesPosition()
    {
        /* Reposition of all the assigned tile into the list*/
        AssignedTiles = AssignedTiles.OrderBy(tile => tile.TileDataSO.name).ToList();
        if (assignedTiles.Count > usableSlots.Count)
        {
            Debug.LogError("Tile list are greater than usableSlots. Please verify the adding to list method of the moving tile");
            return;
        }
        /* Use that list to give all tiles their new position based on slot*/
        for (int i = 0; i < assignedTiles.Count; i++)
        {
            Tile tile = assignedTiles[i];
            Transform targetTransform = usableSlots[i].transform;
            tile.TryGetComponent(out MoveToSlot mover);
            mover.MoveToNewPostion(targetTransform);
        }
        Debug.Log("Sorting!");
    }
    public void TileTripleMatching()
    {
        List<TileDataSO> assignedTileDataToRemove = new List<TileDataSO>();

        foreach (var tile in assignedTiles)
        {
                MoveToSlot mover;
                if (tile.TryGetComponent(out mover))
                {
                    if (mover.IsMoving)
                    {
                    Debug.Log("There is a moving tile, Stop checking");
                        return;
                    }
                }
        }
        /* Count the occurrences of each unique TileDataSO in the list */
        Dictionary<TileDataSO, int> tileDataCount = new Dictionary<TileDataSO, int>();

        foreach (var tile in assignedTiles)
        {
            if (tile.TryGetComponent(out MoveToSlot mover) && mover.IsMoving)
            {
                continue;
            }
            TileDataSO tileData = tile.TileDataSO;
            if (tileDataCount.ContainsKey(tileData))
            {
                tileDataCount[tileData]++;
            }
            else
            {
                tileDataCount[tileData] = 1;
            }
        }

        /* Identify TileDataSO that appears three or more times */
        foreach (var kvp in tileDataCount)
        {
            if (kvp.Value >= 3)
            {
                assignedTileDataToRemove.Add(kvp.Key);
            }
        }

        /* Remove identified TileDataSO from the list */
        List<Tile> tilesToRemove = new List<Tile>();

        int tilesRemovedCount = 0;

        foreach (var tile in assignedTiles)
        {
            if (tilesRemovedCount >= 3)
            {
                break; 
            }

            if (assignedTileDataToRemove.Contains(tile.TileDataSO))
            {
                MoveToSlot mover;
                if (tile.TryGetComponent(out mover))
                {
                    if (mover.IsMoving)
                    {
                        Debug.Log($"Do not remove the {tile.name} if it's moving");
                        continue;
                    }

                    tilesToRemove.Add(tile);
                    tilesRemovedCount++; // Increment the count
                }
            }
        }
        /* Disable the associated GameObjects of the removed tiles */
        foreach (var tileToRemove in tilesToRemove)
        {
            PlayTileRemovalAnimation(tileToRemove, 1f);
            //tileToRemove.gameObject.SetActive(false);
            assignedTiles.Remove(tileToRemove);
            TileManager.Instance.AllActiveTile.Remove(tileToRemove);
        }

        /* Sort again if there is a match */
        if (tilesToRemove.Count == 3)
        {
            AudioSFXManager.PlaySFX(AudioKey.Matching);
            OnTileMatching?.Invoke();
            Debug.Log("There is a match, MATCH IT!");
            SortingAssignedTilesPosition();
            if(TileManager.Instance.AllActiveTile.Count == 0) 
            {
                UIManager.Instance.HandleGameStateChange(GameState.Victory);
            }
            return;
        }
        else
        {
            if(assignedTiles.Count >= usableSlots.Count)
            {
                GameManager.Instance.UpdateGameState(GameState.Lose);
            }
        }
    }
    #endregion

    #region UI
    public void RestartContainerButton()
    {
        LevelDataSO = levelDataSO;
    }
    #endregion

    private void PlayTileRemovalAnimation(Tile tile, float animationDuration)
    {
        // Create a sequence of animations
        Sequence sequence = DOTween.Sequence();

        // Animation 1: Zoom out and shake a little bit
        sequence.Append(tile.transform.DOScale(Vector3.one * 1.2f, animationDuration / 3))
            .AppendCallback(() => tile.transform.DOShakePosition(0.5f, 0.2f, 20));

        // Animation 2: Scale to as small as possible
        sequence.Append(tile.transform.DOScale(Vector3.zero, animationDuration / 3));

        // Animation 3: Set the GameObject inactive
        sequence.AppendCallback(() => tile.gameObject.SetActive(false));

        // Play the entire sequence
        sequence.Play();
    }
}
