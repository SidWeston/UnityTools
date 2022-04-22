using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu()]
public class BehaviourTree : ScriptableObject
{
    public BTNode rootNode;
    public BTNode.State treeState = BTNode.State.Running;
    public List<BTNode> nodes = new List<BTNode>();
    public Blackboard blackboard = new Blackboard();

    public BTNode.State Update()
    {
        if(rootNode.state == BTNode.State.Running)
        {
            return rootNode.Update();
        }
        return treeState;

    }

    public BTNode CreateNode(System.Type type)
    {
        BTNode node = ScriptableObject.CreateInstance(type) as BTNode;
        node.name = type.Name;
        node.guid = GUID.Generate().ToString();
        nodes.Add(node);

        if(!Application.isPlaying)
        {
            AssetDatabase.AddObjectToAsset(node, this);
        }
        AssetDatabase.SaveAssets();
        return node;
    }

    public void DeleteNode(BTNode node)
    {
        nodes.Remove(node);
        AssetDatabase.RemoveObjectFromAsset(node);
        AssetDatabase.SaveAssets();
    }

    public void AddChild(BTNode parent, BTNode child)
    {
        RootNode root = parent as RootNode;
        if (root)
        {
            root.child = child;
            EditorUtility.SetDirty(root);
        }

        DecoratorNode decorator = parent as DecoratorNode;
        if(decorator)
        {
            decorator.child = child;
            EditorUtility.SetDirty(decorator);
        }

        CompositeNode composite = parent as CompositeNode;
        if (composite)
        {
            composite.children.Add(child);
            EditorUtility.SetDirty(composite);
        }
    }

    public void RemoveChild(BTNode parent, BTNode child)
    {
        RootNode root = parent as RootNode;
        if (root)
        {
            root.child = null;
        }

        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator)
        {
            decorator.child = null;
        }

        CompositeNode composite = parent as CompositeNode;
        if (composite)
        {
            composite.children.Remove(child);
        }
    }

    public List<BTNode> GetChildren(BTNode parent)
    {
        List<BTNode> children = new List<BTNode>();

        //check if the parent node is the root
        RootNode root = parent as RootNode;
        if (root && root.child != null)
        {
            children.Add(root.child);
        }

        //check if parent node is a decorator node
        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator && decorator.child != null)
        {
            children.Add(decorator.child);
        }

        //check if parent node is a composite node
        CompositeNode composite = parent as CompositeNode;
        if (composite)
        {
            return composite.children;
        }

        return children;
    }

    public void Traverse(BTNode node, System.Action<BTNode> visitor)
    {
        if (node)
        {
            visitor.Invoke(node);
            var children = GetChildren(node);
            children.ForEach((n) => Traverse(n, visitor));
        }

    }

    public BehaviourTree Clone()
    {
        BehaviourTree tree = Instantiate(this);
        tree.rootNode = tree.rootNode.Clone();
        tree.nodes = new List<BTNode>();
        Traverse(tree.rootNode, (n) =>
        {
            tree.nodes.Add(n);
        });
        return tree;
    }

    public void Bind(AIController controller)
    {
        Traverse(rootNode, node => {
            node.controller = controller;
            node.blackboard = blackboard;
        });
    }

}
