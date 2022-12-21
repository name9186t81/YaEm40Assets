using UnityEngine;

public class WeaponHoldInfo : WeaponComponent
{
    [SerializeField] private Transform _rightHandPosition;
    [SerializeField] private Transform _leftHandPosition;

    public bool TryGetRightHandPosition(out Vector2 position) => TryGetPosition(out position, _rightHandPosition);
    public bool TryGetLeftHandPosition(out Vector2 position) => TryGetPosition(out position, _leftHandPosition);

    private bool TryGetPosition(out Vector2 position, Transform hand)
    {
        if (hand != null)
        {
            position = hand.position;
            return true;
        }
        position = default;
        return false;
    }
}
