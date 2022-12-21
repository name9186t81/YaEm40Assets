using TMPro;
using UnityEngine;

public class TeamColorAdapter : UnitComponent
{
    [SerializeField] private SpriteRenderer[] _renderers;
    [SerializeField] private LineRenderer[] _lineRenderers;
    [SerializeField] private TrailRenderer[] _trailRenderers;
    private Unit _owner;

    public override void Init(Unit owner)
    {
        base.Init(owner);

        Change();
        _owner = owner;
        owner.OnTeamChange += Change;
    }

    private void Change()
    {
        for (int i = 0, length = _renderers.Length; i < length; i++)
        {
            _renderers[i].color = GetTeamColor(_renderers[i].color.a);
        }

        for (int i = 0, length = _lineRenderers.Length; i < length; i++)
        {
            _lineRenderers[i].startColor = GetTeamColor(_lineRenderers[i].startColor.a);
            _lineRenderers[i].endColor = GetTeamColor(_lineRenderers[i].endColor.a);
        }

        for (int i = 0, length = _trailRenderers.Length; i < length; i++)
        {
            _trailRenderers[i].startColor = GetTeamColor(_trailRenderers[i].startColor.a);
            _trailRenderers[i].endColor = GetTeamColor(_trailRenderers[i].endColor.a);
        }
    }

    protected override void AddToComponentSystem()
    {
        Owner.ComponentSystem.AddComponent(this);
    }

    public void OnDisable()
    {
        if (_owner == null) return;
        _owner.OnTeamChange -= Change;
    }

    private Color GetTeamColor(float alpha)
    {
        var teamColor = TeamColorData.GetTeamColor(Owner.teamNumber);
        return new Color(teamColor.r, teamColor.g, teamColor.b, alpha);
    }
}
