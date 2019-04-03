using System;

namespace BinFord.MegaBeeper5000
{
    public class Beep
    {
        public void Init()
        {
            Console.WriteLine("Beeper wird geladen..");
        }

        int c;
        public void Engage(int power)
        {
            Console.Beep(power, 1000);

            if (++c % 3 == 0)
                Explosion?.Invoke(c);
        }

        public event Action<int> Explosion;
     
    }
}
