using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC4Ever.Tables
{
    public interface ITable : IDisposable
    {
        bool IsDisposed { get; }
        byte NextByte();
        byte ReverseByte();
        void Reset();
        Bitmap ToBitmap();
    }
}
