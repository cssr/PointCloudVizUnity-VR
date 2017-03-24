using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class VertexParticles : MonoBehaviour
{

    [SerializeField]
    private SkinnedMeshRenderer skinnedMesh;
    [SerializeField]
    private MeshFilter meshFilter;
    [SerializeField]
    private Mesh mesh;
    [SerializeField]
    private Transform ObjectTransform;

    private ParticleSystem particleSystemComp;
    private ParticleSystem.Particle[] particlesCreated;

    [SerializeField]
    private int m_maxParticles = 1000;
    private int particlesPerSpawn = 200;
    [SerializeField]
    private int spawnCount = 5;
    private int particlesSetIndex = 0;
    [SerializeField]
    private float maxLifetime = 2f;
    [SerializeField]
    private float minLifetime = 0f;
    [SerializeField]
    private float lifetimeDelta = 0.1f;

    [SerializeField]
    private float spawnDistance = 5;
    private Vector3 lastPosition = Vector3.zero;

    private float timeDelta = 1f;
    private float lastSpawnTime = 1;

    [SerializeField]
    private bool spawnLights = true;
    private Light[] lights;
    private float[] lightTimers;
    [SerializeField]
    private Color lightColor;
    private int lightIndex = 0;
    [SerializeField]
    private Vector3 positionOffset;
    [SerializeField]
    private LightRenderMode renderMode;
    [SerializeField]
    private float lightIntensity = 5;

    private void Awake()
    {

        mesh = GetComponent<MeshFilter>().mesh;
        meshFilter = GetComponent<MeshFilter>();
        ObjectTransform = transform;

        /*// use animated mesh
        if (skinnedMesh != null)
        {
            mesh = new Mesh();
        }
        else if (meshFilter != null)
        { // use static mesh
            mesh = meshFilter.mesh;
        }// use static mesh from assets
         */

        particleSystemComp = this.GetComponent<ParticleSystem>();
        particleSystemComp.startLifetime = maxLifetime;
        particleSystemComp.startSize = 0.1f;
        

        ParticleSystem.ShapeModule sh = particleSystemComp.shape;
        sh.enabled = true;
        sh.shapeType = ParticleSystemShapeType.Mesh;
        sh.mesh = mesh;

        particleSystemComp.maxParticles = m_maxParticles;
        particlesPerSpawn = m_maxParticles / spawnCount;

        particlesCreated = new ParticleSystem.Particle[m_maxParticles];

        if (spawnLights)
        {
            lights = new Light[spawnCount - 1];
            lightTimers = new float[spawnCount - 1];
            for (int i = 0; i < lights.Length; i++)
            {
                var _go = new GameObject("Light");
                lights[i] = _go.AddComponent<Light>();
                lights[i].color = lightColor;
                lights[i].range = spawnDistance * 5;
                lights[i].intensity = lightIntensity;
                lights[i].enabled = false;
                lights[i].renderMode = renderMode;
                lights[i].bounceIntensity = 0f;
            }
        }
    }

    private void Update()
    {
        mesh = GetComponent<MeshFilter>().mesh;

        var distance = (ObjectTransform.position - lastPosition).magnitude;

        // update lights fadeout
        if (spawnLights)
        {
            for (int i = 0; i < spawnCount - 1; i++)
            {
                if (lightTimers[i] < Time.time)
                {
                    lights[i].enabled = false;
                }
                else
                {
                    lights[i].intensity = (lightTimers[i] - Time.time) / timeDelta * lightIntensity;
                }
            }
        }

        // check distance threshold
        //if (distance < spawnDistance)
        //{
        //    return;
        //}

        // calculate spawning delta time for particles lifetime
        timeDelta = Mathf.Clamp((Time.time - lastSpawnTime) * spawnCount, minLifetime, maxLifetime);
        lastSpawnTime = Time.time;

        // emit and get particles
        particleSystemComp.Emit(m_maxParticles);
        var currentParticlesCount = particleSystemComp.GetParticles(particlesCreated);

        if (skinnedMesh != null)
        { // bake for animation
            skinnedMesh.BakeMesh(mesh);
        }

        // get vertices and calculate vertex index step
        var vertices = mesh.vertices;
        var vertexStep = vertices.Length / particlesPerSpawn;
        vertexStep = vertexStep == 0 ? 1 : vertexStep;
        var baseIdx = particlesSetIndex * particlesPerSpawn;

        int j = 0;
        float lifetimeStep = 0;
        for (int i = baseIdx; i < baseIdx + particlesPerSpawn; i++)
        {
            particlesCreated[i].remainingLifetime = timeDelta * lifetimeStep;
            particlesCreated[i].position = ObjectTransform.TransformPoint(vertices[j]);

            j += vertexStep;
            if (j >= vertices.Length)
            {
                j = 1;
            }
            lifetimeStep = (lifetimeStep + lifetimeDelta) % 1f;
        }
        // update particles
        particleSystemComp.SetParticles(particlesCreated, m_maxParticles);

        if (spawnLights)
        { // setup light
            lights[lightIndex].transform.position = ObjectTransform.position + positionOffset;
            lights[lightIndex].intensity = lightIntensity;
            lights[lightIndex].enabled = true;
            lightTimers[lightIndex] = Time.time + timeDelta;
            lightIndex = (++lightIndex % lights.Length);
        }

        lastPosition = ObjectTransform.position;
        // move to next set of particles
        particlesSetIndex = (++particlesSetIndex % spawnCount);

        // set particle system shape mesh
        ParticleSystem.ShapeModule sh = particleSystemComp.shape;
        sh.enabled = true;
        sh.shapeType = ParticleSystemShapeType.Mesh;
        sh.mesh = mesh;
    }

    
}
