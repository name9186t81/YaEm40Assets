using UnityEngine;

public class TeamColorData
{
    private static Color[] _teamColors = new Color[] { Color.white, Color.green, Color.cyan, Color.yellow, Color.magenta, Color.grey, Color.yellow };

    public static Color GetTeamColor(int team)
    {
        if(team == -1)
        {
            return Color.red;
        }

        if (team < -1 || team > _teamColors.Length)
        {
            return Color.white;
        }

        return _teamColors[team];
    }
}
