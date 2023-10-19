using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class KickTheTileBack : MonoBehaviour
{
    [SerializeField] Transform targetPos;

    public void KickTileBack()
    {
        Container container = Container.Instance;
        if (container.AssignedTiles.Count < 1) return;
        Tile tileToKick = container.AssignedTiles[container.AssignedTiles.Count - 1];
        tileToKick.gameObject.TryGetComponent(out TileCollisionController tileCollisionController);

        // Set up the initial position and velocity for the arc
        Vector3 initialPosition = tileToKick.transform.position;
        Vector3 controlPoint = (initialPosition + targetPos.position) / 2; // Calculate a control point for the curve

        // Create a sequence to perform both animations simultaneously
        Sequence kickSequence = DOTween.Sequence();
        float duration = 0.5f;

        // Add a move animation
        kickSequence.Append(tileToKick.transform.DOMove(targetPos.position, duration).SetEase(Ease.InOutQuad));

        // Add a scale animation to set the tile's scale back to its original scale
        kickSequence.Join(tileToKick.transform.DOScale(tileToKick.OriginalScale, duration));
      
        //Remove the tile after the animation duration
        if (container.AssignedTiles.Contains(tileToKick))
        {
            container.AssignedTiles.Remove(tileToKick);
        }
        AudioSFXManager.PlaySFX(AudioKey.Kick);
        kickSequence.OnComplete(() =>
        {
            // Enable tileCollisionController 
            tileCollisionController.enabled = true;
        });

        // Play the sequence
        kickSequence.Play();
    }
}
