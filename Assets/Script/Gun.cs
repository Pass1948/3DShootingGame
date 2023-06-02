using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] float maxDistance;
    [SerializeField] float bulletSpeed;
    [SerializeField] int damage;
   // [SerializeField] ParticleSystem hitEffect; //�ð�ȭ
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
            effect.transform.parent = hit.transform;        // ����Ʈ�� ������Ʈ�� ���� �ٴϰ� ����Ʈ�� �ش� ������Ʈ�� �����ڽ����� ������
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

        // ������ƮǮ���� �������� ����Ȳ�� ���Ѱ� �ʱ���·� ������ �ϱ�� clear�� �����
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

        
        // ������ƮǮ�� ����ҽ� ���ǻ���
        // �ν��Ͻ� ��ü���� ���°� �ƴ� ��Ȱ��ȭ�� ���·� �����ϱ⿡ ==null�̶�� ���ǹ����� ��������
        // ���� üũ�� �ؾߵ� ���trail.gameObject.activeSelf == false�� ���� ���ӿ�����Ʈ�� Ȱ��ȭ ��츦 üũ�ؼ� ����ؾ��Ѵ�
        /*
        yield return null;
        if (!trail.IsValid())    
        {
            Debug.Log("trail�� ����");
        }
        else
        {
            Debug.Log("trail�� ����");
        }
        */
    }
}
