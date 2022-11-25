using System.Numerics;
using System.Xml.Linq;

class AStar
{
    public class NodeAstar
    {
        public List<Link> links;
        public NodeAstar parentNode;
        public PosInArray posInArray;
        public int estimateCost;
        public int d;
        public int f;

        public NodeAstar(List<Link> afterNodes, NodeAstar parentNode, PosInArray posInArray, int estimateCost, int d, int f)
        {
            this.links = afterNodes;
            this.parentNode = parentNode;
            this.posInArray = posInArray;
            this.estimateCost = estimateCost;
            this.d = d;
            this.f = f;
        }

        public NodeAstar(List<Link> afterNodes, PosInArray posInArray)
        {
            this.links = afterNodes;
            this.posInArray = posInArray;
            estimateCost = 0;
            d = 0;
            f = 0;
        }

        public NodeAstar(List<Link> afterNodes)
        {
            this.links = afterNodes;
            posInArray = new PosInArray();
            estimateCost = 0;
            d = 0;
            f = 0;
        }

        public NodeAstar()
        {
            this.links = new List<Link>();
            posInArray = new PosInArray(-1, -1);
            estimateCost = 0;
            d = 0;
            f = 0;
        }
    }

    public class PosInArray
    {
        public int column;
        public int row;

        public PosInArray(int column, int row)
        {
            this.column = column;
            this.row = row;
        }

        public PosInArray()
        {
            column = -1;
            row = -1;
        }

        public int TotalBySubWithAbs(PosInArray otherPos)
        {
            return Math.Abs(column - otherPos.column) + Math.Abs(row - otherPos.row);
        }
    }

    public class Link
    {
        public NodeAstar nodeAstar;
        public int weight;

        public Link(NodeAstar nodeAstar, int weight)
        {
            this.nodeAstar = nodeAstar;
            this.weight = weight;
        }
    }

    private NodeAstar _startNodeAstar;
    private NodeAstar _endNodeAstar;
    private NodeAstar[,] _matrix;

    private PriorityQueue<NodeAstar, int> _openList = new PriorityQueue<NodeAstar, int>();
    private List<NodeAstar> _closedList = new List<NodeAstar>();
    private LinkedList<NodeAstar> rightWay = new LinkedList<NodeAstar>();

    public AStar(NodeAstar[,] matrix, PosInArray startPos, PosInArray endPos)
    {
        _matrix = ConvertRow_ColToCol_Row(matrix);
        //SetupPosInArray(_matrix);

        _startNodeAstar = matrix[startPos.row, startPos.column];
        _endNodeAstar = matrix[endPos.row, endPos.column];

        CalculateEstimateCost(_matrix);
    }

