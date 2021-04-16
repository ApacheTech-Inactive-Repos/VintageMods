using System.Collections.Generic;

namespace VintageMods.CinematicCamStudio.Camera.Pathfinding
{
    internal class Path
    {
        private List<CamNode> _nodes;

        public IReadOnlyList<CamNode> Nodes => _nodes.AsReadOnly();

        public Path()
        {
            _nodes = new List<CamNode>();
        }

        internal void Add(CamNode node)
        {
            _nodes.Add(node);
        }

        internal void AddAfter(CamNode node, int? index = null)
        {
            if (index is null)
            {
                _nodes.Add(node);
            }
            else
            {
                _nodes.Insert(index.Value + 1, node);
            }
        }

        internal void AddBefore(CamNode node, int? index = null)
        {
            _nodes.Insert(index ?? 0, node);
        }

        internal void Update(CamNode node, int? index = null)
        {
            _nodes[index ?? _nodes.Count] = node;
        }

        internal void Remove(CamNode node, int? index = null)
        {
            _nodes[index ?? _nodes.Count - 1] = node;
        }

        internal void Clear(CamNode node, int? index = null)
        {
            _nodes.Clear();
        }
    }
}
