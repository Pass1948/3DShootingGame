using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] float maxDistance;
    [SerializeField] float bulletSpeed;
    [SerializeField] int damage;
   // [SerializeField] ParticleSystem hitEffect; //시각화
    [SerializeField] ParticleSystem muzzleEffect;
    [SerializeField] TrailRenderer bulletTrail;

    private void Awake()
    {
        //hitEffect = GameManager.Resource.Load<ParticleSystem>("Prefabs/HitEffect");
    }

    public void Fire() 
    {
        muzzleEffect.Play();

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance))
        {
            IHittable hittable = hit.transform.GetComponent<IHittable>();
            //ParticleSystem effect = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            ParticleSystem effect = GameManager.Resource.Instantiate<ParticleSystem>("Prefabs/HitEffect", hit.point, Quaternion.LookRotation(hit.normal), true);
            effect.transform.parent = hit.transform;        // 이펙트가 오브젝트를 따라 다니게 이펙트를 해당 오브젝트의 하위자식으로 설정함
           // Destroy(effect.gameObject, 3f);

            StartCoroutine(ReleaseRoutine(effect.gameObject));
            StartCoroutine(TrailRoutinue(muzzleEffect.transform.position, hit.point));

            hittable?.Hit(hit, damage);
        }
        else
        {
            StartCoroutine(TrailRoutinue(muzzleEffect.transform.position, Camera.main.transform.forward * maxDistance));
        }
    }

    IEnumerator ReleaseRoutine(GameObject effect)
    {
        yield return new WaitForSeconds(3f);
        GameManager.Resource.Destroy(effect);
    }

   IEnumerator TrailRoutinue(Vector3 startPoint, Vector3 endPoint)
    {
        // TrailRenderer trail = Instantiate(bulletTrail, muzzleEffect.transform.position, Quaternion.identity);
        TrailRenderer trail = GameManager.Resource.Instantiate(bulletTrail, startPoint, Quaternion.identity,true);

        // 오브젝트풀에서 꺼내기전 전상황에 대한걸 초기상태로 돌려야 하기애 clear를 사용함
        trail.Clear();

        float totalTime = Vector2.Distance(startPoint, endPoint) / bulletSpeed;
        float rate = 0;

        while (rate < 1)
        {
            trail.transform.position = Vector3.Lerp(startPoint, endPoint, rate);
            rate += Time.deltaTime / totalTime;
            yield return null;
        }

        //Destroy(trail);
        GameManager.Resource.Destroy(trail.gameObject);

        
        // 오브젝트풀을 사용할시 주의사항
        // 인스턴스 객체들이 없는게 아닌 비활성화된 상태로 존재하기에 ==null이라는 조건문만은 쓸수없다
        // 만약 체크를 해야될 경우trail.gameObject.activeSelf == false와 같이 게임오브젝트의 활성화 경우를 체크해서 사용해야한다
        /*
        yield return null;
        if (!trail.IsValid())    
        {
            Debug.Log("trail이 없음");
        }
        else
        {
            Debug.Log("trail이 있음");
        }
        */
    }
}
