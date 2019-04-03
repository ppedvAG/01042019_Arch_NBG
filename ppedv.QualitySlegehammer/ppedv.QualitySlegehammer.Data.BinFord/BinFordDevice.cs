using BinFord.MegaBeeper5000;
using ppedv.QualitySlegehammer.Model.Contracts;
using System;

namespace ppedv.QualitySlegehammer.Data.BinFord
{
    public class BinFordDevice : IDevice
    {
        Beep megaBeep = new Beep();

        public BinFordDevice()
        {
            megaBeep.Explosion += i => ErrorEvent?.Invoke($"Eine explosion wurde verursacht", i);
            megaBeep.Init();
        }

        public event Action<string, int> ErrorEvent;

        public void Start(int power)
        {
            megaBeep.Engage(power);
        }
    }
}
