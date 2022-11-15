using System.Numerics;

class AKT
{
    private class NodeMaTran
    {
        public int[,] matrix;
        public NodeMaTran parentNode;
        public List<NodeMaTran> childNodes;
        public int f;
        public int g;
        public string action;

        public NodeMaTran(int[,] matrix, NodeMaTran parentNode, List<NodeMaTran> childNodes, int f, int g, string action)
        {
            this.matrix = matrix;
            this.parentNode = parentNode;
            this.childNodes = childNodes;
            this.f = f;
            this.g = g;
            this.action = action;
        }
    }

    private struct PosInArray
    {
        public int row;
        public int column;
    }

    private NodeMaTran firstNodeMaTran;

    private PriorityQueue<NodeMaTran, int> _openList = new PriorityQueue<NodeMaTran, int>();
    private List<NodeMaTran> _closedList = new List<NodeMaTran>();
    private LinkedList<NodeMaTran> rightWay = new LinkedList<NodeMaTran>();

    private int[,] targetMatrix = new int[3,3];

    private AKT(int[,] matrix)
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

    private NodeMaTran InitFirstNodeMaTran(int[,] firstMatrix)
    {
        int[,] matrix = CopyMatrix(firstMatrix);

        List<NodeMaTran> childNodeMaTran = new List<NodeMaTran>();

        NodeMaTran nodeMaTran = new NodeMaTran(matrix, null, childNodeMaTran, CalculateH_Value(matrix), 0, "");

        _closedList.Add(nodeMaTran);

        return nodeMaTran;
    }

    private int[,] CopyMatrix(int[,] target)
    {
        int[,] matrix = new int[3, 3];

        for (int i = 0; i < target.GetLength(0); i++)
        {
            for (int j = 0; j < target.GetLength(1); j++)
            {
                matrix[i, j] = target[i, j];
            }
        }

        return matrix;
    }

