using System.Numerics;
using System.Xml.Linq;

class AStar
{
    private class NodeMatran
    {
        public List<Link> links;
        public int estimateCost;
        public int trueCost;
        public PosInArray posInArray;

        public NodeMatran(List<Link> afterNodes, int estimateCost, int trueCost, PosInArray posInArray)
        {
            this.links = afterNodes;
            this.estimateCost = estimateCost;
            this.trueCost = trueCost;
            this.posInArray = posInArray;
        }       
    }

    private class PosInArray
    {
        public int column;
        public int row;

        public PosInArray(int column, int row)
        {
            this.column = column;
            this.row = row;
        }
    }

    private class Link
    {
        public NodeMatran nodeMaTran;
        public int weight;

        public Link(NodeMatran nodeMatran, int weight)
        {
            this.nodeMaTran = nodeMatran;
            this.weight = weight;
        }
    }

    private PosInArray _startPos;
    private PosInArray _endPos;
    private NodeMatran[,] _matrix;

    private PriorityQueue<NodeMatran, int> _openList = new PriorityQueue<NodeMatran, int>();
    private List<NodeMatran> _closedList = new List<NodeMatran>();
    private LinkedList<NodeMatran> rightWay = new LinkedList<NodeMatran>();

    private int[,] targetMatrix = new int[3, 3];

    private AStar(NodeMatran[,] matrix, PosInArray startPos, PosInArray endPos)
    {
        _matrix = matrix;
        _startPos = startPos;
        _endPos = endPos;
    }

    private NodeMatran InitFirstNodeMaTran(int[,] firstMatrix)
    {
        int[,] matrix = CopyMatrix(firstMatrix);

        List<NodeMatran> childNodeMaTran = new List<NodeMatran>();

        NodeMatran nodeMaTran = new NodeMatran(matrix, null, childNodeMaTran, CalculateH_Value(matrix), 0, "");

        _closedList.Add(nodeMaTran);

        return nodeMaTran;
    }

    private int CalculateH_Value(int[,] matrix)
    {
        int hValue = 0;

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (matrix[i, j] != targetMatrix[i, j] && matrix[i, j] != 0)
                {
                    hValue++;
                }
            }
        }

        return hValue;
    }

    private void Travelled(NodeMatran currentNode)
    {
        if (CalculateH_Value(currentNode.matrix) == 0)
        {
            return;
        }

        MoveMatrix(currentNode);

        NodeMatran nextNodeMaTran = _openList.Dequeue();

        _closedList.Add(nextNodeMaTran);

        Travelled(nextNodeMaTran);
    }

    private void MoveMatrix(NodeMatran currentNode)
    {
        int[,] matrix = CopyMatrix(currentNode.matrix);

        int posValue = matrix[directPos.row, directPos.column];

        // Di chuyen object
        string action = "";
        matrix[emptyPos.row, emptyPos.column] = posValue;
        matrix[directPos.row, directPos.column] = 0;
        action += "Move " + posValue + " " + GetDirectAction(emptyPos, directPos);

        // Tính f
        int g = currentNode.g + 1;
        int f = CalculateH_Value(matrix) + g;

        // Tạo NodeMaTran với object đã di chuyển
        List<NodeMatran> childNodeThap = new List<NodeMatran>();
        NodeMatran newNode = new NodeMatran(matrix, currentNode, childNodeThap, f, g, action);

        // Kiểm tra xem đã có trong CloseList chưa
        if (!IsNodeInClosedList(newNode) && !IsNodeInOpenedList(newNode))
        {
            currentNode.childNodes.Add(newNode);

            _openList.Enqueue(newNode, newNode.f);
        }
    }

    private bool IsNodeInClosedList(NodeMatran nodeToCheck)
    {
        foreach (NodeMatran nodeMaTran in _closedList)
        {
            if (nodeToCheck.posInArray.Equals(nodeMaTran.posInArray))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsNodeInOpenedList(NodeMatran nodeToCheck)
    {
        List<NodeMatran> tempOpenedList = new List<NodeMatran>();

        while (_openList.Count > 0)
        {
            tempOpenedList.Add(_openList.Dequeue());
        }
        foreach (NodeMatran nodeMaTran in tempOpenedList)
        {
            _openList.Enqueue(nodeMaTran, nodeMaTran.f);
        }

        return false;
    }

    private void PrintAction()
    {
        Travelled(_startNodeMatran);

        GetRightWay(_closedList.Last());

        int step = 1;
        foreach (NodeMatran nodeMaTran in rightWay)
        {
            if (nodeMaTran.action.Equals("")) continue;

            Console.WriteLine("Step " + step + ": " + nodeMaTran.action);
            step++;
        }
    }

    private void GetRightWay(NodeMatran nodeMaTran)
    {
        if (CalculateH_Value(nodeMaTran.matrix) == 0)
        {
            rightWay.AddFirst(nodeMaTran);
        }

        if (nodeMaTran.parentNode == null) return;

        rightWay.AddFirst(nodeMaTran.parentNode);

        GetRightWay(nodeMaTran.parentNode);
    }

    private static NodeMatran[,] SetupMatrixData()
    {
        List<Link> links = new List<Link>();
        int estimateCost = -1;
        int trueCose = -1;
        PosInArray pos;

        pos = new PosInArray(0, 0);
        NodeMatran node0_0 = new NodeMatran(links, estimateCost, trueCose, pos);

        pos = new PosInArray(0, 1);
        NodeMatran node0_1 = new NodeMatran(links, estimateCost, trueCose, pos);

        pos = new PosInArray(0, 2);
        NodeMatran node0_2 = new NodeMatran(links, estimateCost, trueCose, pos);

        pos = new PosInArray(1, 0);
        NodeMatran node1_0 = new NodeMatran(links, estimateCost, trueCose, pos);

        pos = new PosInArray(1, 1);
        NodeMatran node1_1 = new NodeMatran(links, estimateCost, trueCose, pos);

        pos = new PosInArray(1, 2);
        NodeMatran node1_2 = new NodeMatran(links, estimateCost, trueCose, pos);

        pos = new PosInArray(2, 0);
        NodeMatran node2_0 = new NodeMatran(links, estimateCost, trueCose, pos);

        pos = new PosInArray(2, 1);
        NodeMatran node2_1 = new NodeMatran(links, estimateCost, trueCose, pos);

        pos = new PosInArray(2, 2);
        NodeMatran node2_2 = new NodeMatran(links, estimateCost, trueCose, pos);

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

        NodeMatran[,] matrix =
        {
            {node0_0, node1_0, node2_0 },
            {node0_1, node1_1, node2_1 },
            {node0_2, node1_2, node2_2 },
        };

        return matrix;
    }

    static void Main(string[] args)
    {
        NodeMatran[,] matrix = SetupMatrixData();       

        PosInArray startPos = new PosInArray(0,0);
        PosInArray endPos = new PosInArray(2,2);

        AStar thn = new AStar(matrix, startPos, endPos);
        thn.PrintAction();
    }

    
}
