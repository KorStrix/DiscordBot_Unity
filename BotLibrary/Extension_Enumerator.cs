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
            if (mapTarget == null || pIterAdd == null)
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

    static public class Extension_Primitive
    {
        static public T ConvertEnum<T>(this string strText)
            where T : struct
        {
            return (T)System.Enum.Parse(typeof(T), strText);
        }

        static public T PrevEnum<T>(this System.Enum eEnum)
            where T : struct
        {
            return eEnum.PrevEnum_String<T>().ConvertEnum<T>();
        }

        static public string PrevEnum_String<T>(this System.Enum eEnum)
            where T : struct
        {
            return System.Enum.GetName(typeof(T), eEnum.GetHashCode() - 1);
        }

        static public T NextEnum<T>(this System.Enum eEnum)
            where T : struct
        {
            return eEnum.NextEnum_String<T>().ConvertEnum<T>();
        }

        static public string NextEnum_String<T>(this System.Enum eEnum)
            where T : struct
        {
            return System.Enum.GetName(typeof(T), eEnum.GetHashCode() + 1);
        }
    }

}
