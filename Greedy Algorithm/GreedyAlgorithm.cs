class ToTien
{
    public int menhGia = 0;
    public int soToTien = 0;
}

class GreedyAlgorithm
{
    LinkedList<ToTien> _toTienList = new LinkedList<ToTien>();

    int[] menhGia = { 500, 200, 100, 50, 20, 10, 5, 2, 1 };

    int tongTien;

    int tongSoToTien = 0;

    public GreedyAlgorithm()
    {
    }

    public void NhapTongTien()
    {
        Console.WriteLine("Nhap so tien: ");
        tongTien = int.Parse(Console.ReadLine());
    }

    public void DemSoToTien()
    {
        int tempTongTien = tongTien;

        for (int i = 0; i < menhGia.Length; i++)
        {
            ToTien toTien = new ToTien();
            while (tempTongTien - menhGia[i] >= 0)
            {
                tongSoToTien++;
                toTien.soToTien++;
                toTien.menhGia = menhGia[i];
                tempTongTien -= menhGia[i];
            }

            if (toTien.soToTien != 0)
            {
                _toTienList.AddLast(toTien);
            }
        }
    }

    public void InKetQua()
    {
        DemSoToTien();

        Console.WriteLine("Tong so to tien: " + tongSoToTien);
        foreach (ToTien toTien in _toTienList)
        {
            if (toTien.soToTien >= 0)
            {
                Console.WriteLine(" " + toTien.menhGia + "k: " + toTien.soToTien);
            }
        }
    }

    static void Main(string[] args)
    {
        GreedyAlgorithm p = new GreedyAlgorithm();
        p.NhapTongTien();

        Console.WriteLine("Ket qua: ");
        p.InKetQua();

        Console.ReadKey();
    }
}