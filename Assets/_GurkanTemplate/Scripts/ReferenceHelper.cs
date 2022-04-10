using System.Collections.Generic;

namespace _GurkanTemplate.Scripts
{
    public static class ReferenceHelper<T1, T2>
    {
        public static Dictionary<T1, T2> RefDictionary => new Dictionary<T1, T2>();
    }
}