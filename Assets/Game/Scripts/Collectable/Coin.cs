using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Coin : MonoBehaviour
{
    [SerializeField] private float minflyTDuration = 2.0f;
    [SerializeField] private float maxflyTDuration = 3.0f;
    [SerializeField] GameEvent gameEvent;

    public void StartMagnet()
    {
        transform.DOKill();
        StartCoroutine(MagnetCoroutine());
    }



    public void Stop()
    {
        transform.DOKill();
        StopAllCoroutines();
    }
    public void Play(Vector3 targetPos)
    {
        float jumpPower = 2.0f; 
        float duration = 0.6f;  


        transform.DOKill();
        transform.localScale = Vector3.zero; 

        transform.DOScale(Vector3.one, 0.2f);

        transform.DORotate(new Vector3(0, 360, 0), 0.8f, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.Linear);

        Sequence s = DOTween.Sequence();

        s.Append(transform.DOJump(
            targetPos,
            jumpPower,
            1,
            duration
        ).SetEase(Ease.OutQuad));

        s.Append(transform.DOScaleY(0.8f, 0.1f)); 
        s.Append(transform.DOScaleY(1f, 0.1f)); 

        s.OnComplete(() =>
        {
            transform.DOMoveY(targetPos.y + 0.2f, 0.6f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
            StartMagnet();
        });
        
    }

    private IEnumerator MagnetCoroutine()
    {
        float timer = 0.0f;
        Vector3 startPos = transform.position;
        float flyDuration = UnityEngine.Random.Range(minflyTDuration, maxflyTDuration); 
        while (timer <= flyDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / flyDuration;

            float distance = Vector3.Distance(Camera.main.transform.position, transform.position);
            Vector3 targetPos = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, distance));
            transform.position = Vector3.Lerp(startPos, targetPos, progress);
            yield return null;
        }
        CoinsManager.Instance.Release(this);
        gameEvent.Raise();
    }
}