    private int CalculateH_Value(int[,] matrix)
    {
        int hValue = 0;

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if(matrix[i,j] != targetMatrix[i, j] && matrix[i,j] != 0)
                {
                    hValue++;
                }
            }
        }

        return hValue;
    }

    private void Travelled(NodeMaTran currentNode)
    {
        if (CalculateH_Value(currentNode.matrix) == 0)
        {
            return;
        }

        MoveMatrix(currentNode);

        NodeMaTran nextNodeMaTran = _openList.Dequeue();

        _closedList.Add(nextNodeMaTran);

        Travelled(nextNodeMaTran);
    }

    private void MoveMatrix(NodeMaTran currentNode)
    {
        PosInArray emptyPos = GetEmptyPos(currentNode.matrix);

        PosInArray topPos;
        topPos.row = emptyPos.row - 1; //Do ma tran ve tu tren xuong
        topPos.column = emptyPos.column;

        PosInArray downPos;
        downPos.row = emptyPos.row + 1;
        downPos.column = emptyPos.column;

        PosInArray rightPos;
        rightPos.row = emptyPos.row;
        rightPos.column = emptyPos.column + 1;

        PosInArray leftPos;
        leftPos.row = emptyPos.row;
        leftPos.column = emptyPos.column - 1;

        if(isValidPosInMatrix_3x3(topPos))
        {
            MoveObjectInMatrix(currentNode, emptyPos, topPos);
        }
        if(isValidPosInMatrix_3x3(downPos))
        {
            MoveObjectInMatrix(currentNode, emptyPos, downPos);
        }
        if(isValidPosInMatrix_3x3(leftPos))
        {
            MoveObjectInMatrix(currentNode, emptyPos, leftPos);
        }
        if(isValidPosInMatrix_3x3(rightPos))
        {
            MoveObjectInMatrix(currentNode, emptyPos, rightPos);
        }
    }

    private bool isValidPosInMatrix_3x3(PosInArray pos)
    {
        if (pos.row < 0 || pos.row > 2) return false;
        if (pos.column < 0 || pos.column > 2) return false;

        return true;
    }

    private void MoveObjectInMatrix(NodeMaTran currentNode, PosInArray emptyPos, PosInArray directPos)
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
        List<NodeMaTran> childNodeThap = new List<NodeMaTran>();
        NodeMaTran newNode = new NodeMaTran(matrix, currentNode, childNodeThap, f, g, action);

        // Kiểm tra xem đã có trong CloseList chưa
        if (!IsNodeInClosedList(newNode) && !IsNodeInOpenedList(newNode))
        {
            currentNode.childNodes.Add(newNode);

            _openList.Enqueue(newNode, newNode.f);
        }
    }

    private string GetDirectAction(PosInArray emptyPos, PosInArray directPos)
    {
        string directAction = "";

        Vector2 up = new Vector2(0, -1);    // Do ma tran ve tu tren xuong
        Vector2 down = new Vector2(0, 1);
        Vector2 right = new Vector2(1, 0);
        Vector2 left = new Vector2(-1, 0);

        Vector2 direct;
        direct.X = emptyPos.column - directPos.column;
        direct.Y = emptyPos.row - directPos.row;

        if(direct.Equals(up))
        {
            directAction = "up";
        } 
        else if (direct.Equals(down))
        {
            directAction = "down";
        }
        else if (direct.Equals(right))
        {
            directAction = "right";
        }
        else if (direct.Equals(left))
        {
            directAction = "left";
        }

        return directAction;
    }

    private PosInArray GetEmptyPos(int[,] matrix)
    {
        PosInArray pos;
        pos.row = -1;
        pos.column = -1;

        for (int row = 0; row < matrix.GetLength(0); row++)
        {
            for (int column = 0; column < matrix.GetLength(1); column++)
            {
                if (matrix[row, column] == 0)
                {
                    pos.row = row;
                    pos.column = column;

                    return pos;
                }
            }
        }

        return pos;
    }

    private bool IsNodeInClosedList(NodeMaTran nodeToCheck)
    {
        foreach (NodeMaTran nodeMaTran in _closedList)
        {
            if (MatrixCompare(nodeToCheck.matrix, nodeMaTran.matrix))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsNodeInOpenedList(NodeMaTran nodeToCheck)
    {
        List<NodeMaTran> tempOpenedList = new List<NodeMaTran>();

        while (_openList.Count > 0)
        {
            tempOpenedList.Add(_openList.Dequeue());
        }
        foreach (NodeMaTran nodeMaTran in tempOpenedList)
        {
            _openList.Enqueue(nodeMaTran, nodeMaTran.f);
        }

        foreach (NodeMaTran nodeMaTran in tempOpenedList)
        {
            if (MatrixCompare(nodeToCheck.matrix, nodeMaTran.matrix))
            {
                return true;
            }
        }

        return false;
    }

    private bool MatrixCompare(int[,] matrix1, int[,] matrix2)
    {
        int objectEqual = 0;

        for (int i = 0; i < matrix1.GetLength(0); i++)
        {
            for (int j = 0; j < matrix1.GetLength(1); j++)
            {
                if (matrix1[i,j] == matrix2[i, j])
                {
                    objectEqual++;
                }
            }
        }

        if (objectEqual == 9) return true;

        return false;
    }

    private void PrintAction()
    {
        Travelled(firstNodeMaTran);

        GetRightWay(_closedList.Last());

        int step = 1;
        foreach (NodeMaTran nodeMaTran in rightWay)
        {
            if (nodeMaTran.action.Equals("")) continue;

            Console.WriteLine("Step " + step + ": " + nodeMaTran.action);
            step++;
        }
    }

    private void GetRightWay(NodeMaTran nodeMaTran)
    {
        if (CalculateH_Value(nodeMaTran.matrix) == 0)
        {
            rightWay.AddFirst(nodeMaTran);
        }

        if (nodeMaTran.parentNode == null) return;

        rightWay.AddFirst(nodeMaTran.parentNode);

        GetRightWay(nodeMaTran.parentNode);
    }

    static void Main(string[] args)
    {
        int[,] startMatrix ={
            { 2, 8, 3 },
            { 1, 6, 4 },
            { 0, 7, 5 }
        };

        int[,] startMatrix2 =
        {
            { 0, 2, 4 },
            { 8, 1, 3 },
            { 7, 6, 5 }
        };

        AKT thn = new AKT(startMatrix2);
        thn.PrintAction();
    }
}
