namespace XperiCad.DigitalDrawingStore.BL.Impl.Extensions
{
    internal static class IDictionaryExtensions
    {
        public static IDictionary<TKey, TValue> OrderByValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
            where TKey : notnull
        {
            if (dictionary is null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            var orderedDictionary = dictionary.OrderBy(e => e.Value);
            var result = new Dictionary<TKey, TValue>();

            foreach (var keyValuePair in orderedDictionary)
            {
                result.Add(keyValuePair.Key, keyValuePair.Value);
            }

            return result;
        }
    }
}
