﻿class ThapHaNoiDeQuy
{
    private static void solveTowers(int n, char startPeg, char endPeg, char tempPeg)
    {
        if (n > 0)
        {
            solveTowers(n - 1, startPeg, tempPeg, endPeg);
            Console.WriteLine("Move disk from " + startPeg + ' ' + endPeg);
            solveTowers(n - 1, tempPeg, endPeg, startPeg);

        }
    }

    static void Main(string[] args)
    {
        char startPeg = 'A';
        char endPeg = 'C';
        char tempPeg = 'B';
        int totalDisks = 3;

        solveTowers(totalDisks, startPeg, endPeg, tempPeg);
    }
}