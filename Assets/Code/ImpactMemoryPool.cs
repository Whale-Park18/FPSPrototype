using UnityEngine;

/// Normal: 벽, 바닥 등
/// Obstacle: 장애물
public enum ImpactType {  Normal = 0, Obstacle, Enemy, InteractionObject, }

public class ImpactMemoryPool : MonoBehaviour
{
    [SerializeField]
    private GameObject[] impactPrefab;  // 피격 이펙트
    private MemoryPool[] memoryPool;    // 피격 이펙트 메모리풀

    private void Awake()
    {
        /// 피격 이벡트가 여러 종류이면 종류별로 memoryPool 생성
        memoryPool = new MemoryPool[impactPrefab.Length];
        for(int i = 0; i < impactPrefab.Length; i++)
        {
            memoryPool[i] = new MemoryPool(impactPrefab[i]);
        }
    }

    /// <summary>
    /// 광선 이용시 이팩트 스폰 메소드
    /// </summary>
    /// <param name="hit"></param>
    public void SpawnImpact(RaycastHit hit)
    {
        /// 부딪힌 오브젝트의 Tag 정보에 따라 다르게 처리
        if(hit.transform.CompareTag("ImpactNormal"))
        {
            OnSpawnImpact(ImpactType.Normal, hit.point, Quaternion.LookRotation(hit.normal));
        }
        else if(hit.transform.CompareTag("ImpactObstacle"))
        {
            OnSpawnImpact(ImpactType.Obstacle, hit.point, Quaternion.LookRotation(hit.normal));
        }
        else if(hit.transform.CompareTag("ImpactEnemy"))
        {
            OnSpawnImpact(ImpactType.Enemy, hit.point, Quaternion.LookRotation(hit.normal));
        }
        else if(hit.transform.CompareTag("InteractionObject"))
        {
            print(hit.transform.GetComponent<MeshRenderer>());
            print(hit.transform.GetComponentInChildren<MeshRenderer>());

            //MeshRenderer renderer = hit.transform.GetComponent<MeshRenderer>();
            //if (renderer == null) hit.transform.GetComponentInChildren<MeshRenderer>();

            //Color color = hit.transform.GetComponent<MeshRenderer>().material.color;
            Color color = hit.transform.GetComponentInChildren<MeshRenderer>().material.color;

            OnSpawnImpact(ImpactType.InteractionObject, hit.point, Quaternion.LookRotation(hit.normal), color);
        }
    }

    /// <summary>
    /// 충돌체 이용시 이펙스 스폰 메소드
    /// </summary>
    /// <param name="other"></param>
    /// <param name="knifeTransform"></param>
    public void SpawnImpact(Collider other, Transform knifeTransform)
    {
        /// 부딪힌 오브젝트의 Tag 정보에 따라 다르게 처리
        if (other.CompareTag("ImpactNormal"))
        {
            OnSpawnImpact(ImpactType.Normal, knifeTransform.position, Quaternion.Inverse(knifeTransform.rotation));
        }
        else if (other.CompareTag("ImpactObstacle"))
        {
            OnSpawnImpact(ImpactType.Obstacle, knifeTransform.position, Quaternion.Inverse(knifeTransform.rotation));
        }
        else if (other.CompareTag("ImpactEnemy"))
        {
            OnSpawnImpact(ImpactType.Enemy, knifeTransform.position, Quaternion.Inverse(knifeTransform.rotation));
        }
        else if (other.CompareTag("InteractionObject"))
        {
            Color color = other.transform.GetComponentInChildren<MeshRenderer>().material.color;
            OnSpawnImpact(ImpactType.InteractionObject, knifeTransform.position, Quaternion.Inverse(knifeTransform.rotation));
        }
    }

    public void OnSpawnImpact(ImpactType type, Vector3 position, Quaternion rotation, Color color = new Color())
    {
        GameObject item = memoryPool[(int)type].ActivePoolItem();
        item.transform.position = position;
        item.transform.rotation = rotation;
        item.GetComponent<Impact>().Setup(memoryPool[(int)type]);

        if(type == ImpactType.InteractionObject)
        {
            ParticleSystem.MainModule main = item.GetComponent<ParticleSystem>().main;
            main.startColor = color;
        }
    }

}
