using System;

namespace Lab5Games
{
    public static class EqualityExtension
    {
        public static bool IsNullable(this object obj)
        {
            return Nullable.GetUnderlyingType(obj.GetType()) != null;
        }

        public static bool IsNull(this object obj)
        {
            if (!obj.IsNullable())
                return false;

            return ReferenceEquals(obj, null);
        }

        public static bool SafeEquals(this object objA, object objB)
        {
            if (objA.IsNull() && objB.IsNull())
                return true;

            return ReferenceEquals(objA, objB);
        }
    }
}
