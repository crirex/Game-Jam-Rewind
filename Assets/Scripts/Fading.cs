using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fading : MonoBehaviour
{
	[SerializeField]
	Material dizolveMaterial;

	public bool isDissolving = false;
	bool isAlreadyDissolving = false;
	float fade = 1f;

	void Start()
	{

	}

	void Update()
	{
		if(transform.position.x < GeneralAttributes.Instance.safePositionMax.x &&
			transform.position.y < GeneralAttributes.Instance.safePositionMax.y &&
			GeneralAttributes.Instance.safePositionMin.x < transform.position.x &&
			GeneralAttributes.Instance.safePositionMin.y < transform.position.y && !isAlreadyDissolving)
		{
			isDissolving = false;
		}
		if (isDissolving)
		{
			isAlreadyDissolving = true;
			fade -= Time.deltaTime/5;
			if (GetComponent<SpriteRenderer>() != null)
			{
				GetComponent<SpriteRenderer>().material = dizolveMaterial;
				GetComponent<SpriteRenderer>().material.SetFloat("_Fade", fade);
			}

			if (fade <= 0f)
			{
				fade = 0f;
				Destroy(this.gameObject);
			}
		}
	}
}