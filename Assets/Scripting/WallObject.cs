using UnityEngine;
using UnityEngine.Tilemaps;

public class WallObject : CellObject
{
    public Tile[] ObstacleTile;  // Tile array ����˹����ʶҹ� HP (3 HP -> 0 HP)
    public int MaxHealth = 3;    // ��ѧ���Ե�٧�ش�ͧ��ᾧ

    private int m_HealthPoint;   // ��ѧ���Ե�Ѩ�غѹ
    private Tile m_OriginalTile; // Tile ��������͹����¹

    public override void Init(Vector2Int cell)
    {
        base.Init(cell);

        if (ObstacleTile == null || ObstacleTile.Length < MaxHealth)
        {
            Debug.LogError("ObstacleTile array is not properly set or is smaller than MaxHealth!");
            return;
        }

        // ������˹� HP ����ᾧ�� 1 ���� 3
        m_HealthPoint = Random.Range(1, 4); // ������� 1 ���� 3

        // ��˹� Tile ������
        m_OriginalTile = GameManager.Instance.BoardManager.GetCellTile(cell);

        // ��� Tile �ͧ��ᾧ������鹵�� MaxHealth
        UpdateWallTile();
    }

    public override bool PlayerWantsToEnter()
    {
        // Ŵ HP
        m_HealthPoint--;

        if (m_HealthPoint > 0)
        {
            // �ѻവ Tile ��� HP
            UpdateWallTile();
            return false; // �ѧ�������ö�����
        }

        // ��� HP ����� 0 ź��ᾧ
        GameManager.Instance.BoardManager.SetCellTile(m_Cell, m_OriginalTile);
        Destroy(gameObject);
        return true; // ͹حҵ�������������
    }

    private void UpdateWallTile()
    {
        // ����¹ Tile ��� HP �������� (�� ObstacleTile[2] ����Ѻ HP = 1)
        if (m_HealthPoint > 0 && m_HealthPoint <= ObstacleTile.Length)
        {
            Tile currentTile = ObstacleTile[m_HealthPoint - 1];
            GameManager.Instance.BoardManager.SetCellTile(m_Cell, currentTile);
        }
    }
}
