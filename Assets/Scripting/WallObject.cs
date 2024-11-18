using UnityEngine;
using UnityEngine.Tilemaps;

public class WallObject : CellObject
{
    public Tile[] ObstacleTile;  // Tile array ที่กำหนดตามสถานะ HP (3 HP -> 0 HP)
    public int MaxHealth = 3;    // พลังชีวิตสูงสุดของกำแพง

    private int m_HealthPoint;   // พลังชีวิตปัจจุบัน
    private Tile m_OriginalTile; // Tile ดั้งเดิมก่อนเปลี่ยน

    public override void Init(Vector2Int cell)
    {
        base.Init(cell);

        if (ObstacleTile == null || ObstacleTile.Length < MaxHealth)
        {
            Debug.LogError("ObstacleTile array is not properly set or is smaller than MaxHealth!");
            return;
        }

        // สุ่มกำหนด HP ให้กำแพงเป็น 1 หรือ 3
        m_HealthPoint = Random.Range(1, 4); // สุ่มค่า 1 หรือ 3

        // กำหนด Tile ดั้งเดิม
        m_OriginalTile = GameManager.Instance.BoardManager.GetCellTile(cell);

        // ตั้ง Tile ของกำแพงเริ่มต้นตาม MaxHealth
        UpdateWallTile();
    }

    public override bool PlayerWantsToEnter()
    {
        // ลด HP
        m_HealthPoint--;

        if (m_HealthPoint > 0)
        {
            // อัปเดต Tile ตาม HP
            UpdateWallTile();
            return false; // ยังไม่สามารถเข้าได้
        }

        // ถ้า HP เหลือ 0 ลบกำแพง
        GameManager.Instance.BoardManager.SetCellTile(m_Cell, m_OriginalTile);
        Destroy(gameObject);
        return true; // อนุญาตให้ผู้เล่นเข้าได้
    }

    private void UpdateWallTile()
    {
        // เปลี่ยน Tile ตาม HP ที่เหลือ (เช่น ObstacleTile[2] สำหรับ HP = 1)
        if (m_HealthPoint > 0 && m_HealthPoint <= ObstacleTile.Length)
        {
            Tile currentTile = ObstacleTile[m_HealthPoint - 1];
            GameManager.Instance.BoardManager.SetCellTile(m_Cell, currentTile);
        }
    }
}
