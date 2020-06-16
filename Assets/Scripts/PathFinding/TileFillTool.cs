using Nav2D;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace Nav2D.TestUtility
{
    public class TileFillTool : MonoBehaviour
    {
        public NavGrid navGrid;
        public Tilemap tileMap;
        public Tile tile;

        private Vector3Int lastPosition = Vector3Int.one;

        private void Update()
        {
            if (Mouse.current.rightButton.isPressed)
            {
                if (navGrid != null && tileMap != null && tile != null)
                {
                    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                    var tileMapIndex = tileMap.WorldToCell(mousePosition);
                    var gridMapIndex = navGrid.PositionToIndex(tileMap.CellToWorld(tileMapIndex));

                    if (tileMapIndex == lastPosition) return;

                    if (navGrid.IsValideIndex(gridMapIndex.x, gridMapIndex.y) == false) return;

                    //Update TileMap
                    var insertTile = !tileMap.HasTile(tileMapIndex);
                    tileMap.SetTile(tileMapIndex, insertTile ? tile : null);

                    //Update NavGrid

                    var nodeType = insertTile ? NavGrid.NodeType.wall : NavGrid.NodeType.free;

                    

                    if(navGrid.CellSize < tileMap.cellSize.x || navGrid.CellSize < tileMap.cellSize.y)
                    {
                        int width = (int)(tileMap.cellSize.x / navGrid.CellSize);
                        int height = (int)(tileMap.cellSize.y / navGrid.CellSize);
                        
                        for (int x = 0; x < width; x++)
                        {
                            for (int y = 0; y < height; y++)
                            {
                                navGrid.SetNode(gridMapIndex.x + x, gridMapIndex.y + y, nodeType);
                            }
                        }
                    }
                    else
                    {
                        navGrid.SetNode(gridMapIndex.x, gridMapIndex.y, nodeType);
                    }

                    if (navGrid.CellSize < tileMap.cellSize.y)
                    {
                        
                    }


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