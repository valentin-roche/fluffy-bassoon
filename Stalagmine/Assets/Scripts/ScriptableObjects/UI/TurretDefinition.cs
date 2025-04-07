using UnityEngine;

[CreateAssetMenu(fileName = "TurretDefinition", menuName = "ScriptableObjects/TurretDefinition", order = 1)]
public class TurretDefinition : ScriptableObject
{
    [SerializeField]
    private TurretSO turret;
    [SerializeField]
    private RenderTexture turretIcon;
    [SerializeField]
    private string turretName;

    public TurretSO Turret => turret;
    public RenderTexture TurretIcon => turretIcon;
    public string TurretName => turretName;
}
