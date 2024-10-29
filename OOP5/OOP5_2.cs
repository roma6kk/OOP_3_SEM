using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

abstract partial class Software
{
    public override bool Equals(object obj)
    {
        if (obj.GetType() != this.GetType()) return false;

        var other = (Software)obj;
        return (this.Name == other.Name);
    }

    public override int GetHashCode()
    {
        if (this.Name.Length == 0) return 0;
        else
        {
            int HashCode = 1;
            for (int i = 0; i < this.Name.Length; i++)
            {
                if (i <= 3)
                    HashCode = HashCode * (int)this.Name[i];
                else
                    HashCode = HashCode * (int)this.Name[i] + i;
            }
            return HashCode;
        }
    }

    public override string ToString()
    {
        return $"Тип: {this.GetType().Name}, Название: {this.Name}";
    }
}

namespace OOP5
{
    internal class OOP5_2
    {

    }
}
