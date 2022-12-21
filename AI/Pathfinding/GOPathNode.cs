using System.Collections.Generic;

public class GOPathNode : MonoBehaviour2D
{
    public List<Connection> Connections { get; private set; } = new List<Connection>();

    public GOPathNode Previous;
    public float G;
    public float H;
    public float F => G + H;

    public void AddConnection(GOPathNode otherNode)
    {
        Connections.Add(new Connection()
        {
            node = otherNode,
            distance = Position2D.RelativeDistance(otherNode.Position2D)
        });
    }

    public struct Connection
    {
        public GOPathNode node;
        public float distance;
    }
}
