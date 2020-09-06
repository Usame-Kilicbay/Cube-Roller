using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
	[SerializeField] private GameObject cellPrefab;

	[Header("Grid Limits")]
	[Range(0, 10)]
	public int gridWidth;
	[Range(0, 10)]
	public int gridDepth;

	[Header("Cell Size")]
	public float cellWidth;
	public float cellHeight;
	public float cellDepth;


	public List<List<Vector3>> cells = new List<List<Vector3>>();

	private void Start()
	{
		CalculateCellSize();
		GenerateGrid();
	}

	private void CalculateCellSize()
	{
		cellWidth = cellPrefab.transform.localScale.x;
		cellHeight = cellPrefab.transform.localScale.y;
		cellDepth = cellPrefab.transform.localScale.z;
	}

	private void GenerateGrid()
	{
		for (int row = 0; row < gridDepth; row++)
		{
			List<Vector3> cellPositions = new List<Vector3>();
			cells.Add(cellPositions);
			
			for (int column = 0; column < gridWidth; column++)
			{
				GameObject newCell = Instantiate(cellPrefab, transform);

				// Origin 0 olmasını istemediğim için (sağ üst +x +y koordinatlarında ilerlemesin diye) 
				// grid boyutuna göre hizalama yaparak - değerlerden + değerelere doğru 0 noktasının iki tarafına eşit şekilde yayılıyor
				newCell.transform.localPosition = new Vector3(column - PosX(), cellHeight / 2, row - PosY());

				// 2 boyutu bu diziye koordinat sistemine uyacak şekilde (0,1 veya 6,4 gibi) index sayesinde pozisyonlar atanıyor bu sayede küp rotasyon olmasa bile kare kare ilerleyebiliyor
				cellPositions.Add(newCell.transform.position);
			}
		}
	}

	// Satır ve sütun değerleri çift ise direk yarısını değil ise yarısı - hücre boyutunun yarısı olarak hesaplanıp geri döndürülüyor
	private float PosX()
	{
		if (gridWidth % 2 == 1)
		{
			return gridWidth / 2;
		}
		else
		{
			return gridWidth / 2 - (cellWidth / 2);
		}
	}

	private float PosY()
	{
		if (gridDepth % 2 == 1)
		{
			return gridDepth / 2;
		}
		else
		{
			Debug.LogError(gridDepth / 2 + (cellDepth));
			return gridDepth / 2 - (cellDepth / 2);
		}
	}
}
