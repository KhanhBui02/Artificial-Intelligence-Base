class ThapHaNoiAKT
{
    private class NodeThap
    {
        public LinkedList<int>[] tower;
        public NodeThap parentNode;
        public List<NodeThap> childNodes;
        public int f;
        public int g;
        public string action;

        public NodeThap(LinkedList<int>[] tower, NodeThap parentNode, List<NodeThap> childNodes, int f, int g, string action)
        {
            this.tower = tower;
            this.parentNode = parentNode;
            this.childNodes = childNodes;
            this.f = f;
            this.g = g;
            this.action = action;
        }
    }

    private int _dishCount;

    private PriorityQueue<NodeThap, int> _openList = new PriorityQueue<NodeThap, int>();
    private List<NodeThap> _closedList = new List<NodeThap>();
    private LinkedList<NodeThap> rightWay = new LinkedList<NodeThap>();

    private LinkedList<int>[] h_With3Dish = new LinkedList<int>[8];
    private LinkedList<int>[] h_With4Dish = new LinkedList<int>[8];
    private LinkedList<int>[] hListToUse;

    private void InitH3Data()
    {
        LinkedList<int> column = new LinkedList<int>();

        column.AddLast(-1); // -1 == rong
        column.AddLast(3);
        column.AddLast(2);
        column.AddLast(1);
        h_With3Dish[0] = new LinkedList<int>(column);

        column.Clear();
        column.AddLast(-1);
        column.AddLast(3);
        column.AddLast(2);
        h_With3Dish[1] = new LinkedList<int>(column);

        column.Clear();
        column.AddLast(-1);
        column.AddLast(3);
        h_With3Dish[2] = new LinkedList<int>(column);

        column.Clear();
        column.AddLast(-1);
        column.AddLast(3);
        column.AddLast(1);
        h_With3Dish[3] = new LinkedList<int>(column);

        column.Clear();
        column.AddLast(-1);
        h_With3Dish[4] = new LinkedList<int>(column);

        column.Clear();
        column.AddLast(-1);
        column.AddLast(1);
        h_With3Dish[5] = new LinkedList<int>(column);

        column.Clear();
        column.AddLast(-1);
        column.AddLast(2);
        h_With3Dish[6] = new LinkedList<int>(column);

        column.Clear();
        column.AddLast(-1);
        column.AddLast(2);
        column.AddLast(1);
        h_With3Dish[7] = new LinkedList<int>(column);
    }

    private void InitH4Data()
    {

    }

    private NodeThap InitFirstNodeThap()
    {
        LinkedList<int>[] tower = new LinkedList<int>[3];
        tower[0] = new LinkedList<int>();
        tower[1] = new LinkedList<int>();
        tower[2] = new LinkedList<int>();

        tower[0].AddLast(-1);
        tower[1].AddLast(-1);
        tower[2].AddLast(-1);

        for (int i = _dishCount; i > 0; i--)
        {
            tower[0].AddLast(i);
        }

        NodeThap nodeThap = new NodeThap(tower, null, null, CalculateH_Value(tower[2]), 0, "");

        _closedList.Add(nodeThap);

        return nodeThap;
    }

    private ThapHaNoiAKT(int dishCount)
    {
        this._dishCount = dishCount;

        switch (_dishCount)
        {
            case 3:
                InitH3Data();
                hListToUse = h_With3Dish;
                break;

            case 4:
                InitH4Data();
                hListToUse = h_With4Dish;
                break;
        }

        NodeThap firstNodeThap = InitFirstNodeThap();

        Travelled(firstNodeThap);
    }

    private int CalculateH_Value(LinkedList<int> column)
    {
        int hValue = -1;

        for (int i = 0; i < hListToUse.Length; i++)
        {
            if (column.SequenceEqual(hListToUse[i]))
            {
                hValue = i;
                break;
            }
        }

        return hValue;
    }

    private void Travelled(NodeThap currentNode)
    {
        if (CalculateH_Value(currentNode.tower[2]) == 0)
        {
            return;
        }

        MoveDish(currentNode);

        NodeThap nextNodeThap = _openList.Dequeue();

        _closedList.Add(nextNodeThap);

        Travelled(nextNodeThap);
    }

    private void MoveDish(NodeThap currentNode)
    {
        bool canMove = true;
        int column = 0;
        int dish;

        while (canMove == false && column < 3)
        {
            canMove = false;
            dish = currentNode.tower[column].Last();

            for (int i = 0; i < currentNode.tower.Length; i++)
            {
                if (i == column) continue;  //không cần so số ở cột đã lấy

                int otherDish = currentNode.tower[i].Last();

                if (otherDish != -1 && dish < otherDish)
                {
                    LinkedList<int>[] tower = CopyTower(currentNode.tower);

                    // Di chuyen dia
                    string action = "";
                    int temp1 = tower[column].Last();
                    tower[column].RemoveFirst();
                    tower[i].AddLast(temp1);
                    action += "Move dish from column " + column + " to column " + i;

                    // Tính f
                    int g = currentNode.g;
                    int f = CalculateH_Value(tower[2]) + g;

                    // Tạo NodeThap với đĩa đã di chuyển
                    NodeThap newNode = new NodeThap(tower, currentNode, null, f, g + 1, action);

                    // Kiểm tra xem đã có trong CloseList chưa
                    if (IsNodeInClosedList(newNode))
                    {
                        currentNode.childNodes.Add(newNode);

                        _openList.Enqueue(newNode, newNode.f);
                    }

                    canMove = true;
                }
            }

            if (canMove == true)
            {
                column++;
            }
        }
    }

    private LinkedList<int>[] CopyTower(LinkedList<int>[] target)
    {
        LinkedList<int>[] tower = new LinkedList<int>[3];
        tower[0] = new LinkedList<int>(target[0]);
        tower[1] = new LinkedList<int>(target[1]);
        tower[2] = new LinkedList<int>(target[2]);

        return tower;
    }

    private bool IsNodeInClosedList(NodeThap nodeToCheck)
    {
        foreach (NodeThap nodeThap in _closedList)
        {
            if (TowerCompare(nodeToCheck.tower, nodeThap.tower))
            {
                return true;
            }
        }

        return false;
    }

    private bool TowerCompare(LinkedList<int>[] tower1, LinkedList<int>[] tower2)
    {
        int columnEqual = 0;

        for (int i = 0; i < 3; i++)
        {
            if (tower1[i].SequenceEqual(tower2[i]))
            {
                columnEqual++;
            }
        }

        if (columnEqual == 3) return true;

        return false;
    }

    private void PrintAction()
    {

    }

    private void GetRightWay(NodeThap nodeThap)
    {
        if (nodeThap.parentNode == null) return;
    }

    static void Main(string[] args)
    {
        ThapHaNoiAKT thn = new ThapHaNoiAKT(3);
        thn.PrintAction();

        LinkedList<int> a = new LinkedList<int>();
        LinkedList<int> b = new LinkedList<int>();

        a.AddLast(1);
        a.AddLast(2);
        a.AddLast(3);
        a.AddLast(4);

        b.AddLast(1);
        b.RemoveLast();

        b.AddLast(1);
        b.AddLast(2);
        b.AddLast(3);
        b.AddLast(4);

        foreach (int i in a)
        {
            Console.Write(" " + i);
        }
    }
}
