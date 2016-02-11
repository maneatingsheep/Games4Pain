using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Holoville.HOTween;

public class TestTRansition : MonoBehaviour {


    public Image StarPF;
    public Image Spaceship;

    private int _steps;

    private List<List<Image>> _stars = new List<List<Image>>();

    private int _currentCol;

    public Sprite MarkedStarSprite;

    private Tweener ShipTweener;

    public void SetMap(int steps) {
        _steps = steps;

        for (int i = 0; i < _stars.Count; i++) {
            for (int j = 0; j < _stars[i].Count; j++) {
                Destroy(_stars[i][j].gameObject);
            }
        }

        _stars.Clear();

        float maxX = 750;
        float maxY = 600;

        

        for (int i = 0; i < _steps; i++) {
            _stars.Add(new List<Image>());

            int starsInrow = (i == 0)?1:(Random.Range(2, 4));
            
            float StepXDistance = maxX / (_steps + 1) ;

            float StepYDistance = maxY / starsInrow;


            

            for (int j = 0; j < starsInrow; j++) {
                _stars[i].Add(Instantiate(StarPF));
                _stars[i][j].transform.SetParent(transform);
                _stars[i][j].transform.localScale = new Vector3(1, 1, 1);

                _stars[i][j].rectTransform.localPosition = new Vector2(StepXDistance * i + StepXDistance / 2, StepYDistance * j + StepYDistance / 2);

                _stars[i][j].rectTransform.localPosition += new Vector3(Random.Range(-20f, 20f), Random.Range(-20f, 20f), 0);
            }

            Spaceship.transform.localPosition = new Vector2(-100, 400);
        }

        _currentCol = -1;
    }

    public void ProgressToNextStar(bool doMark) {

        _currentCol++;

        int targetStar = Random.Range(0, _stars[_currentCol].Count);

        Vector3 offset = new Vector3(0, 50, 0);

        ShipTweener = HOTween.To(Spaceship.transform, 2.5f, new TweenParms().Prop("localPosition", _stars[_currentCol][targetStar].transform.localPosition + offset).Ease(EaseType.Linear).Delay(2));

        if (doMark) {
            _stars[_currentCol][targetStar].sprite = MarkedStarSprite;
        }
    }

    public void Pause() {
        if (ShipTweener != null) ShipTweener.Pause();
    }

    public void Resume() {
        if (ShipTweener != null && !ShipTweener.isComplete) ShipTweener.Play();
    }
}
