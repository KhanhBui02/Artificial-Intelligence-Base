class GTS2
{
    private int[,] _matrix;

    private struct VerticeValue
    {
        public int value;
        public int i;
        public int j;
    }

    public GTS2(int[,] matrix)
    {
        _matrix = matrix;
    }

    private bool isColumnOverlap(VerticeValue[] verticeValues, int j)
    {
        for (int k = 0; k < verticeValues.Length; k++)
        {
            if (verticeValues[k].value != 0 && (j == verticeValues[k].j || j == verticeValues[0].i))
            {
                return true;
            }
        }

        return false;
    }

    private VerticeValue[] GTS(int vertice)
    {
        VerticeValue[] verticeValues = new VerticeValue[_matrix.GetLength(0)];
        VerticeValue vv = new VerticeValue();

        int verticeVIndex = 0;
        int i = vertice;
        while (verticeVIndex < _matrix.GetLength(0) - 1)
        {
            vv.value = int.MaxValue;
            for (int j = 0; j < _matrix.GetLength(1); j++)
            {
                if (i == j)
                {
                    continue;
                }

                if (isColumnOverlap(verticeValues, j))
                {
                    continue;
                }

                if (_matrix[i, j] < vv.value)
                {
                    vv.value = _matrix[i, j];
                    vv.i = i;
                    vv.j = j;
                }
            }

            i = vv.j;
            verticeValues[verticeVIndex] = vv;
            verticeVIndex++;
        }

        //Last value to find
        vv.i = _matrix.GetLength(0) - 1;
        vv.j = vertice;
        vv.value = _matrix[vv.i, vv.j];
        verticeValues[verticeVIndex] = vv;

        return verticeValues;
    }

    private int GTSSumResult(int vertice)
    {
        VerticeValue[] verticeValues = GTS(vertice);
        int total = 0;
        for (int i = 0; i < verticeValues.Length; i++)
        {
            total += verticeValues[i].value;
        }

        return total;
    }

    private void GTS_2_Result(int[] verticesToCompare)
    {
        int minCost = int.MaxValue;
        int totalValue;
        int index = 0;
        for (int i = 0; i < verticesToCompare.Length; i++)
        {
            totalValue = GTSSumResult(i);
            if (totalValue < minCost)
            {
                minCost = totalValue;
                index = i;
            }
        }

        Console.Write("Vertice has min cost: " + verticesToCompare[index] + ", Cost: " + minCost);
    }

    // Use to test check vertice
    private void Print()
    {
        VerticeValue[] verticeValue = GTS(2);

        for (int i = 0; i < verticeValue.GetLength(0); i++)
        {
            Console.WriteLine("i: " + verticeValue[i].i + ", j:" + verticeValue[i].j + ", value:" + verticeValue[i].value);
        }
    }

    static void Main(string[] args)
    {
        int[,] matrix = {   { -1, 20, 42, 31, 6, 24},
                            { 10, -1, 17, 6, 35, 18 },
                            { 25, 5, -1, 27, 14, 9 },
                            { 12, 9, 24, -1, 30, 12},
                            { 14, 7, 21, 15, -1, 38},
                            { 40, 15, 16, 5, 20, -1} };
        GTS2 a = new GTS2(matrix);
        //a.Print();

        int[] verticeToCheck = { 2, 3, 4 };

        a.GTS_2_Result(verticeToCheck);
        Console.WriteLine();
    }
}