    private void CalculateEstimateCost(NodeAstar[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                matrix[i,j].estimateCost = _endNodeAstar.posInArray.TotalBySubWithAbs(matrix[i, j].posInArray);
                //matrix[j, i].estimateCost = _endNodeAstar.posInArray.TotalBySubWithAbs(matrix[j, i].posInArray);
            }
        }
    }

    private void SetupPosInArray(NodeAstar[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                matrix[i, j].posInArray = new PosInArray(i, j);
            }
        }
    }

    private NodeAstar[,] ConvertRow_ColToCol_Row(NodeAstar[,] matrix)
    {
        NodeAstar[,] newMatrix = new NodeAstar[matrix.GetLength(1), matrix.GetLength(0)];

        for (int i = 0; i < newMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < newMatrix.GetLength(1); j++)
            {
                newMatrix[i, j] = matrix[j, i];
            }
        }

        return newMatrix;
    }

    private void Travelled(NodeAstar currentNode)
    {
        if (currentNode.posInArray.Equals(_endNodeAstar.posInArray))
        {
            return;
        }

        MoveNextNode(currentNode);

        NodeAstar nextnodeAstar = _openList.Dequeue();

        _closedList.Add(nextnodeAstar);

        Travelled(nextnodeAstar);
    }

    private void MoveNextNode(NodeAstar currentNode)
    {
        for (int i = 0; i < currentNode.links.Count; i++)
        {
            NodeAstar nextNode = currentNode.links[i].nodeAstar;
            if (!IsNodeInClosedList(nextNode) && !IsNodeInOpenedList(nextNode))
            {
                nextNode.parentNode = currentNode;
                nextNode.d += currentNode.links[i].weight;
                nextNode.f = nextNode.d + nextNode.estimateCost;
                _openList.Enqueue(nextNode, nextNode.f);
            }
        }
    }

    private bool IsNodeInClosedList(NodeAstar nodeToCheck)
    {
        foreach (NodeAstar nodeAstar in _closedList)
        {
            if (nodeToCheck.posInArray.Equals(nodeAstar.posInArray))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsNodeInOpenedList(NodeAstar nodeToCheck)
    {
        List<NodeAstar> tempOpenedList = new List<NodeAstar>();

        while (_openList.Count > 0)
        {
            tempOpenedList.Add(_openList.Dequeue());
        }
        foreach (NodeAstar nodeAstar in tempOpenedList)
        {
            _openList.Enqueue(nodeAstar, nodeAstar.f);
        }

        foreach (NodeAstar nodeAstar in tempOpenedList)
        {
            if (nodeToCheck.posInArray.Equals(nodeAstar.posInArray))
            {
                return true;
            }
        }

        return false;
    }

    private void PrintAction()
    {
        _closedList.Add(_startNodeAstar);

        Travelled(_startNodeAstar);

        GetRightWay(_closedList.Last());

        foreach (NodeAstar nodeAstar in rightWay)
        {
            Console.WriteLine("Move to (" + nodeAstar.posInArray.column + ", " + nodeAstar.posInArray.row + ")");
        }
    }

    private void GetRightWay(NodeAstar nodeAstar)
    {
        // Add node dich vao rightWay
        if (nodeAstar.posInArray.Equals(_endNodeAstar.posInArray))
        {
            rightWay.AddFirst(nodeAstar);
        }

        if (nodeAstar.posInArray.Equals(_startNodeAstar.posInArray))
        {
            return;
        }

        if (nodeAstar.parentNode == null) return;

        rightWay.AddFirst(nodeAstar.parentNode);

        GetRightWay(nodeAstar.parentNode);
    }

    private static NodeAstar[,] SetupMatrixData()
    {
        List<Link> links = new List<Link>();
        PosInArray pos;

        /*links = new List<Link>();
        NodeAstar node0_1 = new NodeAstar(links);

        links = new List<Link>();
        NodeAstar node0_2 = new NodeAstar(links);

        links = new List<Link>();
        NodeAstar node1_0 = new NodeAstar(links);

        links = new List<Link>();
        NodeAstar node1_1 = new NodeAstar(links);

        links = new List<Link>();
        NodeAstar node1_2 = new NodeAstar(links);

        links = new List<Link>();
        NodeAstar node2_0 = new NodeAstar(links);

        links = new List<Link>();
        NodeAstar node2_1 = new NodeAstar(links);

        links = new List<Link>();
        NodeAstar node2_2 = new NodeAstar(links);*/

        pos = new PosInArray(0, 0);
        NodeAstar node0_0 = new NodeAstar(links, pos);

        links = new List<Link>();
        pos = new PosInArray(0, 1);
        NodeAstar node0_1 = new NodeAstar(links, pos);

        links = new List<Link>();
        pos = new PosInArray(0, 2);
        NodeAstar node0_2 = new NodeAstar(links, pos);

        links = new List<Link>();
        pos = new PosInArray(1, 0);
        NodeAstar node1_0 = new NodeAstar(links, pos);

        links = new List<Link>();
        pos = new PosInArray(1, 1);
        NodeAstar node1_1 = new NodeAstar(links, pos);

        links = new List<Link>();
        pos = new PosInArray(1, 2);
        NodeAstar node1_2 = new NodeAstar(links, pos);

        links = new List<Link>();
        pos = new PosInArray(2, 0);
        NodeAstar node2_0 = new NodeAstar(links, pos);

        links = new List<Link>();
        pos = new PosInArray(2, 1);
        NodeAstar node2_1 = new NodeAstar(links, pos);

        links = new List<Link>();
        pos = new PosInArray(2, 2);
        NodeAstar node2_2 = new NodeAstar(links, pos);

        node0_0.links.Add(new Link(node0_1, 2));
        node0_0.links.Add(new Link(node1_0, 5));

        node0_1.links.Add(new Link(node0_0, 2));
        node0_1.links.Add(new Link(node0_2, 2));

        node0_2.links.Add(new Link(node0_1, 2));
        node0_2.links.Add(new Link(node1_2, 1));

        node1_0.links.Add(new Link(node0_0, 5));
        node1_0.links.Add(new Link(node1_1, 3));
        node1_0.links.Add(new Link(node2_0, 1));

        node1_1.links.Add(new Link(node1_0, 3));
        node1_1.links.Add(new Link(node1_2, 1));
        node1_1.links.Add(new Link(node2_1, 3));

        node1_2.links.Add(new Link(node0_2, 1));
        node1_2.links.Add(new Link(node1_1, 1));

        node2_0.links.Add(new Link(node1_0, 1));

        node2_1.links.Add(new Link(node1_1, 3));
        node2_1.links.Add(new Link(node2_2, 1));

        node2_2.links.Add(new Link(node2_1, 1));

        NodeAstar[,] matrix =
        {
            {node0_0, node1_0, node2_0 },
            {node0_1, node1_1, node2_1 },
            {node0_2, node1_2, node2_2 },
            /*{node0_0, node0_1, node0_2},
            {node1_0, node1_1, node1_2},
            {node2_0, node2_1, node2_2 },*/
        };

        return matrix;
    }

    static void Main(string[] args)
    {
        NodeAstar[,] matrix = SetupMatrixData();       

        PosInArray startPos = new PosInArray(0,0);
        PosInArray endPos = new PosInArray(2,2);

        AStar thn = new AStar(matrix, startPos, endPos);
        thn.PrintAction();
    }

    
}
