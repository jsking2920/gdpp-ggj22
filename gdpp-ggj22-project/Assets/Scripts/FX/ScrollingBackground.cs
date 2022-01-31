using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Based on tutorial by Brackeys: https://www.youtube.com/watch?v=CwGjwnjmg2w&t=2s
public class ScrollingBackground : MonoBehaviour
{
	[SerializeField] private float speed = -0.1f;

	public bool hasARightClone = false;
	private float spriteWidth = 0.0f;

	[SerializeField] private Camera cam;

	void Start()
	{
		spriteWidth = GetComponent<SpriteRenderer>().sprite.bounds.size.x * transform.localScale.x;
	}

	void Update()
	{
		transform.position += new Vector3(speed * Time.deltaTime, 0, 0);

		if (!hasARightClone)
		{
			float camHorizontalExtend = cam.orthographicSize * Screen.width / Screen.height;

			float edgeVisiblePositionRight = (transform.position.x + spriteWidth / 2) - camHorizontalExtend;
			float edgeVisiblePositionLeft = (transform.position.x - spriteWidth / 2) + camHorizontalExtend;

			if (cam.transform.position.x >= edgeVisiblePositionRight && !hasARightClone)
			{
				MakeNewClone();
				hasARightClone = true;
			}
		}
		else if (transform.position.x <= -20f)
        {
			Destroy(gameObject);
        }
	}

	void MakeNewClone()
	{
		Vector3 newPosition = new Vector3(transform.position.x + spriteWidth, transform.position.y, transform.position.z);
		Transform newClone = Instantiate(transform, newPosition, transform.rotation);
		newClone.parent = transform.parent;
	}
}
