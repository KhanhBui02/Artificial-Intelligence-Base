using System.Numerics;

class AStar
{
    private class NodeMatran
    {
        public List<NodeLink> beforeNodes;
        public NodeMatran afterNodes;
        public int estimateCost;
        public int trueCost;
        public PosInArray posInArray;

        public NodeMatran(List<NodeLink> beforeNodes, NodeMatran afterNodes, int estimateCost, int trueCost, PosInArray posInArray)
        {
            this.beforeNodes = beforeNodes;
            this.afterNodes = afterNodes;
            this.estimateCost = estimateCost;
            this.trueCost = trueCost;
            this.posInArray = posInArray;
        }       
    }
    private struct NodeLink
    {
        public NodeMatran nodeMaTran;
        public int weight;
    }

    private struct PosInArray
    {
        public int row;
        public int column;
    }

    private NodeMatran firstNodeMaTran;

    private PriorityQueue<NodeMatran, int> _openList = new PriorityQueue<NodeMatran, int>();
    private List<NodeMatran> _closedList = new List<NodeMatran>();
    private LinkedList<NodeMatran> rightWay = new LinkedList<NodeMatran>();

    private int[,] targetMatrix = new int[3, 3];

    private AStar(NodeMatran[,] matrix, PosInArray startPos, PosInArray endPos)
    {
        InitTargetMatrix();
        firstNodeMaTran = InitFirstNodeMaTran(matrix);
    }

    private void InitTargetMatrix()
    {
        int[,] temp ={
            { 1, 2, 3 },
            { 8, 0, 4 },
            { 7, 6, 5 }
        };

        targetMatrix = temp;
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
            if (//MatrixCompare(nodeToCheck.matrix, nodeMaTran.matrix))
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
        Travelled(firstNodeMaTran);

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
        NodeMatran[,] matrix = new NodeMatran[3, 3];



        return matrix;
    }

    static void Main(string[] args)
    {
        NodeMatran[,] matrix = SetupMatrixData();       

        PosInArray startPos;
        startPos.row = 0;
        startPos.column = 0;

        PosInArray endPos;
        endPos.row = 2;
        endPos.column = 2;

        AStar thn = new AStar(matrix, startPos, endPos);
        thn.PrintAction();
    }

    
}
