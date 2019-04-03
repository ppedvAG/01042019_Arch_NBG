using System;
using System.Collections.Generic;
using System.Text;

namespace ppedv.QualitySlegehammer.Model.Contracts
{
    public interface IDevice
    {
        void Start(int power);

        event Action<string, int> ErrorEvent;
    }
}
