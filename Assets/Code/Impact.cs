using UnityEngine;

using WhalePark18.MemoryPool;

public class Impact : MonoBehaviour
{
    private ParticleSystem particle;
    private MemoryPool memoryPool;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    public void Setup(MemoryPool pool)
    {
        memoryPool = pool;
    }

    private void Update()
    {
        /// ��ƼŬ�� ������� �ƴϸ� ��Ȱ��ȭ
        if (particle.isPlaying == false)
        {
            memoryPool.DeactivePoolItem(gameObject);
        }
    }
}