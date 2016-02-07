using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TestProgress : MonoBehaviour {

    public static TestProgress Instance;

    public Image CirclePrefab;

    public Sprite[] States;

    private List<Image> _images = new List<Image>();

    void Awake() {
        Instance = this;
    }

    public void SetNumOfQuestions(int tests) {
        while (_images.Count > 0) {
            Destroy(_images[0].gameObject);
            _images.RemoveAt(0);
        }

        while (_images.Count < tests) {
            _images.Add(Instantiate(CirclePrefab));
            _images[_images.Count - 1].transform.SetParent(transform);
            _images[_images.Count - 1].transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void SetQuestionState(int testIndex, int state) {
        _images[testIndex].sprite = States[state];
    }
}
