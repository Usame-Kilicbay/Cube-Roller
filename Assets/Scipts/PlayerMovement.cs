using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private Bounds bounds;

    [Header("Scripts")]
    [SerializeField] private GridGenerator gridGenerator;

    [Range(0f, 1f)]
    [SerializeField] private float movementSpeed;
    [Range(0f, 10)]
    [SerializeField] private float rotationSpeed;

    [SerializeField] private int gridX;
    [SerializeField] private int gridZ;

    [SerializeField] private Vector3 forward;
    [SerializeField] private Vector3 back;
    [SerializeField] private Vector3 left;
    [SerializeField] private Vector3 right;

    float startPosY;

    public bool canRoll;

	private void Start()
	{
        SetSpawnPos();

        CalculateBounds();
	}

    // Min-Max değerler arası spawn pozisyonu oluşturuluyor, yükseklik ise küpün yarısı + üzerinde durduğu hücrenin yüksekliği olarak belirleniyor
	private void SetSpawnPos()
	{
        gridZ = Random.Range(0, gridGenerator.gridDepth);
        gridX = Random.Range(0, gridGenerator.gridWidth);

        startPosY = transform.localScale.y / 2 + gridGenerator.cellHeight;
        
        Vector3 spawnPos = gridGenerator.cells[gridZ][gridX];
        spawnPos.y = startPosY;

        transform.position = spawnPos;
    }

    // Collier sınırlarına göre dönüş açısının origini hesaplanıyor
    private void CalculateBounds()
    {
        bounds = boxCollider.bounds;

		forward = new Vector3(0, -bounds.size.y / 2, bounds.size.z / 2);
		back = new Vector3(0, -bounds.size.y / 2, -bounds.size.z / 2);
		left = new Vector3(-bounds.size.x / 2, -bounds.size.y / 2, 0);
		right = new Vector3(bounds.size.x / 2, -bounds.size.y / 2, 0);
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            MoveForward();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            MoveBack();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            MoveLeft();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            MoveRight();
        }

		//Move();
	}

    // Metodlarda belirtiler grid değerleri maksimum değere veya minimum değere ulaşırsa metoddan çıkılıyor
    private void MoveForward()
    {
        if (gridZ == gridGenerator.gridDepth - 1 || !canRoll)
        {
            return;
        }

		StartCoroutine(Roll(forward));

		gridZ++;
    }

    private void MoveBack()
    {
        if (gridZ == 0 || !canRoll)
        {
            return;
        }

		StartCoroutine(Roll(back));

		gridZ--;
    }

    private void MoveLeft()
    {
        if (gridX == 0 || !canRoll)
        {
            return;
        }

		StartCoroutine(Roll(left));

		gridX--;
    }

    private void MoveRight()
    {
        if (gridX == gridGenerator.gridWidth - 1 || !canRoll)
        {
            return;
        }

		StartCoroutine(Roll(right));

		gridX++;
    }


    // Bu metod Roll metodu varken stabil çalışmıyor. Roll işlemişden vazgeçilirse koordinat sistemine göre stabil ve doğru bir hareket sağlanabilir.
    private void Move()
    {
        Vector3 targetPos = gridGenerator.cells[gridZ][gridX];
        targetPos.y = transform.localScale.y / 2;

        //transform.rotation = Quaternion.Euler(Vector3.Lerp(transform.rotation, Vector3.up, rotationSpeed));

        transform.position = Vector3.Lerp(transform.position, targetPos, movementSpeed);
    }

    private IEnumerator Roll(Vector3 positionToRotation) 
    {
        canRoll = false;

		Vector3 point = transform.position + positionToRotation;
		Vector3 axis = Vector3.Cross(Vector3.up, positionToRotation).normalized;

		float angle = 0;

        // 90 derece dönene kadar numerator sayesinde döngü bitmiyor
		while (angle < 90f)
		{
			float angleSpeed = Time.deltaTime + rotationSpeed;

			transform.RotateAround(point, axis, angleSpeed);
			angle += angleSpeed;

			yield return null;
        }

		transform.RotateAround(point, axis, 90 - angle);

        // Olası Float sapmalarını engellemek için iki boyutlu dizideki karşılığı olan hücrenin pozisyonuna göre ayarlanıyor
        Vector3 snapPos = gridGenerator.cells[gridZ][gridX];
        snapPos.y = startPosY;
        transform.position = snapPos;

        canRoll = true;
	}
}
