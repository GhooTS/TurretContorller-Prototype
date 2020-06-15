using Nav2D;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace Nav2D.TestUtility
{
    public class TileFillTool : MonoBehaviour
    {
        public NavGrid gridMap;
        public Tilemap tileMap;
        public Tile tile;

        private Vector3Int lastPosition = Vector3Int.one;

        private void Update()
        {
            if (Mouse.current.rightButton.isPressed)
            {
                if (gridMap != null && tileMap != null && tile != null)
                {
                    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                    var tileMapIndex = Vector3Int.FloorToInt(mousePosition);
                    var gridMapIndex = gridMap.FromPositionToIndex(mousePosition);

                    if (tileMapIndex == lastPosition) return;

                    if (gridMap.IsValideIndex(gridMapIndex.x, gridMapIndex.y) == false) return;

                    //Update TileMap
                    var insertTile = !tileMap.HasTile(tileMapIndex);
                    tileMap.SetTile(tileMapIndex, insertTile ? tile : null);

                    //Update NavGrid
                    gridMap.SetNodeType(gridMapIndex.x, gridMapIndex.y, insertTile ? Node.NodeType.wall : Node.NodeType.free);

                    lastPosition = tileMapIndex;
                }
            }

            if (Mouse.current.rightButton.wasReleasedThisFrame)
            {
                lastPosition = Vector3Int.one;
            }
        }
    }
}