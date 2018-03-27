using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strix
{
    public interface IDictionaryItem<TKey>
    {
        TKey IDictionaryItem_GetKey();
    }

    static public class Extension_Enumerator
    {
        static public void AddRange<TKey, TValue>(this Dictionary<TKey, TValue> mapTarget, IEnumerable<TValue> pIterAdd, bool bIsClear = true)
            where TValue : IDictionaryItem<TKey>
        {
            if (mapTarget == null)
                return;

            if (bIsClear)
                mapTarget.Clear();

            var pIter = pIterAdd.GetEnumerator();
            while(pIter.MoveNext())
            {
                mapTarget.Add(pIter.Current.IDictionaryItem_GetKey(), pIter.Current);
            }
        }
    }
}
