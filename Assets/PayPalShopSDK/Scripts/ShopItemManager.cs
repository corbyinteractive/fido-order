using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemManager : MonoBehaviour {

	public List<GameObject> items;
	public List<Vector3> positions;
	public int maxVisibleItems = 5;
	private int currentIndex = 0;

	private void Start()
	{
		foreach (Transform child in transform)
		{
			items.Add(child.gameObject);
			positions.Add(child.GetComponent<RectTransform>().position);
		}
		if(items.Count > maxVisibleItems)
		{
			int i = maxVisibleItems;
			foreach(GameObject item in items)
			{
				if (i < (items.Count))
					items[i].SetActive(false);
				i++;
			}
		}
	}

	public void OnLeft()
	{
		if (currentIndex > 0)
		{
			currentIndex--;
			items[currentIndex].SetActive(true);
			items[currentIndex + 5].SetActive(false);
			int i = 1;
			foreach(GameObject item in items)
			{
				if (item.activeInHierarchy)
				{
					item.GetComponent<RectTransform>().position = positions[i - 1];
					i++;
				}
			}
		}
	}

	public void OnRight()
	{
		if (currentIndex + 5 < items.Count)
		{
			items[currentIndex].SetActive(false);
			items[currentIndex + 5].SetActive(true);
			currentIndex++;
			int i = 1;
			foreach (GameObject item in items)
			{
				if (item.activeInHierarchy)
				{
					item.GetComponent<RectTransform>().position = positions[i - 1];
					i++;
				}
			}
		}
	}
}
