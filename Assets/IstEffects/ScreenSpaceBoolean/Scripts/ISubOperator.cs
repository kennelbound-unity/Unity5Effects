﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR


public abstract class ISubOperator : MonoBehaviour
{
    #region static
    static private List<ISubOperator> s_instances;
    static private Dictionary<int, List<ISubOperator>> s_groups;
    static private bool s_dirty = true;

    static public List<ISubOperator> GetInstances()
    {
        if (s_instances == null) { s_instances = new List<ISubOperator>(); }
        return s_instances;
    }

    static public Dictionary<int, List<ISubOperator>> GetGroups()
    {
        if (s_groups == null) { s_groups = new Dictionary<int, List<ISubOperator>>(); }
        if (s_dirty)
        {
            s_dirty = false;
            s_groups.Clear();
            var instances = GetInstances();
            for (int i = 0; i < instances.Count; ++i)
            {
                var instance = instances[i];
                for (int j = 0; j < instance.m_groups.Length; ++j)
                {
                    int k = instance.m_groups[j];
                    if (!s_groups.ContainsKey(k))
                    {
                        s_groups.Add(k, new List<ISubOperator>());
                    }
                    s_groups[k].Add(instance);
                }
            }
        }
        return s_groups;
    }
    #endregion

    #region fields
    public int[] m_groups = new int[] { 0 };
    #endregion

    public int[] groups
    {
        get { return m_groups; }
        set { m_groups = value; s_dirty = true; }
    }

#if UNITY_EDITOR
    public virtual void Reset()
    {
    }

    public virtual void OnValidate()
    {
        s_dirty = true;
    }
#endif

    public virtual void OnEnable()
    {
        GetInstances().Add(this);
    }

    public virtual void OnDisable()
    {
        GetInstances().Remove(this);
    }

    public abstract void IssueDrawCall_DepthMask(SubRenderer br, CommandBuffer cb);
}