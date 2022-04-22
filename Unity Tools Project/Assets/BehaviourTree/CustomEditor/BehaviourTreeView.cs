using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System;
using System.Linq;

public class BehaviourTreeView : GraphView
{
    public Action<NodeView> OnNodeSelected;
    public new class UxmlFactory : UxmlFactory<BehaviourTreeView, GraphView.UxmlTraits> { }
    BehaviourTree tree;
    public BehaviourTreeView()
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/BehaviourTree/CustomEditor/BehaviourTreeEditor.uss");
        styleSheets.Add(styleSheet);

        Undo.undoRedoPerformed += OnUndoRedo;
    }

    private void OnUndoRedo()
    {
        PopulateView(tree);
        AssetDatabase.SaveAssets();
    }

    NodeView FindNodeView(BTNode node)
    {
        return GetNodeByGuid(node.guid) as NodeView;
    }

    internal void PopulateView(BehaviourTree tree)
    {
        this.tree = tree;

        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;

        if(tree.rootNode == null)
        {
            tree.rootNode = tree.CreateNode(typeof(RootNode)) as RootNode;
            EditorUtility.SetDirty(tree);
            AssetDatabase.SaveAssets();
        }

        //creates node view
        tree.nodes.ForEach(n => CreateNodeView(n));
        //create edges
        tree.nodes.ForEach(n =>
        {
            var children = tree.GetChildren(n);
            children.ForEach(c =>
            {
                NodeView parentView = FindNodeView(n);
                NodeView childView = FindNodeView(c);

                Edge edge = parentView.outputPort.ConnectTo(childView.inputPort);
                AddElement(edge);
            });
        });

    }

    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        //if any node or connection is removed
        if (graphViewChange.elementsToRemove != null)
        {
            //loop through all the elements being removed
            graphViewChange.elementsToRemove.ForEach(elem =>
            {
                //check if node is being removed
                NodeView nodeView = elem as NodeView;
                if (nodeView != null)
                {
                    RootNode root = nodeView.node as RootNode;
                    if(root == null)
                    {
                        tree.DeleteNode(nodeView.node);
                    }
                }

                //check if edge is being removed
                Edge edge = elem as Edge;
                if(edge != null)
                {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;
                    tree.RemoveChild(parentView.node, childView.node);
                    //set the child node index to 0 as it wont be connected to the existing tree
                    childView.node.parentIndex = 0;
                    SetNodeIndex(parentView.node);
                    //check to see if the parent has other children
                    List<BTNode> nodeChildren = tree.GetChildren(parentView.node);
                    if (nodeChildren.Count > 0)
                    {
                        //update the index order of the other children
                        for(int i = 0; i < nodeChildren.Count; i++)
                        {
                            NodeView tempChildView = GetNodeByGuid(nodeChildren[i].guid) as NodeView;
                            tempChildView.UpdateNameIndex();
                        }
                    }

                    childView.UpdateNameIndex();
                }
            });
        }
        //create edges/connections between nodes
        if(graphViewChange.edgesToCreate != null)
        {
            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                NodeView parentView = edge.output.node as NodeView;
                NodeView childView = edge.input.node as NodeView;
                //add node as a child
                tree.AddChild(parentView.node, childView.node);
                //update the index of the child node
                SetNodeIndex(parentView.node);
                childView.UpdateNameIndex();
            });
        }
        //if any node is moved on the graph
        if(graphViewChange.movedElements != null)
        {
            //go through each node
            nodes.ForEach((n) =>
            {
                //current node on for each is the parent
                NodeView parentView = n as NodeView;
                //sort the children of the node
                parentView.SortChildren();
                //update the node index of the parent
                SetNodeIndex(parentView.node);
                //check if the node has children
                List<BTNode> nodeChildren = tree.GetChildren(parentView.node);
                if (nodeChildren.Count > 0)
                {
                    //loop through the children and update the index
                    for (int i = 0; i < nodeChildren.Count; i++)
                    {
                        NodeView childView = GetNodeByGuid(nodeChildren[i].guid) as NodeView;
                        if(childView != null)
                        {
                            childView.UpdateNameIndex();
                        }
                        
                    }
                }

            });
        }
        return graphViewChange;
    }



    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        //base.BuildContextualMenu(evt);
        Vector2 nodePosition = this.ChangeCoordinatesTo(contentViewContainer, evt.localMousePosition);

        {
            var types = TypeCache.GetTypesDerivedFrom<ActionNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[Action]/{type.Name}", (a) => CreateNode(type, nodePosition));
            }
        }
        {
            var types = TypeCache.GetTypesDerivedFrom<CompositeNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[Composite]/{type.Name}", (a) => CreateNode(type, nodePosition));
            }
        }
        {
            var types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[Decorator]/{type.Name}", (a) => CreateNode(type, nodePosition));
            }
        }

    }

    //set the index of the node, which will display as the run order on the behaviour tree
    public void SetNodeIndex(BTNode node)
    {
        //check if node is the root
        RootNode root = node as RootNode;
        if (root && root.child != null)
        {
            //root node can only have one child
            root.child.parentIndex = 1;
            //early out 
            return;
        }

        //check if node is a decorator
        DecoratorNode decorator = node as DecoratorNode;
        if (decorator && decorator.child != null)
        {
            //decorator node can only have one child
            decorator.child.parentIndex = 1;
            //early out
            return;
        }

        //check if node is composite
        CompositeNode composite = node as CompositeNode;
        if (composite && composite.children != null)
        {
            //composite nodes can have multiple children
            //loop through children array and set index
            for (int i = 0; i < composite.children.Count; i++)
            {
                composite.children[i].parentIndex = i + 1;
            }
            //early out 
            return;
        }

    }

    void CreateNode(System.Type type, Vector2 position)
    {
        BTNode node = tree.CreateNode(type);
        node.position = position;
        SetNodeIndex(node);
        CreateNodeView(node);
    }

    void CreateNodeView(BTNode node)
    {
        NodeView nodeView = new NodeView(node);
        nodeView.OnNodeSelected = OnNodeSelected;
        AddElement(nodeView);
    }

    public void UpdateNodeStates()
    {
        nodes.ForEach(n => {
            NodeView view = n as NodeView;
            view.UpdateState();
        });
    }
}
