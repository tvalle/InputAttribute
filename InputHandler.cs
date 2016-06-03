namespace Prism
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    [AttributeUsage(AttributeTargets.Method)]
    public class InputSequenceAttribute : Attribute
    {
        public Key[] KeySequence;

        public InputSequenceAttribute(params Key[] keySequence)
        {
            this.KeySequence = keySequence;
        }
    }

    public class KeySequenceContent
    {
        public MethodInfo Method { get; set; }
        public Type ComponentType { get; set; }
    }

    public class InputHandler : MonoBehaviour
    {
        private TreeNode<Key, KeySequenceContent> sequenceTree;

        //Declared here so that in the update we don't instantiate stuff all the time
        private List<Key> keys = new List<Key>();
        private KeySequenceContent content;
        private KeyCode ckey;
        private KeyCode[] keycodes = (KeyCode[])Enum.GetValues(typeof(KeyCode));
        private GameObject[] gameObjectsInScene;

        void Awake()
        {
            sequenceTree = new TreeNode<Key, KeySequenceContent>();

            MonoScript[] scripts = Resources.FindObjectsOfTypeAll<MonoScript>();

            foreach (MonoScript s in scripts)
            {
                if (!s.text.Contains("Prism"))
                    continue;

                Type t = s.GetClass();
                if (t != null)
                {
                    {
                        var m = t.GetMethods();
                        for (int j = 0; j < m.Length; j++)
                        {
                            var customAttributes = m[j].GetCustomAttributes(typeof(InputSequenceAttribute), false);
                            if (customAttributes.Length > 0)
                            {
                                var ks = new List<Key>((customAttributes[0] as InputSequenceAttribute).KeySequence);
                                ks.Sort();
                                sequenceTree.Add(ks, new KeySequenceContent()
                                {
                                    Method = m[j],
                                    ComponentType = t
                                });
                            }
                        }
                    }
                }
            }
        }

        void Start()
        {
            gameObjectsInScene = FindObjectsOfType<GameObject>();
        }

        void Update()
        {
            if (Input.anyKey)
            {
                for (int i = 0; i < keycodes.Length; i++)
                {
                    ckey = keycodes[i];

                    if (Input.GetKey(ckey))
                    {
                        keys.Add((Key)((int)ckey));
                    }
                    if (Input.GetKeyDown(ckey))
                    {
                        keys.Remove((Key)((int)ckey));
                        keys.Add((Key)((int)ckey) + 1000);
                    }
                    if (Input.GetKeyUp(ckey))
                    {
                        keys.Remove((Key)((int)ckey));
                        keys.Add((Key)((int)ckey) + 2000);
                    }
                }
            }

            if (keys.Count > 0)
            {
                keys.Sort();
                content = sequenceTree.Get(keys);
                keys.Clear();

                if (content == null)
                    return;

                foreach (GameObject go in gameObjectsInScene)
                {
                    var component = go.GetComponent(content.ComponentType);
                    if (component != null)
                        go.SendMessage(content.Method.Name);
                }
            }
        }
    }
}