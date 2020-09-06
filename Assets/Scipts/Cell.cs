using UnityEngine;

public class Cell : MonoBehaviour
{
	[SerializeField] private MeshRenderer meshRenderer;

	private void Start()
	{
		SetRandomColor();
	}

	// Bu metod değiştirilecek, şimdilik hareket belli olsun diye hücreler random renklendiriliyor
	private void SetRandomColor()
	{
		meshRenderer.material.color = Random.ColorHSV(0f, 1f, 0.8f, 1f, 0.6f, 1f);
	}
}
