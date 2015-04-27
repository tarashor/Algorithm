using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompletionTime
{
    public class Work:IComparable
    {
        public int Weight { get; set; }
        public int Length { get; set; }

        public int CompareTo(object obj)
        {
            Work work = obj as Work;
            double otherC = (double)work.Weight / (double)work.Length;
            double thisC = (double)this.Weight / (double)this.Length;
            if (otherC > thisC)
            {
                return 1;
            }
            else {
                if (otherC < thisC)
                {
                    return -1;
                }
                else
                {
                    if (this.Weight > work.Weight)
                    {
                        return -1;
                    }
                    else {
                        if (this.Weight < work.Weight)
                        {
                            return 1;
                        }
                        else {
                            return 0;
                        }
                    }
                    
                }
            }
        }
    }
}
