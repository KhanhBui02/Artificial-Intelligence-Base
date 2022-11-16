using System.Numerics;
using System.Xml.Linq;

class AStar
{
    private class NodeMatran
    {
        public List<Link> links;
        public int estimateCost;
        public PosInArray posInArray;

        public NodeMatran(List<Link> afterNodes, int estimateCost, PosInArray posInArray)
        {
            this.links = afterNodes;
            this.estimateCost = estimateCost;
            this.posInArray = posInArray;
        }       
    }

    private class NodeAstar
    {
        public NodeAstar parentNode;
        public PosInArray posInArray;
        public int d;
        public int f;
        public string action;

        public NodeAstar(NodeAstar parentNode, PosInArray posInArray, int d, int f, string action)
        {
            this.parentNode = parentNode;
            this.posInArray = posInArray;
            this.d = d;
            this.f = f;
            this.action = action;
        }
        public NodeAstar()
        {

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
    private NodeAstar _startNodeAstar;
    private NodeAstar _endNodeAstar;
    private NodeMatran[,] _matrix;

    private PriorityQueue<NodeAstar, int> _openList = new PriorityQueue<NodeAstar, int>();
    private List<NodeAstar> _closedList = new List<NodeAstar>();
    private LinkedList<NodeAstar> rightWay = new LinkedList<NodeAstar>();

    private AStar(NodeMatran[,] matrix, PosInArray startPos, PosInArray endPos)
    {
        NodeAstar parentStart = new NodeAstar();
        int dStart = 0;
        int fStart = matrix[startPos.column, startPos.row].estimateCost;
        string actionStart = "";
        _startNodeAstar = new NodeAstar(parentStart, startPos, dStart, fStart, actionStart);

        NodeAstar parentEnd = new NodeAstar();
        int dEnd = 0;
        int fEnd = matrix[endPos.column, endPos.row].estimateCost;
        string actionEnd = "";
        _endNodeAstar = new NodeAstar(parentEnd, startPos, dEnd, fEnd, actionEnd);

        _matrix = matrix;
        _startPos = startPos;
        _endPos = endPos;
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

    private void Travelled(NodeAstar currentNode)
    {
        if (currentNode.posInArray.Equals(_matrix[_endPos.column, _endPos.row].posInArray))
        {
            return;
        }

        MoveNextNode(currentNode);

        NodeAstar nextNodeAstar = _openList.Dequeue();

        _closedList.Add(nextNodeAstar);

        Travelled(nextNodeAstar);
    }

    private void MoveNextNode(NodeAstar currentNode)
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
        List<NodeAstar> childNodeThap = new List<NodeAstar>();
        NodeAstar newNode = new NodeAstar(matrix, currentNode, childNodeThap, f, g, action);

        // Kiểm tra xem đã có trong CloseList chưa
        if (!IsNodeInClosedList(newNode) && !IsNodeInOpenedList(newNode))
        {
            currentNode.childNodes.Add(newNode);

            _openList.Enqueue(newNode, newNode.f);
        }
    }

    private bool IsNodeInClosedList(NodeAstar nodeToCheck)
    {
        foreach (NodeAstar nodeMaTran in _closedList)
        {
            if (nodeToCheck.posInArray.Equals(nodeMaTran.posInArray))
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
        Travelled(_startNodeAstar);

        GetRightWay(_closedList.Last());

        int step = 1;
        foreach (NodeAstar nodeAstar in rightWay)
        {
            if (nodeAstar.action.Equals("")) continue;

            Console.WriteLine("Step " + step + ": " + nodeAstar.action);
            step++;
        }
    }

    private void GetRightWay(NodeAstar nodeAstar)
    {
        // Add node dich vao rightWay
        if (nodeAstar.posInArray.Equals(_matrix[_endPos.column, _endPos.row].posInArray))
        {
            rightWay.AddFirst(nodeAstar);
        }

        if (nodeAstar.parentNode == null) return;

        rightWay.AddFirst(nodeAstar.parentNode);

        GetRightWay(nodeAstar.parentNode);
    }

    private static NodeMatran[,] SetupMatrixData()
    {
        List<Link> links = new List<Link>();
        int estimateCost = -1;
        PosInArray pos;

        pos = new PosInArray(0, 0);
        NodeMatran node0_0 = new NodeMatran(links, estimateCost, pos);

        pos = new PosInArray(0, 1);
        NodeMatran node0_1 = new NodeMatran(links, estimateCost, pos);

        pos = new PosInArray(0, 2);
        NodeMatran node0_2 = new NodeMatran(links, estimateCost, pos);

        pos = new PosInArray(1, 0);
        NodeMatran node1_0 = new NodeMatran(links, estimateCost, pos);

        pos = new PosInArray(1, 1);
        NodeMatran node1_1 = new NodeMatran(links, estimateCost, pos);

        pos = new PosInArray(1, 2);
        NodeMatran node1_2 = new NodeMatran(links, estimateCost, pos);

        pos = new PosInArray(2, 0);
        NodeMatran node2_0 = new NodeMatran(links, estimateCost, pos);

        pos = new PosInArray(2, 1);
        NodeMatran node2_1 = new NodeMatran(links, estimateCost, pos);

        pos = new PosInArray(2, 2);
        NodeMatran node2_2 = new NodeMatran(links, estimateCost, pos);

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
