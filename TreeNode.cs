namespace Prism
{
    using System.Collections.Generic;

    //It is orderer by K and hold contents of V
    public class TreeNode<K, V>
    {
        Dictionary<K, TreeNode<K, V>> keysDictionary;
        public V value;

        public void Add(List<K> k, V value)
        {
            if (k == null || k.Count == 0)
                return;

            if (keysDictionary == null)
                keysDictionary = new Dictionary<K, TreeNode<K, V>>();

            if (k.Count == 1)
            {
                keysDictionary[k[0]] = new TreeNode<K, V>();
                keysDictionary[k[0]].value = value;
                return;
            }

            if (!keysDictionary.ContainsKey(k[0]))
            {
                keysDictionary[k[0]] = new TreeNode<K, V>();
                keysDictionary[k[0]].Add(k.GetRange(1, k.Count - 1), value);
            }
        }

        public V Get(List<K> k)
        {
            if (k == null || k.Count == 0)
                return default(V);

            if (keysDictionary == null || keysDictionary.Count == 0)
            {
                return default(V);
            }

            if (k.Count == 1)
            {
                if (keysDictionary.ContainsKey(k[0]))
                    return keysDictionary[k[0]].value;
            }
            else
            {
                if (keysDictionary.ContainsKey(k[0]))
                    return keysDictionary[k[0]].Get(k.GetRange(1, k.Count - 1));
                else
                    return default(V);
            }

            return default(V);
        }
    }
}